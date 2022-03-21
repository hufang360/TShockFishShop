using Terraria;
using TShockAPI;


namespace FishShop
{
    public class UnlockID
    {
        // 需要完成多少次渔夫任务
        public const int FishQuestCompleted = -100;
        // 当天任务鱼
        public const int ItemIDQuestFish = -101;
        public const int BloodMoon = -104;
        public const int Raining = -105;
        public const int HpUnder400 = -106;


        // ------------------------------------------------------------------------------------------
        // 需要 击败指定boss
        // -2000-[npcID]
        //  击败任意 机械boss = -2901
        private const int DownedStart = -2000;
        private const int DownedEnd = -2999;


        // ------------------------------------------------------------------------------------------
        // 需要 NPC在场
        // NPC活着 = -3000-[npcID]
        private const int PresentStart = -3000;
        private const int PresentEnd = -3999;


        // ------------------------------------------------------------------------------------------
        // 获得buff
        // 获得buff = -5000-[buffID]
        // public const int BuffStart = -5000;
        // public const int BuffEnd = -5999;


        // ------------------------------------------------------------------------------------------
        // 已知的需要 击败指定boss
        // 一王后
        public const int downedAnyMech = -2901;
        // 四柱后
        public const int downedAllMech = -2902;
        // 一柱后
        public const int downedAnyTower = -2903;
        // 四柱后
        public const int downedAllTower = -2904;
        // 哥布林入侵
        public const int downedGoblins = -2905;
        // 海盗入侵
        public const int downedPirates = -2906;
        // 霜月
        public const int downedFrost = -2907;


        public const int downedSlimeKing = -2050;
        public const int downedBoss1 = -2004;
        public const int downedDeerclops = -2668;

        // 世界吞噬者 13  克苏鲁之脑 266
        public const int downedBoss2 = -2013;
        // 蜂王
        public const int downedQueenBee = -2222;
        // 骷髅王
        public const int downedBoss3 = -2035;

        // 血肉墙
        public const int downedWallofFlesh = -2113;
        // 毁灭者
        public const int downedMechBoss1 = -2134;
        // 双子魔眼
        public const int downedMechBoss2 = -2125;
        // 机械骷髅王
        public const int downedMechBoss3 = -2127;
        // 世纪之花
        public const int downedPlantBoss = -2262;
        // 石巨人
        public const int downedGolemBoss = -2245;
        // 史莱姆皇后
        public const int downedQueenSlime = -2657;
        // 光之女皇
        public const int downedEmpressOfLight = -2636;
        // 猪龙鱼公爵
        public const int downedFishron = -2370;
        // 拜月教邪教徒
        public const int downedAncientCultist = -2439;
        // 月亮领主
        public const int downedMoonlord = -2396;
        // 哀木
        public const int downedHalloweenTree = -2325;
        // 南瓜王
        public const int downedHalloweenKing = -2327;
        // 常绿尖叫怪
        public const int downedChristmasTree = -2344;
        // 冰雪女王
        public const int downedChristmasIceQueen = -2345;
        // 圣诞坦克
        public const int downedChristmasSantank = -2346;

        // 火星飞碟
        public const int downedMartians = -2392;
        // 小丑
        public const int downedClown = -2109;

        // 日耀柱
        public const int downedTowerSolar = -2517;
        // 星旋柱
        public const int downedTowerVortex = -2422;
        // 星云柱
        public const int downedTowerNebula = -2507;
        // 星旋柱
        public const int downedTowerStardust = -2493;


        
        private static int GetDownedID(int id)   {return -(2000+id);}
        public static int GetRealDownedID(int id)    {return id>DownedEnd && id<DownedStart ? DownedStart-id : 0;}

        private static int GetPresentID(int id)  {return -(3000+id);}
        public static int GetRealPresentID(int id)    {return id>PresentEnd && id<PresentStart ? PresentStart-id : 0;}

        public static string GetNameByID(int id)
        {
            switch( id )
            {
                case FishQuestCompleted: return "完成{0}次渔夫任务";
                case ItemIDQuestFish: return "当天任务鱼";
                case BloodMoon: return "血月";
                case Raining: return "雨天";
                case HpUnder400: return "生命<400";

                case downedAnyMech: return "一王后";
                case downedAllMech: return "三王后";
                case downedAnyTower: return "一柱后";
                case downedAllTower: return "四柱后";
                case downedBoss3: return "骷髅王后";
            }

            int npcID = 0;
            string npcName = "";
            npcID = GetRealDownedID(id);
            if( npcID!=0 ){
                npcName = NPCHelper.GetNameByID(npcID);
                if( !string.IsNullOrEmpty(npcName) )
                    return $"击败 {npcName}";

            } 
            
            npcID = GetRealPresentID(id);
            if( npcID!=0 ){
                npcName = NPCHelper.GetNameByID(npcID);
                if( !string.IsNullOrEmpty(npcName) )
                    return $"{npcName} 活着";
            }

            return "";
        }

