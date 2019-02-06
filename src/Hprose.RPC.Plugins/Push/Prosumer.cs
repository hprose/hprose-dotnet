/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Prosumer.cs                                             |
|                                                          |
|  Prosumer plugin for C#.                                 |
|                                                          |
|  LastModified: Feb 3, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Push {
    public class Prosumer {
        static Prosumer() {
            TypeManager.Register<Message>("@");
        }
        private ConcurrentDictionary<string, Action<Message>> callbacks = new ConcurrentDictionary<string, Action<Message>>();
        public Action<Exception> OnError { get; set; } = null;
        public Action<string> OnSubscribe { get; set; } = null;
        public Action<string> OnUnsubscribe { get; set; } = null;
        public Client Client { get; private set; }
        public string Id {
            get {
                return GetId();
            }
            set {
                Client.RequestHeaders.Id = value;
            }
        }
        private string GetId() {
            if (((IDictionary<string, object>)Client.RequestHeaders).ContainsKey("Id")) {
                return Client.RequestHeaders.Id.ToString();
            }
            throw new KeyNotFoundException("client unique id not found");
        }
        public Prosumer(Client client, string id = null) {
            Client = client;
            if (id != null && id.Length > 0) {
                Id = id;
            }
        }
        private async void Dispatch(Dictionary<string, Message[]> topics) {
            foreach (var topic in topics) {
#if NET40
                await TaskEx.Yield();
#else
                await Task.Yield();
#endif
                if (callbacks.TryGetValue(topic.Key, out var callback)) {
                    if (topic.Value == null) {
                        callbacks.TryRemove(topic.Key, out callback);
                        OnUnsubscribe?.Invoke(topic.Key);
                    }
                    else {
                        foreach (var message in topic.Value) {
#if NET40
                            await TaskEx.Yield();
#else
                            await Task.Yield();
#endif
                            callback(message);
                        }
                    }
                }
            }
        }
        private async void Message() {
            while(!callbacks.IsEmpty) {
                try {
                    var topics = await Client.InvokeAsync<Dictionary<string, Message[]>>("<").ConfigureAwait(false);
                    if (topics == null) return;
                    Dispatch(topics);
                }
                catch (Exception e) {
                    OnError?.Invoke(e);
                }
            }
        }
        public async Task<bool> Subscribe(string topic, Action<Message> callback) {
            if (Id != null && Id.Length > 0) {
                callbacks[topic] = callback;
                bool result = await Client.InvokeAsync<bool>("+", new object[] { topic }).ConfigureAwait(false);
                Message();
                OnSubscribe?.Invoke(topic);
                return result;
            }
            return false;
        }
        public async Task<bool> Subscribe<T>(string topic, Action<T, string> callback) {
            return await Subscribe(topic, (message) => callback(Converter<T>.Convert(message.Data), message.From)).ConfigureAwait(false);
        }
        public async Task<bool> Subscribe<T>(string topic, Action<T> callback) {
            return await Subscribe(topic, (message) => callback(Converter<T>.Convert(message.Data))).ConfigureAwait(false);
        }
        public async Task<bool> Unsubscribe(string topic) {
            if (Id != null && Id.Length > 0) {
                bool result = await Client.InvokeAsync<bool>("-", new object[] { topic }).ConfigureAwait(false);
                callbacks.TryRemove(topic, out var callback);
                OnUnsubscribe?.Invoke(topic);
                return result;
            }
            return false;
        }
        public Task<bool> Unicast(object data, string topic, string id) {
            return Client.InvokeAsync<bool>(">", new object[] { data, topic, id });
        }
        public Task<IDictionary<string, bool>> Multicast(object data, string topic, IEnumerable<string> ids) {
            return Client.InvokeAsync<IDictionary<string, bool>>(">?", new object[] { data, topic, ids });
        }
        public Task<IDictionary<string, bool>> Broadcast(object data, string topic) {
            return Client.InvokeAsync<IDictionary<string, bool>>(">*", new object[] { data, topic });
        }
        public Task<bool> Push(object data, string topic, string id) => Unicast(data, topic, id);
        public Task<IDictionary<string, bool>> Push(object data, string topic, IEnumerable<string> ids) => Multicast(data, topic, ids);
        public Task<IDictionary<string, bool>> Push(object data, string topic) => Broadcast(data, topic);
        public Task<bool> Exists(string topic, string id = null) {
            if (id == null || id.Length == 0) {
                id = Id;
            }
            return Client.InvokeAsync<bool>("?", new object[] { topic, id });
        }
        public Task<IList<string>> IdList(string topic) {
            return Client.InvokeAsync<IList<string>>("|", new object[] { topic });
        }
    }
}