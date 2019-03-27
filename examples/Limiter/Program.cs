using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Hprose.RPC;
using Hprose.RPC.Plugins.Limiter;

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
        var client = new Client("tcp4://127.0.0.1:8412");
        client.Use(new ConcurrentLimiter(64).Handler).Use(new RateLimiter(10000).InvokeHandler);
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
        IPAddress iPAddress = Dns.GetHostAddresses("127.0.0.1")[0];
        TcpListener server = new TcpListener(iPAddress, 8412);
        server.Start();
        var service = new Service();
        service.AddInstanceMethods(new MyService()).Bind(server);
        RunClient().Wait();
        server.Stop();
    }
}