namespace InventoryManagementSystem.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Role { get; private set; }

        public User(int id, string username, string role)
        {
            Id = id;
            Username = username;
            Role = role;
        }
    }
}
