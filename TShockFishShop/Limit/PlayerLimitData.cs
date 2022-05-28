using System.Collections.Generic;
using System.ComponentModel;

namespace FishShop
{
    public class PlayerLimitData
    {
        [DefaultValue("")]
        public string name = "";
        public List<LimitData> datas = new List<LimitData>();

        public PlayerLimitData(string playerName)
        {
            name = playerName;
        }
    }
}