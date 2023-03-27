

using MicroRabbit.Transfer.Domian.Models;


namespace MicroRabbit.Transfer.Application.Interfaces
{
    public interface ITransferService
    {
        IEnumerable<Item> GetAllItems();
        IEnumerable<Item> GetItems();

    }
}
