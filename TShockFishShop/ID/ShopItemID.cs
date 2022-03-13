using System.Collections.Generic;
using TShockAPI;
using Terraria;


namespace Plugin
{
    public class ShopItemID
    {
        //  自定义商品id
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
        //  打神鞭
        public const int BuffWhipPlayer = -166;

        //  逮虾户
        public const int BuffFaster = -167;

        //  黄金矿工
        public const int BuffMining = -168;
        // 钓鱼佬
        public const int BuffFishing = -169;
        // 兴奋剂
        public const int BuffIncitant = -170;


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


        private static int GetSpawnID(int id)    {return -(1000+id);}
        public static int GetRealSpawnID(int id)    {return id>SpawnEnd && id<SpawnStart ? SpawnStart-id : 0;}

        private static int GetClearNPCID(int id)    {return -(4000+id);}
        public static int GetRealClearNPCID(int id)    {return id>ClearNPCEnd && id<ClearNPCStart ? ClearNPCStart-id : 0;}

        private static int GetSetBuffID(int id)    {return -(5000+id);}
        public static int GetRealBuffID(int id)    {return id>SetBuffEnd && id<SetBuffStart ? SetBuffStart-id : 0;}



        public static string GetNameByID(int id, string prefix="", int stack=1)
        {
            switch( id )
            {
                case MoonphaseNext: return "下个月相";
                case Moonphase1: return "满月";
                case Moonphase2: return "亏凸月";
                case Moonphase3: return "下弦月";
                case Moonphase4: return "残月";
                case Moonphase5: return "新月";
                case Moonphase6: return "娥眉月";
                case Moonphase7: return "上弦月";
                case Moonphase8: return "盈凸月";

                case FireworkRocket: return "烟花起飞";
                case Firework: return "烟花";
                case AnglerQuestSwap: return "更换任务鱼";

                case RainingStart: return "雨来";
                case RainingStop: return "雨停";
                case InvasionStop: return "跳过入侵";
                case TimeToDay: return "调白天";
                case TimeToNight: return "调晚上";
                case TimeToNoon: return "调中午";
                case TimeToMidNight: return "调午夜";
                case BloodMoonStart: return "召唤血月";
                case BloodMoonStop: return "跳过血月";
                case RawCmd: return $"指令{prefix}";
                case ReliveNPC: return "复活NPC";
                case TPHereAll: return "集合打团";
                case CelebrateAll: return "集体庆祝";
                case BuffGoodLucky: return "好运来";
                case BuffWhipPlayer: return "打神鞭";
                case BuffFaster: return "逮虾户";
                case BuffMining: return "黄金矿工";
                case BuffFishing: return "钓鱼佬";
                case BuffIncitant: return "兴奋剂";
            }

            int npcID = 0;
            string npcName = "";

            // 召唤NPC
            if( id>=SpawnEnd && id<= SpawnStart ){
                npcID = SpawnStart-id;
                npcName = NPCHelper.GetNameByID(npcID);
                if( !string.IsNullOrEmpty(npcName) )
                    return $"召唤{npcName}";
            }

            // 清除NPC
            if( id>=ClearNPCEnd && id<= ClearNPCStart ){
                npcID = ClearNPCStart-id;
                npcName = NPCHelper.GetNameByID(npcID);
                if( !string.IsNullOrEmpty(npcName) )
                    return $"清除{npcName}";
            }

            // 获得buff
            if( id>=SetBuffEnd && id<= SetBuffStart ){
                int buffID = SetBuffStart-id;
                string buffName = TShock.Utils.GetBuffName(buffID);
                if( !string.IsNullOrEmpty(buffName) ){
                    return $"{buffName}{BuffHelper.GetTimeDesc(stack)}";
                }
            }

            return "";
        }


