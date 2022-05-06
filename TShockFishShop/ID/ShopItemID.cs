using System.Collections.Generic;
using Terraria;
using TShockAPI;


namespace FishShop
{
    public class ShopItemID
    {
        // 自定义商品id
        // 月相、烟花起飞、更换任务鱼等
        // 召唤boss、召唤npc、召唤敌怪
        // ------------------------------------------------------------------------------------------
        // 月相
        public const int MoonphaseStart = -131;
        // 满月
        public const int Moonphase1 = -131;
        // 亏凸月
        public const int Moonphase2 = -132;
        //下弦月
        public const int Moonphase3 = -133;
        // 残月
        public const int Moonphase4 = -134;
        // 新月
        public const int Moonphase5 = -135;
        // 娥眉月
        public const int Moonphase6 = -136;
        // 上弦月
        public const int Moonphase7 = -137;
        // 盈凸月
        public const int Moonphase8 = -138;
        // 下个月相
        public const int MoonphaseNext = -139;

        // 烟花起飞
        public const int Firework = -140;
        public const int FireworkRocket = -141;
        // 更换任务鱼
        public const int AnglerQuestSwap = -142;


        // 雨来
        public const int RainingStart = -143;
        // 雨停
        public const int RainingStop = -144;

        // 好运来
        public const int BuffGoodLucky = -145;


        // 跳过入侵
        public const int InvasionStop = -146;

        // 调白天
        public const int TimeToDay = -147;

        // 调晚上
        public const int TimeToNight = -148;

        // 召唤血月
        public const int BloodMoonStart = -149;
        // 跳过血月
        public const int BloodMoonStop = -150;

        // 执行指令
        public const int RawCmd = -160;

        // 复活NPC
        public const int ReliveNPC = -161;

        // 集合打团
        public const int TPHereAll = -162;

        // 集体庆祝
        public const int CelebrateAll = -163;

        // 调中午
        public const int TimeToNoon = -164;

        // 调午夜
        public const int TimeToMidNight = -165;

        // ------------------------------------------------------------------------------------------
        // buff
        // 打神鞭
        public const int BuffWhipPlayer = -166;

        // 逮虾户
        public const int BuffFaster = -167;

        // 黄金矿工
        public const int BuffMining = -168;
        // 钓鱼佬
        public const int BuffFishing = -169;
        // 兴奋剂
        public const int BuffIncitant = -170;


        // ------------------------------------------------------------------------------------------
        // 召唤入侵
        // 召唤 哥布林军队
        public const int InvasionGoblins = -171;

        // 召唤 雪人军团
        public const int InvasionSnowmen = -172;

        // 召唤 海盗入侵
        public const int InvasionPirates = -173;

        // 召唤 南瓜月
        public const int InvasionPumpkinmoon = -174;

        // 召唤 霜月
        public const int InvasionFrostmoon = -175;

        // 召唤 火星暴乱
        public const int InvasionMartians = -176;

        // 召唤 沙尘暴、史莱姆雨、派对、日食、陨石、大风天、撒旦军队、月亮事件、派对



        // ------------------------------------------------------------------------------------------
        // 召唤NPC
        // -1000-[npcID]
        private const int SpawnStart = -1000;
        private const int SpawnEnd = -1999;

        // ------------------------------------------------------------------------------------------
        // 清除NPC
        // -4000-[npcID]
        private const int ClearNPCStart = -4000;
        private const int ClearNPCEnd = -4999;

        // ------------------------------------------------------------------------------------------
        // 获得buff
        // -5000-[buffID]
        private const int SetBuffStart = -5000;
        private const int SetBuffEnd = -5999;


        private static int GetSpawnID(int id) { return -(1000 + id); }
        public static int GetRealSpawnID(int id) { return id > SpawnEnd && id < SpawnStart ? SpawnStart - id : 0; }

        private static int GetClearNPCID(int id) { return -(4000 + id); }
        public static int GetRealClearNPCID(int id) { return id > ClearNPCEnd && id < ClearNPCStart ? ClearNPCStart - id : 0; }

