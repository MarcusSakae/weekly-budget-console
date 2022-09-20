using Newtonsoft.Json;

public class UsageEntry
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = "";
    [JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; } = "";
    public int Amount { get; set; }
    public System.DateTime CreatedAt { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