        public static int GetIDByName(string name="")
        {
            if( string.IsNullOrEmpty(name) )
                return 0;

            switch (name)
            {
                case "钓鱼任务": case "渔夫任务": return FishQuestCompleted;
                case "任务鱼": return ItemIDQuestFish;
                case "血月": return BloodMoon;
                case "雨": case "雨天": case "下雨": return Raining;
                case "生命<400": case "血量<400": case "hp<400": return HpUnder400;

                case "一王后": return downedAnyMech;
                case "三王后": return downedAllMech;
                case "一柱后": return downedAnyTower;
                case "四柱后": return downedAllTower;

                case "骷髅王后": return downedBoss3;
                case "肉后": case "困难模式": return downedWallofFlesh;
                case "花后": return downedPlantBoss;
                case "石后": return downedGolemBoss;
                case "月后": return downedMoonlord;
            }

            string s = "";
            int npcID = 0;
            if( name.Contains("击败") )
            {
                // 击败boss
                s = name.Replace(" ", "").Replace("击败","").ToLowerInvariant();
                npcID = NPCHelper.GetIDByName(s);
                if( npcID!=0 )
                    return GetDownedID(npcID);

            } else if( name.Contains("活着") || name.Contains("在场") ){
                // npc 活着
                s = name.Replace(" ", "").Replace("活着","").Replace("在场", "").ToLowerInvariant();
                npcID = NPCHelper.GetIDByName(s);
                if( npcID!=0 )
                    return GetPresentID(npcID);
            }
            return 0;
        }


