using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Todo.Localziration;

public class Localizer
{
    private readonly JsonSerializer _jsonSerializer = new JsonSerializer();
    private Dictionary<string, string> LocalizedItemsOfVn = new();
    private Dictionary<string, string> LocalizedItemsOfEn = new();

    public Localizer()
    {
        LocalizedItemsOfVn = LoadResources("vi-Vi");
        LocalizedItemsOfEn = LoadResources("en-US");
    }


   
    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    private string GetString(string name)
    {
        if (LocalizeData().ContainsKey(name))
        {
            return LocalizeData()[name];
        }

        return null;
    }

    public Dictionary<string, string> LoadResources(string code)
    {
        var LocalizedItems = new Dictionary<string, string>();
        string filePath = $@"LanguageResources/Resources/{code}.json";
        using (var str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var sReader = new StreamReader(str))
        using (var reader = new JsonTextReader(sReader))
        {
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;
                string key = (string) reader.Value;
                reader.Read();
                string value = _jsonSerializer.Deserialize<string>(reader);
                LocalizedItems.Add(key, value);
            }
        }

        return LocalizedItems;
    }

    private Dictionary<string, string> LocalizeData()
    {
        if (Thread.CurrentThread.CurrentCulture.Name == "vi-VI")
        {
            return LocalizedItemsOfVn;
        }

        return LocalizedItemsOfEn;
    }
}