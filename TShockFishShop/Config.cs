using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;


namespace FishShop
{
    #region 商品配置
    public class Config
    {
        // 商店名称
        public string name = "鱼店";

        // 货架展示容量
        public int pageSlots = 40;

        // 一行展示多少个
        public int rowSlots = 10;

        // 解锁条件
        public List<ItemData> unlock = new List<ItemData>();

        // 货架物品
        public List<ShopItem> shop = new List<ShopItem>();


        public static Config Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path), new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
                });
            }
            else
            {
                // 读取内嵌配置文件
                string text = utils.FromEmbeddedPath("FishShop.res.config.json");
                Config c = JsonConvert.DeserializeObject<Config>(text, new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
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


    public class ItemData
    {
        public string name = "";

        public int id = 0;

        public int stack = 1;

        public ItemData(string _name = "", int _id = 0, int _stack = 1)
        {
            name = _name;
            id = _id;
            stack = _stack;
        }

        public void FixIDByName()
        {
            if (id == 0 && name != "")
                id = IDSet.GetIDByName(name);
            if (stack == 0)
                stack = 1;
        }

        public string GetItemDesc()
        {
            if (id == 0)
                FixIDByName();
            return utils.GetItemDesc("", id, stack);
        }
    }
    #endregion


    #region 限购配置
    public class ConfigLimit
    {
        public List<PlayerLimitData> player = new List<PlayerLimitData>();
        public List<LimitData> server = new List<LimitData>();

        public static ConfigLimit Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<ConfigLimit>(File.ReadAllText(path), new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
                });
            }
            else
            {
                var c = new ConfigLimit();
                File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                }));
                return c;
            }
        }
    }

    public class LimitData
    {
        [DefaultValue("")]
        public string name = "";
        public int id = 0;
        public int count = 0;
        public int total = 0;

        public LimitData(string _name, int _id, int _count)
        {
            name = _name;
            id = _id;
            count = _count;
            total = _count;

        }

        public void Record(int num)
        {
            count += num;
            total += num;
        }
    }

    public class PlayerLimitData
    {
        [DefaultValue("")]
        public string name = "";
        public List<LimitData> datas = new List<LimitData>();

        public PlayerLimitData(string playerName)
        {
            name = playerName;
        }
    }
    #endregion


    #region 插件配置
    public class Settings
    {
        private static SetConfig _settings;
        public static Dictionary<int, SetShopData> datas = new Dictionary<int, SetShopData>();

        public static void Load(string path)
        {
            _settings = SetConfig.Load(path);
            datas.Clear();
            foreach (SetShopData d in _settings.shopItem)
            {
                if (!datas.ContainsKey(d.id))
                {
                    datas.Add(d.id, d);
                }
            }
        }

        public static void GenConfig(string path)
        {
            SetConfig.GenConfig(path);
        }

        public static SetConfig Con { get { return _settings; } }


        // 获得商品名
        public static string GetShopItemNameByID(int id)
        {
            return datas.ContainsKey(id) ? datas[id].name : "";
        }

        // 获得物品id
        public static int GetItemIDByName(string _name)
        {
            // 自定义物品
            foreach (SetShopData d in Con.shopItem)
            {
                if (d.name == _name) return d.id;
            }

            // 物品名 mapping
            foreach (SetMappingData d in Con.itemMap)
            {
                if (d.map.Contains(_name)) return d.id;
            }
            return 0;
        }

        // 获得npc id
        public static int GetNPCIDByName(string _name)
        {
            // NPC名 mapping
            foreach (SetMappingData d in Con.npcMap)
            {
                if (d.map.Contains(_name)) return d.id;
            }
            return 0;
        }

        // 获得npc 名称
        public static string GetNPCNameByID(int id)
        {
            // NPC名 mapping
            foreach (SetMappingData d in Con.npcMap)
            {
                if (d.id == id) return d.map[0];
            }
            return "";
        }
    }


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


    public class SetShopData
    {
        // 商品名称
        [DefaultValue("")]
        public string name = "";

        // 商品id
        public int id = 0;

        [DefaultValue("")]
        public string comment = "";

        // 单次最大购买数
        public int buyMax = 0;

        // 是否需要玩家或者
        public bool needAlive = false;

        //public void Init()
        //{
        //        name = "好运来",
        //        id = ShopItemID.BuffGoodLucky,
        //        comment = "获得10分钟 幸运 buff",
        //        buyMax = 1,
        //        needAlive = true
        //}
    }


    public class SetMappingData
    {
        // id（物品id、商品id）
        public int id = 0;

        public List<string> map = new List<string>();
    }
    #endregion

}