        // 检查需求
        public static bool CheckUnlock(ItemData data, TSPlayer player, out string msg)
        {
            string s = "";
            bool passed=false;
            switch (data.id)
            {
                case downedSlimeKing:
                    if( !NPC.downedSlimeKing )
                        s = "未击败 史莱姆王";
                    break;

                case downedBoss1:
                    if( !NPC.downedBoss1 )
                        s = "未击败 克苏鲁之眼";
                    break;

                case downedDeerclops:
                    s = Compatible.CheckUnlockDownedDeerclops();
                    break;

                case downedBoss2:
                    if( !NPC.downedBoss2 )
                        s = "未击败 世界吞噬怪/克苏鲁之脑";
                    break;

                case downedQueenBee:
                    if( !NPC.downedQueenBee )
                        s = "未击败 蜂王";
                    break;

                case downedBoss3:
                    if( !NPC.downedBoss3 )
                        s = "未击败 骷髅王";
                    break;

                case downedWallofFlesh:
                    if( !Main.hardMode )
                        s = "未击败 血肉墙";
                    break;

                case downedMechBoss1:
                    if( !NPC.downedMechBoss1 )
                        s = "未击败 毁灭者";
                    break;

                case downedMechBoss2:
                    if( !NPC.downedMechBoss2 )
                        s = "未击败 双子魔眼";
                    break;

                case downedMechBoss3:
                    if( !NPC.downedMechBoss3 )
                        s = "未击败 机械骷髅王";
                    break;

                case downedPlantBoss:
                    if( !NPC.downedPlantBoss )
                        s = "未击败 世纪之花";
                    break;

                case downedGolemBoss:
                    if( !NPC.downedGolemBoss )
                        s = "未击败 石巨人";
                    break;

                case downedQueenSlime:
                    if( !NPC.downedQueenSlime )
                        s = "未击败 史莱姆皇后";
                    break;

                case downedEmpressOfLight:
                    if( !NPC.downedEmpressOfLight )
                        s = "未击败 光之女皇";
                    break;

                case downedFishron:
                    if( !NPC.downedFishron )
                        s = "未击败 猪龙鱼公爵";
                    break;

                case downedAncientCultist:
                    if( !NPC.downedAncientCultist )
                        s = "未击败 拜月教邪教徒";
                    break;

                case downedMoonlord:
                    if( !NPC.downedMoonlord )
                        s = "未击败 月亮领主";
                    break;

                case downedHalloweenTree:
                    if( !NPC.downedHalloweenTree )
                        s = "未击败 哀木";
                    break;

                case downedHalloweenKing:
                    if( !NPC.downedHalloweenKing )
                        s = "未击败 南瓜王";
                    break;

                case downedChristmasTree:
                    if( !NPC.downedChristmasTree )
                        s = "未击败 常绿尖叫怪";
                    break;

                case downedChristmasIceQueen:
                    if( !NPC.downedChristmasIceQueen )
                        s = "未击败 冰雪女王";
                    break;

                case downedChristmasSantank:
                    if( !NPC.downedChristmasSantank )
                        s = "未击败 圣诞坦克";
                    break;

                case downedMartians:
                    if( !NPC.downedMartians )
                        s = "未击败 火星飞碟";
                    break;

                case downedGoblins:
                    if( !NPC.downedGoblins )
                        s = "未击败 哥布林入侵";
                    break;

                case downedPirates:
                    if( !NPC.downedPirates )
                        s = "未击败 海盗入侵";
                    break;

                case downedFrost:
                    if( !NPC.downedFrost )
                        s = "未击败 霜月";
                    break;

                case downedClown:
                    if( !NPC.downedClown )
                        s = "未击败 小丑";
                    break;

                case downedTowerSolar:
                    if( !NPC.downedTowerSolar )
                        s = "未击败 日耀柱";
                    break;

                case downedTowerVortex:
                    if( !NPC.downedTowerVortex )
                        s = "未击败 星旋柱";
                    break;

                case downedTowerNebula:
                    if( !NPC.downedTowerNebula )
                        s = "未击败 星云柱";
                    break;

                case downedTowerStardust:
                    if( !NPC.downedTowerStardust )
                        s = "未击败 星尘柱";
                    break;

                case downedAnyMech:
                    if(!NPC.downedMechBossAny)
                        s = "未击败 任意机械boss";
                    break;

                case downedAllMech:
                    if(!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3)
                        s = "未击败 机械boss";
                    break;

                case downedAnyTower:
                    if( NPC.downedTowerNebula || NPC.downedTowerSolar || NPC.downedTowerStardust || NPC.downedTowerVortex ){
                    } else {
                        s = "未击败 四柱之一";
                    }
                    break;

                case downedAllTower:
                    if( !NPC.downedTowerNebula || !NPC.downedTowerSolar || !NPC.downedTowerStardust || !NPC.downedTowerVortex )
                        s = "未击败 四柱";
                    break;


                case ShopItemID.Moonphase1:
                    if( Main.moonPhase != 0 )
                        s="需要 满月";
                    break;

                case ShopItemID.Moonphase2:
                    if( Main.moonPhase != 1 )
                        s="需要 亏凸月";
                    break;

                case ShopItemID.Moonphase3:
                    if( Main.moonPhase != 2 )
                        s="需要 下弦月";
                    break;

                case ShopItemID.Moonphase4:
                    if( Main.moonPhase != 3 )
                        s="需要 残月";
                    break;

                case ShopItemID.Moonphase5:
                    if( Main.moonPhase != 4 )
                        s="需要 新月";
                    break;

                case ShopItemID.Moonphase6:
                    if( Main.moonPhase != 5 )
                        s="需要 娥眉月";
                    break;

                case ShopItemID.Moonphase7:
                    if( Main.moonPhase != 6 )
                        s="需要 上弦月";
                    break;

                case ShopItemID.Moonphase8:
                    if( Main.moonPhase != 7 )
                        s="需要 盈凸月";
                    break;

                case BloodMoon:
                    if ( !Main.bloodMoon )
                        s = "需要 血月";
                    break;

                case Raining:
                    if ( !Main.raining )
                        s = "需要 雨天";
                    break;

                // 需要完成多少次渔夫任务
                case FishQuestCompleted:
                    if( player.RealPlayer )
                    {
                        if( player.TPlayer.anglerQuestsFinished < data.stack ){
                            s = $"需要 完成{data.stack}次渔夫任务";
                        }
                    }
                    break;

                // 不满400血量
                case HpUnder400:
                    if ( player.RealPlayer )
                    {
                        if ( player.TPlayer.statLifeMax >=400 )
                            s = "生命值 超过了400";
                    }
                    break;


                default:
                    break;
            }

            // 指定npc是否活着
            int npcID = GetRealPresentID(data.id);
            if( npcID!=0 )
            {
                passed = NPCHelper.CheckNPCActive(npcID.ToString());
                if( !passed ){
                    if(npcID < Main.maxNPCTypes)
                        s = $"{TShock.Utils.GetNPCById(npcID).FullName} 不在";
                    else
                        s = $"找不到id为 {npcID} 的NPC";
                }
            }

            msg = s;
            passed = s=="" ? true:false;
            return passed;
        }

    }
}