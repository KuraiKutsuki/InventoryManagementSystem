namespace InventoryManagementSystem.Models
{
    public class Supplier
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ContactInfo { get; private set; }

        public Supplier(int id, string name, string contactInfo)
        {
            Id = id;
            Name = name;
            ContactInfo = contactInfo;
        }
    }
}
