namespace FishShop
{
    /// <summary>
    /// 鱼店ID集合
    /// 说明：商品ID、解锁ID、交易物品ID 共用一套id规则，部分id即使解锁ID又是商品ID。
    /// </summary>
    public class IDSet
    {
        public static string GetNameByID(int id, string prefix = "", int stack = 1)
        {
            // 交易物品
            string s = CostID.GetNameByType(id, stack);
            if (!string.IsNullOrEmpty(s)) return s;

            // 解锁条件
            s = UnlockID.GetNameByType(id, stack);
            if (!string.IsNullOrEmpty(s)) return s;

            // 商品名称
            s = ShopItemID.GetNameByID(id, prefix, stack);
            if (!string.IsNullOrEmpty(s)) return s;

            return "";
        }

        // 修复id
        public static int GetIDByName(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                return 0;

            // 交易物品名称
            int id = CostID.GetTypeByName(name);
            if (id != 0) return id;

            // 解锁条件
            id = UnlockID.GetTypeByName(name);
            if (id != 0) return id;

            // 商品名称
            id = ShopItemID.GetIDByName(name);
            if (id != 0) return id;

            return 0;
        }
    }
}