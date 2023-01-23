using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Events;
using TShockAPI;


namespace FishShop
{
    /// <summary>
    /// 解锁ID
    /// 说明：商品ID、解锁ID、交易物品ID 共用一套id规则，部分id即使解锁ID又是商品ID。
    /// </summary>
    public class UnlockID
    {
        public const int FishQuestCompleted = -100;     // 需要完成多少次渔夫任务
        public const int BloodMoon = -104;              // 血月
        public const int Raining = -105;                // 雨天
        public const int HpUnder400 = -106;             // 生命值<400

        public const int Day = -107;                    // 白天
        public const int Night = -108;                  // 晚上
        public const int Xmas = -109;                   // 圣诞节
        public const int Halloween = -110;              // 万圣节
        public const int Party = -111;                  // 派对
        public const int Lantern = -112;                // 灯笼夜
        public const int NeedSandstorm = -113;          // 沙尘暴
        public const int WindyDay = -114;               // 大风天
        public const int Thunderstorm = -115;           // 雷雨
        public const int starfallBoost = -116;          // 流星雨
        public const int SlimeRain = -117;              // 史莱姆雨
        public const int Eclipse = -118;                // 日食

        public const int GoblinInvasion = -119;         // 哥布林军队
        public const int ForstMoon = -120;              // 霜月
        public const int PirateInvasion = -121;         // 海盗入侵
        public const int Martian = -122;                // 火星暴乱

        public const int PumpkinMoon = -123;            // 南瓜月
        public const int SnowMoon = -124;               // 雪人军团
        public const int NeedDD2Event = -125;           // 撒旦军队
        public const int LunarEvent = -126;             // 月亮事件


        public const int ZoneForest = -300;             // 森林
        public const int ZoneJungle = -301;             // 丛林
        public const int ZoneDesert = -302;             // 沙漠
        public const int ZoneSnow = -303;               // 雪原
        public const int ZoneUnderworld = -304;         // 洞穴
        public const int ZoneBeach = -305;              // 海洋
        public const int ZoneHallow = -306;             // 神圣
        public const int ZoneGlowshroom = -307;         // 蘑菇
        public const int ZoneCorrupt = -308;            // 腐化之地
        public const int ZoneCrimson = -309;            // 猩红之地
        public const int ZoneDungeon = -310;            // 地牢
        public const int ZoneGraveyard = -311;          // 墓地
        public const int ZoneHive = -312;               // 蜂巢
        public const int ZoneLihzhardTemple = -313;     // 神庙
        public const int ZoneSandstorm = -314;          // 沙尘暴
        public const int ZoneSky = -315;                // 天空
        public const int ZonePond = -316;               // 池塘旁（附近有300个液体）


        // -101、-102、-103 为交易物品ID
        // -131 ~ -139 为月相，和商品ID共享

        // ------------------------------------------------------------------------------------------
        // 获得buff
        // 获得buff = -5000-[buffID]
        // public const int BuffStart = -5000;
        // public const int BuffEnd = -5999;


        #region 击败boss，npc在场
        // 需要 击败指定boss
        // -2000-[npcID]
        // 击败任意 机械boss = -2901
        private const int DownedStart = -2000;
        private const int DownedEnd = -2999;

        // 已知的需要 击败指定boss
        public const int downedAnyMech = -2901;     // 一王后
        public const int downedAllMech = -2902;     // 四柱后
        public const int downedAnyTower = -2903;    // 一柱后
        public const int downedAllTower = -2904;    // 四柱后
        public const int downedGoblins = -2905;     // 哥布林入侵
        public const int downedPirates = -2906;     // 海盗入侵
        public const int downedFrost = -2907;       // 霜月

        public const int downedSlimeKing = -2050;   // 史莱姆王
        public const int downedBoss1 = -2004;       // 克苏鲁之眼
        public const int downedDeerclops = -2668;   // 鹿角怪

        public const int downedBoss2 = -2013;       // 世界吞噬者 13  克苏鲁之脑 266
        public const int downedQueenBee = -2222;    // 蜂王
        public const int downedBoss3 = -2035;       // 骷髅王

