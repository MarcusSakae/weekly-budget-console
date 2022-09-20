using System.Diagnostics;
using Hanssens.Net; // LocalStorage

internal class Program
{
    private static string? Identifier = "";
    private static Menu menu = new();
    private static Api? api;

    ///
    ///
    ///
    private static async Task Main(string[] args)
    {
        Identifier = GetOrCreateIdentifier();
        api = new(Identifier);
        Console.WriteLine("Identifier: " + Identifier);
        menu.AddItem(" ■ Add entry", () => AddEntry());
        menu.AddItem(" ■ Graph", () => DrawGraph());
        menu.AddItem(" ■ Exit", () => Environment.Exit(0));

        while (true)
        {
            if (menu.Enabled)
            {
                menu.process(); // render, handle input
                api.showPendingTasks();
            }
                await api.handlePendingTasks();
        }
    }




    ///
    /// Gets user identifier from local storage 
    /// (or creates one if it doesn't exist)
    /// 
    private static string GetOrCreateIdentifier()
    {
        LocalStorage storage = new();
        string? id = null;
        bool exists = storage.Exists(Constants.LOCALSTORAGE_ID_KEY);

        // Get an existing identifier
        if (exists) id = storage.Get(Constants.LOCALSTORAGE_ID_KEY).ToString();


        // Create a new identifier
        // Note: Just using "else" here would overwrite an already set id. (So don't do that.)
        if (!exists || id == null || id == "") id = Guid.NewGuid().ToString();

        storage.Store(Constants.LOCALSTORAGE_ID_KEY, id);
        return id;
    }


    ///
    ///
    ///
    private static void AddEntry()
    {
        string input = "";
        menu.Enabled = false;
        Console.Clear();

        // Read keys 0-9, backspace, enter. ignore everything else
        while (true)
        {
            ClearLine();
            Console.Write("Add new entry: " + input);
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    ClearLine();
                }
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key.KeyChar >= '0' && key.KeyChar <= '9')
            {
                input += key.KeyChar;
                ClearLine();
            }
        }

        if (input.Length > 0)
        {
            api!.AddEntry(int.Parse(input));
        }
        menu.Enabled = true;
        Console.Clear();
    }

    ///
    ///
    ///
    private static void ClearLine()
    {
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, menu.MenuHeight);
    }

    ///
    ///
    ///
    private static void DrawGraph()
    {
        Console.WriteLine("Graph");
    }
}