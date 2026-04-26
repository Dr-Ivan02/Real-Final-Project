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

        public bool UpdateStatus(int id, string newStatus)
        {
            var item = GetRepairItemById(id);
            if (item == null) return false;

            if (item.Status == "Pending" && newStatus == "In Repair")
            {
                item.Status = newStatus;
            }
            else if (item.Status == "In Repair" && newStatus == "Repaired")
            {
                item.Status = newStatus;
            }
            else
            {
                return false;
            }

            context.SaveChanges();
            return true;
        }

        public void UpdateNotes(int id, string notes)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
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
        public int GetNoteCountByRepairItem(int repairItemId)
        {
            return context.RepairNotes.Count(n => n.RepairItemId == repairItemId);
        }

        public void UpdateRepairItem(int id, string itemName, string damageDescription)
        {
            RepairItem? item = GetRepairItemById(id);

            if (item != null)
            {
                item.ItemName = itemName;
                item.DamageDescription = damageDescription;
                context.SaveChanges();
            }
        }

        public List<RepairItem> SearchByName(string name)
        {
            return context.RepairItems
                .Where(i => i.ItemName.Contains(name))
                .ToList();
        }
    }
}