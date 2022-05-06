using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TShockAPI;


namespace FishShop
{
    public class DocsHelper
    {
        public static void GenDocs(TSPlayer op, string path)
        {
            // https://terraria.fandom.com/zh/wiki/Category:Data_IDs
            // https://terraria.fandom.com/zh/wiki/Item_IDs
            // https://terraria.fandom.com/zh/wiki/NPC_IDs
            // https://terraria.fandom.com/zh/wiki/Buff_IDs

            bool needRecover = true;
            GameCulture culture = Language.ActiveCulture;
            if ( culture.LegacyId != (int)GameCulture.CultureName.Chinese ){
                LanguageManager.Instance.SetLanguage(GameCulture.FromCultureName(GameCulture.CultureName.Chinese));
                needRecover = false;
            }

            List<string> paths = new List<string>() {
                Path.Combine(path, "[fish]鱼店ID.txt"),
                Path.Combine(path, "[fish]能识别的关键字.txt"),
                Path.Combine(path, "[fish]ItemList.txt"),
                Path.Combine(path, "[fish]NPCList.txt"),
                Path.Combine(path, "[fish]BuffList.txt"),
                Path.Combine(path, "[fish]PrefixList.txt"),
                Path.Combine(path, "[fish]ProjectileList.txt"),
                };

            DumpShopItems( paths[0] );
            DumpKeywords( paths[1] );
            DumpItems( paths[2] );
            DumpNPCs( paths[3] );
            DumpBuffs( paths[4] );
            DumpPrefixes( paths[5]);
            DumpProjectiles( paths[6] );

            op.SendInfoMessage($"已生成参考文档:\n{string.Join("\n", paths)}");
            
            if( needRecover )
                LanguageManager.Instance.SetLanguage(culture);
        }
        private static void DumpShopItems(string path)
		{
            Regex newLine = new Regex(@"\n");

            List<string> li = new List<string>() {
                "-24~5124 为泰拉原版的物品id",
                "-100起为本插件自定义id",
                "-1000-[npcID] 为 召唤NPC，含boss、敌怪、城镇NPC",
                "-2000-[npcID] 为 击败boss（仅boss）",
                "-3000-[npcID] 为 xxNPC活着，含boss、敌怪、城镇NPC",
                "-4000-[npcID] 为 清除NPC，含boss、敌怪、城镇NPC",
                "-5000-[buffID] 为 获得增益（buff）",
                "",
                "ID,关键词,鱼店商品,解锁条件,交易物品,说明",
                "-100,渔夫任务,×,√,×,完成多少次渔夫任务",
                "-101,任务鱼,×,×,√,当天任务鱼",
                "-104,血月,×,√,×,血月期间",
                "-105,雨天,×,√,×,下雨期间",
                "-106,hp<400,×,√,×,生命值<400",
                "-132,亏凸月,√,√,×",
                "-133,下弦月,√,√,×",
                "-134,残月,√,√,×",
                "-135,新月,√,√,×",
                "-136,娥眉月,√,√,×",
                "-137,上弦月,√,√,×",
                "-138,盈凸月,√,√,×",
                "-139,下个月相,√,×,×,支持单次购买多件",
                "-140,烟花,√,×,×",
                "-141,烟花起飞,√,×,×",
                "-142,更换任务鱼,√,×,×,随机任务鱼",
                "-143,雨来,√,×,×",
                "-144,雨停,√,×,×",
                "-145,好运来,√,×,×,获得 好运 buff 52分钟",
                "-146,跳过入侵,√,×,×",
                "-147,调白天,√,×,×,将时间调到 4:30",
                "-148,调晚上,√,×,×,将时间调到 19:30",
                "-164,调中午,√,×,×,将时间调到 12:00",
                "-165,调午夜,√,×,×,将时间调到 00:00",
                "-149,召唤血月,√,×,×",
                "-150,跳过血月,√,×,×",
                "-160,执行指令,√,×,×,prefix填写具体的指令内容",
                "-161,复活NPC,√,×,×,复活怪物图鉴记录过的NPC",
                "-162,集合打团,√,×,×,将全服玩家tp到购买者位置",
                "-163,集体庆祝,√,×,×,全服玩家集体 小跳+烟花",
                "",
                "",
                "-1000-[npcID],召唤xx,√,×,×,召唤NPC，含boss、敌怪、城镇NPC",
                "-2000-[npcID],击败boss,×,√,×,击败指定boss（仅boss）",
                "-3000-[npcID],xx活着,×,√,×,NPC活着，含boss、敌怪、城镇NPC",
                "-4000-[npcID],清除xx,√,×,×,清除NPC，含boss、敌怪、城镇NPC",
                "-5000-[buffID],获得buff,√,×,×,获得buff，stack填写秒数",
                "",
                "",
                "-2050,击败史莱姆王,×,√,×",
                "-2004,击败克苏鲁之眼,×,√,×",
                "-2668,击败鹿角怪,×,√,×",
                "-2013,击败世界吞噬者,×,√,×",
                "-2013,击败克苏鲁之脑,×,√,×",
                "-2222,击败蜂王,×,√,×",
                "-2035,击败骷髅王,×,√,×",
                "-2113,击败血肉墙,×,√,×",
                "-2134,击败毁灭者,×,√,×",
                "-2125,击败双子魔眼,×,√,×",
                "-2127,击败机械骷髅王,×,√,×",
                "-2262,击败世纪之花,×,√,×",
                "-2245,击败石巨人,×,√,×",
                "-2657,击败史莱姆皇后,×,√,×",
                "-2636,击败光之女皇,×,√,×",
                "-2370,击败猪龙鱼公爵,×,√,×",
                "-2439,击败拜月教邪教徒,×,√,×",
                "-2396,击败月亮领主,×,√,×",
                "-2325,击败哀木,×,√,×",
                "-2327,击败南瓜王,×,√,×",
                "-2344,击败常绿尖叫怪,×,√,×",
                "-2345,击败冰雪女王,×,√,×",
                "-2346,击败圣诞坦克,×,√,×",
                "-2392,击败火星飞碟,×,√,×",
                "-2108,击败小丑,×,√,×",
                "-2517,击败日耀柱,×,√,×",
                "-2422,击败星旋柱,×,√,×",
                "-2507,击败星云柱,×,√,×",
                "-2493,击败星旋柱,×,√,×",
                "-2901,一王后,×,√,×,击败任意机械boss",
                "-2902,三王后,×,√,×,击败全部的机械boss",
                "-2903,一柱后,×,√,×,击败任意四柱",
                "-2904,四柱后,×,√,×,击败全部的四柱",
                "-2905,击败哥布林入侵,×,√,×",
                "-2906,击败海盗入侵,×,√,×",
                "-2907,击败霜月,×,√,×"
                };

			StringBuilder buffer = new StringBuilder();
            foreach (var line in li)
            {
                buffer.AppendLine(line);
            }

            File.WriteAllText(path, buffer.ToString());
        }

