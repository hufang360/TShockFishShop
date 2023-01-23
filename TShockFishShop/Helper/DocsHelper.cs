using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using TShockAPI;


namespace FishShop
{
    public class DocsHelper
    {
        public static void GenDocs(TSPlayer op, string path)
        {
            // https://terraria.wiki.gg/zh/wiki/Category:Data_IDs
            // https://terraria.wiki.gg/zh/wiki/Item_IDs
            // https://terraria.wiki.gg/zh/wiki/NPC_IDs
            // https://terraria.wiki.gg/zh/wiki/Buff_IDs

            bool needRecover = true;
            GameCulture culture = Language.ActiveCulture;
            if (culture.LegacyId != (int)GameCulture.CultureName.Chinese)
            {
                LanguageManager.Instance.SetLanguage(GameCulture.FromCultureName(GameCulture.CultureName.Chinese));
                needRecover = false;
            }

            List<string> paths = new List<string>() {
                Path.Combine(path, "[fish]ItemList.txt"),
                Path.Combine(path, "[fish]NPCList.txt"),
                Path.Combine(path, "[fish]BuffList.txt"),
                Path.Combine(path, "[fish]PrefixList.txt"),
                //Path.Combine(path, "[fish]ProjectileList.txt"),
                };

            DumpItems(paths[1]);
            DumpNPCs(paths[2]);
            DumpBuffs(paths[3]);
            DumpPrefixes(paths[4]);
            //DumpProjectiles(paths[5]);

            op.SendInfoMessage($"已生成参考文档:\n{string.Join("\n", paths)}");

            if (needRecover)
                LanguageManager.Instance.SetLanguage(culture);
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
                for (int x = 0; x < item.ToolTip.Lines; x++)
                {
                    tt += item.ToolTip.GetLine(x) + "\n";
                }

                buffer.AppendLine($"{i},{newLine.Replace(item.Name, @" ")},{newLine.Replace(tt, @" ").TrimEnd()}");
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
                    buffer.AppendLine($"{i},{npc.FullName}");
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
                    buffer.AppendLine($"{i},{Lang.GetBuffName(i)},{Lang.GetBuffDescription(i)}");
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
                    buffer.AppendLine($"{i},{prefix}");
                }
            }

            File.WriteAllText(path, buffer.ToString());
        }

        // private static void DumpProjectiles(string path)
        // {
        //     StringBuilder buffer = new StringBuilder();
        //     buffer.AppendLine("id,射弹名称");

        //     for (int i = 0; i < Main.maxProjectileTypes; i++)
        //     {
        //         Projectile projectile = new Projectile();
        //         projectile.SetDefaults(i);
        //         if (!string.IsNullOrEmpty(projectile.Name))
        //         {
        //             buffer.AppendLine($"{i},{projectile.Name}");
        //         }
        //     }

        //     File.WriteAllText(path, buffer.ToString());
        // }

    }
}