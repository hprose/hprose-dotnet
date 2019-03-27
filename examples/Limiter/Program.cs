using Hprose.RPC;
using Hprose.RPC.Plugins.Limiter;
using System;
using System.Net;
using System.Threading.Tasks;

class MyService {
    public int Sum(int x, int y) {
        return x + y;
    }
}

public interface IMyService {
    Task<int> Sum(int x, int y);
}

class Program {
    static async Task RunClient() {
        var client = new Client("http://127.0.0.1:8080/");
        client.Use(new ConcurrentLimiter(64).Handler).Use(new RateLimiter(2000).InvokeHandler);
        var begin = DateTime.Now;
        var proxy = client.UseService<IMyService>();
        var n = 5000;
        var tasks = new Task<int>[n];
        for (int i = 0; i < n; ++i) {
            tasks[i] = proxy.Sum(i, i);
        }
        await Task.WhenAll(tasks);
        var end = DateTime.Now;
        Console.WriteLine(end - begin);
    }
    static void Main(string[] args) {
        HttpListener server = new HttpListener();
        server.Prefixes.Add("http://127.0.0.1:8080/");
        server.Start();
        var service = new Service();
        service.AddInstanceMethods(new MyService()).Bind(server);
        RunClient().Wait();
        server.Stop();
    }
}