        public const int downedWallofFlesh = -2113;     // 血肉墙
        public const int downedMechBoss1 = -2134;       // 毁灭者
        public const int downedMechBoss2 = -2125;       // 双子魔眼
        public const int downedMechBoss3 = -2127;       // 机械骷髅王
        public const int downedPlantBoss = -2262;       // 世纪之花
        public const int downedGolemBoss = -2245;       // 石巨人
        public const int downedQueenSlime = -2657;      // 史莱姆皇后
        public const int downedEmpressOfLight = -2636;  // 光之女皇
        public const int downedFishron = -2370;         // 猪龙鱼公爵
        public const int downedAncientCultist = -2439;  // 拜月教邪教徒
        public const int downedMoonlord = -2396;        // 月亮领主
        public const int downedHalloweenTree = -2325;   // 哀木
        public const int downedHalloweenKing = -2327;   // 南瓜王
        public const int downedChristmasTree = -2344;   // 常绿尖叫怪
        public const int downedChristmasIceQueen = -2345;   // 冰雪女王
        public const int downedChristmasSantank = -2346;    // 圣诞坦克
        public const int downedMartians = -2392;            // 火星飞碟
        public const int downedClown = -2109;               // 小丑

        public const int downedTowerSolar = -2517;      // 日耀柱
        public const int downedTowerVortex = -2422;     // 星旋柱
        public const int downedTowerNebula = -2507;     // 星云柱
        public const int downedTowerStardust = -2493;   // 星旋柱

        // 需要 NPC在场
        // NPC活着 = -3000-[npcID]
        private const int PresentStart = -3000;
        private const int PresentEnd = -3999;


        /// <summary>
        /// 获得 击败xxNPC 类解锁条件 id
        /// </summary>
        /// <param name="npcID">仅boss的npcID</param>
        /// <returns></returns>
        private static int GetDownedID(int npcID) { return -(2000 + npcID); }
        public static int GetRealDownedID(int id) { return id > DownedEnd && id < DownedStart ? DownedStart - id : 0; }

        /// <summary>
        /// 获得 xxNPC在场 类解锁条件 id
        /// </summary>
        /// <param name="npcID">任意npcID</param>
        /// <returns></returns>
        private static int GetPresentID(int npcID) { return -(3000 + npcID); }
        public static int GetRealPresentID(int id) { return id > PresentEnd && id < PresentStart ? PresentStart - id : 0; }

        #endregion


        /// <summary>
        /// id 和 名称 对照
        /// </summary>
        static readonly Dictionary<int, string> mapping = new()
        {
            {FishQuestCompleted, "完成{0}次渔夫任务"},
            {BloodMoon, "血月"},
            {Raining, "雨天"},
            {HpUnder400, "生命<400"},

            {downedAnyMech, "一王后" },
            {downedAllMech, "三王后"},
            {downedAnyTower, "一柱后"},
            {downedAllTower, "四柱后"},
            {downedBoss3, "骷髅王后"},

            {downedWallofFlesh, "肉后"},
            {downedPlantBoss, "花后"},
            {downedGolemBoss, "石后"},
            {downedMoonlord, "月后"},

            {ZoneForest, "森林" },
            {ZoneJungle, "丛林"},
            {ZoneDesert, "沙漠"},
            {ZoneSnow, "雪原"},
            {ZoneUnderworld, "洞穴"},
            {ZoneBeach, "海洋"},
            {ZoneHallow, "神圣"},
            {ZoneGlowshroom, "蘑菇"},
            {ZoneCorrupt, "腐化"},
            {ZoneCrimson, "猩红"},
            {ZoneDungeon, "地牢"},
            {ZoneGraveyard, "墓地"},
            {ZoneHive, "蜂巢"},
            {ZoneLihzhardTemple, "神庙"},
            {ZoneSandstorm, "沙尘暴"},
            {ZoneSky, "天空"},
            {ZonePond, "池塘"},


            {Day, "白天" },
            {Night, "晚上"},
            {Xmas, "圣诞节"},
            {Halloween, "万圣节"},
            {Party, "派对"},
            {Lantern, "灯笼夜"},
            {NeedSandstorm, "沙尘暴"},
            {WindyDay, "大风天"},
            {Thunderstorm, "雷雨"},
            {starfallBoost, "流星雨"},
            {SlimeRain, "史莱姆雨"},
            {Eclipse, "日食"},

            {GoblinInvasion, "哥布林军队"},
            {ForstMoon, "霜月"},
            {PirateInvasion, "海盗入侵"},
            {Martian, "火星暴乱"},

            {PumpkinMoon, "南瓜月"},
            {SnowMoon, "雪人军团"},
            {NeedDD2Event, "撒旦军队"},
            {LunarEvent, "月亮事件"},
    };



