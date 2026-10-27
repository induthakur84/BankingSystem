namespace Account.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        // MANY-TO-MANY RELATIONSHIP
        // One Category can contain multiple Products
        // One Product can belong to multiple Categories
        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}