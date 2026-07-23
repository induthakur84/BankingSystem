namespace Account.Domain
{
    public class UserProfile
    {

        public int Id {  get; set; }
        public string Address { get; set; }

        public int UserId { get; set; }

        //navigation property to the user entity

        public User User { get; set; }

    }
}
