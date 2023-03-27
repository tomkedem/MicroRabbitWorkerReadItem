using MicroRabbit.Domain.Core.Commands;

namespace MicroRabbit.Transfer.Domain.Commands
{
    public abstract class TransferCommand : Command
    {
        public int ItemId { get; protected set; }
        public string Name { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public int UserId { get; protected set; }
    }
}
