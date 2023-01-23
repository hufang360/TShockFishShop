using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FishShop
{
    // 购买记录
    public class RecordFile
    {
        public List<PlayerRecordData> player = new List<PlayerRecordData>();

        public static RecordFile Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<RecordFile>(File.ReadAllText(path), new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
                });
            }
            else
            {
                var c = new RecordFile();
                File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                }));
                return c;
            }
        }
    }
}