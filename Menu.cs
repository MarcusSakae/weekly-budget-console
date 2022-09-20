class Menu
{

    private List<(string, Action)> Actions = new List<(string, Action)>();
    private int Selected = 0;
    public bool Enabled { get; set; } = true;
    private Thread InputThread;
    public int MenuHeight = 4;

    // Add an menu item
    public void AddItem(string name, Action action) {
        Actions.Add((name, action));
        MenuHeight = Actions.Count + 2;
    }

    // Menu preparations
    public Menu()
    {        
        Console.CursorVisible = false;
        Console.Clear();

        // Start input thread
        InputThread = new Thread(new ThreadStart(handleInput));
        InputThread.Start();
    }

    ///
    ///
    ///
    public void process()
    {
        if (Enabled)
        {
            InputThread.Join(10);
            renderMenu();
        }
    }


    ///
    ///
    ///
    private void handleInput()
    {
        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey();
            if (input.Key == ConsoleKey.UpArrow)
            {
                Selected--;
                if (Selected < 0)
                {
                    Selected = Actions.Count - 1;
                }
            }
            else if (input.Key == ConsoleKey.DownArrow)
            {
                Selected++;
                if (Selected >= Actions.Count)
                {
                    Selected = 0;
                }
            }
            if (input.Key == ConsoleKey.Enter)
            {
                var (_, action) = Actions[Selected];
                action();
            }
        }
    }


    ///    
    ///    
    ///    
    private void renderMenu()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Menu (select using arrowkey & enter):");
        foreach (var item in Actions)
        {
            if (item == Actions[Selected])
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            Console.Write(item.Item1 + Environment.NewLine);
            Console.ResetColor();
        }
    }
}