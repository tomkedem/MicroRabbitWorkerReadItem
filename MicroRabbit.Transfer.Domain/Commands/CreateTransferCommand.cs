
namespace MicroRabbit.Transfer.Domain.Commands
{
    public class CreateTransferCommand : TransferCommand
    {
        public CreateTransferCommand(int itemId, string name, bool isDeleted, int userId)
        {
            ItemId = itemId;
            Name = name;
            IsDeleted = isDeleted;
            UserId = userId;
        }        
    }
}
