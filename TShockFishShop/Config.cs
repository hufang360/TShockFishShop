using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Plugin
{
    public class Config
    {
        // 商店名称
        public string name = "鱼店";

        // 货架展示容量
        public int pageSlots = 50;

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
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
            }
            else
            {
                var c = new Config();
                c.InitDefault();
                File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented));
                return c;
            }
        }

        public void InitDefault()
        {
            // 商店解锁条件
            unlock.Add( new ItemData("渔夫任务",0,1) );

            // 定义货架物品
            ShopItem goods = new ShopItem("蠕虫罐头", 4345, 10);
            goods.AddCost("金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("无趣弓", 3853);
            goods.AddUnlock("石后");
            goods.AddCost("双足翼龙圣物", 4948, 1);
            shop.Add(goods);

            goods = new ShopItem("第一分形", 4722);
            goods.AddUnlock("石后");
            goods.AddCost("铂金币", 0, 30);
            shop.Add(goods);

            goods = new ShopItem("下个月相");
            goods.AddUnlock("渔夫任务", 0, 10);
            goods.AddCost("金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("更换任务鱼");
            goods.AddUnlock("渔夫任务", 0, 10);
            goods.AddCost("金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("烟花起飞");
            goods.AddCost("金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("雨来");
            goods.AddCost("金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("雨停");
            goods.AddCost("金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("调白天");
            goods.AddCost("金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("调晚上");
            goods.AddCost("金币", 0, 3);
            shop.Add(goods);

            goods = new ShopItem("跳过入侵");
            goods.AddCost("金币", 0, 3);
            shop.Add(goods);

            goods = new ShopItem("召唤世纪之花");
            goods.AddUnlock("三王后");
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("复活NPC");
            goods.AddCost("金币", 0, 5);
            shop.Add(goods);

            goods = new ShopItem("集合打团");
            goods.AddCost("金币", 0, 5);
            shop.Add(goods);
        }
    }

}