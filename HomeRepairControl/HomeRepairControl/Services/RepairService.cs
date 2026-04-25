using HomeRepairControl.Data;
using HomeRepairControl.Models;

namespace HomeRepairControl.Services
{
    public class RepairService
    {
        private AppDbContext context = new AppDbContext();

        public void AddRepairItem(RepairItem item)
        {
            context.RepairItems.Add(item);
            context.SaveChanges();
        }

        public void AddRepairItem(string itemName, string damageDescription)
        {
            RepairItem item = new RepairItem(itemName, damageDescription);
            AddRepairItem(item);
        }

        public List<RepairItem> GetAllRepairItems()
        {
            return context.RepairItems.ToList();
        }

        public RepairItem? GetRepairItemById(int id)
        {
            return context.RepairItems.FirstOrDefault(i => i.Id == id);
        }

        public List<RepairItem> SearchByStatus(string status)
        {
            return context.RepairItems
                .Where(i => i.Status == status)
                .ToList();
        }

        public void UpdateStatus(int id, string status)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
                item.Status = status;
                context.SaveChanges();
            }
        }

        public void UpdateNotes(int id, string notes)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
                item.Notes = notes;
                context.SaveChanges();
            }
        }

        public void DeleteRepairItem(int id)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
                List<RepairNote> notes = context.RepairNotes
                    .Where(n => n.RepairItemId == id)
                    .ToList();

                context.RepairNotes.RemoveRange(notes);
                context.RepairItems.Remove(item);
                context.SaveChanges();
            }
        }

        public void AddRepairNote(int repairItemId, string text)
        {
            RepairNote note = new RepairNote(repairItemId, text);

            context.RepairNotes.Add(note);
            context.SaveChanges();
        }

        public List<RepairNote> GetNotesByRepairItem(int repairItemId)
        {
            return context.RepairNotes
                .Where(n => n.RepairItemId == repairItemId)
                .ToList();
        }
    }
}