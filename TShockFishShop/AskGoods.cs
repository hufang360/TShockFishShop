using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;

namespace FishShop;

public partial class Plugin
{

    // 询价
    static void AskGoods(CommandArgs args)
    {
        // 商店是否解锁
        if (!ShopIsReady(args.Player))
        {
            return;
        }

        if (args.Parameters.Count < 2)
        {
            args.Player.SendErrorMessage("需要输入商品编号，例如: /fish ask 1");
            return;
        }

        TSPlayer op = args.Player;
        List<ShopItemData> FindGoods(int _id, string _prefix = "")
        {
            return _config.shop.Where(obj => obj.id == _id && (_prefix == "" || obj.prefix == _prefix)).ToList();
        }

        string itemNameOrId = args.Parameters[1];
        List<ShopItemData> goods = new();

        if (int.TryParse(itemNameOrId, out int goodsSerial))
        {
            // 编号有效性
            int count = _config.shop.Count;
            if (goodsSerial <= 0 || goodsSerial > count)
            {
                op.SendErrorMessage($"最大编号为: {count}，请输入 /fish list 查看货架.");
                return;
            }
            goods.Add(_config.shop[goodsSerial - 1]);
        }
        else
        {
            // 通过名字 匹配编号
            int customID = IDSet.GetIDByName(itemNameOrId);
            if (customID != 0)
            {
                goods = FindGoods(customID);
            }
            else
            {
                List<Item> matchedItems = TShock.Utils.GetItemByIdOrName(itemNameOrId);
                if (matchedItems.Count == 0)
                {
                    op.SendErrorMessage($"物品名/物品id: {itemNameOrId} 不正确");
                    return;
                }
                utils.Log(matchedItems.Count.ToString());
                foreach (Item item in matchedItems)
                {
                    goods.AddRange(FindGoods(item.netID));
                }
            }
        }


        /// <summary>
        /// 商品描述
        /// </summary>
        string Detail(ShopItemData d)
        {
            bool RealPlayer = op != null && op.RealPlayer;

            string iconDesc = d.GetIcon();
            string shopDesc = d.GetItemDesc();
            string costDesc = d.GetCostDesc();
            string s = $"{d.serial}.{iconDesc}{shopDesc} = {costDesc}";

            string unlockDesc = d.GetUnlockDesc();
            if (unlockDesc != "")
            {
                s += $"\n解锁条件：需 {unlockDesc}";
            }

            string comment = d.GetComment();
            if (comment != "")
            {
                s += $"\n商品备注：{comment}";
            }

            if (RealPlayer)
            {
                string limitDesc = d.GetLimitDesc(op);
                if (limitDesc != "")
                {
                    s += $"\n商品限购：{limitDesc}";
                }
            }

            string allowGroupDesc = d.GetAllowGroupDesc();
            if (allowGroupDesc != "")
            {
                s += $"\n用户组限制：{allowGroupDesc}";
            }

            if (RealPlayer)
            {
                string remainDesc = InventoryHelper.GetCoinsCountDesc(op);
                if (string.IsNullOrEmpty(remainDesc))
                {
                    s += $"\n你的余额：{remainDesc}";
                }
            }
            return s;
        }

        foreach (ShopItemData shopItemData in goods)
        {
            var s = Detail(shopItemData);
            if (s != "")
                op.SendInfoMessage(s);
        }

        if (goods.Count == 0)
        {
            op.SendErrorMessage($"没卖过 {itemNameOrId}!");
        }
    }

}