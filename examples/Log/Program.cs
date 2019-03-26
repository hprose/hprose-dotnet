using Hprose.RPC;
using Hprose.RPC.Plugins.Log;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

class MyService {
    public int Sum(int x, int y) {
        return x + y;
    }
    public string Hello(string name, ServiceContext context) {
        var endPoint = context.RemoteEndPoint as IPEndPoint;
        return "Hello " + name + " from " + endPoint.Address + ":" + endPoint.Port;
    }
}

public interface IMyService {
    [Log(false)]
    Task<int> Sum(int x, int y);
    Task<string> Hello(string name);
}

class Program {
    static async Task RunClient() {
        var client = new Client("http://127.0.0.1:8080/");
        client.Use(Log.InvokeHandler);
        var proxy = client.UseService<IMyService>();
        Console.WriteLine(await proxy.Hello("world"));
        Console.WriteLine(await proxy.Sum(1, 2));
    }
    static void Main(string[] args) {
        Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        HttpListener server = new HttpListener();
        server.Prefixes.Add("http://127.0.0.1:8080/");
        server.Start();
        var service = new Service();
        service.Use(Log.IOHandler)
            .AddInstanceMethods(new MyService())
            .Bind(server);
        RunClient().Wait();
        server.Stop();
    }
}