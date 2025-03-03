namespace Bank.DTO
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<Customer>? Customers { get; set; }
    }
}
