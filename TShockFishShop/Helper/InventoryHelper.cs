using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;


namespace FishShop
{
    public class InventoryHelper
    {
        // 获取余额
        public static int GetCoinsCount(TSPlayer player)
        {
            bool overFlowing;
            long num = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.inventory, 58, 57, 56, 55, 54);
            long num2 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank.item);
            long num3 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank2.item);
            long num4 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank3.item);
            long num5 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank4.item);
            int total = ((int)Terraria.Utils.CoinsCombineStacks(out overFlowing, num, num2, num3, num4, num5));

            return total;
        }

        // 余额描述
        public static string GetCoinsCountDesc(TSPlayer player, bool tagStyle = true)
        {
            int total = GetCoinsCount(player);
            return utils.GetMoneyDesc(total, tagStyle);
        }

        #region 检查钱是否足够
        public static bool CheckCost(TSPlayer player, ShopItem shopItem, int amount, out string msg)
        {
            // 取出要扣除的物品id
            List<ItemData> costItems = shopItem.GetCostItem(amount);

            msg = "";

            // 计算金钱
            int costMoney = shopItem.GetCostMoney(amount);

            // 建筑师购物价格打1折
            if (IsBuilder(player))
            {
                float discountMoney = costMoney * 0.1f;
                costMoney = (int)Math.Ceiling(discountMoney);
            }

            if (GetCoinsCount(player) < costMoney)
            {
                msg = "钱不够";
                return false;
            }


            // 检查 对应的物品以及数量
            Item itemNet;
            ItemData itemData;
            for (int i = 0; i < NetItem.MaxInventory; i++)
            {
                if (i >= NetItem.InventorySlots)
                    break;

                itemNet = player.TPlayer.inventory[i];
                if (itemNet.stack < 1)
                    continue;

                itemData = shopItem.PickCostItem(costItems, itemNet.netID);
                if (itemData.id != 0)
                {
                    if (itemNet.stack >= itemData.stack)
                    {
                        costItems.Remove(itemData);
                    }
                    else
                    {
                        itemData.stack -= itemNet.stack;
                    }
                }

            }
            if (costItems.Count > 0)
            {
                msg = "物品不够";
                return false;
            }

            // 任意物品
            // ……


            return true;
        }
        #endregion


        #region 减扣物品
        /// <summary>
        /// 减扣物品
        /// </summary>
        public static void DeductCost(TSPlayer player, ShopItem shopItem, int amount, out int costMoney, out int costFish)
        {
            // 取出要扣除的物品
            List<ItemData> costItems = shopItem.GetCostItem(amount);

            Item itemNet;
            ItemData itemData;
            costFish = 0;
            for (int i = 0; i < NetItem.MaxInventory; i++)
            {
                if (i >= NetItem.InventorySlots) break;

                itemNet = player.TPlayer.inventory[i];
                if (itemNet.stack < 1) continue;
                if (itemNet.IsACoin) continue;

                itemData = shopItem.PickCostItem(costItems, itemNet.netID);
                if (itemData.id != 0)
                {
                    // 记录鱼减扣
                    bool IsFish = CostID.Fishes.Contains(itemNet.netID);
                    int stack;
                    if (itemNet.stack >= itemData.stack)
                    {
                        stack = itemData.stack;
                        
                        itemNet.stack -= stack;
                        costItems.Remove(itemData);
                        
                        if (IsFish) costFish += stack;
                    }
                    else
                    {
                        stack = itemNet.stack;

                        itemData.stack -= stack;
                        itemNet.stack = 0;
                        
                        if (IsFish) costFish += stack;
                    }
                    utils.PlayerSlot(player, itemNet, i);
                }

            }
            if (costItems.Count > 0)
            {
                utils.Log($"有 {costItems.Count} 个东西减扣失败！");
            }

            // 扣钱
            costMoney = shopItem.GetCostMoney(amount);
            // 建筑师购物价格打1折
            if (IsBuilder(player))
            {
                float discountMoney = costMoney * 0.1f;
                costMoney = (int)Math.Ceiling(discountMoney);
            }

            bool success = DeductMoney(player, costMoney);
            if (!success)
            {
                utils.Log($"金币扣除失败！金额: {costMoney} 铜");
            }

            // 执行指令
            List<string> cmds = shopItem.GetCostCMD();
            for (int i = 0; i < amount; i++)
            {
                foreach (string cmd in cmds)
                {
                    CmdHelper.ExecuteRawCmd(player, cmd);
                }
            }

            // NetMessage.SendData(4, -1, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
            // NetMessage.SendData(4, player.Index, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
            // // RemoveItemOwner
            // NetMessage.SendData(39, player.Index, -1, NetworkText.Empty, 400);
        }

        public static bool IsBuilder(TSPlayer op)
        {
            return op.Group.Name == "builder" || op.Group.Name == "architect";
        }
        #endregion

        #region 减扣金币
        private static bool DeductMoney(TSPlayer player, int price)
        {

            int b1 = 0;
            int b2 = 0;
            int b3 = 0;
            int b4 = 0;
            List<Item> items = new List<Item>();
            List<int> indexs = new List<int>();

            // 找出当前货币的格子索引
            void record(Item _item, int _index)
            {
                if (_item.IsACoin)
                {
                    indexs.Add(_index);
                    items.Add(_item);
                }
            }
            for (int i = 0; i < 260; i++)
            {
                if (i < 54)
                {
                    record(player.TPlayer.inventory[i], i);

                }
                else if (i >= 99 && i < 139)
                {
                    record(player.TPlayer.bank.item[b1], i);
                    b1++;

                }
                else if (i >= 139 && i < 179)
                {
                    record(player.TPlayer.bank2.item[b2], i);
                    b2++;

                }
                else if (i >= 180 && i < 220)
                {
                    record(player.TPlayer.bank3.item[b3], i);
                    b3++;

                }
                else if (i >= 220 && i < 260)
                {
                    record(player.TPlayer.bank4.item[b4], i);
                    b4++;
                }
            }


            // 购买物品
            bool success = player.TPlayer.BuyItem(price);


            // 找出货币的格子索引（减扣后）
            b1 = 0;
            b2 = 0;
            b3 = 0;
            b4 = 0;
            List<Item> items2 = new List<Item>();
            List<int> indexs2 = new List<int>();

            void record2(Item _item, int _index)
            {
                if (_item.IsACoin)
                {
                    indexs2.Add(_index);
                    items2.Add(_item);
                    if (indexs.Contains(_index))
                    {
                        var newIndex = indexs.IndexOf(_index);
                        indexs.RemoveAt(newIndex);
                        items.RemoveAt(newIndex);
                    }
                }
            }

            for (int i = 0; i < 260; i++)
            {
                if (i < 54)
                {
                    record2(player.TPlayer.inventory[i], i);

                }
                else if (i >= 99 && i < 139)
                {
                    record2(player.TPlayer.bank.item[b1], i);
                    b1++;

                }
                else if (i >= 139 && i < 179)
                {
                    record2(player.TPlayer.bank2.item[b2], i);
                    b2++;

                }
                else if (i >= 180 && i < 220)
                {
                    record2(player.TPlayer.bank3.item[b3], i);
                    b3++;

                }
                else if (i >= 220 && i < 260)
                {
                    record2(player.TPlayer.bank4.item[b4], i);
                    b4++;
                }
            }

            //player.SendInfoMessage( $"购买前：{string.Join(" ",indexs)}" );
            //player.SendInfoMessage( $"购买后：{string.Join(" ",indexs2)}" );
            indexs.AddRange(indexs2);
            items.AddRange(items2);
            //player.SendInfoMessage( $"合并：{string.Join(" ",indexs)}" );

            // 刷新背包和储蓄罐
            for (int i = 0; i < indexs.Count; i++)
            {
                utils.PlayerSlot(player, items[i], indexs[i]);
            }
            return success;
        }
        #endregion
    }

}