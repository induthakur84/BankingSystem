namespace Account.Domain
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }


        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }
}
