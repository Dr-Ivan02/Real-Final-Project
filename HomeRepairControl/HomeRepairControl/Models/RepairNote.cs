namespace HomeRepairControl.Models
{
    public class RepairNote
    {
        public int Id { get; set; }
        public int RepairItemId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public RepairNote() { }

        public RepairNote(int repairItemId, string text)
        {
            RepairItemId = repairItemId;
            Text = text;
            Date = DateTime.Now;
        }
    }
}