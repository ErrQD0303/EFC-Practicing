namespace EFC_Attributes;
using Helpers;

public class DBTypesAttribute : Attribute
{
    public Dictionary<string, string> TypeMappings { get; set; }

    public DBTypesAttribute(string tableName, string propertyName)
    {
        var mappings = AppConstants.MODEL_TYPE_MAPPINGS[tableName];
        TypeMappings = mappings.FirstOrDefault(pair => pair.Key == propertyName).Value.Select((m, _) =>
        {
            var pair = m.Split(":");
            if (pair.Length != 2)
            {
                throw new ArgumentException("Invalid mapping format. Expected format: 'ProviderName:Type'");
            }
            var (key, value) = (pair[0], pair[1]);
            return new KeyValuePair<string, string>(key, value);
        }).ToDictionary();
    }
}
