using FishShop.Helper;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace FishShop.Shop;

public class ForgeItem : ShopItem
{
    public ForgeItem(ShopItemData si) : base(si)
    {
    }

    // 能否购买
    public override string CanBuy()
    {
        var msg = base.CanBuy();
        if (msg != "") return msg;

        Item forgeItem = op.TPlayer.inventory[0];
        int id = forgeItem.type;
        if (id == 0) return "需将要重铸的物品，放在背包的第1格！";
        if (!Prefix.CanHavePrefixes(forgeItem)) return $"[i:{id}]不能重铸";

        if (extra == "")
        {
            return "需在购买指令后面加上词缀名称，例如：/fish buy 重铸 虚幻";
        }
        else
        {
            byte targetPrefix = (byte)Prefix.GetPrefix(extra);
            if (targetPrefix == forgeItem.prefix)
            {
                return "现在的词缀就是，不需要再重铸了！";
            }
            else if (targetPrefix == 0)
            {
                return "词缀输入错误！可用数字 1~84 代替中文名称。";
            }
        }

        var npc = NPCHelper.FindNearNPC(op, 107);
        if (npc == null) return "需要在哥布林工匠附近！";

        long ownedCoins = InventoryHelper.GetCoinsCount(op);
        int needCoins = ForgeCost(forgeItem, op.TPlayer, npc) * 10;
        if (ownedCoins < needCoins)
        {
            return $"钱不够！本次重铸的预算是{utils.GetMoneyDesc(needCoins)}";
        }

        return "";
    }

    public override void DeductCost(out int costMoney, out int costFish)
    {
        var npc = NPCHelper.FindNearNPC(op, 107);
        Item forgeItem = op.TPlayer.inventory[0];
        int needCoins = ForgeCost(forgeItem, op.TPlayer, npc) * 10;
        costMoney = needCoins;
        costFish = 0;
        InventoryHelper.DeductMoney(op, needCoins);
    }

    public override void ProvideGoods()
    {
        Item forgeItem = op.TPlayer.inventory[0];
        int id = forgeItem.type;

        Item item = new();
        item.SetDefaults(id);
        item.prefix = forgeItem.prefix;

        var npc = NPCHelper.FindNearNPC(op, 107);

        List<byte> history = new();
        long totalCoins = 0;
        for (int i = 0; i < 10; i++)
        {
            totalCoins += ForgeCost(item, op.TPlayer, npc);
            item.ResetPrefix();
            item.Prefix(-2);
            history.Add(item.prefix);
        }

        byte targetPrefix = (byte)Prefix.GetPrefix(extra);
        var last = targetPrefix;

        int needCoins = ForgeCost(forgeItem, op.TPlayer, npc) * 10;
        var coinsTips = $"花费: {utils.GetMoneyDesc(needCoins)} | 余额: {InventoryHelper.GetCoinsCountDesc(op)}";
        var li = history.Select(p => Prefix.GetName(p));
        var hisStr = " | " + string.Join(" ", li);
        if (history.Contains(targetPrefix))
        {
            var luckyNum = history.IndexOf(targetPrefix) + 1;
            op.SendSuccessMessage($"重铸成功: [i/p{targetPrefix}:{id}] | {coinsTips}{hisStr}");
        }
        else
        {
            last = history.Last();
            var tips = "";
            if (!byte.TryParse(extra, out byte tempPrefix))
            {
                tips = $" | 再次重铸可输入 /fish forge {targetPrefix}";
            }
            op.SendInfoMessage($"重铸完成: [i/p{last}:{id}] | {coinsTips}{tips}{hisStr}");
        }


        // 更新物品词缀
        //forgeItem.Prefix(last);
        forgeItem.prefix = last;
        utils.PlayerSlot(op, forgeItem, 0);

        op.SendData(PacketTypes.PlayerSlot, "", op.Index, 0, last);

        // 归还
        var remain = needCoins - totalCoins;
        if (remain > 0)
        {
            InventoryHelper.Refund(op, remain);
            op.SendInfoMessage($"预算结余，返还 {utils.GetMoneyDesc(remain)}");
        }
        // op.SendInfoMessage($"{needCoins}   {totalCoins}");
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