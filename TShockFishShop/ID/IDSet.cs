namespace FishShop
{
    public class IDSet
    {
        public static string GetNameByID( int id, string prefix="", int stack=1)
        {
            string s = "";

            // 解锁条件
            s = UnlockID.GetNameByID(id);
            if( !string.IsNullOrEmpty(s) )
                return s;

            // 商品名称
            s = ShopItemID.GetNameByID(id, prefix, stack);
            if( !string.IsNullOrEmpty(s) )
                return s;

            return "";
        }

        // 修复id
        public static int GetIDByName(string name="")
        {
            if( string.IsNullOrEmpty(name) )
                return 0;
            
            // 解锁条件
            int id = 0;
            id = UnlockID.GetIDByName(name);
            if( id!=0 )
                return id;

            // 商品名称
           id = ShopItemID.GetIDByName(name);
            if( id!=0 )
                return id;

            return 0;
        }
    }
}