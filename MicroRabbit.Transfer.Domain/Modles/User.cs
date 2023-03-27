namespace MicroRabbit.Transfer.Domian.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        // Other properties
        public ICollection<Item> Items { get; set; }
    }
}
