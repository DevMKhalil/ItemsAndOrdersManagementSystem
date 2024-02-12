namespace ItemsAndOrdersManagementSystem.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public Order Order { get; set; }
        public Item Item { get; set; }
    }
}
