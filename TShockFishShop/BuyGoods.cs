using FishShop.Record;
using FishShop.Shop;
using System;
using Terraria;
using TShockAPI;

namespace FishShop;

public partial class Plugin
{
    // 购买
    static void BuyGoods(CommandArgs args)
    {
        if (!args.Player.RealPlayer)
        {
            args.Player.SendErrorMessage("此指令需要在游戏内执行！");
            return;
        }

        // 商店是否解锁
        if (!ShopIsReady(args.Player))
        {
            return;
        }

        TSPlayer op = args.Player;
        if (args.Parameters.Count < 2)
        {
            op.SendErrorMessage("需输入 物品名 / 商品编号，例如: /fish buy 1，/fish buy 生命水晶");
            return;
        }


        // 找到对应商品
        if (int.TryParse(args.Parameters[1], out int goodsSerial))
        {
            // 编号有效性
            int count = _config.shop.Count;
            if (goodsSerial <= 0 || goodsSerial > count)
            {
                op.SendErrorMessage($"最大编号为: {count}，请输入 /fish list 查看货架");
                return;
            }
        }
        else
        {
            // 通过名字 匹配编号
            int goodsID = IDSet.GetIDByName(args.Parameters[1]);
            if (goodsID != 0)
            {
                for (int i = 0; i < _config.shop.Count; i++)
                {
                    if (_config.shop[i].id == goodsID)
                    {
                        goodsSerial = i + 1;
                        break;
                    }
                }
            }

            if (goodsSerial == 0)
            {
                op.SendErrorMessage($"没有名为 {args.Parameters[1]} 的 物品");
                return;
            }
        }
        ShopItemData shopItemData = _config.shop[goodsSerial - 1];

        // 购买数量/额外参数
        int amount = 1;
        string extra = "";
        if (args.Parameters.Count > 2)
        {
            int.TryParse(args.Parameters[2], out amount);
            extra = args.Parameters[2];
        }
        if (amount < 1)
        {
            amount = 1;
        }
        // 单次至多买一件的物品
        amount = Math.Min(amount, shopItemData.BuyMax());
        if (amount < 1) amount = 1;


        // 创建商店物品（一些逻辑处理）
        ShopItem shopItem = ShopItemCreate.Create(shopItemData);
        shopItem.op = args.Player;
        shopItem.amount = amount;
        shopItem.extra = extra;

        // 检查购买
        string result = shopItem.CanBuy();
        if (result != "")
        {
            op.SendInfoMessage(result);
            return;
        }

        // 检查物品堆叠上线[待优化]
        if (shopItemData.id > 0)
        {
            Item itemNet = new();
            itemNet.SetDefaults(shopItemData.id);
            if (shopItemData.stack * amount > itemNet.maxStack)
            {
                float num = itemNet.maxStack / shopItemData.stack;
                amount = (int)Math.Floor(num);
                if (amount == 0)
                {
                    op.SendErrorMessage($"[鱼店]此商品的堆叠数量配置错误,name={shopItemData.name},id={shopItemData.id},stack={shopItemData.stack}");
                    return;
                }
            }
        }


        // 询价
        string msg = shopItem.CheckCost();
        if (msg=="")
        {
            // 扣钱
            shopItem.DeductCost(out int costMoney, out int costFish);

            // 提供商品/服务
            shopItem.ProvideGoods();

            string s = "";
            if (InventoryHelper.IsBuilder(op))
            {
                s = $"（你是建筑师，享1折优惠，钱币只收 {utils.GetMoneyDesc(costMoney)}）";
            }

            msg = $"你购买了 {amount}件 {shopItemData.GetItemDesc()} | 花费: {shopItemData.GetCostDesc(amount)}{s} | 余额: {InventoryHelper.GetCoinsCountDesc(op)}";
            op.SendSuccessMessage(msg);
            utils.Log($"{op.Name} 买了 {shopItemData.GetItemDesc()}");
            Records.Record(op, shopItemData, amount, costMoney, costFish);
        }
        else
        {
            op.SendInfoMessage($"没买成功，因为: {msg}，请输入 /fish ask {goodsSerial} 查询购买条件");
        }

    }

}