using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using TShockAPI;


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

		// 好运来
		public static void BuffGoodLucky(TSPlayer op, int amount=1)
		{
			var time = 10*60 * amount;
			BuffHelper.SetPlayerBuff(op, BuffID.Lucky, time);
		}

		// 打神鞭
		public static void BuffWhipPlayer(TSPlayer op, int amount=1)
		{
			var time = 10*60 * amount;

			// 311  荆鞭	丛林之怒
			// 312  冷鞭	一种不错的增益
			// 308  迪朗达尔	迪朗达尔的祝福
			// 314  暗黑收割	收割时刻
			List<int> buff = new List<int>() {
				BuffID.ScytheWhipPlayerBuff, 
				BuffID.CoolWhipPlayerBuff, 
				BuffID.SwordWhipPlayerBuff, 
				BuffID.ThornWhipPlayerBuff, 
			};

			foreach (int id in buff)
			{
				BuffHelper.SetPlayerBuff(op, id, time);
			}

		}

		// 逮虾户
		public static void BuffFaster(TSPlayer op, int amount=1)
		{
			var time = 10*60 * amount;

			// 146 快乐
        	//  --28 狼人
        	//  --34 人鱼
        	// 3 敏捷
        	// 207 酒足饭饱
        	// 192 糖果冲刺
        	// 63 恐慌
			List<int> buff = new List<int>() {
				BuffID.Sunflower, 
				// BuffID.Werewolf, 
				BuffID.Swiftness, 
				BuffID.WellFed3, 
				BuffID.SugarRush, 
				BuffID.Panic
			};

			foreach (int id in buff)
			{
				BuffHelper.SetPlayerBuff(op, id, time);
			}
		}

		// 黄金矿工
		public static void BuffMining(TSPlayer op, int amount=1)
		{
			var time = 10*60 * amount;

			// 104 挖矿
			// 107 建筑工
			// 11 光芒
			// 12 夜猫子
        	// 207 酒足饭饱
			// 9 洞穴探险
			// 111 危险感知
			// 17 猎人（狩猎药水）
        	// 192 糖果冲刺
			List<int> buff = new List<int>() {
				BuffID.Mining,
				BuffID.Builder,
				BuffID.Shine,
				BuffID.NightOwl,
				BuffID.WellFed3,
				BuffID.Spelunker,
				BuffID.Dangersense,
				BuffID.Hunter,
				BuffID.SugarRush
			};

			foreach (int id in buff)
			{
				BuffHelper.SetPlayerBuff(op, id, time);
			}
		}

		// 钓鱼佬
		public static void BuffFishing(TSPlayer op, int amount=1)
		{
			var time = 10*60 * amount;

			// 121 钓鱼
			// 122 声呐
			// 123 宝匣
			// 12 夜猫子
			List<int> buff = new List<int>() {
				BuffID.Fishing,
				BuffID.Sonar,
				BuffID.Crate,
				BuffID.NightOwl,
				BuffID.Lucky
			};

			foreach (int id in buff)
			{
				BuffHelper.SetPlayerBuff(op, id, time);
			}
		}

		// 兴奋剂
		public static void BuffIncitant(TSPlayer op, int amount=1)
		{
			var time = 10*60 * amount;

			// 207 酒足饭饱 3级饱食
			// 2 再生药水
			// 3 敏捷
			// 5 铁皮
			// 117 怒气
			// 113 生命力	生命回复
			// 115 暴怒
			// 114 耐力
			List<int> buff = new List<int>() {
				BuffID.WellFed3,
				BuffID.Regeneration,
				BuffID.Swiftness,
				BuffID.Ironskin,
				BuffID.Wrath,
				BuffID.Lifeforce,
				BuffID.Rage,
				BuffID.Endurance
			};

			foreach (int id in buff)
			{
				BuffHelper.SetPlayerBuff(op, id, time);
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
					op2.velocity.Y = -8;
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
			// int p = Projectile.NewProjectile(Projectile.GetNoneSource(), player.TPlayer.position.X, player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
			int p = Projectile.NewProjectile(player.TPlayer.position.X, player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
			Main.projectile[p].Kill();
		}
	}

}