namespace FishShop.Shop;

public class ShopItemCreate
{
    /// <summary>
    /// 创建对象 根据id分配对应对象
    /// </summary>
    public static ShopItem Create(ShopItemData data)
    {
        switch (data.id)
        {
            case ShopItemID.Forge: return new ForgeItem(data);
            case ShopItemID.DirtiestBlock: return new DirtiestItem(data);
        }

        if (data.id > 0)
        {
            return new NormalItem(data);
        }
        else
        {
            return new ShopItem(data);
        }

    }
}