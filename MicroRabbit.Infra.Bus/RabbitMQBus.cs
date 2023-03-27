using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handler;
        private readonly List<Type> _eventTypes;
        
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _handler = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command); 
        }
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection()) 
            using(var channel = connection.CreateModel()) 
            {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);

                var body = Encoding.UTF8.GetBytes(message); 

                channel.BasicPublish("", eventName, null, body);
            }
        }       

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if(!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if(!_handler.ContainsKey(eventName))
            {
                _handler.Add(eventName, new List<Type>());
            }

            if (_handler[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentOutOfRangeException(
                    @"Handler Type {handlerType.Name} alredy is registered for {eventName}", nameof(handlerType));                    
            }

            _handler[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof (T).Name;
            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName,true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                await ProssesEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task ProssesEvent(string eventName, string message)
        {
            if(_handler.ContainsKey(eventName))
            {
                using (var scop = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handler[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scop.ServiceProvider.GetService(subscription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
                
            }
        }
    }
}
