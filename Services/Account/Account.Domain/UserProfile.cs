namespace Account.Domain
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string Address { get; set; }

        // Foreign Key
        // UserId connects UserProfile with User
        public int UserId { get; set; }

        // Navigation Property
        // Each UserProfile belongs to one User
        public User User { get; set; }
    }
}