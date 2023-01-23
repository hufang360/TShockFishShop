using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;


namespace FishShop
{
    /// <summary>
    /// 商品ID
    /// 说明：商品ID、解锁ID、交易物品ID 共用一套id规则，部分id即使解锁ID又是商品ID。
    /// </summary>
    public class ShopItemID
    {
        // 自定义商品id
        // 月相、烟花起飞、更换任务鱼等
        // 召唤boss、召唤npc、召唤敌怪
        // ------------------------------------------------------------------------------------------
        public const int MoonphaseStart = -131;
        public const int MoonphaseNext = -139;      // 下个月相
        public const int Moonphase1 = -131;         // 满月
        public const int Moonphase2 = -132;         // 亏凸月
        public const int Moonphase3 = -133;         // 下弦月
        public const int Moonphase4 = -134;         // 残月
        public const int Moonphase5 = -135;         // 新月
        public const int Moonphase6 = -136;         // 娥眉月
        public const int Moonphase7 = -137;         // 上弦月
        public const int Moonphase8 = -138;         // 盈凸月

        public const int Firework = -140;           // 烟花
        public const int FireworkRocket = -141;     // 烟花起飞
        public const int AnglerQuestSwap = -142;    // 更换任务鱼
        public const int RainingStart = -143;       // 雨来
        public const int RainingStop = -144;        // 雨停
        public const int BuffGoodLucky = -145;      // 好运来
        public const int InvasionStop = -146;       // 跳过入侵
        public const int TimeToDay = -147;          // 调白天
        public const int TimeToNight = -148;        // 调晚上
        public const int BloodMoonStart = -149;     // 召唤血月
        public const int BloodMoonStop = -150;      // 跳过血月

        public const int Buff = -159;               // 增益
        public const int RawCmd = -160;             // 指令

        public const int ReliveNPC = -161;          // 复活NPC
        public const int TPHereAll = -162;          // 集合打团
        public const int CelebrateAll = -163;       // 集体庆祝
        public const int TimeToNoon = -164;         // 调中午
        public const int TimeToMidNight = -165;     // 调午夜

        // ------------------------------------------------------------------------------------------
        // buff
        public const int BuffWhipPlayer = -166;     // 打神鞭
        public const int BuffFaster = -167;         // 逮虾户
        public const int BuffMining = -168;         // 黄金矿工
        public const int BuffFishing = -169;        // 钓鱼佬
        public const int BuffIncitant = -170;       // 兴奋剂

        // ------------------------------------------------------------------------------------------
        // 召唤入侵

        public const int InvasionGoblins = -171;        // 召唤 哥布林军队
        public const int InvasionSnowmen = -172;        // 召唤 雪人军团
        public const int InvasionPirates = -173;        // 召唤 海盗入侵
        public const int InvasionPumpkinmoon = -174;    // 召唤 南瓜月
        public const int InvasionFrostmoon = -175;      // 召唤 霜月
        public const int InvasionMartians = -176;       // 召唤 火星暴乱


        // ------------------------------------------------------------------------------------------
        // 事件
        public const int ToggleRain = -177;             // 雨
        public const int ToggleSlimeRain = -178;        // 史莱姆雨
        public const int ToggleSandStorm = -179;        // 沙尘暴
        public const int ToggleWindyDay = -180;         // 大风天
        public const int ToggleStorming = -181;         // 暴风雨

        public const int ToggleBloodMoon = -182;        // 血月
        public const int ToggleEclipse = -183;          // 日食

        public const int ToggleParty = -184;            // 派对
        public const int TriggerDropMeteor = -185;      // 陨石
        public const int StarfallStart = -186;          // 流星雨
        public const int LanternsNightStart = -187;     // 灯笼夜

        public const int OverworldDayStart = -188;      // 风和日丽

        public const int DirtiestBlock = -189;          // 臭臭仪式
        public const int OneDamage = -190;              // 灵犀一指


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
            //if (id == RawCmd)
            //{
            //    if (!string.IsNullOrEmpty(prefix)) return $"指令{prefix}";
            //}

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

                string buffName = BuffHelper.GetBuffNameByID(buffID);
                if (!string.IsNullOrEmpty(buffName))
                    return $"{buffName}{BuffHelper.GetTimeDesc(stack)}";
            }

