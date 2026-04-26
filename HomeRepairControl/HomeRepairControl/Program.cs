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
    Console.WriteLine("3. Edit repair item");
    Console.WriteLine("4. Search repair item");
    Console.WriteLine("5. Update repair status");
    Console.WriteLine("6. Add repair note");
    Console.WriteLine("7. View repair notes");
    Console.WriteLine("8. Delete repair item");
    Console.WriteLine("0. Exit");
    Console.Write("Select option: ");

    option = Console.ReadLine() ?? string.Empty;

    switch (option)
    {
        case "1": AddDamagedItem(repairService); break;
        case "2": ViewAllItems(repairService); break;
        case "3": EditRepairItem(repairService); break;
        case "4": SearchRepairItem(repairService); break;
        case "5": UpdateRepairStatus(repairService); break;
        case "6": AddRepairNote(repairService); break;
        case "7": ViewRepairNotes(repairService); break;
        case "8": DeleteRepairItem(repairService); break;
        case "0": Console.WriteLine("Exiting..."); break;
        default:
            Console.WriteLine("Invalid option. Try again.");
            Pause();
            break;
    }

} while (option != "0");

// Adds a new repair item
void AddDamagedItem(RepairService service)
{
    Console.Clear();

    string itemName = ReadRequiredText("Item name: ");
    string damageDescription = ReadRequiredText("Damage description: ");

    service.AddRepairItem(itemName, damageDescription);

    Console.WriteLine("Damaged item added successfully.");
    Pause();
}

// Displays all repair items with note count
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
        int noteCount = service.GetNoteCountByRepairItem(item.Id);

        Console.WriteLine("---------------");
        item.ShowInfo();
        Console.WriteLine($"Notes Count: {noteCount}");
    }

    Pause();
}

// Edits an existing repair item
void EditRepairItem(RepairService service)
{
    Console.Clear();

    if (!ShowItemsForSelection(service))
    {
        Pause();
        return;
    }

    int id = ReadInt("Repair item ID to edit: ");
    RepairItem? item = service.GetRepairItemById(id);

    if (item == null)
    {
        Console.WriteLine("This repair item does not exist.");
        Pause();
        return;
    }

    // Prevent editing completed items
    if (item.Status == "Repaired")
    {
        Console.WriteLine("Cannot modify a repaired item.");
        Pause();
        return;
    }

    Console.WriteLine($"Editing item: {item.ItemName}");

    string newName = ReadRequiredText("New item name: ");
    string newDescription = ReadRequiredText("New damage description: ");

    service.UpdateRepairItem(id, newName, newDescription);

    Console.WriteLine("Repair item updated successfully.");
    Pause();
}

// Searches items by name or status
void SearchRepairItem(RepairService service)
{
    Console.Clear();

    string option;

    do
    {
        Console.WriteLine("Search by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Status");
        Console.Write("Select option: ");

        option = Console.ReadLine() ?? string.Empty;

        if (option != "1" && option != "2")
        {
            Console.WriteLine("Invalid option. Try again.\n");
        }

    } while (option != "1" && option != "2");

    List<RepairItem> results;

    if (option == "1")
    {
        string name = ReadRequiredText("Enter item name: ");
        results = service.SearchByName(name);
    }
    else
    {
        string status = SelectStatus();
        results = service.SearchByStatus(status);
    }

    if (results.Count == 0)
    {
        Console.WriteLine("No results found.");
        Pause();
        return;
    }

    foreach (RepairItem item in results)
    {
        Console.WriteLine("---------------");
        item.ShowInfo();
    }

    Pause();
}

// Updates the status with validation rules
void UpdateRepairStatus(RepairService service)
{
    Console.Clear();

    if (!ShowItemsForSelection(service))
    {
        Pause();
        return;
    }

    int id = ReadInt("Repair item ID: ");
    RepairItem? item = service.GetRepairItemById(id);

    if (item == null)
    {
        Console.WriteLine("This repair item does not exist.");
        Pause();
        return;
    }

    // Prevent modifying completed items
    if (item.Status == "Repaired")
    {
        Console.WriteLine("Cannot modify a repaired item.");
        Pause();
        return;
    }

    Console.WriteLine($"Selected item: {item.ItemName}");

    string newStatus = SelectStatus();

    bool success = service.UpdateStatus(id, newStatus);

    if (!success)
    {
        Console.WriteLine("Invalid state transition.");
    }
    else
    {
        Console.WriteLine("Repair status updated successfully.");
    }

    Pause();
}

// Adds a note to a repair item
void AddRepairNote(RepairService service)
{
    Console.Clear();

    if (!ShowItemsForSelection(service))
    {
        Pause();
        return;
    }

    int id = ReadInt("Repair item ID: ");
    RepairItem? item = service.GetRepairItemById(id);

    if (item == null)
    {
        Console.WriteLine("This repair item does not exist.");
        Pause();
        return;
    }

    // Prevent adding notes to completed items
    if (item.Status == "Repaired")
    {
        Console.WriteLine("Cannot add notes to a repaired item.");
        Pause();
        return;
    }

    Console.WriteLine($"Selected item: {item.ItemName}");

    string text = ReadRequiredText("Repair note: ");

    service.AddRepairNote(id, text);

    Console.WriteLine("Repair note added successfully.");
    Pause();
}

// Displays notes for a specific item
void ViewRepairNotes(RepairService service)
{
    Console.Clear();

    if (!ShowItemsForSelection(service))
    {
        Pause();
        return;
    }

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
        Console.WriteLine($"{note.Date:dd/MM/yyyy hh:mm tt} - {note.Text}");
    }

    Pause();
}

// Deletes a repair item with confirmation
void DeleteRepairItem(RepairService service)
{
    Console.Clear();

    if (!ShowItemsForSelection(service))
    {
        Pause();
        return;
    }

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

// Shows items to help user select an ID
bool ShowItemsForSelection(RepairService service)
{
    List<RepairItem> items = service.GetAllRepairItems();

    if (items.Count == 0)
    {
        Console.WriteLine("No repair items found.");
        return false;
    }

    Console.WriteLine("Available repair items:");
    Console.WriteLine("-----------------------");

    foreach (RepairItem item in items)
    {
        Console.WriteLine($"{item.Id}. {item.ItemName} - {item.Status}");
    }

    Console.WriteLine();
    return true;
}

// Forces user to select a valid status
string SelectStatus()
{
    string option;

    do
    {
        Console.WriteLine("Select status:");
        Console.WriteLine("1. Pending");
        Console.WriteLine("2. In Repair");
        Console.WriteLine("3. Repaired");
        Console.Write("Option: ");

        option = Console.ReadLine() ?? string.Empty;

        if (option != "1" && option != "2" && option != "3")
        {
            Console.WriteLine("Invalid status option. Try again.\n");
        }

    } while (option != "1" && option != "2" && option != "3");

    return option switch
    {
        "1" => "Pending",
        "2" => "In Repair",
        "3" => "Repaired",
        _ => "Pending"
    };
}

// Reads a valid integer input
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

// Ensures non-empty text input
string ReadRequiredText(string message)
{
    string value;

    do
    {
        Console.Write(message);
        value = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            Console.WriteLine("This field is required.");
        }

    } while (string.IsNullOrWhiteSpace(value));

    return value;
}

// Pauses execution until key press
void Pause()
{
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}