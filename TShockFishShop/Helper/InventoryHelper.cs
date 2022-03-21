using System;
using Terraria;
using TShockAPI;
using System.Linq;
using System.Collections.Generic;


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
        public static string GetCoinsCountDesc(TSPlayer player)
        {
            int total = GetCoinsCount(player);
            return MyUtils.GetMoneyDesc( total );
        }

        // 检查钱是否足够
        public static bool CheckCost(TSPlayer player, ShopItem shopItem, int amount, out string msg)
        {
            // 取出要扣除的物品id
            List<ItemData> costItems = shopItem.GetCostItem(amount);

            msg = "";

            // 计算金钱
            int costMoney = shopItem.GetCostMoney(amount);

            // 建筑师购物价格打1折
            if( player.Group.Name == "builder" || player.Group.Name=="architect" ){
                float discountMoney = costMoney * 0.1f;
                costMoney = (int)Math.Ceiling(  discountMoney );
            }

			if (GetCoinsCount(player) < costMoney)
			{
                msg = "钱不够";
                return false;
            }


            // 检查 对应的物品以及数量
            Item itemNet;
            ItemData itemData;
            for (int i=0; i<NetItem.MaxInventory; i++)
            {
                if( i>=NetItem.InventorySlots )
                    break;

                itemNet = player.TPlayer.inventory[i];
                if( itemNet.stack<1 )
                    continue;

                itemData = shopItem.GetOneCostItem(costItems, itemNet.netID);
                if( itemData.id !=0 )
                {
                    if( itemNet.stack>=itemData.stack ){
                        costItems.Remove(itemData);
                    } else {
                        itemData.stack -= itemNet.stack;
                    }
                }

            }
            if( costItems.Count>0 ){
                msg = "物品不够";
                return false;
            }

            // 任意物品
            // ……


            return true;
        }

        // 减扣物品
        public static void DeductCost(TSPlayer player, ShopItem shopItem, int amount=1)
        {
            // 取出要扣除的物品
            List<ItemData> costItems = shopItem.GetCostItem(amount);

            Item itemNet;
            ItemData itemData;
            for (int i=0; i<NetItem.MaxInventory; i++)
            {
                if( i>=NetItem.InventorySlots )
                    break;

                itemNet = player.TPlayer.inventory[i];
                if( itemNet.stack<1 )
                    continue;

                if(itemNet.IsACoin)
                    continue;

                itemData = shopItem.GetOneCostItem(costItems, itemNet.netID);
                if( itemData.id !=0 )
                {
                    if( itemNet.stack>=itemData.stack ){
                        itemNet.stack -= itemData.stack;
                        costItems.Remove(itemData);
                    } else {
                        itemNet.stack = 0;
                        itemData.stack -= itemNet.stack;
                    }
                    MyUtils.updatePlayerSlot(player, itemNet, i);
                }

            }
            if( costItems.Count>0 ){
                Log.info($"有 {costItems.Count} 个东西减扣失败！");
            }

            // 扣钱
            int costMoney = shopItem.GetCostMoney(amount);
            // 建筑师购物价格打1折
            if( player.Group.Name == "builder" || player.Group.Name=="architect" ){
                float discountMoney = costMoney * 0.1f;
                costMoney = (int)Math.Ceiling(  discountMoney );
            }

            bool success = DeductMoney(player, costMoney);
            if( !success ){
                Log.info($"金币扣除失败！金额: {costMoney} 铜");
            }

            // NetMessage.SendData(4, -1, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
            // NetMessage.SendData(4, player.Index, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
            // // RemoveItemOwner
            // NetMessage.SendData(39, player.Index, -1, NetworkText.Empty, 400);
        }


        private static bool DeductMoney(TSPlayer player, int price)
        {
            // 找出当前货币的格子索引
            int b1 = 0;
            int b2 = 0;
            int b3 = 0;
            int b4 = 0;
            Item item;
            List <Item> items = new List<Item>();
            List<int> indexs = new List<int>();
            for (int i = 0; i < 260; i++)
            {
                if (i < 54){
                    item =player.TPlayer.inventory[i];
                    if( item.IsACoin )
                    {
                        indexs.Add(i);
                        items.Add(item);
                    }

                } else if (i >= 99 && i < 139){
                    item =player.TPlayer.bank.item[b1];
                    if( item.IsACoin )
                    {
                        indexs.Add(i);
                        items.Add(item);
                    }
                    b1++;

                } else if (i >= 139 && i < 179){
                    item =player.TPlayer.bank2.item[b2];
                    if( item.IsACoin )
                    {
                        indexs.Add(i);
                        items.Add(item);
                    }
                    b2++;

                } else if (i >= 180 && i < 220){
                    item =player.TPlayer.bank3.item[b3];
                    if( item.IsACoin )
                    {
                        indexs.Add(i);
                        items.Add(item);
                    }
                    b3++;

                } else if (i >= 220 && i < 260){
                    item =player.TPlayer.bank4.item[b4];
                    if( item.IsACoin )
                    {
                        indexs.Add(i);
                        items.Add(item);
                    }
                    b4++;
                }
            }

            // 购买物品
            bool success = player.TPlayer.BuyItem(price);
            // player.SendInfoMessage( $"前：{String.Join(" ",indexs)}" );

            // 找出货币的格子索引（减扣后）
            b1 = 0;
            b2 = 0;
            b3 = 0;
            b4 = 0;
            List <Item> items2 = new List<Item>();
            List<int> indexs2 = new List<int>();
            for (int i = 0; i < 260; i++)
            {
                if(indexs.Contains(i))
                {
                    var newIndex = indexs.IndexOf(i);
                    indexs.RemoveAt(newIndex);
                    items.RemoveAt(newIndex);
                }

                if (i < 54){
                    item =player.TPlayer.inventory[i];
                    if( item.IsACoin )
                    {
                        indexs2.Add(i);
                        items2.Add(item);
                    }

                } else if (i >= 99 && i < 139){
                    item =player.TPlayer.bank.item[b1];
                    if( item.IsACoin )
                    {
                        indexs2.Add(i);
                        items2.Add(item);
                    }
                    b1++;

                } else if (i >= 139 && i < 179){
                    item =player.TPlayer.bank2.item[b2];
                    if( item.IsACoin )
                    {
                        indexs2.Add(i);
                        items2.Add(item);
                    }
                    b2++;

                } else if (i >= 180 && i < 220){
                    item =player.TPlayer.bank3.item[b3];
                    if( item.IsACoin )
                    {
                        indexs2.Add(i);
                        items2.Add(item);
                    }
                    b3++;

                } else if (i >= 220 && i < 260){
                    item =player.TPlayer.bank4.item[b4];
                    if( item.IsACoin )
                    {
                        indexs2.Add(i);
                        items2.Add(item);
                    }
                    b4++;
                }
            }
            // player.SendInfoMessage( $"后：{String.Join(" ",indexs2)}" );

            indexs.AddRange( indexs2 );
            items.AddRange( items2 );
            // player.SendInfoMessage( $"合并：{String.Join(" ",indexs)}" );

            // 刷新背包和储蓄罐
            for (int i =0; i<indexs.Count; i++)
            {
                MyUtils.updatePlayerSlot(player, items[i], indexs[i]);
            }
            return success;
        }
    }
}