        private static int GetSetBuffID(int id) { return -(5000 + id); }
        public static int GetRealBuffID(int id) { return id > SetBuffEnd && id < SetBuffStart ? SetBuffStart - id : 0; }



        public static string GetNameByID(int id, string prefix = "", int stack = 1)
        {
            if (id == RawCmd) return $"指令{prefix}";

            string text = Settings.GetShopItemNameByID(id);
            if (!string.IsNullOrEmpty(text)) return text;

            int npcID;
            string npcName;
            // 召唤NPC
            if (id >= SpawnEnd && id <= SpawnStart)
            {
                npcID = SpawnStart - id;
                npcName = NPCHelper.GetNameByID(npcID);
                if (!string.IsNullOrEmpty(npcName)) return $"召唤{npcName}";
            }

            // 清除NPC
            if (id >= ClearNPCEnd && id <= ClearNPCStart)
            {
                npcID = ClearNPCStart - id;
                npcName = NPCHelper.GetNameByID(npcID);
                if (!string.IsNullOrEmpty(npcName)) return $"清除{npcName}";
            }

            // 获得buff
            if (id >= SetBuffEnd && id <= SetBuffStart)
            {
                int buffID = SetBuffStart - id;
                string buffName = TShock.Utils.GetBuffName(buffID);
                if (!string.IsNullOrEmpty(buffName))
                {
                    return $"{buffName}{BuffHelper.GetTimeDesc(stack)}";
                }
            }

            return "";
        }


        public static int GetIDByName(string name = "")
        {
            if (string.IsNullOrEmpty(name)) return 0;

            switch (name)
            {
                case "指令": return RawCmd;

                // 原生物品
                case "铜": case "铜币": return 71;
                case "银": case "银币": return 72;
                case "金": case "金币": return 73;
                case "铂": case "铂金": case "铂金币": return 74;

                case "第一分形": return 4722;
                case "无趣弓": return 3853;
            }

            if (name.StartsWith("指令")) return RawCmd;

            // public static int[] anglerQuestItemNetIDs = new int[41]
            // {
            //     2450, 2451, 2452, 2453, 2454, 2455, 2456, 2457, 2458, 2459,
            //     2460, 2461, 2462, 2463, 2464, 2465, 2466, 2467, 2468, 2469,
            //     2470, 2471, 2472, 2473, 2474, 2475, 2476, 2477, 2478, 2479,
            //     2480, 2481, 2482, 2483, 2484, 2485, 2486, 2487, 2488, 4393,
            //     4394
            // };
            // 自定义物品 以及 物品id mapping
            int id = Settings.GetItemIDByName(name);
            if (id != 0) return id;


            // 召唤boss
            if (name.Contains("召唤"))
            {
                string s = name.Replace(" ", "").Replace("召唤", "").ToLowerInvariant();
                int npcID = NPCHelper.GetIDByName(s);
                if (npcID != 0) return GetSpawnID(npcID);
            }

            // 清除npc
            if (name.Contains("清除"))
            {
                string s = name.Replace(" ", "").Replace("清除", "").ToLowerInvariant();
                int npcID = NPCHelper.GetIDByName(s);
                if (npcID != 0) return GetClearNPCID(npcID);
            }

            // 尝试使用物品名匹配
            id = GetItemIDByName(name);
            if (id != 0) return id;

            // 使用buff名匹配
            id = GetBuffIDByName(name);
            if (id != 0) return id;

            return 0;
        }

        private static int GetItemIDByName(string name)
        {
            List<Item> items = TShock.Utils.GetItemByName(name);
            if (items.Count > 0) return items[0].netID;

            return 0;
        }

        private static int GetBuffIDByName(string name)
        {
            List<int> ids = TShock.Utils.GetBuffByName(name);
            if (ids.Count > 0) return ids[0];

            return 0;
        }

    }
}