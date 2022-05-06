using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using TShockAPI;


namespace FishShop
{
    public class CmdHelper
    {

        // 调时间
        public static void SwitchTime(TSPlayer player, string type = "noon")
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
            if (on)
            {
                if (!Main.raining) //Toggle rain on
                {
                    Main.StartRain();
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} 让苍天下起了雨.", player.Name);
                    return;
                }
                else
                {
                    player.SendInfoMessage("已经在下雨了~");
                }
            }
            else
            {
                if (Main.raining) //Toggle rain off
                {
                    Main.StopRain();
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} 让雨停了下来.", player.Name);
                    return;
                }
                else
                {
                    player.SendInfoMessage("没在下雨");
                }
            }
        }


        #region 入侵
        // 跳过入侵
        public static void StopInvasion(TSPlayer player)
        {
            if (Main.invasionSize > 0)
            {
                Main.invasionSize = 0;
                TSPlayer.All.SendInfoMessage("{0} 跳过了入侵事件", player.Name);
            }
            else
            {
                player.SendInfoMessage("当前没有任何入侵事件");
            }
        }
        public static bool NeedBuyStopInvasion(TSPlayer player)
        {
            if(Main.invasionSize == 0)
            {
                player.SendInfoMessage("当前没有任何入侵事件，无需购买");
                return false;
            }
            return true;
        }

        // 召唤入侵
        public static void StartInvasion(TSPlayer op, int shopID)
        {
            if (Main.invasionSize <= 0)
            {
                int wave = 1;
                switch (shopID)
                {
                    case ShopItemID.InvasionGoblins:
                        TSPlayer.All.SendInfoMessage("{0} 召唤了 哥布林军队", op.Name);
                        StartInvasion(1);
                        break;

                    case ShopItemID.InvasionSnowmen:
                        TSPlayer.All.SendInfoMessage("{0} 召唤了 雪人军团", op.Name);
                        StartInvasion(2);
                        break;

                    case ShopItemID.InvasionPirates:
                        TSPlayer.All.SendInfoMessage("{0} 召唤了 海盗入侵", op.Name);
                        StartInvasion(3);
                        break;

                    case ShopItemID.InvasionPumpkinmoon:
                        TSPlayer.Server.SetPumpkinMoon(true);
                        Main.bloodMoon = false;
                        NPC.waveKills = 0f;
                        NPC.waveNumber = wave;
                        TSPlayer.All.SendInfoMessage("{0} 召唤了 南瓜月", op.Name);
                        break;

                    case ShopItemID.InvasionFrostmoon:
                        TSPlayer.Server.SetFrostMoon(true);
                        Main.bloodMoon = false;
                        NPC.waveKills = 0f;
                        NPC.waveNumber = wave;
                        TSPlayer.All.SendInfoMessage("{0} 召唤了 霜月", op.Name);
                        break;

                    case ShopItemID.InvasionMartians:
                        TSPlayer.All.SendInfoMessage("{0} 召唤了 火星暴乱", op.Name);
                        StartInvasion(4);
                        break;
                }
            }
            else if (DD2Event.Ongoing)
            {
                //DD2Event.StopInvasion();
                //TSPlayer.All.SendInfoMessage("{0} 终止了 撒旦军队", op.Name);
                op.SendInfoMessage("已有撒旦军队在进行.");
            }
            else
            {
                op.SendInfoMessage("已有入侵在进行.");
            }
        }


        private static void StartInvasion(int type)
        {
            int invasionSize = 0;

            if (TShock.Config.Settings.InfiniteInvasion)
            {
                invasionSize = 20000000;
            }
            else
            {
                invasionSize = 100 + (TShock.Config.Settings.InvasionMultiplier * TShock.Utils.GetActivePlayerCount());
            }

            Main.StartInvasion(type);

            Main.invasionSizeStart = invasionSize;
            Main.invasionSize = invasionSize;
        }


        public static bool NeedBuyStartInvasion(TSPlayer op)
        {
            if (Main.invasionSize <= 0)
            {
            }
            else if (DD2Event.Ongoing)
            {
                op.SendInfoMessage("已有撒旦军队在进行.");
                return false;
            }
            else
            {
                op.SendInfoMessage("已有入侵在进行.");
                return false;
            }
            return true;
        }
        #endregion


        // 执行指令
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

            if (on)
            {
                if (!Main.bloodMoon)
                {
                    TSPlayer.Server.SetBloodMoon(on);
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} 召唤了 血月.", player.Name);
                    return;
                }
                else
                {
                    player.SendInfoMessage("已经是血月期间了.");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    TSPlayer.Server.SetBloodMoon(on);
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} 跳过了血月.", player.Name);
                    return;
                }
                else
                {
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

        public static void Jump(TSPlayer op)
        {
            float x = op.TPlayer.position.X;
            float y = op.TPlayer.position.Y;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                // if( i==op.Index )
                // 	continue;
                if (!Main.player[i].active)
                    continue;

                Player op2 = Main.player[i];
                float x2 = op2.position.X;
                float y2 = op2.position.Y;
                if (x2 <= x + 970 && x2 >= x - 970 && y2 <= y + 530 && y2 >= y - 530)
                {
                    // 起跳
                    op2.velocity.Y = -8;
                    TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", i);
                    // 烟花
                    Firework(TShock.Players[i]);
                }
            }
        }

        public static void Firework(TSPlayer op)
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
            int p = Projectile.NewProjectile(Projectile.GetNoneSource(), op.TPlayer.position.X, op.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
            //int p = Projectile.NewProjectile(op.TPlayer.position.X, op.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
            Main.projectile[p].Kill();
        }
    }

}