using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using TShockAPI;
using TShockAPI.Localization;


namespace FishShop
{
    public class NPCHelper
    {

        public static string GetNameByID(int id)
        {
            // 城镇NPC 和 boss
            string s = Settings.GetNPCNameByID(id);
            if (!string.IsNullOrEmpty(s)) return s;

            // 其它NPC
            s = GetNPCNameValue(id);
            if (!string.IsNullOrEmpty(s)) return s;


            return "";
        }

        public static int GetIDByName(string name = "")
        {
            int id = Settings.GetNPCIDByName(name);
            if (id != 0) return id;


            id = GetNPCIDByName(name);
            if (id != 0) return id;

            return 0;
        }


        // 检查NPC是否在场（活着）
        public static bool CheckNPCActive(string npcNameOrId)
        {
            int id = 0;
            if (!int.TryParse(npcNameOrId, out id))
                id = GetIDByName(npcNameOrId);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].netID == id)
                    return true;
            }
            return false;
        }


        private static string GetNPCNameValue(int id)
        {
            if (id < NPCID.Count && id != 0)
                return Lang.GetNPCNameValue(id);
            return "";
        }


        private static int GetNPCIDByName(string name)
        {
            var found = new List<int>();
            NPC npc = new();
            string nameLower = name.ToLowerInvariant();
            for (int i = -17; i < NPCID.Count; i++)
            {
                string englishName = EnglishLanguage.GetNpcNameById(i).ToLowerInvariant();

                npc.SetDefaults(i);
                if (npc.FullName.ToLowerInvariant() == nameLower || npc.TypeName.ToLowerInvariant() == nameLower
                    || nameLower == englishName)
                    return npc.netID;
                if (npc.FullName.ToLowerInvariant().StartsWith(nameLower) || npc.TypeName.ToLowerInvariant().StartsWith(nameLower)
                    || englishName?.StartsWith(nameLower) == true)
                    found.Add(npc.netID);
            }
            for (int i = -17; i < NPCID.Count; i++)
            {
                string englishName = Lang.GetNPCNameValue(i);

                npc.SetDefaults(i);
                if (npc.FullName.ToLowerInvariant() == nameLower || npc.TypeName.ToLowerInvariant() == nameLower
                    || nameLower == englishName)
                    return npc.netID;
                if (npc.FullName.ToLowerInvariant().StartsWith(nameLower) || npc.TypeName.ToLowerInvariant().StartsWith(nameLower)
                    || englishName?.StartsWith(nameLower) == true)
                    found.Add(npc.netID);
            }

            if (found.Count >= 1)
                return found[0];

            return 0;
        }





        /// <summary>
        /// 找出附近的指定NPC
        /// </summary>
        /// <returns>未找到返回null</returns>
        public static NPC FindNearNPC(TSPlayer op, int npcID)
        {
            Rectangle rect = new(op.TileX - 59, op.TileY - 35 + 3, 120, 68);
            foreach (var npc in Main.npc.Where(npc => npc != null && npc.active && npc.netID == npcID))
            {
                Point pos = new((int)(npc.position.X / 16), (int)(npc.position.Y / 16));
                if (rect.Contains(pos))
                {
                    return npc;
                }
            }
            return null;
        }



        /// <summary>
        /// 生成NPC
        /// </summary>
        public static void SpawnNPC(TSPlayer op, int npcID, int times = 0)
        {
            string bossType = "";
            switch (npcID)
            {
                case 266: bossType = "brain of cthulhu"; break;
                case 134: bossType = "destroyer"; break;
                case 370: bossType = "duke fishron"; break;
                case 13: bossType = "eater of worlds"; break;
                case 4: bossType = "eye of cthulhu"; break;
                case 245: bossType = "golem"; break;
                case 50: bossType = "king slime"; break;
                case 262: bossType = "plantera"; break;
                case 127: bossType = "skeletron prime"; break;
                case 222: bossType = "queen bee"; break;
                case 35: bossType = "skeletron"; break;

                case 125: bossType = "twins"; break;
                case 126: bossType = "twins"; break;

                case 113: bossType = "wall of flesh"; break;
                case 396: bossType = "moon lord"; break;
                case 636: bossType = "empress of light"; break;
                case 657: bossType = "queen slime"; break;
                case 439: bossType = "lunatic cultist"; break;
                case 551: bossType = "betsy"; break;
                case 491: bossType = "flying dutchman"; break;
                case 325: bossType = "mourning wood"; break;
                case 327: bossType = "pumpking"; break;
                case 344: bossType = "everscream"; break;
                case 346: bossType = "santa-nk1"; break;
                case 345: bossType = "ice queen"; break;

                case 392: bossType = "martian saucer"; break;
                case 393: bossType = "martian saucer"; break;
                case 394: bossType = "martian saucer"; break;
                case 395: bossType = "martian saucer"; break;

                case 517: bossType = "solar pillar"; break;
                case 507: bossType = "nebula pillar"; break;
                case 422: bossType = "vortex pillar"; break;
                case 493: bossType = "stardust pillar"; break;
                case 668: bossType = "deerclops"; break;
            }

            if (!string.IsNullOrEmpty(bossType))
            {
                // 召唤boss
                List<string> args = new() { bossType };
                if (times > 0)
                    args.Add(times.ToString());
                SpawnBossRaw(new CommandArgs("", op, args));
            }
            else
            {
                // 生成npc
                NPC npc = new();
                npc.SetDefaults(npcID);

                bool pass = true;
                if (npc.townNPC)
                {
                    if (CheckNPCActive(npcID.ToString()))
                    {
                        pass = false;
                    }
                }

                if (pass)
                    TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, times, op.TileX, op.TileY);
            }
        }

        /// <summary>
        /// 清除NPC
        /// </summary>
        /// <param name="op"></param>
        /// <param name="npcID"></param>
        /// <param name="times"></param>
        public static void ClearNPC(TSPlayer op, int npcID, int times = 0)
        {
            List<NPC> npcs = TShock.Utils.GetNPCByIdOrName(npcID.ToString());
            if (npcs.Count == 0)
            {
                op.SendErrorMessage("找不到对应的 NPC");
            }
            else if (npcs.Count > 1)
            {
                op.SendMultipleMatchError(npcs.Select(n => $"{n.FullName}({n.type})"));
            }
            else
            {
                var npc = npcs[0];
                TSPlayer.All.SendSuccessMessage("{0} 清理了 {1} 个 {2}", op.Name, ClearNPCByID(npc.netID), npc.FullName);
            }
        }

        // 通过npcid清理npc
        private static int ClearNPCByID(int npcID)
        {
            int cleared = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].netID == npcID)
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                    cleared++;
                }
            }
            return cleared;
        }

        /// <summary>
        /// SpawnBoss
        /// </summary>
        /// <param name="args"></param>
        private static void SpawnBossRaw(CommandArgs args)
        {
            if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}spawnboss <boss type> [amount]", Commands.Specifier);
                return;
            }

            int amount = 1;
            if (args.Parameters.Count == 2 && (!int.TryParse(args.Parameters[1], out amount) || amount <= 0))
            {
                args.Player.SendErrorMessage("无效的boss名!");
                return;
            }

            string message = "{0} 召唤了 {1} {2} 次";
            string spawnName = "";
            int npcID = 0;
            NPC npc = new();
            switch (args.Parameters[0].ToLower())
            {
                case "*":
                case "all":
                    int[] npcIds = { 4, 13, 35, 50, 125, 126, 127, 134, 222, 245, 262, 266, 370, 398, 439, 636, 657 };
                    TSPlayer.Server.SetTime(false, 0.0);
                    foreach (int i in npcIds)
                    {
                        npc.SetDefaults(i);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
                    }
                    spawnName = "Boss全明星";
                    return;

                case "brain":
                case "brain of cthulhu":
                case "boc":
                    npcID = 266;
                    break;

                case "destroyer":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 134;
                    break;

                case "duke":
                case "duke fishron":
                case "fishron":
                    npcID = 370;
                    break;

                case "eater":
                case "eater of worlds":
                case "eow":
                    npcID = 13;
                    break;

                case "eye":
                case "eye of cthulhu":
                case "eoc":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 4;
                    break;

                case "golem":
                    npcID = 245;
                    break;

                case "king":
                case "king slime":
                case "ks":
                    npcID = 50;
                    break;

                case "plantera":
                    npcID = 262;
                    break;

                case "prime":
                case "skeletron prime":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 127;
                    break;

                case "queen bee":
                case "qb":
                    npcID = 222;
                    break;

                case "skeletron":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 35;
                    break;

                case "twins":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npc.SetDefaults(125);
                    TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
                    npc.SetDefaults(126);
                    TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
                    spawnName = "双子魔眼";
                    break;

                case "wof":
                case "wall of flesh":
                    if (Main.wofNPCIndex != -1)
                    {
                        args.Player.SendErrorMessage("血肉墙已存在!");
                        return;
                    }
                    if (args.Player.Y / 16f < Main.maxTilesY - 205)
                    {
                        args.Player.SendErrorMessage("血肉墙只能在地狱进行召唤!");
                        return;
                    }
                    NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
                    spawnName = "血肉墙";
                    break;

                case "moon":
                case "moon lord":
                case "ml":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 398;
                    break;

                case "empress":
                case "empress of light":
                case "eol":
                    npcID = 636;
                    break;

                case "queen slime":
                case "qs":
                    npcID = 657;
                    break;

                case "lunatic":
                case "lunatic cultist":
                case "cultist":
                case "lc":
                    npcID = 439;
                    break;

                case "betsy":
                    npcID = 551;
                    break;

                case "flying dutchman":
                case "flying":
                case "dutchman":
                    npcID = 491;
                    break;

                case "mourning wood":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 325;
                    break;

                case "pumpking":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 327;
                    break;

                case "everscream":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 344;
                    break;

                case "santa-nk1":
                case "santa":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 346;
                    break;

                case "ice queen":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 345;
                    break;

                case "martian saucer":
                    npcID = 395;
                    break;

                case "solar pillar":
                    npcID = 517;
                    break;

                case "nebula pillar":
                    npcID = 507;
                    break;

                case "vortex pillar":
                    npcID = 422;
                    break;

                case "stardust pillar":
                    npcID = 493;
                    break;

                case "deerclops":
                    npcID = 668;
                    break;

                default:
                    args.Player.SendErrorMessage("无法识别此boss名!");
                    return;
            }

            if (npcID != 0)
            {
                npc.SetDefaults(npcID);
                TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);

                // boss的名字
                if (string.IsNullOrEmpty(spawnName))
                    spawnName = GetNameByID(npcID);
            }

            //"<player> spawned <spawn name> <x> time(s)"
            TSPlayer.All.SendSuccessMessage(message, args.Player.Name, spawnName, amount);
        }

        /// <summary>
        /// 是否有boss存在
        /// </summary>
        public static bool AnyBoss()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc != null && npc.active && npc.boss)
                    return true;
            }
            return false;
        }
    }
}