            return "";
        }


        public static int GetIDByName(string name = "")
        {
            if (string.IsNullOrEmpty(name)) return 0;

            int id = Settings.GetShopItemIDByName(name);
            if (id != 0) return id;

            switch (name)
            {
                case "指令": return RawCmd;
                case "增益": return Buff;

                // 原生物品
                case "铜": case "铜币": return 71;
                case "银": case "银币": return 72;
                case "金": case "金币": return 73;
                case "铂": case "铂金": case "铂金币": return 74;

                case "第一分形": return 4722;
                case "无趣弓": return 3853;
            }

            //if (name.StartsWith("指令")) return RawCmd;

            // public static int[] anglerQuestItemNetIDs = new int[41]
            // {
            //     2450, 2451, 2452, 2453, 2454, 2455, 2456, 2457, 2458, 2459,
            //     2460, 2461, 2462, 2463, 2464, 2465, 2466, 2467, 2468, 2469,
            //     2470, 2471, 2472, 2473, 2474, 2475, 2476, 2477, 2478, 2479,
            //     2480, 2481, 2482, 2483, 2484, 2485, 2486, 2487, 2488, 4393,
            //     4394
            // };
            // 自定义物品 以及 物品id mapping
            id = Settings.GetItemIDByName(name);
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
            id = BuffHelper.GetBuffIDByName(name);
            if (id != 0) return id;

            return 0;
        }

        private static int GetItemIDByName(string name)
        {
            List<Item> items = TShock.Utils.GetItemByName(name);
            if (items.Count > 0) return items[0].netID;

            return 0;
        }

        // 检查是否需要购买
        public static bool CanBuy(TSPlayer op, ShopItem shopItem, int amount = 1)
        {
            int id = shopItem.id;
            if (id >= -24) return true;
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
                case MoonphaseNext: return FishHelper.NeedBuyChangeMoonPhase(op, id, amount);


                case InvasionGoblins:
                case InvasionSnowmen:
                case InvasionPirates:
                case InvasionPumpkinmoon:
                case InvasionFrostmoon:
                case InvasionMartians: return CmdHelper.NeedBuyStartInvasion(op);

                case InvasionStop: return CmdHelper.NeedBuyStopInvasion(op);
                case ReliveNPC: return NPCHelper.NeedBuyReliveNPC(op);

                case BloodMoonStart: if (Main.bloodMoon) { op.SendInfoMessage("正处在血月，无需购买"); return false; } break;
                case BloodMoonStop: if (!Main.bloodMoon) { op.SendInfoMessage("没发生血月，无需购买"); return false; } break;

                case RainingStart: if (Main.raining) { op.SendInfoMessage("正在下雨，无需购买"); return false; }; break;
                case RainingStop: if (!Main.raining) { op.SendInfoMessage("没在下雨，无需购买"); return false; }; break;
            }

            return true;
        }


        public static void ProvideGoods(TSPlayer player, ShopItem shopItem, int amount = 1)
        {
            int id = shopItem.id;
            // 自定义物品
            switch (id)
            {
                // 月相
                case Moonphase1:
                case Moonphase2:
                case Moonphase3:
                case Moonphase4:
                case Moonphase5:
                case Moonphase6:
                case Moonphase7:
                case Moonphase8:
                case MoonphaseNext: FishHelper.ChangeMoonPhaseByID(player, id, amount); return;

                case Firework: CmdHelper.Firework(player); return;
                case FireworkRocket: CmdHelper.FireworkRocket(player); return;
                case AnglerQuestSwap: FishHelper.AnglerQuestSwap(player); return;

                // 白天 中午 晚上 午夜
                case TimeToDay: CmdHelper.SwitchTime(player, "day"); return;
                case TimeToNoon: CmdHelper.SwitchTime(player, "noon"); return;
                case TimeToNight: CmdHelper.SwitchTime(player, "night"); return;
                case TimeToMidNight: CmdHelper.SwitchTime(player, "midnight"); return;

                // =====
                // buff 类
                // ====
                // 好运来、打神鞭、逮虾户、黄金矿工、钓鱼佬、兴奋剂
                case BuffGoodLucky:
                case BuffWhipPlayer:
                case BuffFaster:
                case BuffMining:
                case BuffFishing:
                case BuffIncitant:
                case Buff:
                    BuffHelper.BuffCommon(player, shopItem.GetBuff(), shopItem.GetBuffSecond(), amount);
                    return;


                // 雨来、雨停
                case RainingStart: CmdHelper.ToggleRaining(player, true); return;
                case RainingStop: CmdHelper.ToggleRaining(player, false); return;

                // 血月
                case BloodMoonStart: CmdHelper.ToggleBloodMoon(player, true); return;
                case BloodMoonStop: CmdHelper.ToggleBloodMoon(player, false); return;


                // 雨、史莱姆雨、 沙尘暴、大风天、雷雨、血月、日食、派对、陨石、流星雨、灯笼夜
                case ToggleRain: CmdHelper.ToggleRaining(player, true, true); return;
                case ToggleSlimeRain: CmdHelper.ToggleSlimeRain(player, true, true); return;
                case ToggleSandStorm: CmdHelper.ToggleSandstorm(player, true, true); return;
                case ToggleWindyDay: CmdHelper.ToggleWindyDay(player, true, true); return;
                case ToggleStorming: CmdHelper.ToggleStorming(player, true, true); return;


                case ToggleBloodMoon: CmdHelper.ToggleBloodMoon(player, true, true); return;
                case ToggleEclipse: CmdHelper.ToggleEclipse(player, true, true); return;


                case ToggleParty: CmdHelper.ToggleParty(player, true, true); return;
                case TriggerDropMeteor: CmdHelper.DropMeteor(player); return;
                case StarfallStart: CmdHelper.Starfall(player); return;
                case LanternsNightStart: CmdHelper.LanternsNightStart(player); return;

                case OverworldDayStart: CmdHelper.OverworldDay(player); return;


                // 入侵
                case InvasionStop: CmdHelper.StopInvasion(player); return;
                case InvasionGoblins:
                case InvasionSnowmen:
                case InvasionPirates:
                case InvasionPumpkinmoon:
                case InvasionFrostmoon:
                case InvasionMartians: CmdHelper.StartInvasion(player, id); return;

                // 执行指令
                case RawCmd:
                    if (shopItem.cmds.Count > 0)
                    {
                        foreach (string cmd in shopItem.cmds)
                        {
                            CmdHelper.ExecuteRawCmd(player, cmd);
                        }
                    }
                    else if (!string.IsNullOrEmpty(shopItem.prefix)) CmdHelper.ExecuteRawCmd(player, shopItem.prefix);
                    return;

                case ReliveNPC: NPCHelper.ReliveNPC(player); return;                    // 复活NPC
                case TPHereAll: CmdHelper.TPHereAll(player); return;                    // 集合打团
                case CelebrateAll: CmdHelper.CelebrateAll(player); return;              // 集体庆祝

                // 最脏的块
                case DirtiestBlock:
                    bool flag = shopItem.FindDirtest();
                    shopItem.MoveDirtest(flag);
                    TSPlayer.All.SendInfoMessage($"{player.Name} 正在举行 [i:5395]臭臭仪式[i:5395]");
                    if (flag)
                        player.SendSuccessMessage("臭臭仪式完成，[i:5400]最脏的块 已生成(σﾟ∀ﾟ)σ");
                    else
                        player.SendErrorMessage("糟糕，找遍整个世界都没有发现 [i:5400]最脏的块 o(´^｀)o");
                    return;

                // 灵犀飞鱼
                case OneDamage:
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc != null && npc.active && npc.boss)
                        {
                            NPCHelper.AttackBoss(player, npc);
                            TSPlayer.All.SendInfoMessage($"{player.Name} 购买了{shopItem.GetItemDesc()}，对boss造成了1点点伤害！");
                        }
                    }
                    return;

                default: break;
            }

            // 召唤NPC类
            int id2 = GetRealSpawnID(id);
            if (id2 != 0)
            {
                NPCHelper.SpawnNPC(player, id2, amount);
                return;
            }

            // 清除NPC类
            id2 = GetRealClearNPCID(id);
            if (id2 != 0)
            {
                NPCHelper.ClearNPC(player, id2, amount);
                return;
            }

            // 获得buff类
            id2 = GetRealBuffID(id);
            if (id2 != 0)
            {
                BuffHelper.SetPlayerBuff(player, id2, shopItem.stack * amount);
                return;
            }

            // 玩家自定义商品
            // 指令清单 和 buff 清单
            if (id <= -600 && id >= -999)
            {
                foreach (string s in shopItem.GetCMD().Where(s => !string.IsNullOrEmpty(s)))
                {
                    CmdHelper.ExecuteRawCmd(player, s);
                }

                BuffHelper.BuffCommon(player, shopItem.GetBuff(), shopItem.GetBuffSecond(), amount);
            }
        }

    }
}