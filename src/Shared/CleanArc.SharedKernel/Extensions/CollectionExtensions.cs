namespace CleanArc.SharedKernel.Extensions;

public static class CollectionExtensions
{
    public static Dictionary<string, List<string>> ToGroupedDictionary(
        this IEnumerable<KeyValuePair<string, string>> keyValuePairs)
    {
        return keyValuePairs
            .GroupBy(kvp => kvp.Key)
            .ToDictionary(
                group => group.Key,
                group => group.Select(kvp => kvp.Value).ToList()
            );
    }
}