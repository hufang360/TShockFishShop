using System;
using System.Collections.Generic;
using Terraria.ID;
using TShockAPI;


namespace FishShop
{
    public class BuffHelper
    {
        // TShock.Utils.GetBuffName(buffID);
        // TShock.Utils.GetBuffDescription(id);

        public static void SetPlayerBuff(TSPlayer op, int id, int time)
        {
            // Max possible buff duration as of Terraria 1.4.2.3 is 35791393 seconds (415 days).
			var timeLimit = (int.MaxValue / 60) - 1;
            if (time < 0 || time > timeLimit)
                time = timeLimit;

			op.SetBuff(id, time * 60);
			op.SendSuccessMessage($"你获得 [c/96FF96:{TShock.Utils.GetBuffName(id)}] ({TShock.Utils.GetBuffDescription(id)}) {GetTimeDesc(time)}");
        }

        public static string GetTimeDesc(int seconds)
        {
            if( seconds==-1 ){
                return "不限时";
            } else if ( seconds==1 ){
                return "";

            } else if ( seconds<60 ){
                return $"{seconds}秒";

            } else if ( seconds<60*60 ){
                int m = (int)Math.Floor( (float) seconds/60);
                return $"{m}分钟";

            } else if ( seconds<60*60*60 ){
                int h = (int)Math.Floor( (float) seconds/(60*60));
                return $"{h}小时";

            } else if ( seconds<60*60*60*24 ){
                int d = (int)Math.Floor( (float) seconds/(60*60*24));
                return $"{d}天";

            } else {
                return "";
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
			// -312  冷鞭	一种不错的增益
			// 308  迪朗达尔	迪朗达尔的祝福
			// 314  暗黑收割	收割时刻
			List<int> buff = new List<int>() {
				BuffID.ScytheWhipPlayerBuff, 
				// BuffID.CoolWhipPlayerBuff, 
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



    }
}