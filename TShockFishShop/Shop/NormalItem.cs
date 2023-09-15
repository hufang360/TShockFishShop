using Terraria;
using TShockAPI;

namespace FishShop.Shop;

public class NormalItem : ShopItem
{
    public NormalItem(ShopItemData si) : base(si)
    {
    }

    public override string CanBuy()
    {
        var msg = base.CanBuy();
        if (msg != "") return msg;

        // 坠落之星白天买会消失
        if (shopItemData.id == 75 && Main.dayTime)
        {
            return "坠落之星 只能在晚上购买！";
        }

        if (!op.InventorySlotAvailable)
        {
            return "背包已满，不能购买！";
        }

        return "";
    }

    public override void ProvideGoods()
    {
        // 下发物品
        op.GiveItem(shopItemData.id, shopItemData.stack * amount, shopItemData.GetPrefixInt());
    }


}