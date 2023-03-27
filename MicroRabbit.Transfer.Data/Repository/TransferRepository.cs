

using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domian.Models;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private ItemDbContext _ctx;

        public TransferRepository(ItemDbContext ctx)
        {
           _ctx = ctx;
        }
        public void DeleteItem(Item item)
        {
            _ctx.Items.Update(item);
            _ctx.SaveChanges();
        }
        public IEnumerable<Item> GetAllItems()
        {
            return _ctx.Items;
            
        }
        public IEnumerable<Item> GetItems()
        {
            return _ctx.Items.Where(c => c.IsDeleted == false);
        }
    }
}
