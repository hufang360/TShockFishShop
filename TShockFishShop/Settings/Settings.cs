using System.Collections.Generic;


// 插件配置
namespace FishShop
{
    public class Settings
    {
        private static SetConfig _settings;
        public static Dictionary<int, SetShopData> shopItem = new Dictionary<int, SetShopData>();
        public static Dictionary<int, string> npcMap = new Dictionary<int, string>();

        public static void Load(string path)
        {
            _settings = SetConfig.Load(path);
            shopItem.Clear();
            npcMap.Clear();
            foreach (SetShopData d in _settings.shopItem)
            {
                if (!shopItem.ContainsKey(d.id)) shopItem.Add(d.id, d);
            }
            foreach (SetMappingData d in _settings.npcMap)
            {
                if (!npcMap.ContainsKey(d.id)) npcMap.Add(d.id, d.map[0]);
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
            return shopItem.ContainsKey(id) ? shopItem[id].name : "";
        }
        public static int GetShopItemIDByName(string _name)
        {
            foreach (var obj in shopItem)
            {
                if (obj.Value.name == _name) return obj.Key;
            }
            return 0;
        }


        // 物品 mapping
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

        // npc mapping
        public static int GetNPCIDByName(string _name)
        {
            foreach (SetMappingData d in Con.npcMap)
            {
                if (d.map.Contains(_name)) return d.id;
            }
            return 0;
        }
        public static string GetNPCNameByID(int id)
        {
            if (npcMap.ContainsKey(id)) return npcMap[id];
            return "";
        }

    }
}
