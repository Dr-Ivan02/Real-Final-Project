namespace HomeRepairControl.Models
{
    public class RepairItem : RepairBase
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string DamageDescription { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string Notes { get; set; } = string.Empty;

        public RepairItem() { }

        public RepairItem(string itemName, string damageDescription)
        {
            ItemName = itemName;
            DamageDescription = damageDescription;
            EntryDate = DateTime.Now;
            Status = "Pending";
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Item: {ItemName}");
            Console.WriteLine($"Damage: {DamageDescription}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine($"Entry Date: {EntryDate:dd/MM/yyyy}");
            Console.WriteLine($"Notes: {Notes}");
        }
    }
}