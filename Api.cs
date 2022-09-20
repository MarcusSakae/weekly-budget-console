using System.Text;
using Newtonsoft.Json;


class Api
{
    private string Identifier = "";
    private List<Exception> Exceptions = new();
    //    private static string BaseUrl = "https://weeklybudget.azurewebsites.net/api/";
    private static string BaseUrl = "http://localhost:7072/api/";
    private static List<Task<HttpResponseMessage>> PendingTasks = new();
    public Api(string id)
    {
        Identifier = id;
    }

    ///
    ///
    ///
    public void AddEntry(int amount)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nAdding entry...");

        UsageEntry entry = new();
        entry.Amount = amount;
        entry.PartitionKey = Identifier;
        Console.WriteLine(JsonConvert.SerializeObject(entry));
        HttpClient client = new();
        string url = BaseUrl + "AddUsage";
        Console.WriteLine("Sending request to " + url + "...");
        PendingTasks.Add(
            client.PostAsync(url, new StringContent(
                JsonConvert.SerializeObject(entry),
                Encoding.UTF8,
                "application/json"
            )));
        Console.ResetColor();
    }

    ///
    ///
    ///
    public void showPendingTasks()
    {
        Console.SetCursorPosition(0, 10);
        Console.WriteLine("--- Pending tasks ---");
        foreach (var task in PendingTasks)
        {
            Console.WriteLine(task.Status + "                    ");
        }
    }

    public async Task handlePendingTasks()
    {

        for (int i = 0; i < PendingTasks.Count; i++)
        {
            Task<HttpResponseMessage> task = PendingTasks[i];
            try
            {
                if (task.Status == TaskStatus.WaitingForActivation)
                {
                    task.WaitAsync(TimeSpan.FromMilliseconds(10));
                }
                else if (task.Status == TaskStatus.Faulted)
                {
                    Console.WriteLine(await task.Result.Content.ReadAsStringAsync());
                }
                else if (task.Status == TaskStatus.Faulted)
                {
                    var response = await task;
                    Console.WriteLine("Response: " + response.StatusCode);

                }
            }
            catch (Exception e)
            {
                PendingTasks.Remove(task);
                Console.WriteLine("Request failed: " + e.Message);
                break;
            }
        }
        // Remove failed/completed tasks
        PendingTasks.RemoveAll(task => task.Status == TaskStatus.Faulted || task.Status == TaskStatus.RanToCompletion);
    }

}