using System;
using System.Collections.Generic;
using TShockAPI;


namespace FishShop
{
    public class BuffHelper
    {
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
            if (seconds == -1) return "不限时";
            else if (seconds == 1) return "";
            else if (seconds < 60) return $"{seconds}秒";
            else if (seconds < 60 * 60) return $"{(int)Math.Floor((float)seconds / 60)}分钟";
            else if (seconds < 60 * 60 * 60) return $"{(int)Math.Floor((float)seconds / (60 * 60))}小时";
            else if (seconds < 60 * 60 * 60 * 24) return $"{(int)Math.Floor((float)seconds / (60 * 60 * 24))}天";
            else return "";
        }


        // 给buff
        public static void BuffCommon(TSPlayer op, List<int> buff, List<int> second, int amount = 1)
        {
            for (int i = 0; i < buff.Count; i++)
            {
                int id = buff[i];
                int time = i < second.Count ? second[i] * amount : 60 * amount;
                SetPlayerBuff(op, id, time);
            }
        }

        public static int GetBuffIDByName(string name)
        {
            List<int> ids = TShock.Utils.GetBuffByName(name);
            if (ids.Count > 0) return ids[0];

            return 0;
        }
        public static string GetBuffNameByID(int buffID)
        {
            return TShock.Utils.GetBuffName(buffID);
        }


    }
}