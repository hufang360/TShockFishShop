using System;
using TShockAPI;


namespace Plugin
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



    }
}