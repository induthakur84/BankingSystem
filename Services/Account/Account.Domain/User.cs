namespace Account.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // ONE-TO-ONE RELATIONSHIP
        // One User can have only one UserProfile
        public UserProfile UserProfile { get; set; }


        // ONE-TO-MANY RELATIONSHIP
        // One User can have multiple Orders
        // Each Order belongs to only one User
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}