using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FishShop
{
    public class SetConfig
    {
        // 自定义的商品信息
        public List<SetShopData> shopItem = new List<SetShopData>();

        // 物品名称映射
        public List<SetMappingData> itemMap = new List<SetMappingData>();

        // npc名称映射
        public List<SetMappingData> npcMap = new List<SetMappingData>();


        public static SetConfig Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<SetConfig>(File.ReadAllText(path), new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
                });
            }
            else
            {
                // 读取内嵌配置
                string text = utils.FromEmbeddedPath("FishShop.res.settings.json");
                SetConfig c = JsonConvert.DeserializeObject<SetConfig>(text, new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
                });

                // 将内嵌配置拷出
                File.WriteAllText(path, text);
                return c;
            }
        }

        public static void GenConfig(string path)
        {
            // 将内嵌配置文件拷出
            if (!File.Exists(path)) File.WriteAllText(path, utils.FromEmbeddedPath("FishShop.res.settings.json"));
        }
    }
}
