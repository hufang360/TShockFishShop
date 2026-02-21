using System.Linq;
using FishShop.Record;
using Terraria;
using TShockAPI;

namespace FishShop.Shop;

public class ShopItem(ShopItemData si)
{
    /// <summary>
    /// 数据项
    /// </summary>
    protected readonly ShopItemData shopItemData = si;

    public TSPlayer op;
    public int amount = 1;
    public string extra = "";

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
        int id = shopItemData.id;
        // 自定义物品
        switch (id)
        {
            // 月相
            case ShopItemID.Moonphase1:
            case ShopItemID.Moonphase2:
            case ShopItemID.Moonphase3:
            case ShopItemID.Moonphase4:
            case ShopItemID.Moonphase5:
            case ShopItemID.Moonphase6:
            case ShopItemID.Moonphase7:
            case ShopItemID.Moonphase8:
            case ShopItemID.MoonphaseNext: FishHelper.ChangeMoonPhaseByID(op, id, amount); return;

            case ShopItemID.Firework: CmdHelper.Firework(op); return;
            case ShopItemID.FireworkRocket: CmdHelper.FireworkRocket(op); return;
            case ShopItemID.AnglerQuestSwap: FishHelper.AnglerQuestSwap(op); return;

            // 白天 中午 晚上 午夜
            case ShopItemID.TimeToDay: CmdHelper.SwitchTime(op, "day"); return;
            case ShopItemID.TimeToNoon: CmdHelper.SwitchTime(op, "noon"); return;
            case ShopItemID.TimeToNight: CmdHelper.SwitchTime(op, "night"); return;
            case ShopItemID.TimeToMidNight: CmdHelper.SwitchTime(op, "midnight"); return;

            // =====
            // buff 类
            // ====
            // 好运来、打神鞭、逮虾户、黄金矿工、钓鱼佬、兴奋剂
            case ShopItemID.BuffGoodLucky:
            case ShopItemID.BuffWhipPlayer:
            case ShopItemID.BuffFaster:
            case ShopItemID.BuffMining:
            case ShopItemID.BuffFishing:
            case ShopItemID.BuffIncitant:
            case ShopItemID.Buff:
                BuffHelper.BuffCommon(op, shopItemData.GetBuff(), shopItemData.GetBuffSecond(), amount);
                return;


            // 雨来、雨停
            case ShopItemID.RainingStart: CmdHelper.ToggleRaining(op, true); return;
            case ShopItemID.RainingStop: CmdHelper.ToggleRaining(op, false); return;

            // 血月
            case ShopItemID.BloodMoonStart: CmdHelper.ToggleBloodMoon(op, true); return;
            case ShopItemID.BloodMoonStop: CmdHelper.ToggleBloodMoon(op, false); return;


            // 雨、史莱姆雨、 沙尘暴、大风天、雷雨、血月、日食、派对、陨石、流星雨、灯笼夜
            case ShopItemID.ToggleRain: CmdHelper.ToggleRaining(op, true, true); return;
            case ShopItemID.ToggleSlimeRain: CmdHelper.ToggleSlimeRain(op, true, true); return;
            case ShopItemID.ToggleSandStorm: CmdHelper.ToggleSandstorm(op, true, true); return;
            case ShopItemID.ToggleWindyDay: CmdHelper.ToggleWindyDay(op, true, true); return;
            case ShopItemID.ToggleStorming: CmdHelper.ToggleStorming(op, true, true); return;


            case ShopItemID.ToggleBloodMoon: CmdHelper.ToggleBloodMoon(op, true, true); return;
            case ShopItemID.ToggleEclipse: CmdHelper.ToggleEclipse(op, true, true); return;


            case ShopItemID.ToggleParty: CmdHelper.ToggleParty(op, true, true); return;
            case ShopItemID.TriggerDropMeteor: CmdHelper.DropMeteor(op); return;
            case ShopItemID.StarfallStart: CmdHelper.Starfall(op); return;
            case ShopItemID.LanternsNightStart: CmdHelper.LanternsNightStart(op); return;

            case ShopItemID.OverworldDayStart: CmdHelper.OverworldDay(op); return;


            // 入侵
            case ShopItemID.InvasionStop: CmdHelper.StopInvasion(op); return;
            case ShopItemID.InvasionGoblins:
            case ShopItemID.InvasionSnowmen:
            case ShopItemID.InvasionPirates:
            case ShopItemID.InvasionPumpkinmoon:
            case ShopItemID.InvasionFrostmoon:
            case ShopItemID.InvasionMartians: CmdHelper.StartInvasion(op, id); return;

            // 执行指令
            case ShopItemID.RawCmd:
                if (shopItemData.cmds.Count > 0)
                {
                    foreach (string cmd in shopItemData.cmds)
                    {
                        CmdHelper.ExecuteRawCmd(op, cmd);
                    }
                }
                else if (!string.IsNullOrEmpty(shopItemData.prefix)) CmdHelper.ExecuteRawCmd(op, shopItemData.prefix);
                return;

            case ShopItemID.TPHereAll: CmdHelper.TPHereAll(op); return;                    // 集合打团
            case ShopItemID.CelebrateAll: CmdHelper.CelebrateAll(op); return;              // 集体庆祝

            default: break;
        }

        // 召唤NPC类
        int id2 = ShopItemID.GetRealSpawnID(id);
        if (id2 != 0)
        {
            NPCHelper.SpawnNPC(op, id2, amount);
            return;
        }

        // 清除NPC类
        id2 = ShopItemID.GetRealClearNPCID(id);
        if (id2 != 0)
        {
            NPCHelper.ClearNPC(op, id2, amount);
            return;
        }

        // 获得buff类
        id2 = ShopItemID.GetRealBuffID(id);
        if (id2 != 0)
        {
            BuffHelper.SetPlayerBuff(op, id2, shopItemData.stack * amount);
            return;
        }

        // 玩家自定义商品
        // 指令清单 和 buff 清单
        if (id <= -600 && id >= -999)
        {
            foreach (string s in shopItemData.GetCMD().Where(s => !string.IsNullOrEmpty(s)))
            {
                CmdHelper.ExecuteRawCmd(op, s);
            }

            BuffHelper.BuffCommon(op, shopItemData.GetBuff(), shopItemData.GetBuffSecond(), amount);
        }
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