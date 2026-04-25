using HomeRepairControl.Models;
using HomeRepairControl.Services;

RepairService repairService = new RepairService();

string option;

do
{
    Console.Clear();
    Console.WriteLine("=== HOME REPAIR CONTROL ===");
    Console.WriteLine("1. Add damaged item");
    Console.WriteLine("2. View all repair items");
    Console.WriteLine("3. Update repair status");
    Console.WriteLine("4. Add repair note");
    Console.WriteLine("5. View repair notes");
    Console.WriteLine("6. Delete repair item");
    Console.WriteLine("0. Exit");
    Console.Write("Select option: ");

    option = Console.ReadLine() ?? string.Empty;

    switch (option)
    {
        case "1":
            AddDamagedItem(repairService);
            break;
        case "2":
            ViewAllItems(repairService);
            break;
        case "3":
            UpdateRepairStatus(repairService);
            break;
        case "4":
            AddRepairNote(repairService);
            break;
        case "5":
            ViewRepairNotes(repairService);
            break;
        case "6":
            DeleteRepairItem(repairService);
            break;
        case "0":
            Console.WriteLine("Exiting...");
            break;
        default:
            Console.WriteLine("Invalid option.");
            Pause();
            break;
    }

} while (option != "0");

void AddDamagedItem(RepairService service)
{
    Console.Clear();

    Console.Write("Item name: ");
    string itemName = Console.ReadLine() ?? string.Empty;

    Console.Write("Damage description: ");
    string damageDescription = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(itemName) || string.IsNullOrWhiteSpace(damageDescription))
    {
        Console.WriteLine("Item name and damage description are required.");
        Pause();
        return;
    }

    service.AddRepairItem(itemName, damageDescription);

    Console.WriteLine("Damaged item added successfully.");
    Pause();
}

void ViewAllItems(RepairService service)
{
    Console.Clear();

    List<RepairItem> items = service.GetAllRepairItems();

    if (items.Count == 0)
    {
        Console.WriteLine("No repair items found.");
        Pause();
        return;
    }

    foreach (RepairItem item in items)
    {
        Console.WriteLine("---------------");
        item.ShowInfo();
    }

    Pause();
}

void UpdateRepairStatus(RepairService service)
{
    Console.Clear();

    int id = ReadInt("Repair item ID: ");

    RepairItem? item = service.GetRepairItemById(id);

    if (item == null)
    {
        Console.WriteLine("This repair item does not exist.");
        Pause();
        return;
    }

    Console.WriteLine($"Selected item: {item.ItemName}");
    Console.WriteLine("1. Pending");
    Console.WriteLine("2. In Repair");
    Console.WriteLine("3. Repaired");
    Console.Write("Select new status: ");

    string statusOption = Console.ReadLine() ?? string.Empty;
    string newStatus;

    switch (statusOption)
    {
        case "1":
            newStatus = "Pending";
            break;
        case "2":
            newStatus = "In Repair";
            break;
        case "3":
            newStatus = "Repaired";
            break;
        default:
            Console.WriteLine("Invalid status option.");
            Pause();
            return;
    }

    service.UpdateStatus(id, newStatus);
    Console.WriteLine("Repair status updated successfully.");
    Pause();
}

void AddRepairNote(RepairService service)
{
    Console.Clear();

    int id = ReadInt("Repair item ID: ");

    RepairItem? item = service.GetRepairItemById(id);

    if (item == null)
    {
        Console.WriteLine("This repair item does not exist.");
        Pause();
        return;
    }

    Console.WriteLine($"Selected item: {item.ItemName}");
    Console.Write("Note: ");
    string text = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(text))
    {
        Console.WriteLine("Note cannot be empty.");
        Pause();
        return;
    }

    service.AddRepairNote(id, text);
    Console.WriteLine("Repair note added successfully.");
    Pause();
}

void ViewRepairNotes(RepairService service)
{
    Console.Clear();

    int id = ReadInt("Repair item ID: ");

    RepairItem? item = service.GetRepairItemById(id);

    if (item == null)
    {
        Console.WriteLine("This repair item does not exist.");
        Pause();
        return;
    }

    List<RepairNote> notes = service.GetNotesByRepairItem(id);

    Console.WriteLine($"Notes for: {item.ItemName}");
    Console.WriteLine("----------------");

    if (notes.Count == 0)
    {
        Console.WriteLine("No notes found for this item.");
        Pause();
        return;
    }

    foreach (RepairNote note in notes)
    {
        Console.WriteLine($"{note.Date:dd/MM/yyyy} - {note.Text}");
    }

    Pause();
}

void DeleteRepairItem(RepairService service)
{
    Console.Clear();

    int id = ReadInt("Repair item ID to delete: ");

    RepairItem? item = service.GetRepairItemById(id);

    if (item == null)
    {
        Console.WriteLine("This repair item does not exist.");
        Pause();
        return;
    }

    Console.WriteLine($"Selected item: {item.ItemName}");
    Console.Write("Are you sure you want to delete this item? (y/n): ");
    string confirmation = Console.ReadLine() ?? string.Empty;

    if (confirmation.ToLower() == "y")
    {
        service.DeleteRepairItem(id);
        Console.WriteLine("Repair item deleted successfully.");
    }
    else
    {
        Console.WriteLine("Delete operation cancelled.");
    }

    Pause();
}

int ReadInt(string message)
{
    int value;

    Console.Write(message);

    while (!int.TryParse(Console.ReadLine(), out value))
    {
        Console.Write("Invalid input. Enter a valid number: ");
    }

    return value;
}

void Pause()
{
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}