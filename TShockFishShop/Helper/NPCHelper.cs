using System.Collections.Generic;
using TShockAPI.Localization;
using Terraria;
using TShockAPI;
using Terraria.GameContent.Bestiary;


namespace Plugin
{
    public class NPCHelper
    {

        public static string GetNameByID(int id)
        {
            switch (id)
            {
                // npc
                case 22: return "向导";
                case 369: return "渔夫";
                case 376: return "沉睡渔夫";
                case 19: return "军火商";
                case 54: return "服装商";
                case 38: return "爆破专家";
                case 20: return "树妖";
                case 207: return "染料商";
                case 107: return "哥布林工匠";
                case 105: return "受缚哥布林";
                case 588: return "高尔夫球手";
                case 589: return "高尔夫球手救援";
                case 124: return "机械师";
                case 123: return "受缚机械师";
                case 17: return "商人";
                case 18: return "护士";
                case 37: return "老人";
                case 227: return "油漆工";
                case 208: return "派对女孩";
                case 453: return "骷髅商人";
                case 353: return "发型师";
                case 354: return "被网住的发型师";
                case 550: return "酒馆老板";
                case 579: return "昏迷男子";
                case 368: return "旅商";
                case 228: return "巫医";
                case 633: return "动物学家";
                case 209: return "机器侠";
                case 229: return "海盗";
                case 663: return "公主";
                case 142: return "圣诞老人";
                case 178: return "蒸汽朋克人";
                case 441: return "税收官";
                case 160: return "松露人";
                case 108: return "巫师";
                case 106: return "受缚巫师";

                // boss
                case 50: return "史莱姆王";
                case 4: return "克苏鲁之眼";
                case 13: return "世界吞噬者";
                case 266: return "克苏鲁之脑";
                case 35: return "骷髅王";
                case 668: return "鹿角怪";
                case 222: return "蜂王";
                case 113: return "血肉墙";

                case 134: return "毁灭者";
                case 125: return "双子魔眼";
                case 127: return "机械骷髅王";
                case 262: return "世纪之花";
                case 245: return "石巨人";
                case 657: return "史莱姆皇后";
                case 636: return "光之女皇";
                case 370: return "猪龙鱼公爵";
                case 439: return "拜月教邪教徒";
                case 396: return "月亮领主";

                case 325: return "哀木";
                case 327: return "南瓜王";
                case 344: return "常绿尖叫怪";
                case 345: return "冰雪女王";
                case 346: return "圣诞坦克";
                case 517: return "日耀柱";
                case 422: return "星旋柱";
                case 507: return "星云柱";
                case 493: return "星尘柱";
            }

            // 其它NPC
            string s = GetNPCNameValue(id);
            if( !string.IsNullOrEmpty(s) )
                return s;


            return "";
        }

        public static int GetIDByName(string name="")
        {
            switch (name)
            {
                // npc
                case "向导": return 22;
                case "渔夫": return 369;
                case "沉睡渔夫":    return 376;
                case "军火商":  return 19;
                case "服装商":  return 54;
                case "爆破专家": return 38;
                case "树妖":    return 20;
                case "染料商":  return 207;
                case "哥布林工匠":  return 107;
                case "受缚哥布林":  return 105;
                case "高尔夫球手":  return 588;
                case "高尔夫球手救援": return 589;
                case "机械师": return 124;
                case "受缚机械师":  return 123;
                case "商人":    return 17;
                case "护士":    return 18;
                case "老人":    return 37;
                case "油漆工":  return 227;
                case "派对女孩":    return 208;
                case "骷髅商人":    return 453;
                case "发型师":  return 353;
                case "被网住的发型师":  case "受缚发型师":  return 354;
                case "酒馆老板":    case "酒保":    return 550;
                case "昏迷男子":    return 579;
                case "旅商":    return 368;
                case "巫医":    return 228;
                case "动物学家":    return 633;
                case "机器侠":  return 209;
                case "海盗":    return 229;
                case "公主":    return 663;
                case "圣诞老人":    return 142;
                case "蒸汽朋克人":  return 178;
                case "税收官":  return 441;
                case "松露人":  return 160;
                case "巫师":    return 108;
                case "受缚巫师":    return 106;

                // boss
                case "血肉墙": return 113;
                case "世纪之花": case "世花": return 262;
                case "石巨人": return 245;

                case "史王": case "史莱姆王": return 50;
                case "克眼": case "克苏鲁之眼": return 4;
                case "世吞": case "世界吞噬者": return 13;
                case "克脑": case "克苏鲁之脑": return 266;
                case "骷髅王": return 35;
                case "巨鹿": case "鹿角怪": return 668;
                case "蜂后": case "蜂王": return 222;

                case "铁长直": case "毁灭者": return 134;
                case "双子魔眼": case "双子": return 125;
                case "机械骷髅王": return 127;

                case "史后": case "史莱姆皇后": return 657;
                case "光女": case "光之女皇": return 636;
                case "猪鲨": case "猪龙鱼公爵": return 370;
                case "邪教徒": case "拜月教邪教徒": return 439;
                case "月总": case "月亮领主": return 396;

                case "哀木": return 325;
                case "南瓜王": return 327;

                case "常绿尖叫怪": return 344;
                case "冰雪皇后": case "冰雪女王": return 345;
                case "坦克": case "圣诞坦克": return 346;

                case "火星飞碟": case "外星飞碟": return 395;

                case "日耀": case "日耀柱": case "日曜": case "日曜柱": return 517;
                case "星旋": case "星旋柱": return 422;
                case "星云": case "星云柱": return 507;
                case "星尘": case "星尘柱": return 493;
            }

            int id = GetNPCIDByName(name);
            if (id!=0)
                return id;

            return 0;
        }


