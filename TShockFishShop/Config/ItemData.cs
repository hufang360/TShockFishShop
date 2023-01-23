using Newtonsoft.Json;

namespace FishShop
{
    /// <summary>
    /// 鱼店物品数据（解锁条件、交易物品）
    /// </summary>
    public class ItemData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name = "";

        /// <summary>
        /// id
        /// </summary>
        public int id = 0;

        /// <summary>
        /// 堆叠数（有时也表示数量）
        /// </summary>
        public int stack = 1;

        /// <summary>
        /// 要执行的指令
        /// </summary>
        public string cmd = "";


        /// <summary>
        /// 是否为解锁条件
        /// </summary>
        [JsonIgnore]
        public bool isUnlock = false;

        public ItemData(string _name = "", int _id = 0, int _stack = 1)
        {
            name = _name;
            id = _id;
            stack = _stack;
        }

        /// <summary>
        /// 通过名字补齐id
        /// </summary>
        public void FixIDByName(bool _isUnlock)
        {
            isUnlock = _isUnlock;
            if (id == 0 && name != "")
            {
                if (isUnlock)
                    id = UnlockID.GetTypeByName(name);
                else
                    id = CostID.GetTypeByName(name);

                if (id == 0)
                    id = ShopItemID.GetIDByName(name);

                if (id == 0)
                    utils.Log($"ItemData fixID 错误");
            }
            if (stack == 0)
                stack = 1;
        }

        /// <summary>
        /// 获得物品描述
        /// </summary>
        public string GetItemDesc()
        {
            if (id == 0)
                FixIDByName(isUnlock);

            // <-24 为本插件自定义id
            if (id < -24)
            {
                if (isUnlock)
                    return UnlockID.GetNameByType(id, stack);
                else
                    return CostID.GetNameByType(id, stack);
            }

            return utils.GetItemDesc(id, stack, cmd);
        }
    }
}