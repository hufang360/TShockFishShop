using System.Collections.Generic;
using System.ComponentModel;

namespace FishShop
{
    public class SetShopData
    {
        // 商品名称
        [DefaultValue("")]
        public string name = "";

        // 图标
        public int icon = 0;

        // 商品id
        public int id = 0;

        // 单次最大购买数
        public int buyMax = 999;

        [DefaultValue("")]
        public string comment = "";

        // 仅活着
        public bool needAlive = false;

        // 仅夜晚
        public bool needNight = false;

        // 仅白天
        public bool needDay = false;

        // 命令
        public List<string> cmds = new List<string>();

        // buff 清单
        public List<int> buffs = new List<int>();

        // buff 持续时间
        public List<int> seconds = new List<int>();

        //public void Init()
        //{
        //        name = "好运来",
        //        id = ShopItemID.BuffGoodLucky,
        //        comment = "获得10分钟 幸运 buff",
        //        buyMax = 1,
        //        needAlive = true
        //}
    }
}
