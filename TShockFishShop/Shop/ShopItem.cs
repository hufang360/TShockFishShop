using FishShop.Record;
using Terraria;
using TShockAPI;

namespace FishShop.Shop;

public class ShopItem
{
    /// <summary>
    /// 数据项
    /// </summary>
    protected readonly ShopItemData shopItemData;

    public TSPlayer op;
    public int amount = 1;
    public string extra = "";


    public ShopItem(ShopItemData si)
    {
        shopItemData = si;
    }

    /// <summary>
    /// 能否购买
    /// </summary>
    virtual public string CanBuy()
    {
        // 通用检查
        // 解锁条件
        string msg = "";
        foreach (ItemData d in shopItemData.unlock)
        {
            if (!UnlockID.CheckUnlock(d, op, out string s))
            {
                msg += " " + s;
            }
        }

        // 用户组
        bool groupPass = op.HasPermission(Permissions.AllowGroup) || shopItemData.allowGroup.Count == 0;
        if (!groupPass) groupPass = shopItemData.allowGroup.Contains(op.Group.Name);
        if (!groupPass)
        {
            msg += $" 你不是 {string.Join("、", shopItemData.allowGroup)} 用户组的玩家";
        }

        if (msg != "")
        {
            return $"暂时不能购买，因为: {msg}";
        }


        int id = shopItemData.id;
        // 限购
        /// <summary>
        /// 限购检查
        /// </summary>
        bool CheckLimit()
        {
            if (string.IsNullOrEmpty(op.Name))
                return true;

            var limit = shopItemData.limit;
            var serverLimit = shopItemData.serverLimit;

            if (limit > 0 && Records.GetPlayerRecord(op, id) >= limit)
                return false;

            if (serverLimit > 0 && Records.CountShopItemRecord(id) >= serverLimit)
                return false;

            return true;
        }

        if (!CheckLimit())
        {
            return "噢噢，这件商品太抢手了，已经卖完了";
        }

        // 时间
        if (Main.dayTime)
        {
            if (!shopItemData.DayCanBuyItem())
            {
                return "只能在晚上购买！";
            }
        }
        else
        {
            if (!shopItemData.NightCanBuyItem())
            {
                return "只能在白天购买！";
            }
        }

        // buff类死亡时不能买
        if (op.Dead && !shopItemData.DeadCanBuyItem())
        {
            return "请待复活后再购买此物品！";
        }

        // 检查是否需要购买
        //public static bool CanBuy(TSPlayer op, ShopItem shopItem, int amount = 1)
        //{
        //    int id = shopItem.id;
        //    if (id >= -24) return true;
        //    switch (id)
        //    {
        //        case Moonphase1:
        //        case Moonphase2:
        //        case Moonphase3:
        //        case Moonphase4:
        //        case Moonphase5:
        //        case Moonphase6:
        //        case Moonphase7:
        //        case Moonphase8:
        //        case MoonphaseNext: return FishHelper.NeedBuyChangeMoonPhase(op, id, amount);


        //        case InvasionGoblins:
        //        case InvasionSnowmen:
        //        case InvasionPirates:
        //        case InvasionPumpkinmoon:
        //        case InvasionFrostmoon:
        //        case InvasionMartians: return CmdHelper.NeedBuyStartInvasion(op);

        //        case InvasionStop: return CmdHelper.NeedBuyStopInvasion(op);
        //        case ReliveNPC: return NPCHelper.NeedBuyReliveNPC(op);

        //        case BloodMoonStart: if (Main.bloodMoon) { op.SendInfoMessage("正处在血月，无需购买"); return false; } break;
        //        case BloodMoonStop: if (!Main.bloodMoon) { op.SendInfoMessage("没发生血月，无需购买"); return false; } break;

        //        case RainingStart: if (Main.raining) { op.SendInfoMessage("正在下雨，无需购买"); return false; }; break;
        //        case RainingStop: if (!Main.raining) { op.SendInfoMessage("没在下雨，无需购买"); return false; }; break;
        //    }

        //    return true;
        //}

        return "";
    }

    /// <summary>
    /// 提供商品/服务
    /// </summary>
    virtual public void ProvideGoods()
    {
    }


    /// <summary>
    /// 检查钱是否足够
    /// </summary>
    virtual public string CheckCost()
    {
        InventoryHelper.CheckCost(op, shopItemData, amount, out string msg);
        return msg;
    }

    /// <summary>
    /// 扣钱
    /// </summary>
    virtual public void DeductCost(out int costMoney, out int costFish)
    {
        // 扣除
        InventoryHelper.DeductCost(op, shopItemData, amount, out costMoney, out costFish);
    }


    /// <summary>
    /// 购买调试日志
    /// </summary>
    public void BuyDebug(TSPlayer op, int amount = 1)
    {
        var d = shopItemData;
        // [日志记录]
        utils.Log(string.Format("{0} 要买{1}个 {2}", op.Name, amount, d.GetItemDesc()));
        utils.Log($"item: {d.name} {d.id} {d.stack} {d.prefix}");
        foreach (ItemData _d in d.unlock)
        {
            utils.Log($"unlock: {_d.name} {_d.id} {_d.stack}");
        }
        foreach (ItemData _d in d.cost)
        {
            utils.Log($"cost: {_d.name} {_d.id} {_d.stack}");
        }
        utils.Log($"余额: {InventoryHelper.GetCoinsCountDesc(op, false)}");
    }
}