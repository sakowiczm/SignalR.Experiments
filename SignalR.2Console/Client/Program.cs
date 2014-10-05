using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://localhost:8080");

            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;

            var testHubProxy = hubConnection.CreateHubProxy("TestHub");

            testHubProxy.On<string>("printMessage", message => Console.WriteLine(message));

            hubConnection.Start().Wait();

            while (true)
            {
                string key = Console.ReadLine().ToUpper();

                if (key == "A")
                {
                    testHubProxy.Invoke("SendMessage", "client message", " sent from console").ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Console.WriteLine("Error opening connection: {0} \n", t.Exception.Flatten());
                        }
                    }).Wait();
                    Console.WriteLine("Client sending printMessage to server");
                }

                if (key == "C")
                {
                    break;
                }
                
                
            }

        }
    }
}
