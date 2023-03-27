

using MicroRabbit.Transfer.Domian.Models;


namespace MicroRabbit.Transfer.Domain.Interfaces
{
    public interface ITransferRepository
    {
        IEnumerable<Item> GetAllItems();
        IEnumerable<Item> GetItems();
        void DeleteItem(Item item);
    }
}
