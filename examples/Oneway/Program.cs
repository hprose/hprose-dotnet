using System.Threading.Tasks;
using Hprose.RPC;
using Hprose.RPC.Plugins.Oneway;

public interface IRestart {
    [Oneway]
    Task Restart();
}
class Program {
    // static void Main(string[] args) {
    //     var client = new Client("http://127.0.0.1/");
    //     client.Use(Oneway.Handler);
    //     client.Invoke("restart", null, new ClientContext() {
    //         ["Oneway"] = true
    //     });
    // }

    static async Task Example() {
        var client = new Client("http://127.0.0.1/");
        client.Use(Oneway.Handler);
        var proxy = client.UseService<IRestart>();
        await proxy.Restart();
    }
    static void Main(string[] args) {
        Example().Wait();
    }
}