        public static int GetIDByName(string name="")
        {
            if( string.IsNullOrEmpty(name) )
                return 0;

            switch (name)
            {
                // 自定义商品
                case "满月": return Moonphase1;
                case "亏凸月": return Moonphase2;
                case "下弦月": return Moonphase3;
                case "残月": return Moonphase4;
                case "新月": return Moonphase5;
                case "娥眉月": return Moonphase6;
                case "上弦月": return Moonphase7;
                case "盈凸月": return Moonphase8;
                case "下个月相": return MoonphaseNext;

                case "烟花": return Firework;
                case "烟花起飞": return FireworkRocket;
                case "更换任务鱼": case "更换钓鱼任务":  case "切换任务鱼": case "切换钓鱼任务": case "换鱼": return AnglerQuestSwap;
                case "雨来": case "下雨": return RainingStart;
                case "雨停": return RainingStop;
                case "跳过入侵": case "停止入侵": return InvasionStop;
                case "调白天": return TimeToDay;
                case "调晚上": return TimeToNight;
                case "调中午": return TimeToNoon;
                case "调午夜": return TimeToMidNight;

                case "好运来": return BuffGoodLucky;
                case "打神鞭": return BuffWhipPlayer;
                case "逮虾户": return BuffFaster;
                case "黄金矿工": return BuffMining;
                case "钓鱼佬": return BuffFishing;
                case "兴奋剂": return BuffIncitant;
                
                case "召唤血月": return BloodMoonStart;
                case "跳过血月": return BloodMoonStop;

                
                case "指令": return RawCmd;
                case "复活npc": return ReliveNPC ;
                case "集合打团": return TPHereAll;
                case "集体庆祝": return CelebrateAll;


                // 原生物品
                case "铜": case "铜币": return 71;
                case "银": case "银币": return 72;
                case "金": case "金币": return 73;
                case "铂": case "铂金": case "铂金币": return 74;

                case "第一分形": return 4722;
                case "无趣弓": return 3853;

                // 任务鱼
                case "向导鱼": return 2472;
                case "毒菌鱼": case "蘑菇鱼": return 2475;
                case "血腥战神": case "猩红鱼": case "血腥鱼": return 2477;
                case "浮游噬鱼": case "腐地鱼": case "腐化鱼": return 2457;
                case "染病鞘鱼": case "染病鱼": return 2485;
                case "鲶鱼": case "猫猫鱼": return 2452;
                case "恶魔地狱鱼": case "地狱鱼": case "恶魔鱼": return 2482;
                case "坠落海星": case "海星鱼": case "落星鱼": return 2458;
                case "克苏鲁鱼": case "克眼鱼": return 2459; case "饥饿鱼": case "肉山鱼": case "血肉墙鱼": return 2462;
                case "幻象鱼": case "混沌鱼": return 2465;
                case "突变雪怪鱼": case "雪怪鱼": case "小雪怪鱼": return 2466;
                case "热带梭鱼": case "热带鱼": case "梭鱼": return 2488;
                case "苔原鳟鱼": case "鳟鱼": case "苔原鱼": return 2470;
                case "飞龙尾": case "飞龙鱼": case "小白龙鱼": return 2473;
            }
            
            // public static int[] anglerQuestItemNetIDs = new int[41]
            // {
            //     2450, 2451, 2452, 2453, 2454, 2455, 2456, 2457, 2458, 2459,
            //     2460, 2461, 2462, 2463, 2464, 2465, 2466, 2467, 2468, 2469,
            //     2470, 2471, 2472, 2473, 2474, 2475, 2476, 2477, 2478, 2479,
            //     2480, 2481, 2482, 2483, 2484, 2485, 2486, 2487, 2488, 4393,
            //     4394
            // };

            // 召唤boss
            if( name.Contains("召唤") ){
                string s = name.Replace(" ", "").Replace("召唤","").ToLowerInvariant();
                int npcID = NPCHelper.GetIDByName(s);
                if( npcID!=0 )
                    return GetSpawnID(npcID);
            }

            // 清除npc
            if( name.Contains("清除") ){
                string s = name.Replace(" ", "").Replace("清除","").ToLowerInvariant();
                int npcID = NPCHelper.GetIDByName(s);
                if( npcID!=0 )
                    return GetClearNPCID(npcID);
            }

            // 尝试使用物品名匹配
            int id = GetItemIDByName(name);
            if( id!=0 )
                return id;

            // 使用buff名匹配
            id = GetBuffIDByName(name);
            if( id!=0 )
                return id;

            return 0;
        }

        private static int GetItemIDByName(string name)
        {
            List<Item> items = TShock.Utils.GetItemByName(name);
            if( items.Count>0 )
                return items[0].netID;

            return 0;
        }

        private static int GetBuffIDByName(string name)
        {
            List<int> ids = TShock.Utils.GetBuffByName(name);
            if( ids.Count>0 )
                return ids[0];

            return 0;
        }

        public static bool CanBuyManyItem(int id)
        {
            switch (id)
            {
                case Moonphase1:
                case Moonphase2:
                case Moonphase3:
                case Moonphase4:
                case Moonphase5:
                case Moonphase6:
                case Moonphase7:
                case Moonphase8:
                case AnglerQuestSwap:
                case InvasionStop:
                case Firework:
                case FireworkRocket:
                case RainingStart:
                case RainingStop:
                case BloodMoonStart:
                case BloodMoonStop:
                case TimeToDay:
                case TimeToNight:
                case TimeToNoon:
                case RawCmd:
                case ReliveNPC:
                case TPHereAll:
                case CelebrateAll:
                    return false;
            }

            // 清除npc类
            if( GetRealClearNPCID(id) != 0 )
                return false;

            return true;
        }

        public static bool DeadCanBuyItem(int id)
        {
            switch (id)
            {
                case BuffGoodLucky:
                case BuffWhipPlayer:
                case BuffFaster:
                case BuffMining:
                case BuffFishing:
                case BuffIncitant:
                case FireworkRocket:
                case Firework:
                    return false;
            }

            // buff 类
            if( GetRealBuffID (id) != 0 )
                return false;

            return true;
        }

        // 商品说明
        public static string GetComment(int id)
        {
            switch (id)
            {
                case BuffGoodLucky: return "获得10分钟 好运气 buff";
                case BuffWhipPlayer: return "获得10分钟 丛林之怒、迪朗达尔的祝福、收割时刻 buff（近战攻速+105%）";
                case BuffFaster: return "获得10分钟 快乐、敏捷、酒足饭饱、糖果冲刺、恐慌 buff";
                case BuffMining: return "获得10分钟 挖矿、建筑工、光芒、夜猫子、酒足饭饱、洞穴探险、危险感知、猎人、糖果冲刺 buff";
                case BuffFishing: return "获得10分钟 钓鱼、声呐、宝匣、夜猫子";
                case BuffIncitant: return "获得10分钟 酒足饭饱、再生药水、敏捷、铁皮、怒气、生命力、暴怒、耐力";

                case TPHereAll: return "将所有玩家传到你身边";
                case AnglerQuestSwap: return "更换今天的任务鱼，并重置任务完成情况";
                
                default: return "";
            }
        }
    }
}