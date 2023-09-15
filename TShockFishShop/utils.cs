using FishShop.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using TShockAPI;


namespace FishShop;

public class utils
{
    public static List<string> moonPhases = new() { "满月", "亏凸月", "下弦月", "残月", "新月", "娥眉月", "上弦月", "盈凸月" };

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
    public static string GetItemDesc(int id = 0, int stack = 1, string prefix = "")
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
            return Prefix.GetPrefix(prefix);
    }

    public static string GetMoneyDesc(long price, bool tagStyle = true)
    {
        List<string> li = new();

        // 铂金币
        float num = price / 1000000;
        int stack = (int)Math.Floor(num);
        if (stack > 0)
        {
            price -= stack * 1000000;

            if (!tagStyle)
            {
                li.Add($"{stack}铂");
            }
            else
            {
                while (stack > 9999)
                {
                    stack -= 9999;
                    li.Add("[i/s{9999}:74]");
                }
                li.Add($"[i/s{stack}:74]");
            }
        }

        // 金币
        num = price / 10000;
        stack = (int)Math.Floor(num);
        if (stack > 0)
        {
            price -= stack * 10000;
            li.Add(tagStyle ? $"[i/s{stack}:73]" : $" {stack}金");
        }

        // 银币
        num = price / 100;
        stack = (int)Math.Floor(num);
        if (stack > 0)
        {
            price -= stack * 100;
            li.Add(tagStyle ? $"[i/s{stack}:72]" : $" {stack}银");
        }

        // 铜币
        if (price > 0)
        {
            li.Add(tagStyle ? $"[i/s{stack}:71]" : $" {stack}铜");
        }

        return string.Join("", li); ;
    }

    public static void GetMoneyStack(long price, out int stack1, out int stack2, out int stack3, out int stack4)
    {
        // 铂金币
        float num = price / 1000000;
        stack4 = (int)Math.Floor(num);

        // 金币
        num = price / 10000;
        stack3 = (int)Math.Floor(num);

        // 银币
        num = price / 100;
        stack2 = (int)Math.Floor(num);

        // 铜币
        stack1 = (int)price;
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
        StreamReader streamReader = new(stream);
        return streamReader.ReadToEnd();
    }


    public static void Log(string msg) { TShock.Log.ConsoleInfo("[fish]" + msg); }
}