using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8080";

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                while (true)
                {
                    var key = Console.ReadLine().ToUpper();

                    if (key == "A")
                    {
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
                        hubContext.Clients.All.printMessage("asdfasdfasdfasdf");
                        Console.WriteLine("Server sending printMessage\n");
                    }

                    if (key == "C")
                    {
                        break;
                    }
                }


                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            var hubConfiguration = new HubConfiguration()
            {
                //EnableDetailedErrors = true,
            };

            app.MapSignalR(hubConfiguration);
        }
    }

    public class TestHub : Hub
    {
        public void SendMessage(string name, string message)
        {
            Console.WriteLine("Server executing: SendMessage");
            Clients.All.printMessage(name + " " + message);
        }
    }
}
