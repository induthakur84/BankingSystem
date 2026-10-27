namespace Account.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        // MANY-TO-MANY RELATIONSHIP
        // One Product can belong to multiple Categories
        // One Category can contain multiple Products
        public ICollection<Category> Categories { get; set; }
            = new List<Category>();
    }
}