        private static void DumpKeywords(string path)
        {
            string s = @"史王    史莱姆王
史后    史莱姆皇后
克眼    克苏鲁之眼
克脑    克苏鲁之脑
世吞    世界吞噬者    世界吞噬怪
骷髅王
巨鹿    鹿角怪
蜂后    蜂王
血肉墙
铁长直    毁灭者
双子    双子魔眼
机械骷髅王
光女    光之女皇
猪鲨    猪龙鱼公爵
世花    世纪之花
石巨人
邪教徒    拜月教邪教徒
月总    月亮领主

哀木    南瓜王
常绿尖叫怪
冰雪皇后    冰雪女王
坦克    圣诞坦克
火星飞碟    外星飞碟
荷兰飞盗船    海盗船
双足翼龙
日耀    日耀柱    日曜    日曜柱
星旋    星旋柱
星云    星云柱
星尘    星尘柱

骷髅王后
肉后    困难模式
花后    石后    月后
一王后    三王后
一柱后    四柱后

向导    商人    护士    军火商    服装商    爆破专家    树妖    染料商
动物学家    高尔夫球手    油漆工    骷髅商人    旅商    巫医    老人
派对女孩    蒸汽朋克人    机器侠    海盗    公主    圣诞老人    税收官    松露人

渔夫    沉睡渔夫
哥布林工匠    受缚哥布林
机械师    受缚机械师
酒保    酒馆老板    昏迷男子
发型师    被网住的发型师    受缚发型师
巫师    受缚巫师
高尔夫球手    高尔夫球手救援";
            File.WriteAllText(path, s);
        }

        private static void DumpItems(string path)
		{
            Regex newLine = new Regex(@"\n");
			StringBuilder buffer = new StringBuilder();
			buffer.AppendLine("id,名称,描述");

            for (int i = 1; i < Main.maxItemTypes; i++)
            {
                Item item = new Item();
                item.SetDefaults(i);

                string tt = "";
				for (int x = 0; x < item.ToolTip.Lines; x++) {
					tt += item.ToolTip.GetLine(x) + "\n";
				}

                buffer.AppendLine( $"{i},{newLine.Replace(item.Name, @" ")},{newLine.Replace(tt, @" ")}" );
            }

            File.WriteAllText(path, buffer.ToString());
        }

        private static void DumpNPCs(string path)
		{
			StringBuilder buffer = new StringBuilder();
			buffer.AppendLine("id,名称");

			for (int i = -65; i < Main.maxNPCTypes; i++)
			{
				NPC npc = new NPC();
				npc.SetDefaults(i);
				if (!string.IsNullOrEmpty(npc.FullName))
				{
					buffer.AppendLine( $"{i},{npc.FullName}");
				}
			}

            File.WriteAllText(path, buffer.ToString());
        }

        private static void DumpBuffs(string path)
		{
			StringBuilder buffer = new StringBuilder();
			buffer.AppendLine("id,名称,描述");

			for (int i = 0; i < Main.maxBuffTypes; i++)
			{
				if (!string.IsNullOrEmpty(Lang.GetBuffName(i)))
				{
					buffer.AppendLine( $"{i},{Lang.GetBuffName(i)},{Lang.GetBuffDescription(i)}" );
				}
			}

            File.WriteAllText(path, buffer.ToString());
        }

        private static void DumpPrefixes(string path)
		{
			StringBuilder buffer = new StringBuilder();
			buffer.AppendLine("id,名称");

			for (int i = 0; i < PrefixID.Count; i++)
			{
				string prefix = Lang.prefix[i].ToString();

				if (!string.IsNullOrEmpty(prefix))
				{
					buffer.AppendLine( $"{i},{prefix}");
				}
			}

            File.WriteAllText(path, buffer.ToString());
        }

        private static void DumpProjectiles(string path)
		{
			StringBuilder buffer = new StringBuilder();
			buffer.AppendLine("id,射弹名称");

			for (int i = 0; i < Main.maxProjectileTypes; i++)
			{
				Projectile projectile = new Projectile();
				projectile.SetDefaults(i);
				if (!string.IsNullOrEmpty(projectile.Name))
				{
					buffer.AppendLine( $"{i},{projectile.Name}");
				}
			}

            File.WriteAllText(path, buffer.ToString());
        }

    }
}