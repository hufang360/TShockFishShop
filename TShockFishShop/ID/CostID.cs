using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace FishShop
{
    /// <summary>
    /// 交易物品ID
    /// 说明：商品ID、解锁ID、交易物品ID 共用一套id规则，部分id即使解锁ID又是商品ID。
    /// </summary>
    public class CostID
    {
        public const int QuestFish = -101;              // 当天任务鱼
        public const int AnyQuestFish = -102;           // 任意任务鱼
        public const int AnyFish = -103;                // 任意鱼

        public const int AnyWood = -200;                // 任意木材
        public const int AnyTorch = -201;               // 任意火把
        public const int AnyIronBar = -202;             // 任意铁锭
        public const int AnySand = -203;                // 任意沙块
        public const int AnyPressurePlate = -204;       // 任意压力板
        public const int AnyBird = -205;                // 任意鸟
        public const int AnyScorpion = -206;            // 任意蝎子
        public const int AnySquirrel = -207;            // 任意松鼠
        public const int AnyJungleBug = -208;           // 任意丛林虫子
        public const int AnyDuck = -209;                // 任意鸭子
        public const int AnyButterfly = -210;           // 任意蝴蝶
        public const int AnyFirefly = -211;             // 任意萤火虫
        public const int AnySnail = -212;               // 任意蜗牛
        public const int AnyTurtle = -213;              // 任意乌龟
        public const int AnyMacaw = -214;               // 任意金刚鹦鹉
        public const int AnyCockatiel = -215;           // 任意玄凤鹦鹉
        public const int AnyDragonfly = -216;           // 任意蜻蜓
        public const int AnyFruit = -217;               // 任意水果
        public const int AnyBalloon = -218;             // 任意气球
        public const int AnyCloudBalloon = -219;        // 任意云朵气球
        public const int AnyBlizzardBalloon = -220;     // 任意暴雪气球
        public const int AnySandstormBalloon = -221;    // 任意沙暴气球


        public const int AnyHorseshoeBalloon = -222;    // 任意马掌气球
        public const int AnyCrate = -223;               // 任意宝匣
        public const int AnyTombstone = -224;           // 任意墓碑
        public const int AnyGoldCritter = -225;         // 任意金色小动物
        public const int AnyParrot = -226;              // 任意鹦鹉

        #region id集合
        // 任意任务鱼 Main.anglerQuestItemNetIDs
        // 任意宝匣 ItemID.Sets.IsFishingCrate

        /// <summary>
        /// 任意鱼
        /// </summary>
        public static readonly int[] Fishes = new int[] { 2303, 2299, 2290, 2317, 2305, 2304, 2313, 2318, 2312, 4401, 2306, 2308, 2319, 2314, 2302, 2315, 2307, 2310, 2301, 4402, 2298, 2316, 2309, 2321, 2297, 2300, 2311 };

        /// <summary>
        /// 任意墓碑
        /// </summary>
        public static readonly int[] Tombstones = new int[] { 321, 1173, 1174, 1175, 1176, 1177, 3230, 3231, 3229, 3233, 3232 };

        /// <summary>
        /// 任意金色小动物
        /// </summary>
        public static readonly int[] GoldCritters = new int[] { 2889, 2890, 2891, 4340, 2892, 4274, 2893, 4362, 2894, 4482, 3564, 4419, 2895 };

        /// <summary>
        /// 任意马掌气球
        /// </summary>
        public static readonly int[] HorseshoeBalloons = new int[] { 1250, 1251, 1252, 3250, 3251, 3252 };


        /// <summary>
        /// 任意鹦鹉
        /// </summary>
        public static readonly int[] Parrots = new int[] { 5212, 5300, 5312, 5313 };

        /// <summary>
        /// 任意宝匣
        /// </summary>
        public static readonly int[] Crates = new int[] { 2334, 2335, 2336, 3203, 3204, 3205, 3206, 3207, 3208, 4405, 4407, 4877, 5002, 3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 4406, 4408, 4878, 5003 };


        #region wiki 定义
        // --------------------------------------------------------------------
        // 参考：https://terraria.wiki.gg/wiki/Alternative_crafting_ingredients
        // --------------------------------------------------------------------

        /// <summary>
        /// 任意木材
        /// </summary>
        public static readonly int[] Woods = new int[] { 9, 619, 620, 621, 911, 1729, 2504, 2503, 5215 };

        /// <summary>
        /// 任意火把
        /// </summary>
        public static readonly int[] Torches = new int[] { 8, 430, 432, 427, 429, 428, 1245, 431, 974, 3114, 3004, 2274, 433, 523, 1333, 3045, 4383, 4384, 4385, 4386, 4387, 4388, 5293 };

        /// <summary>
        /// 任意铁锭
        /// </summary>
        public static readonly int[] IronBars = new int[] { 22, 704 };

        /// <summary>
        /// 任意沙块
        /// </summary>
        public static readonly int[] Sands = new int[] { 169, 408, 1246, 370, 3272, 3338, 3274, 3275 };

        /// <summary>
        /// 任意压力板
        /// </summary>
        public static readonly int[] PressurePlates = new int[] { 852, 543, 542, 541, 1151, 529, 853, 4261 };

        /// <summary>
        /// 任意鸟
        /// </summary>
        public static readonly int[] Birds = new int[] { 2015, 2016, 2017 };

        /// <summary>
        /// 任意蝎子
        /// </summary>
        public static readonly int[] Scorpions = new int[] { 2157, 2156 };

        /// <summary>
        /// 任意松鼠
        /// </summary>
        public static readonly int[] Squirrels = new int[] { 2018, 3563 };

        /// <summary>
        /// 任意丛林虫子
        /// </summary>
        public static readonly int[] JungleBugs = new int[] { 3194, 3192, 3193 };

        /// <summary>
        /// 任意鸭子
        /// </summary>
        public static readonly int[] Ducks = new int[] { 2123, 2122 };

        /// <summary>
        /// 任意蝴蝶
        /// </summary>
        public static readonly int[] Butterflies = new int[] { 1998, 2001, 1994, 1995, 1996, 1999, 1997, 2000 };

        /// <summary>
        /// 任意萤火虫
        /// </summary>
        public static readonly int[] Fireflies = new int[] { 1992, 2004 };

        /// <summary>
        /// 任意蜗牛
        /// </summary>
        public static readonly int[] Snails = new int[] { 2006, 2007 };

        /// <summary>
        /// 任意乌龟
        /// </summary>
        public static readonly int[] Turtles = new int[] { 2006, 2007 };

        /// <summary>
        /// 任意金刚鹦鹉
        /// </summary>
        public static readonly int[] Macaws = new int[] { 5212, 5300 };

        /// <summary>
        /// 任意玄凤鹦鹉
        /// </summary>
        public static readonly int[] Cockatiels = new int[] { 5312, 5313 };

        /// <summary>
        /// 任意蜻蜓
        /// </summary>
        public static readonly int[] Dragonflies = new int[] { 4334, 4335, 4336, 4338, 4339, 4337 };

        /// <summary>
        /// 任意水果
        /// </summary>
        public static readonly int[] Fruits = new int[] { 4009, 4282, 4283, 4284, 4285, 4286, 4287, 4288, 4289, 4290, 4291, 4292, 4293, 4294, 4295, 4296, 4297, 5277, 5278 };

        /// <summary>
        /// 任意气球
        /// </summary>
        public static readonly int[] Balloons = new int[] { 3738, 3736, 3737 };

        /// <summary>
        /// 任意云朵气球
        /// </summary>
        public static readonly int[] CloudBalloons = new int[] { 399, 1250 };

        /// <summary>
        /// 任意暴雪气球
        /// </summary>
        public static readonly int[] BlizzardBalloons = new int[] { 1163, 1251 };

        /// <summary>
        /// 任意沙暴气球
        /// </summary>
        public static readonly int[] SandstormBalloons = new int[] { 983, 1252 };
        #endregion

        #endregion



        /// <summary>
        /// id 和 名称 对照
        /// </summary>
        static readonly Dictionary<int, string> mapping = new()
        {
            {QuestFish, "当天任务鱼"},
            {AnyQuestFish, "任意任务鱼"},
            {AnyFish, "任意一种鱼"},

            {AnyWood, "任意木材"},
            {AnyTorch, "任意火把"},
            {AnyIronBar, "任意铁锭"},
            {AnySand, "任意沙块"},
            {AnyPressurePlate, "任意压力板"},
            {AnyBird, "任意鸟"},
            {AnyScorpion, "任意蝎子"},
            {AnySquirrel, "任意松鼠"},
            {AnyJungleBug, "任意丛林虫子"},
            {AnyDuck, "任意鸭子"},
            {AnyButterfly, "任意蝴蝶"},
            {AnyFirefly, "任意萤火虫"},
            {AnySnail, "任意蜗牛"},
            {AnyTurtle, "任意乌龟"},
            {AnyMacaw, "任意金刚鹦鹉"},
            {AnyCockatiel, "任意玄凤鹦鹉"},
            {AnyDragonfly, "任意蜻蜓"},
            {AnyFruit, "任意水果"},
            {AnyBalloon, "任意气球"},
            {AnyCloudBalloon, "任意云朵气球"},
            {AnyBlizzardBalloon, "任意暴雪气球"},
            {AnySandstormBalloon, "任意沙暴气球"},
            {AnyHorseshoeBalloon, "任意马掌气球"},
            {AnyCrate, "任意宝匣"},
            {AnyTombstone, "任意墓碑"},
            {AnyGoldCritter, "任意金色小动物"},
            {AnyParrot, "任意鹦鹉"},
        };

        public static string GetNameByType(int type, int stack = 1)
        {
            if (mapping.ContainsKey(type))
            {
                string s = mapping[type];

                int icon = GetIconID(type);
                if (icon != 0) s = $"{s}[i/s{stack}:{icon}]";
                if (stack > 1) s = $"{s}x{stack}";

                return s;
            }
            return "";
        }

        /// <summary>
        /// 任意物品对应的物品id，fish ask时会显示对应的物品图标
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static int GetIconID(int type)
        {
            return type switch
            {
                QuestFish => 0,

                AnyQuestFish => 2472,
                AnyFish => 2290,
                AnyWood => 9,
                AnyTorch => 8,
                AnyIronBar => 22,
                AnySand => 169,
                AnyPressurePlate => 529,
                AnyBird => 2015,
                AnyScorpion => 2157,
                AnySquirrel => 2018,
                AnyJungleBug => 3194,
                AnyDuck => 2123,
                AnyButterfly => 1998,
                AnyFirefly => 1992,
                AnySnail => 2006,
                AnyTurtle => 4464,
                AnyMacaw => 5212,
                AnyCockatiel => 5312,
                AnyDragonfly => 4336,
                AnyFruit => 4009,
                AnyBalloon => 3738,
                AnyCloudBalloon => 389,
                AnyBlizzardBalloon => 1163,
                AnySandstormBalloon => 983,
                AnyHorseshoeBalloon => 1250,
                AnyCrate => 2334,
                AnyTombstone => 1176,
                AnyGoldCritter => 2890,
                AnyParrot => 5300,
                _ => 0,
            };
        }


        public static int GetTypeByName(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                return 0;

            if (mapping.ContainsValue(name))
            {
                foreach (var obj in mapping)
                {
                    if (obj.Value == name) return obj.Key;
                }
            }

            // 其它可用的代名词
            return name switch
            {
                "任务鱼" => AnyQuestFish,
                "鱼" => AnyFish,
                "宝匣" => AnyCrate,
                "金色小动物" or "金动物" => AnyGoldCritter,
                "任意龟" => AnyTurtle,
                "任意蛆虫" => AnyJungleBug,
                "任意小鸟" => AnyBird,
                "任意沙尘暴气球" => AnySandstormBalloon,
                "任意云气球" => AnyCloudBalloon,
                _ => 0,
            };
        }


        /// <summary>
        /// 通过物品id 获得鱼店任意类物品id
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns> 交易物品ID </returns>
        public static int GetAnyType(int itemID)
        {
            if (Main.anglerQuestItemNetIDs.Contains(itemID)) return AnyQuestFish; // 任意任务鱼
            if (Fishes.Contains(itemID)) return AnyFish;  // 任意一种鱼
            if (Woods.Contains(itemID)) return AnyWood; // 任意木材
            if (Torches.Contains(itemID)) return AnyTorch; // 任意火把
            if (IronBars.Contains(itemID)) return AnyIronBar;   // 任意铁锭
            if (Sands.Contains(itemID)) return AnySand; // 任意沙块
            if (PressurePlates.Contains(itemID)) return AnyPressurePlate;   // 任意压力板
            if (Birds.Contains(itemID)) return AnyBird; // 任意鸟
            if (Scorpions.Contains(itemID)) return AnyScorpion; // 任意蝎子
            if (Squirrels.Contains(itemID)) return AnySquirrel; // 任意松鼠
            if (JungleBugs.Contains(itemID)) return AnyJungleBug;   // 任意丛林虫子
            if (Ducks.Contains(itemID)) return AnyDuck; // 任意鸭子
            if (Butterflies.Contains(itemID)) return AnyButterfly; // 任意蝴蝶
            if (Fireflies.Contains(itemID)) return AnyFirefly; // 任意萤火虫
            if (Snails.Contains(itemID)) return AnySnail;   // 任意蜗牛
            if (Turtles.Contains(itemID)) return AnyTurtle; // 任意乌龟
            if (Macaws.Contains(itemID)) return AnyMacaw;   // 任意金刚鹦鹉
            if (Cockatiels.Contains(itemID)) return AnyCockatiel;   // 任意玄凤鹦鹉
            if (Dragonflies.Contains(itemID)) return AnyDragonfly;  // 任意蜻蜓
            if (Fruits.Contains(itemID)) return AnyFruit;   // 任意水果
            if (Balloons.Contains(itemID)) return AnyBalloon;   // 任意气球
            if (CloudBalloons.Contains(itemID)) return AnyCloudBalloon; // 任意云朵气球
            if (BlizzardBalloons.Contains(itemID)) return AnyBlizzardBalloon;   // 任意暴雪气球
            if (SandstormBalloons.Contains(itemID)) return AnySandstormBalloon; // 任意沙暴气球
            if (HorseshoeBalloons.Contains(itemID)) return AnyHorseshoeBalloon; // 任意马掌气球

            if (Crates.Contains(itemID)) return AnyCrate;  // 任意宝匣
            if (Tombstones.Contains(itemID)) return AnyTombstone;   //任意墓碑
            if (GoldCritters.Contains(itemID)) return AnyGoldCritter;   //任意金色小动物
            if (Parrots.Contains(itemID)) return AnyParrot; // 任意鹦鹉

            return 0;
        }

    }
}