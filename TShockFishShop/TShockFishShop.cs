﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using Terraria;
using TShockAPI.Hooks;
using TerrariaApi.Server;
using TShockAPI;
using Terraria.Localization;


namespace Plugin
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Name => "FishShop";
        public override string Description => "鱼店";
        public override string Author => "hufang360";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;


        public static readonly string PermissionFinish = "fishshop.finish";
        public static readonly string PermissionChange = "fishshop.change";
        public static readonly string PermissionChangeSuper = "fishshop.changesuper";

        public static readonly string savedir = Path.Combine(TShock.SavePath, "FishShop");

        // 配置文件
        public static Config _config;

        private static bool _configIsLoading = false;

        public Plugin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(new List<string>() {}, FishShop, "fishshop", "fish", "fs") { HelpText = "鱼店"});
            GeneralHooks.ReloadEvent += OnReload;

            if( !Directory.Exists(savedir) )
                Directory.CreateDirectory(savedir);

            MyUtils.init();
        }
        private void OnReload(ReloadEventArgs args)
        {
            args.Player.SendSuccessMessage("[fishshop]鱼店配置已重载");
            LoadConfig(true);
        }

        private void FishShop(CommandArgs args)
        {
            void ShowHelpText()
            {
                args.Player.SendInfoMessage("/fish list，查看货架");
                args.Player.SendInfoMessage("/fish ask <编号>，问价格");
                args.Player.SendInfoMessage("/fish buy <编号>，购买");
                args.Player.SendInfoMessage("/fish info，显示钓鱼信息");

                if( args.Player.HasPermission(PermissionFinish) )
                    args.Player.SendInfoMessage("/fish finish <次数>，修改自己的渔夫任务完成次数");
                if( args.Player.HasPermission(PermissionChange) )
                    args.Player.SendInfoMessage("/fish change，更换今天的任务鱼");
                if( args.Player.HasPermission(PermissionChangeSuper) )
                    args.Player.SendInfoMessage("/fish changesuper <物品id|物品名>，指定今天的任务鱼");
            }

            if (args.Parameters.Count<string>() == 0)
            {
                args.Player.SendErrorMessage("语法错误，使用 /fish help 查询用法");
                return;
            }

            switch (args.Parameters[0].ToLowerInvariant())
            {
                // 帮助
                case "help":
                case "h":
                    ShowHelpText();
                    return;

                default:
                    args.Player.SendErrorMessage("请输入 /fish help 查询用法");
                    break;

                // 浏览
                case "list":
                case "l":
                    ListGoods(args);
                    break;

                // 询价
                case "ask":
                case "a":
                    AskGoods(args);
                    break;

                // 购买
                case "buy":
                case "b":
                    BuyGoods(args);
                    break;

                // // 查询
                // case "search":
                // case "s":
                //     SearchGoods(args);
                //     break;


                // 钓鱼信息
                case "info":
                case "i":
                    FishHelper.FishInfo(args.Player);
                    break;


                // 修改钓鱼次数
                case "finish":
                case "f":
                    if ( !args.Player.RealPlayer ){
                        args.Player.SendErrorMessage("此指令需要在游戏内才能执行！");
                        break;
                    }
                    if( !args.Player.HasPermission(PermissionFinish) ){
                        args.Player.SendErrorMessage("你无权更改钓鱼次数！");
                        break;
                    }
                    if( args.Parameters.Count <2 ){
                        args.Player.SendErrorMessage("需要输入完成次数，例如: /fish finish 10");
                        break;
                    }
                    int finished = 0;
                    if ( int.TryParse(args.Parameters[1], out finished) ){
                        args.Player.TPlayer.anglerQuestsFinished = finished;
                        NetMessage.SendData(76, args.Player.Index, -1, NetworkText.Empty, args.Player.Index);
                        NetMessage.SendData(76, -1, -1, NetworkText.Empty, args.Player.Index);
                        args.Player.SendSuccessMessage($"你的渔夫任务完成次数已改成 {finished} 次");
                    } else {
                        args.Player.SendErrorMessage("次数输入错误，例如: /fish finish 10");
                    }
                    break;


                // 切换钓鱼任务
                case "change":
                case "swap":
                case "next":
                case "pass":
                    if( !args.Player.HasPermission(PermissionChange) )
                        args.Player.SendErrorMessage("你无权切换钓鱼任务！");
                    else
                        FishHelper.AnglerQuestSwap(args.Player);
                    break;


                // 指定今天的任务鱼
                case "changesuper":
                case "cs":
                    if( !args.Player.HasPermission(PermissionChangeSuper) ){
                        args.Player.SendErrorMessage("你无权指定今天的任务鱼！");
                    } else {
                        if( args.Parameters.Count<2 ){
                            args.Player.SendErrorMessage("需输入任务鱼的 名称/物品id！，例如: /fish cs 向导巫毒鱼");
                            break;
                        }
                        FishHelper.FishQuestSwap( args.Player, args.Parameters[1]  );
                    }
                    break;
            }
        }


        // 查看商店
        private void ListGoods(CommandArgs args)
        {
            LoadConfig();

            // 商店是否解锁
            string msg = "";
            string s = "";
            foreach( ItemData d in _config.unlock )
            {
                if( !UnlockID.CheckUnlock(d, args.Player, out s) )
                    msg += " " + s;
            }
            if( msg!="" ){
                if( args.Player!=null )
                    args.Player.SendInfoMessage( $"【{_config.name}】已打烊，因为: {msg}" );
                else
                   Log.info( $"【{_config.name}】已打烊，因为: {msg}" );
                return;
            }

            // 更新货架
            List<ShopItem> founds = UpdateShelf(args.Player);

            float num = (float)founds.Count/_config.pageSlots;
            int totalPage = (int)Math.Ceiling( num );

            int pageNum = 1;
            if( args.Parameters.Count>1 )
                int.TryParse(args.Parameters[1], out pageNum);
            if( pageNum>totalPage )
                pageNum = totalPage;
            else if ( pageNum<=0 )
                pageNum = 1;

            int totalSlots = _config.pageSlots*pageNum;


            // 显示指定页的内容
            int rowCount = 0;
            int pageCount = 0;
            int totalCount = 0;
            int startSlot = _config.pageSlots*(pageNum-1);
            for(int i =0; i<founds.Count; i++)
            {
                if( i<startSlot )
                    continue;

                rowCount ++;
                pageCount ++;

                ShopItem item = founds[i];
                msg += $"{i+1}.{item.GetItemDesc()}  ";

                totalCount = i+1;
                if( i>=(totalSlots-1) ){
                    break;
                }

                if( rowCount==_config.rowSlots )
                {
                    rowCount = 0;
                    msg += "\n";
                }
            }

            if( founds.Count>(totalCount) )
                msg += $"\n输入 /fish list {pageNum+1} 查看更多.";

            if ( msg=="" )
                msg = "今天卖萌，不卖货！ɜː";
            else
                msg = $"欢迎光临【{_config.name}】,货架 ({pageNum}/{totalPage}): \n" + msg;
            if( args.Player!=null )
                args.Player.SendInfoMessage(msg);
            else
                Log.info(msg);
        }

        // 加载配置
        private void LoadConfig(bool forceLoad=false)
        {
            if( !_configIsLoading || forceLoad )
            {
                _config = Config.Load(Path.Combine(savedir,"config.json"));

                foreach (ItemData d in _config.unlock )
                {
                    d.fixIDByName();
                }

                foreach (ShopItem item in _config.shop)
                {
                    item.filling();
                    foreach (ItemData d in item.cost )
                    {
                        d.fixIDByName();
                    }
                    foreach (ItemData d in item.unlock )
                    {
                        d.fixIDByName();
                    }
                }
                _configIsLoading = true;
            }
        }

        /// 更新货架
        private List<ShopItem> UpdateShelf(TSPlayer player)
        {
            // 筛选出已解锁的物品
            // string s="";
            // bool passed = true;
            // List<ShopItem> founds = new List<ShopItem>();
            // foreach(ShopItem item in _config.shop)
            // {
            //     passed = true;
            //     foreach( ItemData d in item.unlock )
            //     {
            //         passed = UnlockHelper.CheckUnlock(d, player, out s);
            //     }
            //     if( passed )
            //         founds.Add(item);
            // }
            // return founds;
            return _config.shop;
        }


        // 询价
        private void AskGoods(CommandArgs args)
        {
            if( args.Parameters.Count<2 ){
                args.Player.SendErrorMessage("需要输入商品编号，例如: /fish ask 1");
                return;
            }

            LoadConfig();

            String itemNameOrId = args.Parameters[1];
            List<ShopItem> founds = UpdateShelf(args.Player);
            List<ShopItem> goods = new List<ShopItem>();

            int goodsSerial = 0;
            if( int.TryParse(itemNameOrId, out goodsSerial) )
            {
                // 编号有效性
                int count = founds.Count;
                if( goodsSerial<=0 || goodsSerial>count ){
                    args.Player.SendErrorMessage($"最大编号为: {count}，请使用 /fish list 查看货架.");
                    return;
                }

                goods.Add(founds[goodsSerial-1]);
            } else {
                 // 通过名字 匹配编号
                int customID = IDSet.GetIDByName( itemNameOrId );
                if( customID!=0 )
                {
                    goods = FindGoods(customID);
                } else {
                    List<Item> matchedItems = TShock.Utils.GetItemByIdOrName(itemNameOrId);
                    if (matchedItems.Count == 0)
                    {
                        args.Player.SendErrorMessage($"物品名/物品id: {itemNameOrId} 不正确");
                        return;
                    }
                    Log.info( matchedItems.Count.ToString() );
                    foreach (Item item in matchedItems)
                    {
                        List<ShopItem> finds = FindGoods(item.netID);
                        foreach( ShopItem sitem in finds ){
                            goods.Add( sitem );
                        }
                    }
                }
            }

            foreach (ShopItem shopItem in goods)
            {
                string shopDesc = shopItem.GetItemDesc();
                string costDesc = shopItem.GetCostDesc();
                string unlockDesc = shopItem.GetUnlockDesc();
                string s = $"{shopDesc} = {costDesc}";
                if( unlockDesc!="" )
                    s += $" | 需 {unlockDesc}";
                args.Player.SendInfoMessage( s );
            }

            if( goods.Count==0 ){
                args.Player.SendErrorMessage( $"没卖过 {itemNameOrId}!" );
            } else{
                if( args.Player.RealPlayer )
                    args.Player.SendInfoMessage($"你的余额: {GetCoinsCountDesc(args.Player)}");
            }
        }


        // 购买
        private void BuyGoods(CommandArgs args)
        {
            if ( !args.Player.RealPlayer ){
                args.Player.SendErrorMessage("此指令需要在游戏内执行！");
                return;
            }
            if( !args.Player.InventorySlotAvailable ){
                args.Player.SendErrorMessage("背包已满，不能购买！");
                return;
            }
            if( args.Parameters.Count<2 ){
                args.Player.SendErrorMessage("需输入 物品名 / 商品编号，例如: /fish buy 1，/fish buy 生命水晶");
                return;
            }

            // 更新货架
            LoadConfig();
            List<ShopItem> goods = UpdateShelf(args.Player);

            int goodsSerial = 1;
            if( int.TryParse(args.Parameters[1], out goodsSerial) )
            {
                // 编号有效性
                int count = goods.Count;
                if( goodsSerial<=0 || goodsSerial>count ){
                    args.Player.SendErrorMessage($"最大编号为: {count}，请输入 /fish list 查看货架");
                    return;
                }
            } else {
                // 通过名字 匹配编号
                goodsSerial = 0;
                int goodsID = IDSet.GetIDByName(args.Parameters[1]);
                if( goodsID!=0 )
                {
                    for (int i=0; i<goods.Count; i++)
                    {
                        if(goods[i].id == goodsID){
                            goodsSerial = i+1;
                            break;
                        }
                    }
                }

                if( goodsSerial==0 ){
                    args.Player.SendErrorMessage($"没有名为 {args.Parameters[1]} 的 物品");
                    return;
                }
            }


            int goodsAmount = 1;
            if( args.Parameters.Count>2)
                int.TryParse(args.Parameters[2], out goodsAmount );
            if( goodsAmount<1 )
                goodsAmount = 1;


            // 找到对应商品
            ShopItem shopItem = goods[goodsSerial-1];

            // [日志记录]
            Log.info( String.Format("{0} 要买{1}个 {2}", args.Player.Name, goodsAmount, shopItem.GetItemDesc()) );
            Log.info( $"item: {shopItem.name} {shopItem.id} {shopItem.stack} {shopItem.prefix}" );
            foreach (ItemData _d in shopItem.unlock)
                Log.info( $"unlock: {_d.name} {_d.id} {_d.stack}" );
            foreach (ItemData _d in shopItem.cost)
                Log.info( $"cost: {_d.name} {_d.id} {_d.stack}" );


            string msg = "";
            string s = "";
            foreach( ItemData d in shopItem.unlock )
            {
                if( !UnlockID.CheckUnlock(d, args.Player, out s) )
                    msg += " " + s;
            }
            if( msg!="" ){
                args.Player.SendInfoMessage( $"暂时不能购买，因为: {msg}" );
                return;
            }

            // 部分物品单次至多买一件
            if( !shopItem.CanBuyManyItem() )
                goodsAmount = 1;

            if( shopItem.id>0 ){
                // 检查物品堆叠上线
                Item itemnet = new Item();
                itemnet.SetDefaults(shopItem.id);
                if( shopItem.stack * goodsAmount > itemnet.maxStack ){
                    float num = itemnet.maxStack / shopItem.stack;
                    goodsAmount = (int)Math.Floor(num);
                    if( goodsAmount==0 ){
                        args.Player.SendErrorMessage($"[鱼店]此商品的堆叠数量配置错误,name={shopItem.name},id={shopItem.id},stack={shopItem.stack}");
                        return;
                    }
                }
            }

            // 询价
            msg = "";
            if( CheckCost(args.Player, shopItem, goodsAmount, out msg) )
            {
                // 检查扣除
                DeductCost(args.Player, shopItem, goodsAmount);
                // 提供商品/服务
                ProvideGoods(args.Player, shopItem, goodsAmount);

                s = "";
                if( args.Player.Group.Name=="builder" || args.Player.Group.Name=="architect" ){
                    float discountMoney = shopItem.GetCostMoney(goodsAmount) * 0.1f;
                    int discountMoneyInt = (int)Math.Ceiling( discountMoney );
                    string s2 = MyUtils.GetMoneyDesc(discountMoneyInt);
                    s =$"（你是建筑师，享1折优惠，钱币只收 {s2}）";
                }

                msg = $"你购买了 {goodsAmount}件 {shopItem.GetItemDesc()} | 花费: {shopItem.GetCostDesc(goodsAmount)}{s} | 余额: {GetCoinsCountDesc(args.Player)}";
                args.Player.SendSuccessMessage( msg  );
                Log.info( $"{args.Player.Name} 买了 {shopItem.GetItemDesc()}" );
            } else {
                args.Player.SendInfoMessage($"没买成功，因为: {msg}，请输入 /fish ask {goodsSerial} 查询购买条件");
            }

        }

        // 获取余额
        private int GetCoinsCount(TSPlayer player)
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
        private string GetCoinsCountDesc(TSPlayer player)
        {
            int total = GetCoinsCount(player);
            return MyUtils.GetMoneyDesc( total );
        }

        // 检查钱是否足够
        private bool CheckCost(TSPlayer player, ShopItem shopItem, int amount, out string msg)
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
        private void DeductCost(TSPlayer player, ShopItem shopItem, int amount=1)
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


        private bool DeductMoney(TSPlayer player, int price)
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

            // 找出货币的格子索引（减扣后）
            b1 = 0;
            b2 = 0;
            b3 = 0;
            b4 = 0;
            for (int i = 0; i < 260; i++)
            {
                if(indexs.Contains(i))
                    continue;

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

            // 刷新背包和储蓄罐
            for (int i =0; i<indexs.Count; i++)
            {
                MyUtils.updatePlayerSlot(player, items[i], indexs[i]);
            }
            return success;
        }

        // 提供商品/服务
        private void ProvideGoods(TSPlayer player, ShopItem shopItem, int amount=1)
        {
            int id = shopItem.id;
            if( id < -24 ){
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
                    case ShopItemID.MoonphaseNext:
                            FishHelper.ChangeMoonPhaseByID(player, id, amount);
                            return;

                    case ShopItemID.Firework: CmdHelper.Firework(player); return;
                    case ShopItemID.FireworkRocket: CmdHelper.FireworkRocket(player); return;
                    case ShopItemID.AnglerQuestSwap: FishHelper.AnglerQuestSwap(player); return;

                    // 白天 晚上
                    case ShopItemID.TimeToDay: CmdHelper.SwitchTime(player, "day"); return;
                    case ShopItemID.TimeToNight: CmdHelper.SwitchTime(player, "night"); return;

                    // 雨
                    case ShopItemID.RainingStart: CmdHelper.ToggleRaining(player, true); return;
                    case ShopItemID.RainingStop: CmdHelper.ToggleRaining(player, false); return;

                    // 血月
                    case ShopItemID.BloodMoonStart: CmdHelper.ToggleBloodMoon(player, true); return;
                    case ShopItemID.BloodMoonStop: CmdHelper.ToggleBloodMoon(player, false); return;

                    // 跳过入侵
                    case ShopItemID.InvasionStop: CmdHelper.StopInvasion(player); return;

                    // 执行指令
                    case ShopItemID.RawCmd: CmdHelper.ExecuteRawCmd(player, shopItem.prefix); return;

                    // 复活NPC
                    case ShopItemID.ReliveNPC: NPCHelper.ReliveNPC(player); return;

                    // 集合打团
                    case ShopItemID.TPHereAll: CmdHelper.TPHereAll(player); return;


                    default:
                        break;
                }

                // 召唤npc类
                if( id>ShopItemID.SpawnEnd && id<ShopItemID.SpawnStart )
                {
                    int npcID = ShopItemID.SpawnStart-id;
                    CmdHelper.SpawnBoss(player, npcID, amount);
                }

            } else {
                // 下发物品
                player.GiveItem( shopItem.id, shopItem.stack*amount, shopItem.GetPrefixInt() );
            }
        }

        // 查询
        private void SearchGoods(CommandArgs args)
        {
            if( args.Parameters.Count<2 ){
                args.Player.SendErrorMessage("需要输入 物品名/物品id，例如: /fish search 2374");
                return;
            }

            LoadConfig();

            String itemNameOrId = args.Parameters[1];
            List<ShopItem> shopItems = new List<ShopItem>();

            int customID = IDSet.GetIDByName( itemNameOrId );
            if( customID!=0 )
            {
                shopItems = FindGoods(customID);
            } else {
                List<Item> matchedItems = TShock.Utils.GetItemByIdOrName(itemNameOrId);
                if (matchedItems.Count == 0)
                {
                    args.Player.SendErrorMessage($"物品名/物品id: {itemNameOrId} 不正确");
                    return;
                }
                Log.info( matchedItems.Count.ToString() );
                foreach (Item item in matchedItems)
                {
                    List<ShopItem> finds = FindGoods(item.netID);
                    foreach( ShopItem sitem in finds ){
                        shopItems.Add( sitem );
                    }
                }
            }

            foreach (ShopItem shopItem in shopItems)
            {
                string shopDesc = shopItem.GetItemDesc();
                string costDesc = shopItem.GetCostDesc();
                string unlockDesc = shopItem.GetUnlockDesc();
                string s = $"{shopDesc} = {costDesc}";
                if( unlockDesc!="" )
                    s += $" | 需 {unlockDesc})";
                args.Player.SendInfoMessage( s );

                // args.Player.SendInfoMessage( $"{shopItem.GetItemDesc()} = {shopItem.GetCostDesc()} ({shopItem.GetUnlockDesc()})" );
            }

            if( shopItems.Count==0 )
                args.Player.SendErrorMessage( $"没卖过 {itemNameOrId}!" );
        }

        private List<ShopItem> FindGoods(int _id, string _prefix="")
        {
            List<ShopItem> shopItems = new List<ShopItem>();
            foreach (ShopItem data in _config.shop)
            {
                if( data.id != _id )
                    continue;

                if( _prefix=="" ){
                    shopItems.Add(data);
                } else {
                    if(data.prefix==_prefix )
                        shopItems.Add(data);
                }
            }
            return shopItems;
        }


        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
                GeneralHooks.ReloadEvent -= OnReload;
			}
			base.Dispose(disposing);
		}
	}

}