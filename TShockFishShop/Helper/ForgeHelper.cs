using System.Collections.Generic;
using System.Linq;
using FishShop.Helper;
using Terraria;
using TShockAPI;

namespace FishShop;

public class ForgeHelper
{
    public static void Manage(CommandArgs args)
    {
        args.Parameters.RemoveAt(0);
        if (!args.Player.RealPlayer)
        {
            args.Player.SendErrorMessage("此指令需要在游戏内执行！");
            return;
        }

        List<string> msgs = new();
        Item forgeItem = args.Player.TPlayer.inventory[0];
        int id = forgeItem.netID;
        if (id == 0)
        {
            msgs.Add("需将要重铸的物品，放在背包的第1格！");
        }
        else if (!Prefix.CanHavePrefixes(forgeItem))
        {
            msgs.Add($"[i:{id}]不能重铸");
        }

        byte targetPrefix = 0;
        if (args.Parameters.Count == 0)
        {
            msgs.Add("需指定词缀，例如：/fish forge 虚幻");
        }
        else
        {
            targetPrefix = (byte)Prefix.GetPrefix(args.Parameters[0]);
            if (targetPrefix == forgeItem.prefix)
            {
                args.Player.SendInfoMessage("现在的词缀就是，不需要再重铸了！");
                return;
            }
            else if (targetPrefix == 0)
            {
                msgs.Add("词缀输入错误！可用数字 1~84 代替中文名称。");
            }
        }
        var npc = NPCHelper.FindNearNPC(args.Player, 107);
        if (npc == null)
        {
            msgs.Add("需要在哥布林工匠附近！");
        }
        if (msgs.Count > 0)
        {
            args.Player.SendInfoMessage(string.Join("\n", msgs));
            return;
        }

        // 扣钱
        long ownedCoins = InventoryHelper.GetCoinsCount(args.Player);
        int needCoins = ForgeCost(forgeItem, args.Player.TPlayer, npc) * 10;
        if (ownedCoins < needCoins)
        {
            args.Player.SendInfoMessage($"钱不够！本次重铸的预算是{utils.GetMoneyDesc(needCoins)}");
            return;
        }
        InventoryHelper.DeductMoney(args.Player, needCoins);


        Item item = new();
        item.SetDefaults(id);
        item.prefix = forgeItem.prefix;

        List<byte> history = new();
        long totalCoins = 0;
        for (int i = 0; i < 10; i++)
        {
            totalCoins += ForgeCost(item, args.Player.TPlayer, npc);
            item.ResetPrefix();
            item.Prefix(-2);
            history.Add(item.prefix);
        }

        var last = targetPrefix;
        var coinsTips = $"花费: {utils.GetMoneyDesc(needCoins)} | 余额: {InventoryHelper.GetCoinsCountDesc(args.Player)}";
        var li = history.Select(p => Prefix.GetName(p));
        var hisStr = " | " + string.Join(" ", li);
        if (history.Contains(targetPrefix))
        {
            var luckyNum = history.IndexOf(targetPrefix) + 1;
            args.Player.SendSuccessMessage($"重铸成功: [i/p{targetPrefix}:{id}] | {coinsTips}{hisStr}");
        }
        else
        {
            last = history.Last();
            var tips = "";
            if (!byte.TryParse(args.Parameters[0], out byte tempPrefix))
            {
                tips = $" | 再次重铸可输入 /fish forge {targetPrefix}";
            }
            args.Player.SendInfoMessage($"重铸完成: [i/p{last}:{id}] | {coinsTips}{tips}{hisStr}");
        }


        // 更新物品词缀
        //forgeItem.Prefix(last);
        forgeItem.prefix = last;
        utils.PlayerSlot(args.Player, forgeItem, 0);

        args.Player.SendData(PacketTypes.PlayerSlot, "", args.Player.Index, 0, last);

        // 归还
        var remain = needCoins - totalCoins;
        if (remain > 0)
        {
            InventoryHelper.Refund(args.Player, remain);
            args.Player.SendInfoMessage($"预算结余，返还 {utils.GetMoneyDesc(remain)}");
        }
        args.Player.SendInfoMessage($"{needCoins}   {totalCoins}");
    }

    static int ForgeCost(Item item, Player plr, NPC npc)
    {
        int coins = item.value;
        if (plr.discountAvailable)
        {
            coins = (int)(coins * 0.8);
        }
        var settings = Main.ShopHelper.GetShoppingSettings(plr, npc);
        coins = (int)(coins * settings.PriceAdjustment);
        coins /= 3;

        return coins;
    }


}