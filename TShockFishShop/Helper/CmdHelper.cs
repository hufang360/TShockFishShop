using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using TShockAPI;
using Microsoft.Xna.Framework;
using System.Linq;


namespace Plugin
{
    public class CmdHelper
    {

		// 调时间
		public static void SwitchTime(TSPlayer player, string type="noon")
		{
			switch (type)
			{
				case "day":
					TSPlayer.Server.SetTime(true, 0.0);
					TSPlayer.All.SendInfoMessage("{0} 将时间调到 早上 （4:30）。", player.Name);
					break;

				case "noon":
					TSPlayer.Server.SetTime(true, 27000.0);
					TSPlayer.All.SendInfoMessage("{0} 将时间调到 中午 （12:00）。", player.Name);
					break;

				case "night":
					TSPlayer.Server.SetTime(false, 0.0);
					TSPlayer.All.SendInfoMessage("{0} 将时间调到 晚上（19:30）。", player.Name);
					break;

				case "midnight":
					TSPlayer.Server.SetTime(false, 16200.0);
					TSPlayer.All.SendInfoMessage("{0} 将时间调到 午夜（00:00）。", player.Name);
					break;
			}
		}

		// 调雨
		public static void ToggleRaining(TSPlayer player, bool on)
		{
			if( on ){
				if (!Main.raining) //Toggle rain on
				{
					Main.StartRain();
					TSPlayer.All.SendData(PacketTypes.WorldInfo);
					TSPlayer.All.SendInfoMessage("{0} 让苍天下起了雨.", player.Name);
					return;
				} else {
					player.SendInfoMessage("已经在下雨了~");
				}
			} else {
				if (Main.raining) //Toggle rain off
				{
					Main.StopRain();
					TSPlayer.All.SendData(PacketTypes.WorldInfo);
					TSPlayer.All.SendInfoMessage("{0} 让雨停了下来.", player.Name);
					return;
				} else {
					player.SendInfoMessage("没在下雨");
				}
			}
		}

		// 跳过入侵
		public static void StopInvasion(TSPlayer player)
		{
			if(Main.invasionSize>0){
				Main.invasionSize = 0;
				TSPlayer.All.SendInfoMessage("{0} 跳过了入侵事件", player.Name);
			} else {
				player.SendInfoMessage("当前没有任何入侵事件");
			}
		}
		public static void ExecuteRawCmd(TSPlayer op, string rawCmd)
		{
			// List<string> args = new List<string>() {bossType};
			// if( times>0 )
			// 	args.Add( times.ToString() );
			// SpawnBossRaw( new CommandArgs("", player, args ) );

			op.tempGroup = new SuperAdminGroup();
			TShockAPI.Commands.HandleCommand(op, rawCmd);
			op.tempGroup = null;
		}

		// 集合打团
		public static void TPHereAll(TSPlayer op)
		{
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				if (Main.player[i].active && (Main.player[i] != op.TPlayer))
				{
					if (TShock.Players[i].Teleport(op.TPlayer.position.X, op.TPlayer.position.Y))
						TShock.Players[i].SendSuccessMessage(String.Format("{0} 将你传送到他身边", op.Name));
				}
			}
			TSPlayer.All.SendInfoMessage($"{op.Name} 购买了 集合打团，将所有玩家召唤到他身边");
		}

		// 集体庆祝
		public static void CelebrateAll(TSPlayer op)
		{
			Jump(op);
			TSPlayer.All.SendInfoMessage($"{op.Name} 购买了集体庆祝");
		}

		// 血月开关
		public static void ToggleBloodMoon(TSPlayer player, bool on)
		{

			if( on ){
				if (!Main.bloodMoon)
				{
					TSPlayer.Server.SetBloodMoon(on);
					TSPlayer.All.SendData(PacketTypes.WorldInfo);
					TSPlayer.All.SendInfoMessage("{0} 召唤了血月.", player.Name);
					return;
				} else {
					player.SendInfoMessage("已经是血月期间了.");
				}
			} else {
				if (Main.bloodMoon)
				{
					TSPlayer.Server.SetBloodMoon(on);
					TSPlayer.All.SendData(PacketTypes.WorldInfo);
					TSPlayer.All.SendInfoMessage("{0} 跳过了血月.", player.Name);
					return;
				} else {
					player.SendInfoMessage("已经不是血月了.");
				}
			}
		}

