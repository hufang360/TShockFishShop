namespace FishShop
{
    public class IDSet
    {
        public static string GetNameByID(int id, string prefix = "", int stack = 1)
        {
            // 商品名称
            string s = ShopItemID.GetNameByID(id, prefix, stack);
            if (!string.IsNullOrEmpty(s))
                return s;

            // 解锁条件
            s = UnlockID.GetNameByID(id);
            if (!string.IsNullOrEmpty(s))
                return s;

            return "";
        }

        // 修复id
        public static int GetIDByName(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                return 0;

            // 商品名称
            int id = ShopItemID.GetIDByName(name);
            if (id != 0)
                return id;

            // 解锁条件
            id = UnlockID.GetIDByName(name);
            if (id != 0)
                return id;

            return 0;
        }
    }
}