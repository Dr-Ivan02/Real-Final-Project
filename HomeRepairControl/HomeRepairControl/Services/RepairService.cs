using HomeRepairControl.Models;

namespace HomeRepairControl.Services
{
    public class RepairService
    {
        private List<RepairItem> repairItems = new List<RepairItem>();
        private List<RepairNote> repairNotes = new List<RepairNote>();
        private int nextItemId = 1;
        private int nextNoteId = 1;

        public void AddRepairItem(RepairItem item)
        {
            item.Id = nextItemId++;
            repairItems.Add(item);
        }

        public void AddRepairItem(string itemName, string damageDescription)
        {
            RepairItem item = new RepairItem(itemName, damageDescription);
            AddRepairItem(item);
        }

        public List<RepairItem> GetAllRepairItems()
        {
            return repairItems;
        }

        public RepairItem? GetRepairItemById(int id)
        {
            return repairItems.FirstOrDefault(i => i.Id == id);
        }

        public List<RepairItem> SearchByStatus(string status)
        {
            return repairItems
                .Where(i => i.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void UpdateStatus(int id, string status)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
                item.Status = status;
            }
        }

        public void UpdateNotes(int id, string notes)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
                item.Notes = notes;
            }
        }

        public void DeleteRepairItem(int id)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
                repairItems.Remove(item);
                repairNotes.RemoveAll(n => n.RepairItemId == id);
            }
        }

        public void AddRepairNote(int repairItemId, string text)
        {
            RepairNote note = new RepairNote(repairItemId, text);
            note.Id = nextNoteId++;
            repairNotes.Add(note);
        }

        public List<RepairNote> GetNotesByRepairItem(int repairItemId)
        {
            return repairNotes
                .Where(n => n.RepairItemId == repairItemId)
                .ToList();
        }
    }
}