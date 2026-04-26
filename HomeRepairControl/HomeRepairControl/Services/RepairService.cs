using HomeRepairControl.Data;
using HomeRepairControl.Models;

namespace HomeRepairControl.Services
{
    public class RepairService
    {
        // Database context
        private AppDbContext context = new AppDbContext();

        // Adds a repair item to the database
        public void AddRepairItem(RepairItem item)
        {
            context.RepairItems.Add(item);
            context.SaveChanges();
        }

        // Overload: creates and adds a repair item from basic data
        public void AddRepairItem(string itemName, string damageDescription)
        {
            RepairItem item = new RepairItem(itemName, damageDescription);
            AddRepairItem(item);
        }

        // Returns all repair items
        public List<RepairItem> GetAllRepairItems()
        {
            return context.RepairItems.ToList();
        }

        // Finds a repair item by ID
        public RepairItem? GetRepairItemById(int id)
        {
            return context.RepairItems.FirstOrDefault(i => i.Id == id);
        }

        // Filters items by status
        public List<RepairItem> SearchByStatus(string status)
        {
            return context.RepairItems
                .Where(i => i.Status == status)
                .ToList();
        }

        // Updates status with transition validation
        public bool UpdateStatus(int id, string newStatus)
        {
            var item = GetRepairItemById(id);
            if (item == null) return false;

            // Valid transitions only
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

        // Deletes item and its related notes
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

        // Adds a note to a repair item
        public void AddRepairNote(int repairItemId, string text)
        {
            RepairNote note = new RepairNote(repairItemId, text);

            context.RepairNotes.Add(note);
            context.SaveChanges();
        }

        // Gets all notes for a specific item
        public List<RepairNote> GetNotesByRepairItem(int repairItemId)
        {
            return context.RepairNotes
                .Where(n => n.RepairItemId == repairItemId)
                .ToList();
        }

        // Counts notes for a specific item
        public int GetNoteCountByRepairItem(int repairItemId)
        {
            return context.RepairNotes.Count(n => n.RepairItemId == repairItemId);
        }

        // Updates item basic data
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

        // Searches items by name
        public List<RepairItem> SearchByName(string name)
        {
            return context.RepairItems
                .Where(i => i.ItemName.Contains(name))
                .ToList();
        }
    }
}