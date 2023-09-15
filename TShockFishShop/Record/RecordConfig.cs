using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FishShop.Record;

// 购买记录
public class RecordConfig
{
    public List<RecordPlayerData> player = new();

    public static RecordConfig Load(string path)
    {
        if (File.Exists(path))
        {
            return JsonConvert.DeserializeObject<RecordConfig>(File.ReadAllText(path), new JsonSerializerSettings()
            {
                Error = (sender, error) => error.ErrorContext.Handled = true
            });
        }
        else
        {
            var c = new RecordConfig();
            File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            }));
            return c;
        }
    }
}