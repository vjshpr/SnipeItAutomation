namespace SnipeItTests.Models;

public record AssetModel
{
    // Generates a random 9-digit numeric string for the Asset Tag
    public string Tag { get; init; } = Random.Shared.Next(100000000, 999999999).ToString();
    public string ModelName { get; init; } = "MacBook Pro 13\"";
    public string Status { get; init; } = "Ready to Deploy";
    public string SerialNumber { get; init; } = "SN-" + Random.Shared.Next(10000, 99999).ToString();
}