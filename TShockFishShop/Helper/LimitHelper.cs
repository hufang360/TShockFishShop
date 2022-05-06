using Newtonsoft.Json;
using System;
using System.IO;

namespace FishShop
{
    /// <summary>
    /// 商品限购
    /// </summary>
    public class LimitHelper
    {
        private static int lastSave = 0;
        private static bool isLoaded;
        private static ConfigLimit config;

        public static string recodFile = "";

        public static void Load(bool forceLoad = false)
        {
            if (!isLoaded || forceLoad)
            {
                config = ConfigLimit.Load(recodFile);
                isLoaded = true;
            }
        }

        private static void Save()
        {
            if (!isLoaded) return;

            // 至少5秒保存一次
            if (utils.GetUnixTimestamp - lastSave > 5)
            {
                lastSave = utils.GetUnixTimestamp;
                File.WriteAllText(recodFile, JsonConvert.SerializeObject(config, Formatting.Indented));
            }
        }

        public static void Record(string itemName, string playerName, int _id, int _amount)
        {
            Load();
            // 单人限购
            bool hasPlayer = false;
            foreach (PlayerLimitData pd in config.player)
            {
                if (pd.name == playerName)
                {
                    hasPlayer = true;
                    bool hasPlayerData = false;
                    foreach (LimitData d in pd.datas)
                    {
                        if (d.id == _id)
                        {
                            d.Record(_amount);
                            hasPlayerData = true;
                            break;
                        }
                    }
                    if (!hasPlayerData)
                    {
                        pd.datas.Add( new LimitData(itemName, _id, _amount) );
                    }
                    break;
                }
            }
            if(!hasPlayer)
            {
                PlayerLimitData pd = new PlayerLimitData(playerName);
                pd.datas.Add(new LimitData(itemName, _id, _amount));
                config.player.Add(pd);
            }

            // 全服限购
            bool hasData = false;
            foreach (LimitData d in config.server)
            {
                if (d.id == _id)
                {
                    d.Record(_amount);
                    hasData = true;
                    break;
                }
            }
            if (!hasData)
                config.server.Add( new LimitData(itemName, _id, _amount) );
            Save();
        }

        public static int GetPlayerRecord(string playerName, int _id)
        {
            Load();
            foreach (PlayerLimitData pd in config.player)
            {
                if (pd.name == playerName)
                {
                    foreach (LimitData d in pd.datas)
                    {
                        if (d.id == _id)
                            return d.count;
                    }
                    break;
                }
            }
            return -1;
        }

        public static int GetServerRecord(int _id)
        {
            Load();
            foreach (LimitData d in config.server)
            {
                if (d.id == _id)
                    return d.count;
            }
            return -1;
        }
        
        public static void ResetRecord()
        {
            Load();
            foreach (PlayerLimitData pd in config.player)
            {
                foreach (LimitData d in pd.datas)
                {
                    d.count = 0;
                }
            }

            foreach (LimitData d in config.server)
            {
                d.count = 0;
            }

            Save();
        }
    }
}