namespace Account.Domain
{
    public class User
    {

        public int Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public UserProfile UserProfile { get; set; }  
        

        // here we are creating relation between user and orders
        // where one user can have the multiple order

        // each order is related only one user


        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
