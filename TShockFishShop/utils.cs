using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TShockAPI;


namespace FishShop
{
    public class utils
    {
        public static List<string> moonPhases = new List<string>() { "满月", "亏凸月", "下弦月", "残月", "新月", "娥眉月", "上弦月", "盈凸月" };

        public static void init()
        {
            // 支持第一分形
            // 清空弃用物品清单
            Array.Clear(ItemID.Sets.Deprecated, 0, ItemID.Sets.Deprecated.Length - 1);
        }
        // public static string GetItemDescByShopItem(ShopItem item)
        // {
        //     return GetItemDesc(item.name, item.id, item.stack, item.prefix);
        // }

        /// <summary>
        /// 获得物品描述
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stack"></param>
        /// <param name="prefix"></param>
        /// <param name="shopItem"></param>
        /// <returns></returns>
        public static string GetItemDesc(int id = 0, int stack = 1, string prefix = "", ShopItem shopItem = null)
        {
            if (id == 0)
                return "";

            string s = "";

            // https://terraria.fandom.com/wiki/Chat
            // [i:29]   数量
            // [i/s10:29]   数量
            // [i/p57:4]    词缀

            // -24~5124 为泰拉原版的物品id
            // <-24 为本插件自定义id
            if (id < -24)
            {
                s = IDSet.GetNameByID(id, prefix, stack);
            }
            else
            {
                if (stack > 1)
                {
                    s = $"[i/s{stack}:{id}]";
                }
                else
                {
                    if (int.TryParse(prefix, out int num) && num != 0)
                        s = $"[i/p{GetPrefixInt(prefix)}:{id}]";
                    else
                        s = $"[i:{id}]";
                }
            }

            return s;
        }

        private static int GetPrefixInt(string prefix)
        {
            if (int.TryParse(prefix, out int num))
                return num;
            else
                return AffixNameToPrefix(prefix);
        }

        public static string GetMoneyDesc(int price, bool tagStyle = true)
        {
            string msg = "";

            // 铂金币
            float num = price / 1000000;
            int stack = (int)Math.Floor(num);
            if (stack > 0)
            {
                price -= stack * 1000000;
                msg = tagStyle ? $"[i/s{stack}:74]" : $"{stack}铂";
            }

            // 金币
            num = price / 10000;
            stack = (int)Math.Floor(num);
            if (stack > 0)
            {
                price -= stack * 10000;
                msg += tagStyle ? $"[i/s{stack}:73]" : $" {stack}金";
            }

            // 银币
            num = price / 100;
            stack = (int)Math.Floor(num);
            if (stack > 0)
            {
                price -= stack * 100;
                msg += tagStyle ? $"[i/s{stack}:72]" : $" {stack}银";
            }

            // 铜币
            if (price > 0)
            {
                msg += tagStyle ? $"[i/s{price}:71]" : $" {stack}铜币";
            }

            return msg;
        }
        /// <summary>
        /// 数字补零
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string AlignZero(int num)
        {
            if (num < 10) return $"00{num}";
            else if (num < 100) return $"0{num}";
            else return $"{num}";
        }

        #region 词缀
        public static int AffixNameToPrefix(string affixname)
        {
            switch (affixname)
            {
                case "大": return 1;
                case "巨大": return 2;
                case "危险": return 3;
                case "凶残": return 4;
                case "锋利": return 5;
                case "尖锐": return 6;
                case "微小": return 7;
                case "可怕": return 8;
                case "小": return 9;
                case "钝": return 10;
                case "倒霉": return 11;
                case "笨重": return 12;
                case "可耻": return 13;
                case "重": return 14;
                case "轻": case "莱特": return 15;
                case "精准": return 16;
                case "迅速": return 17;
                case "急速": return 18;
                case "恐怖": return 19;
                case "致命": return 20;
                case "可靠": return 21;
                case "可畏": return 22;
                case "无力": return 23;
                case "粗笨": return 24;
                case "强大": return 25;
                case "神秘": return 26;
                case "精巧": return 27;
                case "精湛": return 28;
                case "笨拙": return 29;
                case "无知": return 30;
                case "错乱": return 31;
                case "威猛": return 32;
                case "禁忌": return 33;
                case "天界": return 34;
                case "狂怒": return 35;
                case "锐利": return 36;
                case "高端": return 37;
                case "强力": return 38;
                case "碎裂": return 39;
                case "破损": return 40;
                case "粗劣": return 41;
                case "迅捷": return 42;
                case "致命2": return 43;
                case "灵活": return 44;
                case "灵巧": return 45;
                case "残暴": return 46;
                case "缓慢": return 47;
                case "迟钝": return 48;
                case "呆滞": return 49;
                case "惹恼": return 50;
                case "凶险": return 51;
                case "狂躁": return 52;
                case "致伤": return 53;
                case "强劲": return 54;
                case "粗鲁": return 55;
                case "虚弱": case "软弱": return 56;
                case "无情": return 57;
                case "暴怒": return 58;
                case "神级": return 59;
                case "恶魔": return 60;
                case "狂热": return 61;
                case "坚硬": return 62;
                case "守护": return 63;
                case "装甲": return 64;
                case "护佑": return 65;
                case "奥秘": return 66;
                case "精确": return 67;
                case "幸运": return 68;
                case "锯齿": return 69;
                case "尖刺": return 70;
                case "愤怒": return 71;
                case "险恶": return 72;
                case "轻快": return 73;
                case "快速": return 74;
                case "急速2": return 75;
                case "迅捷2": return 76;
                case "狂野": return 77;
                case "鲁莽": return 78;
                case "勇猛": return 79;
                case "暴力": return 80;
                case "传奇": return 81;
                case "虚幻": return 82;
                case "神话": return 83;
                case "传奇2": return 84; // 泰拉悠悠球变体
            }

            // 纯数字
            if (int.TryParse(affixname, out int num))
            {
                if (num <= 83 && num > 0)
                {
                    return num;
                }
            }

            return 0;
        }
        #endregion


        public static void PlayerSlot(TSPlayer player, Item item, int slotIndex)
        {
            NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, null, player.Index, slotIndex);
            NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, null, player.Index, slotIndex);
            //NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, NetworkText.FromLiteral(item.Name), player.Index, slotIndex, (float)item.prefix);
        }

        public static int GetUnixTimestamp
        {
            get { return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; }
        }

        public static Rectangle GetScreen(TSPlayer op) { return GetScreen(op.TileX, op.TileY); }
        public static Rectangle GetScreen(int playerX, int playerY) { return new Rectangle(playerX - 61, playerY - 34 + 3, 122, 68); }

        public static int InvalidItemID { get { return -24; } }


        /// <summary>
        /// 获取内嵌文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FromEmbeddedPath(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }


        public static void Log(string msg) { TShock.Log.ConsoleInfo("[fish]" + msg); }
    }
}