        // 检查NPC是否在场（活着）
        public static bool CheckNPCActive(string npcNameOrId)
        {
            int id = 0;
            if( !int.TryParse(npcNameOrId, out id) )
                id = GetIDByName( npcNameOrId );

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].netID==id )
                    return true;
            }
            return false;
        }


        private static string GetNPCNameValue(int id )
        {
            if( id<Main.maxNPCTypes && id!=0 )
                return Lang.GetNPCNameValue(id);
            return "";
        }


        private static int GetNPCIDByName(string name)
		{
			var found = new List<int>();
			NPC npc = new NPC();
			string nameLower = name.ToLowerInvariant();
			for (int i = -17; i < Main.maxNPCTypes; i++)
			{
				string englishName = EnglishLanguage.GetNpcNameById(i).ToLowerInvariant();

				npc.SetDefaults(i);
				if (npc.FullName.ToLowerInvariant() == nameLower || npc.TypeName.ToLowerInvariant() == nameLower
					|| nameLower == englishName)
					return npc.netID;
				if (npc.FullName.ToLowerInvariant().StartsWith(nameLower) || npc.TypeName.ToLowerInvariant().StartsWith(nameLower)
					|| englishName?.StartsWith(nameLower) == true)
					found.Add((int)npc.netID);
			}
			for (int i = -17; i < Main.maxNPCTypes; i++)
			{
				string englishName = Lang.GetNPCNameValue(i);

				npc.SetDefaults(i);
				if (npc.FullName.ToLowerInvariant() == nameLower || npc.TypeName.ToLowerInvariant() == nameLower
					|| nameLower == englishName)
					return npc.netID;
				if (npc.FullName.ToLowerInvariant().StartsWith(nameLower) || npc.TypeName.ToLowerInvariant().StartsWith(nameLower)
					|| englishName?.StartsWith(nameLower) == true)
					found.Add((int)npc.netID);
			}

            if (found.Count >= 1)
                return found[0];

			return 0;
		}


        
		// NPC重生
        public static void ReliveNPC(TSPlayer op)
        {
            List<int> found = new List<int>();

            // 向导 
            found.Add(22);

            // 解救状态
            // 渔夫
            if( NPC.savedAngler )
                found.Add(369);

            // 哥布林
            if( NPC.savedGoblin )
                found.Add(107);

            // 机械师
            if( NPC.savedMech )
                found.Add(124);

            // 发型师
            if( NPC.savedStylist )
                found.Add(353);

            // 酒馆老板
            if( NPC.savedBartender )
                found.Add(550);

            // 高尔夫球手
            if( NPC.savedGolfer )
                found.Add(588);

            // 巫师
            if( NPC.savedWizard )
                found.Add(108);

            // 税收管
            if( NPC.savedTaxCollector )
                found.Add(441);

            // 猫
            if( NPC.boughtCat )
                found.Add(637);

            // 狗
            if( NPC.boughtDog )
                found.Add(638);

            // 兔
            if( NPC.boughtBunny )
                found.Add(656);

            // 怪物图鉴解锁情况
            List<int> remains = new List<int>() {
                // 22, //向导
                19, //军火商
                54, //服装商
                38, //爆破专家
                20, //树妖
                207, //染料商
                17, //商人
                18, //护士
                227, //油漆工
                208, //派对女孩
                228, //巫医
                633, //动物学家
                209, //机器侠
                229, //海盗
                178, //蒸汽朋克人
                160, //松露人
                663 //公主

                // 453, //骷髅商人
                // 368, //旅商
                // 37, // 老人
            };
            // 142, //圣诞老人
            if( Main.xMas )
                remains.Add(142);

            foreach (int npcID1 in remains)
            {
                if( DidDiscoverBestiaryEntry( npcID1 ) )
                    found.Add(npcID1);
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if( !Main.npc[i].active || !Main.npc[i].townNPC )
                    continue;

                found.Remove(Main.npc[i].type);
            }

            // 生成npc
            List<string> names = new List<string>();
            foreach (int npcID in found)
            {
				NPC npc = new NPC();
				npc.SetDefaults(npcID);
				TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, 1, op.TileX, op.TileY);

                if( names.Count!=0 && names.Count%10==0 ){
                    names.Add("\n"+npc.FullName);
                } else {
                    names.Add(npc.FullName);
                }
            }

            // 找家
            // for (int i = 0; i < Main.maxNPCs; i++)
            // {
            //     if( !Main.npc[i].active || !Main.npc[i].townNPC )
            //         continue;

            //     if( found.Contains(Main.npc[i].type) )
            //         WorldGen.QuickFindHome(i);
            // }

            if( found.Count>0 ){
				TSPlayer.All.SendInfoMessage($"{op.Name} 复活了 {found.Count}个 NPC:");
				TSPlayer.All.SendInfoMessage($"{string.Join("、", names)}");
            } else {
                op.SendInfoMessage("入住过的NPC都活着");
            }
        }
        
		private static bool DidDiscoverBestiaryEntry(int npcId)
		{
			return Main.BestiaryDB.FindEntryByNPCID(npcId).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
		}


    }
}