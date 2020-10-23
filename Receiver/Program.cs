using System;
using System.Linq;
using Common;
using Grpc.Net.Client;
using GrpcAgent;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(EndpointsConstants.SubscribersAddress)
                .Build();

            host.Start();

            Subscribe(host);

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();

        }

        private static void Subscribe(IWebHost host)
        {
            var chanel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress);
            var client = new Subscriber.SubscriberClient(chanel);
            
            var address = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            Console.WriteLine($"Subscriber listening at: {address}");

            Console.Write("Enter the topic:");
            var topic = Console.ReadLine().ToLower();

            var request = new SubscribeRequest() { Topic = topic, Address = address };


            try
            {
                var reply = client.Subscribe(request);
                Console.WriteLine($"Subscribed reply: {reply.IsSuccess}");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error subscribing: {e.Message}");   
            }

            }

    }
}