		// 生成NPC
        public static void SpawnNPC(TSPlayer op, int npcID, int times=0)
		{
			string bossType = "";
			switch (npcID)
			{
				case 266: bossType="brain of cthulhu"; break;
				case 134: bossType="destroyer"; break;
				case 370: bossType="duke fishron"; break;
				case 13: bossType="eater of worlds"; break;
				case 4: bossType="eye of cthulhu"; break;
				case 245: bossType="golem"; break;
				case 50: bossType="king slime"; break;
				case 262: bossType="plantera"; break;
				case 127: bossType="skeletron prime"; break;
				case 222: bossType="queen bee"; break;
				case 35: bossType="skeletron"; break;
				
				case 125: bossType="twins"; break;
				case 126: bossType="twins"; break;
				
				case 113: bossType="wall of flesh"; break;
				case 396: bossType="moon lord"; break;
				case 636: bossType="empress of light"; break;
				case 657: bossType="queen slime"; break;
				case 439: bossType="lunatic cultist"; break;
				case 551: bossType="betsy"; break;
				case 491: bossType="flying dutchman"; break;
				case 325: bossType="mourning wood"; break;
				case 327: bossType="pumpking"; break;
				case 344: bossType="everscream"; break;
				case 346: bossType="santa-nk1"; break;
				case 345: bossType="ice queen"; break;
				
				case 392: bossType="martian saucer"; break;
				case 393: bossType="martian saucer"; break;
				case 394: bossType="martian saucer"; break;
				case 395: bossType="martian saucer"; break;
				
				case 517: bossType="solar pillar"; break;
				case 507: bossType="nebula pillar"; break;
				case 422: bossType="vortex pillar"; break;
				case 493: bossType="stardust pillar"; break;
				case 668: bossType="deerclops"; break;
			}

			if( !string.IsNullOrEmpty(bossType) )
			{
				// 召唤boss
				List<string> args = new List<string>() {bossType};
				if( times>0 )
					args.Add( times.ToString() );
				SpawnBossRaw( new CommandArgs("", op, args ) );
			} else {
				// 生成npc
				NPC npc = new NPC();
				npc.SetDefaults(npcID);

				bool pass = true;
				if( npc.townNPC ){
					if(NPCHelper.CheckNPCActive(npcID.ToString())){
						pass = false;
					}
				}

				if( pass )
					TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, times, op.TileX, op.TileY);
			}
        }

		// 清除NPC
        public static void ClearNPC(TSPlayer op, int npcID, int times=0)
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
                if (Main.npc[i].active && Main.npc[i].netID==npcID )
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                    cleared++;
                }
            }
            return cleared;
        }

        // SpawnBoss
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
			string spawnName="";
			int npcID = 0;
			NPC npc = new NPC();
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

			if( npcID!=0 ){
				npc.SetDefaults(npcID);
				TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);

				// boss的名字
				if( string.IsNullOrEmpty(spawnName) )
					spawnName = NPCHelper.GetNameByID(npcID);
			}

			//"<player> spawned <spawn name> <x> time(s)"
			TSPlayer.All.SendSuccessMessage(message, args.Player.Name, spawnName, amount);
		}

		public static void FireworkRocket(TSPlayer player)
		{
			// 火箭
			player.TPlayer.velocity.Y = -50;
			TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", player.Index);

			// 烟花
			Firework(player);
		}

		public static void Jump (TSPlayer op)
		{
			// float x = op.TPlayer.position.X;
			// float y = op.TPlayer.position.Y;
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				if (!Main.player[i].active)
					continue;

				Player op2 = Main.player[i];
				// float x2 = op2.position.X;
				// float y2 = op2.position.Y;
				// if( x2<=x+60 && x2>=x-60 && y2<=y+35 && y2>=y-35 )
				// {
					// 起跳
					op2.velocity.Y = -6;
					TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", i );
					
					// 烟花
					Firework(TShock.Players[i]);
				// }
			}
		}

		public static void Firework(TSPlayer player)
		{
			// 烟花
			// 随机烟花
			int[] types = {
				ProjectileID.RocketFireworkRed,
				ProjectileID.RocketFireworkGreen,
				ProjectileID.RocketFireworkBlue,
				ProjectileID.RocketFireworkYellow,
				ProjectileID.RocketFireworksBoxRed,
				ProjectileID.RocketFireworksBoxGreen,
				ProjectileID.RocketFireworksBoxBlue,
				ProjectileID.RocketFireworksBoxYellow,
				ProjectileID.RocketFireworkRed
			};
			Random rnd = new Random();
            int index = rnd.Next(types.Length);
			int type = types[index];
			int p = Projectile.NewProjectile(Projectile.GetNoneSource(), player.TPlayer.position.X, player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
			// int p = Projectile.NewProjectile(player.TPlayer.position.X, player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
			Main.projectile[p].Kill();
		}
	}

}