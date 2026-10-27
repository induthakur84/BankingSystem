namespace Account.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        // Foreign Key
        // UserId connects the Order table with the User table
        public int UserId { get; set; }


        // Navigation Property
        // Each Order belongs to only ONE User
        public User User { get; set; }
    }
}