using System.Collections.Generic;
using System.ComponentModel;

namespace FishShop
{
    public class PlayerLimitData
    {
        [DefaultValue("")]
        public string name = "";
        public List<ServerLimitData> datas = new List<ServerLimitData>();

        public PlayerLimitData(string playerName)
        {
            name = playerName;
        }
    }
}