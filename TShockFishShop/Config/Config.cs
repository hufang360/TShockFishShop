using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


namespace FishShop;

public class Config
{
    // 商店名称
    public string name = "鱼店";

    // 货架展示容量
    public int pageSlots = 40;

    // 一行展示多少个
    public int rowSlots = 10;

    // 解锁条件
    public List<ItemData> unlock = [];

    // 货架物品
    public List<ShopItemData> shop = [];


    public static Config Load(string path)
    {
        if (File.Exists(path))
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path), new JsonSerializerSettings()
            {
                Error = (sender, error) =>
                {
                    utils.Log($"JSON解析错误: {error.ErrorContext.Error.Message}");
                    error.ErrorContext.Handled = true;
                }
            });
        }
        else
        {
            // 读取内嵌配置文件
            string text = utils.FromEmbeddedPath("FishShop.res.config.json");
            Config c = JsonConvert.DeserializeObject<Config>(text, new JsonSerializerSettings()
            {
                Error = (sender, error) =>
                {
                    utils.Log($"内嵌配置JSON解析错误: {error.ErrorContext.Error.Message}");
                    error.ErrorContext.Handled = true;
                }
            });

            // 将内嵌配置文件拷出
            File.WriteAllText(path, text);

            return c;
        }
    }

    public static void GenConfig(string path)
    {
        // 将内嵌配置文件拷出
        if (!File.Exists(path)) File.WriteAllText(path, utils.FromEmbeddedPath("FishShop.res.config.json"));
    }

}