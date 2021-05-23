/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Broker.cs                                               |
|                                                          |
|  Broker plugin for C#.                                   |
|                                                          |
|  LastModified: Jul 2, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Push {
    public class Broker {
        static Broker() {
            TypeManager.Register<Message>("@");
        }
        private static readonly Dictionary<string, Message[]> emptyMessage = new(0);
        protected ConcurrentDictionary<string, ConcurrentDictionary<string, BlockingCollection<Message>>> Messages { get; } = new();
        protected ConcurrentDictionary<string, TaskCompletionSource<Dictionary<string, Message[]>>> Responders { get; } = new();
        protected ConcurrentDictionary<string, TaskCompletionSource<bool>> Timers { get; } = new();
        public Service Service { get; private set; }
        public int MessageQueueMaxLength { get; set; } = 10;
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 2, 0);
        public TimeSpan HeartBeat { get; set; } = new TimeSpan(0, 0, 10);
        public event Action<string, string, ServiceContext> OnSubscribe;
        public event Action<string, string, Message[], ServiceContext> OnUnsubscribe;
        public Broker(Service service) {
            Service = service;
            Service.Add<string, ServiceContext, bool>(Subscribe, "+")
                   .Add<string, ServiceContext, bool>(Unsubscribe, "-")
                   .Add<ServiceContext, Task<Dictionary<string, Message[]>>>(Message, "<")
                   .Add<object, string, string, string, bool>(Unicast, ">")
                   .Add<object, string, IEnumerable<string>, string, IDictionary<string, bool>>(Multicast, ">?")
                   .Add<object, string, string, IDictionary<string, bool>>(Broadcast, ">*")
                   .Add<string, string, bool>(Exists, "?")
                   .Add<string, IList<string>>(IdList, "|")
                   .Use(Handler);
        }
        protected bool Send(string id, TaskCompletionSource<Dictionary<string, Message[]>> responder) {
            if (!Messages.TryGetValue(id, out var topics)) {
                responder.TrySetResult(null);
                return true;
            }
            if (topics.IsEmpty) {
                responder.TrySetResult(null);
                return true;
            }
            var result = new Dictionary<string, Message[]>();
            int count = 0;
            foreach (var topic in topics) {
                var name = topic.Key;
                var messages = topic.Value;
                if (messages == null) {
                    ++count;
                    result.Add(name, null);
                    topics.TryRemove(name, out var temp);
                    temp?.Dispose();
                }
                else if (messages.Count > 0) {
                    ++count;
                    var newmessages = new BlockingCollection<Message>(MessageQueueMaxLength);
                    if (!topics.TryUpdate(name, newmessages, messages)) {
                        newmessages.Dispose();
                    }
                    result.Add(name, messages.ToArray());
                    messages.Dispose();
                }
            }
            if (count == 0) return false;
            responder.TrySetResult(result);
            DoHeartBeat(id);
            return true;
        }
        protected async void DoHeartBeat(string id) {
            if (HeartBeat <= TimeSpan.Zero) {
                return;
            }
            var timer = new TaskCompletionSource<bool>();
            Timers.AddOrUpdate(id, timer, (_, oldtimer) => {
                oldtimer.TrySetResult(false);
                return timer;
            });
            using (var source = new CancellationTokenSource()) {
#if NET40
                var delay = TaskEx.Delay(HeartBeat, source.Token);
                var task = await TaskEx.WhenAny(timer.Task, delay).ConfigureAwait(false);
#else
                var delay = Task.Delay(HeartBeat, source.Token);
                var task = await Task.WhenAny(timer.Task, delay).ConfigureAwait(false);
#endif
                source.Cancel();
                if (task == delay) {
                    timer.TrySetResult(true);
                }
            }
            if (await timer.Task.ConfigureAwait(false) && Messages.TryGetValue(id, out var topics)) {
                foreach (var topic in topics.Keys) {
                    Offline(topics, id, topic, new ServiceContext(Service));
                }
            }
        }
        protected static string GetId(ServiceContext context) {
            if (context.RequestHeaders.TryGetValue("id", out var id)) {
                return id.ToString();
            }
            throw new KeyNotFoundException("Client unique id not found");
        }
        protected bool Subscribe(string topic, ServiceContext context) {
            var id = GetId(context);
            var topics = Messages.GetOrAdd(id, (_) => new ConcurrentDictionary<string, BlockingCollection<Message>>());
            if (topics.TryGetValue(topic, out var messages)) {
                if (messages != null) return false;
            }
            messages = new BlockingCollection<Message>(MessageQueueMaxLength);
            topics.AddOrUpdate(topic, messages, (_, oldmessages) => {
                if (oldmessages != null) {
                    messages.Dispose();
                    return oldmessages;
                }
                return messages;
            });
            OnSubscribe?.Invoke(id, topic, context);
            return true;
        }
        protected void Response(string id) {
            if (Responders.TryGetValue(id, out var responder)) {
                if (responder != null) {
                    if (Send(id, responder)) {
                        Responders.TryUpdate(id, null, responder);
                    }
                }
            }
        }
        protected bool Offline(ConcurrentDictionary<string, BlockingCollection<Message>> topics, string id, string topic, ServiceContext context) {
            if (topics.TryRemove(topic, out var messages)) {
                OnUnsubscribe?.Invoke(id, topic, messages?.ToArray(), context);
                messages?.Dispose();
                Response(id);
                return true;
            }
            return false;
        }
        protected bool Unsubscribe(string topic, ServiceContext context) {
            var id = GetId(context);
            if (Messages.TryGetValue(id, out var topics)) {
                return Offline(topics, id, topic, context);
            }
            return false;
        }
        protected async Task<Dictionary<string, Message[]>> Message(ServiceContext context) {
            var id = GetId(context);
            if (Responders.TryRemove(id, out var responder)) {
                responder?.TrySetResult(null);
            }
            if (Timers.TryRemove(id, out var timer)) {
                timer.TrySetResult(false);
            }
            responder = new TaskCompletionSource<Dictionary<string, Message[]>>();
            if (!Send(id, responder)) {
                Responders.AddOrUpdate(id, responder, (_, oldResponder) => {
                    oldResponder?.TrySetResult(null);
                    return responder;
                });
                if (Timeout > TimeSpan.Zero) {
                    using var source = new CancellationTokenSource();
#if NET40
                    var delay = TaskEx.Delay(Timeout, source.Token);
                    var task = await TaskEx.WhenAny(responder.Task, delay).ConfigureAwait(false);
#else
                    var delay = Task.Delay(Timeout, source.Token);
                    var task = await Task.WhenAny(responder.Task, delay).ConfigureAwait(false);
#endif
                    source.Cancel();
                    if (task == delay) {
                        responder.TrySetResult(emptyMessage);
                        DoHeartBeat(id);
                    }
                }
            }
            return await responder.Task.ConfigureAwait(false);
        }
        public bool Unicast(object data, string topic, string id, string from = "") {
            try {
                if (Messages.TryGetValue(id, out var topics)) {
                    if (topics.TryGetValue(topic, out var messages)) {
                        if (messages.TryAdd(new Message() { Data = data, From = from })) {
                            Response(id);
                            return true;
                        }
                    }
                }
            }
            catch (Exception) { }
            return false;
        }
        public IDictionary<string, bool> Multicast(object data, string topic, IEnumerable<string> ids, string from = "") {
            var result = new Dictionary<string, bool>();
            foreach (var id in ids) {
                result.Add(id, Unicast(data, topic, id, from));
            }
            return result;
        }
        public IDictionary<string, bool> Broadcast(object data, string topic, string from = "") {
            var result = new Dictionary<string, bool>();
            foreach (var topics in Messages) {
                if (topics.Value.TryGetValue(topic, out var messages)) {
                    var id = topics.Key;
                    if (messages.TryAdd(new Message() { Data = data, From = from })) {
                        Response(id);
                        result.Add(id, true);
                        continue;
                    }
                    result.Add(id, false);
                }
            }
            return result;
        }
        public bool Push(object data, string topic, string id, string from = "") {
            return Unicast(data, topic, id, from);
        }
        public IDictionary<string, bool> Push(object data, string topic, IEnumerable<string> ids, string from = "") {
            return Multicast(data, topic, ids, from);
        }
        public IDictionary<string, bool> Push(object data, string topic, string from = "") {
            return Broadcast(data, topic, from);
        }
        public void Deny(string id, string topic = null) {
            if (Messages.TryGetValue(id, out var topics)) {
                if (topic != null && topic.Length > 0) {
                    if (topics.TryGetValue(topic, out var messages)) {
                        topics.TryUpdate(topic, null, messages);
                        messages.Dispose();
                    }
                }
                else {
                    foreach (var pair in topics) {
                        topics.TryUpdate(pair.Key, null, pair.Value);
                        pair.Value.Dispose();
                    }
                }
                Response(id);
            }
        }
        public bool Exists(string topic, string id) {
            if (Messages.TryGetValue(id, out var topics)) {
                if (topics.TryGetValue(topic, out var messages)) {
                    return messages != null;
                }
            }
            return false;
        }
        public IList<string> IdList(string topic) {
            var idlist = new List<string>(Messages.Count);
            foreach (var topics in Messages) {
                var id = topics.Key;
                if (topics.Value.TryGetValue(topic, out var messages) && messages != null) {
                    idlist.Add(id);
                }
            }
            return idlist;
        }
        protected async Task<object> Handler(string name, object[] args, Context context, NextInvokeHandler next) {
            var serviceContext = context as ServiceContext;
            var from = "";
            if (serviceContext.RequestHeaders.TryGetValue("id", out var id)) {
                from = id.ToString();
            }
            switch (name) {
                case ">":
                case ">?":
                    args[3] = from;
                    break;
                case ">*":
                    args[2] = from;
                    break;
            }
            return await next(name, args, new BrokerContext(new Producer(this, from), serviceContext)).ConfigureAwait(false);
        }
        private class Producer : IProducer {
            private readonly Broker Broker;
            public string From { get; private set; }
            public Producer(Broker broker, string from) {
                Broker = broker;
                From = from;
            }
            public bool Unicast(object data, string topic, string id) => Broker.Unicast(data, topic, id, From);
            public IDictionary<string, bool> Multicast(object data, string topic, IEnumerable<string> ids) => Broker.Multicast(data, topic, ids, From);
            public IDictionary<string, bool> Broadcast(object data, string topic) => Broker.Broadcast(data, topic, From);
            public bool Push(object data, string topic, string id) => Broker.Push(data, topic, id, From);
            public IDictionary<string, bool> Push(object data, string topic, IEnumerable<string> ids) => Broker.Push(data, topic, ids, From);
            public IDictionary<string, bool> Push(object data, string topic) => Broker.Push(data, topic, From);
            public void Deny(string id = null, string topic = null) => Broker.Deny(id ?? From, topic);
            public bool Exists(string topic, string id = null) => Broker.Exists(topic, id ?? From);
            public IList<string> IdList(string topic) => Broker.IdList(topic);
        }
    }
}