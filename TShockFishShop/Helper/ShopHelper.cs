using System.Collections.Generic;
using Terraria;


namespace Plugin
{
    public class ShopItem
    {
        public string name = "";
        public int id = 0;
        public int stack = 1;
        public string prefix = "";
        public List<ItemData> unlock = new List<ItemData>();
        public List<ItemData> cost = new List<ItemData>();
        
        public ShopItem(string _name="", int _id = 0, int _stack=1, string _prefixOrName="")
        {
            name = _name;
            id = _id;
            stack = _stack;
            prefix = _prefixOrName;
        }


        public void filling(){
            if( id==0 )
                id = IDSet.GetIDByName(name);

            // // 执行指令
            // if( id==ShopItemID.RawCmd ){
            //     name = string.Format( $"指令{prefix}");
            // } else {
            //     prefix = MyUtils.AffixNameToPrefix(prefix).ToString();
            // }
            if( id!=ShopItemID.RawCmd )
                prefix = MyUtils.AffixNameToPrefix(prefix).ToString();
        }

        public void AddUnlock(string _name="", int _id = 0, int _stack=1 )
        {
            unlock.Add(new ItemData(_name, _id, _stack));
        }

        public void AddCost(string _name="", int _id = 0, int _stack=1 )
        {
            cost.Add(new ItemData(_name, _id, _stack));
        }

        public string GetItemDesc()
        {
            return MyUtils.GetItemDesc( name, id, stack, prefix );
        }

        public bool CanBuyManyItem()
        {
            return ShopItemID.CanBuyManyItem(id);
        }

        // 汇总要减扣的物品
        // 货币不计算在内
        // 任意类的物品不计算在内
        public List<ItemData> GetCostItem(int amount)
        {
            // 取出要扣除的物品id
            List<ItemData> _items = new List<ItemData>();
            foreach (ItemData d in cost)
            {
                int _stack = d.stack * amount;
                int _id = d.id;
                if( _id == 0 || _id>Main.maxItemTypes){
                    Log.info($"[CheckCost]物品id{_id}无效");
                    continue;
                }

                // 跳过 铜、银、金、铂金
                if( _id == 71 ||  _id==72 || _id==73 || _id==74  )
                    continue;

                // 任务鱼
                if( _id==UnlockID.ItemIDQuestFish ){
                    _id = Main.anglerQuestItemNetIDs[Main.anglerQuest];
                }

                _items.Add( new ItemData("", _id, _stack) );
            }
            return _items;
        }

        public string GetAnyItemDesc()
        {
            // 取出要扣除的物品id
            List<ItemData> _costItems = new List<ItemData>();
            string msg = "";
            foreach (ItemData data in cost)
            {
                if( data.id == 0 ){
                    switch (data.name)
                    {
                        // 任务鱼可以确定一个id
                        // 其它物品可能得到多个id
                        case "任务鱼":
                            _costItems.Add( new ItemData("", Main.anglerQuestItemNetIDs[Main.anglerQuest], 1) );
                            break;

                        case "宝匣":
                        case "任意宝匣":
                            msg += "任意宝匣[i:2334]";
                            break;

                        case "金色小动物":
                        case "任意金色小动物":
                            msg += "任意金色小动物[i:2889]";
                            break;

                        case "任意墓碑":
                            msg += "任意墓碑[i:321]";
                            break;
                    }
                }
            }
            return msg;
        }


        public ItemData GetOneCostItem(List<ItemData> _data, int _id)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if(_data[i].id == _id)
                    return _data[i];
            }
            return new ItemData();
        }

        // 计算金钱消耗
        public int GetCostMoney(int goodsAmount)
        {
            int amount = 0;
            foreach (ItemData data in cost)
            {
                if( data.id==0 )
                {
                    switch (data.name)
                    {
                        case "铜币":
                        case "铜":
                            id = 71;
                            break;

                        case "银币":
                        case "银":
                            id = 72;
                            break;

                        case "金币":
                        case "金":
                            id = 73;
                            break;

                        case "铂金币":
                        case "铂金":
                        case "铂":
                            id = 74;
                            break;
                    }
                }
                switch (data.id)
                {
                    case 71:
                        amount += data.stack;
                        break;
                    case 72:
                        amount += data.stack*100;
                        break;
                    case 73:
                        amount += data.stack*10000;
                        break;
                    case 74:
                        amount += data.stack*1000000;
                        break;
                }
            }
            return amount*goodsAmount;
        }

        public int GetPrefixInt()
        {
            int num = 0;
            if ( int.TryParse( prefix, out num ) )
                return num;
            else
                return MyUtils.AffixNameToPrefix(prefix);
        }


        public string GetCostDesc(int amount=1)
        {
            // 钱
            string msg = MyUtils.GetMoneyDesc(GetCostMoney(amount));

            // 物品
            List<ItemData> costItems = GetCostItem(amount);
            List<string> msgs = new List<string>();
            if( !string.IsNullOrEmpty(msg) )
                msgs.Add(msg);

            foreach (ItemData _d in costItems){
                msgs.Add( _d.GetItemDesc() );
            }
            msg = string.Join("", msgs);

            // 任意类型的物品
            // string s = GetAnyItemDesc();
            // if( s!="" )
            //     msg += $"{s} ";

            return msg;
        }

        public string GetUnlockDesc()
        {
            string msg = "";
            string s = "";
            foreach (ItemData d in unlock){
                if( msg!="" )
                    s = "、";
                msg += $"{s}{ GetOneUnlockDesc(d) }";
            }
            return msg;
        }

        // 显示解锁条件
        private string GetOneUnlockDesc(ItemData re)
        {
            string s = IDSet.GetNameByID( re.id );

            // 完成钓鱼任务
            if( re.id==UnlockID.FishQuestCompleted )
                s = string.Format( s, re.stack );

            return s;
        }

    }
    

    public class ShopItem2
    {
        public int serial = 0;

        public ShopItem item;
    }

    // public class Shelf
    // {
    //     private List<ShelfData> _datas = new List<ShelfData>();

    //     //获得指定玩家的货架
    //     public ShelfData GetOne( int index)
    //     {
    //         foreach( ShelfData d in _datas){
    //             if(d.playerIndex == index)
    //                 return d;
    //         }
    //         return new ShelfData();
    //     }

    //     //添加一个玩家的货架
    //     // 控制台执行时，传过来的 playerindex 是 -1
    //     public void AddOne( int playerIndex, ShelfData playerData)
    //     {
    //         int index = -1;
    //         for( int i=0; i<_datas.Count; i++)
    //         {
    //             if(_datas[i].playerIndex == playerIndex){
    //                 index = i;
    //                 break;
    //             }
    //         }
    //         if(index==-1)
    //             _datas.Add( playerData );
    //         else
    //             _datas[index] = playerData;
    //     }

    // }

    // public class ShelfData
    // {
    //     public int playerIndex=-1;
    //     public string playerName = "";
    //     public List<ShopItem> goods = new List<ShopItem>();

    //     public ShelfData(int _index=-1, string _name=""){
    //         playerIndex = _index;
    //         playerName = _name;
    //     }
    // }

}