        public static string GetNameByType(int type, int stack = 1)
        {
            if (mapping.ContainsKey(type))
            {
                if (stack > 1)
                {
                    if (type == FishQuestCompleted)
                        return mapping[type].SFormat(stack);
                    else
                        return $"{mapping[type]}x{stack}";
                }
                else
                    return mapping[type];
            }

            string npcName;
            int npcID = GetRealDownedID(type);
            if (npcID != 0)
            {
                npcName = NPCHelper.GetNameByID(npcID);
                if (!string.IsNullOrEmpty(npcName))
                    return $"击败 {npcName}";
            }

            npcID = GetRealPresentID(type);
            if (npcID != 0)
            {
                npcName = NPCHelper.GetNameByID(npcID);
                if (!string.IsNullOrEmpty(npcName))
                    return $"{npcName} 在场";
            }

            return "";
        }

        public static int GetTypeByName(string name)
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
            switch (name)
            {
                case "钓鱼任务": case "渔夫任务": case "任务": return FishQuestCompleted;
                case "困难模式": return downedWallofFlesh;
            }

            string s = "";
            int npcID = 0;
            if (name.Contains("击败"))
            {
                // 击败boss
                s = name.Replace(" ", "").Replace("击败", "").ToLowerInvariant();
                npcID = NPCHelper.GetIDByName(s);
                if (npcID != 0)
                    return GetDownedID(npcID);

            }
            else if (name.Contains("活着") || name.Contains("在场"))
            {
                // npc 活着
                s = name.Replace(" ", "").Replace("活着", "").Replace("在场", "").ToLowerInvariant();
                npcID = NPCHelper.GetIDByName(s);
                if (npcID != 0)
                    return GetPresentID(npcID);
            }
            return 0;
        }


