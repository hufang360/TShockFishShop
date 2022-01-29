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

            goods = new ShopItem("蠕虫罐头", 4345);
            goods.AddCost("鲈鱼", 2290, 30);
            shop.Add(goods);

            goods = new ShopItem("沙块", 169, 999);
            goods.AddUnlock("渔夫任务",0, 15);
            goods.AddCost("金币", 0, 33);
            shop.Add(goods);

            goods = new ShopItem("金币", 73, 5);
            goods.AddCost("鲈鱼", 2290, 30);

            goods = new ShopItem("生命水晶", 29);
            goods.AddUnlock("肉后");
            goods.AddUnlock("生命<400");
            goods.AddCost("金币", 0, 2);
            shop.Add(goods);


            goods = new ShopItem("十字墓石碑", 1174, 10);
            goods.AddCost("银币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("草药袋", 3093, 5);
            goods.AddCost("金币", 0, 5);
            shop.Add(goods);

            goods = new ShopItem("水瓶", 126, 100);
            goods.AddCost("金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("木材", 9, 100);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 3);
            shop.Add(goods);

            goods = new ShopItem("暗影木", 911, 300);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 60);
            shop.Add(goods);

            goods = new ShopItem("红木", 620, 300);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 60);
            shop.Add(goods);

            goods = new ShopItem("建筑师发明背包", 3061);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("金币", 0, 5);
            shop.Add(goods);

            goods = new ShopItem("无底水桶", 3031);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("金币", 0, 20);
            shop.Add(goods);

            goods = new ShopItem("超级吸水棉", 3032);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("熔岩吸收棉", 4872);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("金币", 0, 40);
            shop.Add(goods);

            goods = new ShopItem("镜面鱼", 2309, 30);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("装甲洞穴鱼", 2303, 30);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("黑檀锦鲤", 2318, 30);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("七彩矿鱼", 2310, 30);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("血腥食人鱼", 2319, 30);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("斑驳油鱼", 2311, 30);
            goods.AddUnlock("渔夫任务",0, 5);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("冤大头钓竿", 2296);
            goods.AddUnlock("骷髅王");
            goods.AddCost("铂金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("优质钓鱼线", 2373);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("铂金币", 0, 1);
            goods.AddCost("任务鱼");
            shop.Add(goods);

            goods = new ShopItem("渔夫耳环", 2374);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("铂金币", 0, 1);
            goods.AddCost("任务鱼");
            shop.Add(goods);

            goods = new ShopItem("钓具箱", 2375);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("铂金币", 0, 1);
            goods.AddCost("任务鱼");
            shop.Add(goods);

            goods = new ShopItem("渔民袖珍宝典", 3120);
            goods.AddUnlock("渔夫任务",0, 20);
            goods.AddCost("铂金币", 0, 1);
            goods.AddCost("任务鱼");
            shop.Add(goods);

            goods = new ShopItem("天气收音机", 3037);
            goods.AddUnlock("渔夫任务",0, 20);
            goods.AddCost("铂金币", 0, 1);
            goods.AddCost("任务鱼");
            shop.Add(goods);

            goods = new ShopItem("六分仪", 3096);
            goods.AddUnlock("渔夫任务",0, 20);
            goods.AddCost("铂金币", 0, 1);
            goods.AddCost("任务鱼");
            shop.Add(goods);

            goods = new ShopItem("魔力花", 555, 1);
            goods.AddUnlock("渔夫任务",0, 10);
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("天界磁石", 2219);
            goods.AddUnlock("渔夫任务",0, 20);
            goods.AddCost("铂金币", 0, 2);
            shop.Add(goods);

            goods = new ShopItem("每秒伤害计数器", 3119);
            goods.AddUnlock("骷髅王");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("生命体分析机", 3118);
            goods.AddUnlock("骷髅王");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("秒表", 3099);
            goods.AddUnlock("骷髅王");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("雷达", 3084);
            goods.AddUnlock("骷髅王");
            goods.AddCost("铂金币", 0, 5);
            goods.AddCost("任务鱼");
            shop.Add(goods);

            goods = new ShopItem("钱币槽", 3213);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("猪鲨翅膀", 2609);
            goods.AddUnlock("猪鲨");
            goods.AddCost("猪鲨宝藏袋", 3330, 3);
            shop.Add(goods);

            goods = new ShopItem("光女翅膀", 4823);
            goods.AddUnlock("光女");
            goods.AddCost("光女宝藏袋", 4782, 3);
            shop.Add(goods);

            goods = new ShopItem("蛙腿", 2423);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("金币", 0, 150);
            shop.Add(goods);

            goods = new ShopItem("猛爪手套", 211);
            goods.AddUnlock("肉后");
            goods.AddCost("丛林匣", 3208, 3);
            shop.Add(goods);

            goods = new ShopItem("闪亮红气球", 159);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("天空匣", 3206, 3);
            shop.Add(goods);

            goods = new ShopItem("罐中臭屁", 1724);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("钛金匣", 3981, 3);
            shop.Add(goods);

            goods = new ShopItem("攀爬爪", 953);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("金币", 0, 80);
            shop.Add(goods);

            goods = new ShopItem("鞋钉", 975);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("金币", 0, 80);
            shop.Add(goods);

            goods = new ShopItem("雨歌", 4057);
            goods.AddUnlock("雨天");
            goods.AddUnlock("肉后");
            goods.AddCost("金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("金金鱼", 4274);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("金币", 0, 20);
            shop.Add(goods);

            goods = new ShopItem("混沌传送杖", 1326);
            goods.AddUnlock("肉后");
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("混沌精旗帜", 1629, 20);
            shop.Add(goods);

            goods = new ShopItem("钛金匣", 3981);
            goods.AddUnlock("肉后");
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddCost("金币", 0, 80);
            shop.Add(goods);

            goods = new ShopItem("木匣", 2334, 99);
            goods.AddUnlock("花后");
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddCost("金币", 0, 99);
            shop.Add(goods);

            goods = new ShopItem("金匣", 2336, 99);
            goods.AddUnlock("花后");
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddCost("铂金币", 0, 99);
            shop.Add(goods);

            goods = new ShopItem("掠夺鲨", 2341);
            goods.AddUnlock("渔夫任务", 0, 10);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("锯齿鲨", 2342);
            goods.AddUnlock("渔夫任务", 0, 10);
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("剑鱼", 2332);
            goods.AddUnlock("渔夫任务", 0, 10);
            goods.AddCost("金币", 0, 5);
            shop.Add(goods);

            goods = new ShopItem("寒霜飞鱼", 3197, 99);
            goods.AddUnlock("渔夫任务", 0, 1);
            goods.AddCost("金币", 0, 3);
            shop.Add(goods);

            goods = new ShopItem("毒弹枪", 3210);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddUnlock("肉后");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("舌锋剑", 3211);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddUnlock("肉后");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("水晶蛇", 3209);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddUnlock("肉后");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("带磷松露", 2429);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddUnlock("肉后");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("黑曜石剑鱼", 2331);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddUnlock("肉后");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("先进战斗技术", 4382);
            goods.AddUnlock("渔夫任务", 0, 20);
            goods.AddUnlock("血月");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("韧皮雕像", 4276);
            goods.AddUnlock("渔夫任务", 0, 10);
            goods.AddCost("金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("雨云魔杖", 1244);
            goods.AddUnlock("渔夫任务", 0, 10);
            goods.AddCost("金币", 0, 20);
            shop.Add(goods);

            goods = new ShopItem("再生法杖", 213);
            goods.AddCost("金币", 0, 10);
            shop.Add(goods);

            goods = new ShopItem("手枪", 164);
            goods.AddUnlock("骷髅王");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("村正", 155);
            goods.AddUnlock("骷髅王");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("钴护盾", 156);
            goods.AddUnlock("骷髅王");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("暗影钥匙", 329);
            goods.AddUnlock("骷髅王");
            goods.AddCost("金币", 0, 50);
            shop.Add(goods);

            goods = new ShopItem("魔法飞弹", 113);
            goods.AddUnlock("骷髅王");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("彩虹枪", 1260);
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddUnlock("花后");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("吸血鬼刀", 1569, 1, "传奇");
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddUnlock("花后");
            goods.AddCost("铂金币", 0, 30);
            shop.Add(goods);

            goods = new ShopItem("腐化者之戟", 1571);
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddUnlock("花后");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("寒霜九头蛇法杖", 1572);
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddUnlock("花后");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("沙漠虎杖", 4607);
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddUnlock("花后");
            goods.AddCost("铂金币", 0, 1);
            shop.Add(goods);

            goods = new ShopItem("食人鱼枪", 1156);
            goods.AddUnlock("渔夫任务", 0, 30);
            goods.AddUnlock("花后");
            goods.AddCost("铂金币", 0, 1);
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