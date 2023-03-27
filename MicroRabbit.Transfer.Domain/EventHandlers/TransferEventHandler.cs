using MicroRabbit.Domain.Core.Bus;

using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domian.Models;


namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {
        private readonly ITransferRepository _transferRepository;
        public TransferEventHandler(ITransferRepository transferRepository) 
        { 
            _transferRepository = transferRepository;   
        }
        public Task Handle(TransferCreatedEvent @event)
        {
            _transferRepository.DeleteItem(new Item()
            {
                ItemId = @event.Id,
                Name = @event.Name,

                DeletedTime = DateTime.Now.ToUniversalTime(),
                UserId = @event.UserId,
                IsDeleted = true,
            });
            return Task.CompletedTask;
        }
    }
}
