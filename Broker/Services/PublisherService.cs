using Broker.Models;
using Broker.Services.Interfaces;
using Google.Protobuf;
using Grpc.Core;
using GrpcAgent;
using System;
using System.Threading.Tasks;

namespace Broker.Services
{
    public class PublisherService : Publisher.PublisherBase
    {
        private readonly IMessageStoregeService _messageStorage;

        public PublisherService(IMessageStoregeService messageStoregeService)
        {
            _messageStorage = messageStoregeService;
        }


        public override Task<PublishReply> PublishMessage(PublishRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Received: {request.Topic} {request.Content}");

            var message = new Message(request.Topic, request.Content);
            _messageStorage.Add(message);

            return Task.FromResult(new PublishReply()
            {
                IsSuccess = true
            });
        }
    }
}