        /// <summary>
        /// 检查解锁条件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="player"></param>
        /// <param name="result">检查结果</param>
        /// <returns></returns>
        public static bool CheckUnlock(ItemData data, TSPlayer player, out string result)
        {
            string s = "";
            bool passed = false;
            switch (data.id)
            {
                case downedSlimeKing: if (!NPC.downedSlimeKing) s = "未击败 史莱姆王"; break;
                case downedBoss1: if (!NPC.downedBoss1) s = "未击败 克苏鲁之眼"; break;
                case downedBoss2: if (!NPC.downedBoss2) s = "未击败 世界吞噬怪/克苏鲁之脑"; break;
                case downedBoss3: if (!NPC.downedBoss3) s = "未击败 骷髅王"; break;
                case downedDeerclops: if (!NPC.downedDeerclops) s = "未击败 鹿角怪"; break;
                case downedQueenBee: if (!NPC.downedQueenBee) s = "未击败 蜂王"; break;
                case downedWallofFlesh: if (!Main.hardMode) s = "未击败 血肉墙"; break;
                case downedMechBoss1: if (!NPC.downedMechBoss1) s = "未击败 毁灭者"; break;
                case downedMechBoss2: if (!NPC.downedMechBoss2) s = "未击败 双子魔眼"; break;
                case downedMechBoss3: if (!NPC.downedMechBoss3) s = "未击败 机械骷髅王"; break;
                case downedPlantBoss: if (!NPC.downedPlantBoss) s = "未击败 世纪之花"; break;
                case downedGolemBoss: if (!NPC.downedGolemBoss) s = "未击败 石巨人"; break;
                case downedQueenSlime: if (!NPC.downedQueenSlime) s = "未击败 史莱姆皇后"; break;
                case downedEmpressOfLight: if (!NPC.downedEmpressOfLight) s = "未击败 光之女皇"; break;
                case downedFishron: if (!NPC.downedFishron) s = "未击败 猪龙鱼公爵"; break;
                case downedAncientCultist: if (!NPC.downedAncientCultist) s = "未击败 拜月教邪教徒"; break;
                case downedMoonlord: if (!NPC.downedMoonlord) s = "未击败 月亮领主"; break;

                case downedHalloweenTree: if (!NPC.downedHalloweenTree) s = "未击败 哀木"; break;
                case downedHalloweenKing: if (!NPC.downedHalloweenKing) s = "未击败 南瓜王"; break;
                case downedChristmasTree: if (!NPC.downedChristmasTree) s = "未击败 常绿尖叫怪"; break;
                case downedChristmasIceQueen: if (!NPC.downedChristmasIceQueen) s = "未击败 冰雪女王"; break;
                case downedChristmasSantank: if (!NPC.downedChristmasSantank) s = "未击败 圣诞坦克"; break;
                case downedMartians: if (!NPC.downedMartians) s = "未击败 火星飞碟"; break;
                case downedGoblins: if (!NPC.downedGoblins) s = "未击败 哥布林入侵"; break;
                case downedPirates: if (!NPC.downedPirates) s = "未击败 海盗入侵"; break;
                case downedFrost: if (!NPC.downedFrost) s = "未击败 霜月"; break;
                case downedClown: if (!NPC.downedClown) s = "未击败 小丑"; break;

                case downedTowerSolar: if (!NPC.downedTowerSolar) s = "未击败 日耀柱"; break;
                case downedTowerVortex: if (!NPC.downedTowerVortex) s = "未击败 星旋柱"; break;
                case downedTowerNebula: if (!NPC.downedTowerNebula) s = "未击败 星云柱"; break;
                case downedTowerStardust: if (!NPC.downedTowerStardust) s = "未击败 星尘柱"; break;

                case downedAnyTower: if (!NPC.downedTowerNebula && !NPC.downedTowerSolar && !NPC.downedTowerStardust && !NPC.downedTowerVortex) s = "未击败 四柱之一"; break;
                case downedAllTower: if (!NPC.downedTowerNebula || !NPC.downedTowerSolar || !NPC.downedTowerStardust || !NPC.downedTowerVortex) s = "未击败 四柱"; break;

                case downedAnyMech: if (!NPC.downedMechBossAny) s = "未击败 任意机械boss"; break;
                case downedAllMech: if (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3) s = "未击败 机械boss"; break;

                case ShopItemID.Moonphase1: if (Main.moonPhase != 0) s = "需要 满月"; break;
                case ShopItemID.Moonphase2: if (Main.moonPhase != 1) s = "需要 亏凸月"; break;
                case ShopItemID.Moonphase3: if (Main.moonPhase != 2) s = "需要 下弦月"; break;
                case ShopItemID.Moonphase4: if (Main.moonPhase != 3) s = "需要 残月"; break;
                case ShopItemID.Moonphase5: if (Main.moonPhase != 4) s = "需要 新月"; break;
                case ShopItemID.Moonphase6: if (Main.moonPhase != 5) s = "需要 娥眉月"; break;
                case ShopItemID.Moonphase7: if (Main.moonPhase != 6) s = "需要 上弦月"; break;
                case ShopItemID.Moonphase8: if (Main.moonPhase != 7) s = "需要 盈凸月"; break;

                // 生态群落
                case ZoneForest: if (!player.TPlayer.ShoppingZone_Forest) s = "需要 身处森林"; break;
                case ZoneJungle: if (!player.TPlayer.ZoneJungle) s = "需要 身处丛林"; break;
                case ZoneDesert: if (!player.TPlayer.ZoneDesert) s = "需要 身处沙漠"; break;
                case ZoneSnow: if (!player.TPlayer.ZoneSnow) s = "需要 身处雪原"; break;
                case ZoneUnderworld: if (!player.TPlayer.ZoneUnderworldHeight) s = "需要 身处洞穴"; break;
                case ZoneBeach: if (!player.TPlayer.ZoneBeach) s = "需要 身处海洋"; break;
                case ZoneHallow: if (!player.TPlayer.ZoneHallow) s = "需要 身处神圣之地"; break;
                case ZoneGlowshroom: if (!player.TPlayer.ZoneGlowshroom) s = "需要 身处发光蘑菇地"; break;
                case ZoneCorrupt: if (!player.TPlayer.ZoneCorrupt) s = "需要 身处腐化之地"; break;
                case ZoneCrimson: if (!player.TPlayer.ZoneCrimson) s = "需要 身处猩红之地"; break;
                case ZoneDungeon: if (!player.TPlayer.ZoneDungeon) s = "需要 身处地牢"; break;
                case ZoneGraveyard: if (!player.TPlayer.ZoneGraveyard) s = "需要 墓地环境"; break;
                case ZoneHive: if (!player.TPlayer.ZoneHive) s = "需要 身处蜂巢"; break;
                case ZoneLihzhardTemple: if (!player.TPlayer.ZoneLihzhardTemple) s = "需要 身处神庙"; break;
                case ZoneSandstorm: if (!player.TPlayer.sandStorm) s = "需要 身处沙尘暴"; break;
                case ZoneSky: if (!player.TPlayer.ZoneSkyHeight) s = "需要 身处天空"; break;
                case ZonePond: if (!CheckPond(player)) s = "需要 池塘"; break;

                case BloodMoon: if (!Main.bloodMoon) s = "需要 血月"; break;
                case Raining: if (!Main.raining) s = "需要 雨天"; break;

                case Day: if (!Main.dayTime) s = "需要 白天"; break;
                case Night: if (Main.dayTime) s = "需要 晚上"; break;
                case Xmas: if (!Main.xMas) s = "需要 圣诞节"; break;
                case Halloween: if (!Main.halloween) s = "需要 万圣节"; break;
                case Party: if (!BirthdayParty._wasCelebrating) s = "需要 派对"; break;
                case Lantern: if (!LanternNight.LanternsUp) s = "需要 灯笼夜"; break;
                case NeedSandstorm: if (!Sandstorm.Happening) s = "需要 沙尘暴"; break;
                case WindyDay: if (!Main.IsItAHappyWindyDay) s = "需要 大风天"; break;
                case Thunderstorm: if (!Main.IsItStorming) s = "需要 雷雨"; break;
                case starfallBoost: if (Star.starfallBoost <= 3f) s = "需要 流星雨"; break;
                case SlimeRain: if (!Main.slimeRain) s = "需要 史莱姆雨"; break;
                case Eclipse: if (!Main.eclipse) s = "需要 日食"; break;

                case GoblinInvasion: if (Main.invasionType != 1) s = "需要 哥布林入侵"; break;
                case ForstMoon: if (Main.invasionType != 2) s = "需要 霜月"; break;
                case PirateInvasion: if (Main.invasionType != 3) s = "需要 海盗入侵"; break;
                case Martian: if (Main.invasionType != 4) s = "需要 火星暴乱"; break;

                case PumpkinMoon: if (Main.pumpkinMoon) s = "需要 南瓜月"; break;
                case SnowMoon: if (!Main.snowMoon) s = "需要 雪人军团"; break;
                case NeedDD2Event: if (!DD2Event.Ongoing) s = "需要 撒旦军队"; break;
                case LunarEvent: if (!NPC.LunarApocalypseIsUp) s = "需要 月亮事件"; break;

                case FishQuestCompleted: if (player.RealPlayer && player.TPlayer.anglerQuestsFinished < data.stack) s = $"需要 完成{data.stack}次渔夫任务"; break;
                case HpUnder400: if (player.RealPlayer && player.TPlayer.statLifeMax >= 400) s = "生命值 超过了400"; break;


                default:
                    break;
            }

            // 指定npc是否活着
            int npcID = GetRealPresentID(data.id);
            if (npcID != 0)
            {
                passed = NPCHelper.CheckNPCActive(npcID.ToString());
                if (!passed)
                {
                    if (npcID < Main.maxNPCTypes)
                        s = $"{TShock.Utils.GetNPCById(npcID).FullName} 不在场";
                    else
                        s = $"找不到id为 {npcID} 的NPC";
                }
            }

            result = s;
            passed = s == "";
            return passed;
        }

        /// <summary>
        /// 玩家附近是否有池塘（200格液体）
        /// </summary>
        private static bool CheckPond(TSPlayer op)
        {
            Rectangle rect = utils.GetScreen(op);
            int count = 0;
            for (int x = rect.X; x < rect.Right; x++)
            {
                for (int y = rect.Y; y < rect.Bottom; y++)
                {
                    ITile tile = Main.tile[x, y];
                    if (tile.liquid == byte.MaxValue)
                        count++;

                    if (count >= 200)
                        return true;
                }
            }
            return false;
        }

    }
}