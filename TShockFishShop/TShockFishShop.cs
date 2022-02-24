using System;
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
        public static readonly string PermissionReload = "fishshop.reload";
        public static readonly string PermissionSpecial= "fishshop.special";

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
            args.Player.SendInfoMessage("欲重载鱼店配置，请使用 /fish reload");
            // LoadConfig(true);
        }

        private void FishShop(CommandArgs args)
        {
            TSPlayer op = args.Player;
            void ShowHelpText()
            {
                op.SendInfoMessage("/fish list，查看货架");
                op.SendInfoMessage("/fish ask <编号>，问价格");
                op.SendInfoMessage("/fish buy <编号>，购买");
                op.SendInfoMessage("/fish info，显示钓鱼信息");

                if( op.HasPermission(PermissionFinish) )
                    op.SendInfoMessage("/fish finish <次数>，修改自己的渔夫任务完成次数");
                if( op.HasPermission(PermissionChange) )
                    op.SendInfoMessage("/fish change，更换今天的任务鱼");
                if( op.HasPermission(PermissionChangeSuper) )
                    op.SendInfoMessage("/fish changesuper <物品id|物品名>，指定今天的任务鱼");
                if( op.HasPermission(PermissionReload) )
                    op.SendInfoMessage("/fish reload，重载配置");

                if( op.HasPermission(PermissionSpecial) ){
                    op.SendInfoMessage("/fish special, 查看特别指令");
                }
            }

            if (args.Parameters.Count<string>() == 0)
            {
                op.SendErrorMessage("语法错误，使用 /fish help 查询用法");
                return;
            }

            switch (args.Parameters[0].ToLowerInvariant())
            {
                // 帮助
                case "h":
                case "help":
                    ShowHelpText();
                    return;

                default:
                    ListGoods(args);
                    op.SendInfoMessage("请输入 /fish help 查询用法");
                    break;

                // 浏览
                case "l":
                case "list":
                    ListGoods(args);
                    break;

                // 询价
                case "a":
                case "ask":
                    AskGoods(args);
                    break;

                // 购买
                case "b":
                case "buy":
                    BuyGoods(args);
                    break;


                // 钓鱼信息
                case "i":
                case "info":
                    FishHelper.FishInfo(op);
                    break;


                // 修改钓鱼次数
                case "f":
                case "finish":
                    if ( !op.RealPlayer ){
                        op.SendErrorMessage("此指令需要在游戏内才能执行！");
                        break;
                    }
                    if( !op.HasPermission(PermissionFinish) ){
                        op.SendErrorMessage("你无权更改钓鱼次数！");
                        break;
                    }
                    if( args.Parameters.Count <2 ){
                        op.SendErrorMessage("需要输入完成次数，例如: /fish finish 10");
                        break;
                    }
                    int finished = 0;
                    if ( int.TryParse(args.Parameters[1], out finished) ){
                        op.TPlayer.anglerQuestsFinished = finished;
                        NetMessage.SendData(76, op.Index, -1, NetworkText.Empty, op.Index);
                        NetMessage.SendData(76, -1, -1, NetworkText.Empty, op.Index);
                        op.SendSuccessMessage($"你的渔夫任务完成次数已改成 {finished} 次");
                    } else {
                        op.SendErrorMessage("次数输入错误，例如: /fish finish 10");
                    }
                    break;


                // 切换钓鱼任务
                case "change":
                case "swap":
                case "next":
                case "pass":
                    if( !op.HasPermission(PermissionChange) )
                        op.SendErrorMessage("你无权切换钓鱼任务！");
                    else
                        FishHelper.AnglerQuestSwap(op);
                    break;


                // 指定今天的任务鱼
                case "cs":
                case "changesuper":
                    if( !op.HasPermission(PermissionChangeSuper) ){
                        op.SendErrorMessage("你无权指定今天的任务鱼！");
                    } else {
                        if( args.Parameters.Count<2 ){
                            op.SendErrorMessage("需输入任务鱼的 名称/物品id！，例如: /fish cs 向导巫毒鱼");
                            break;
                        }
                        FishHelper.FishQuestSwap( op, args.Parameters[1]  );
                    }
                    break;
                
                case "reload":
                case "r":
                    if( !op.HasPermission(PermissionReload) ){
                        op.SendErrorMessage("你无权执行重载操作！");
                    } else {
                        args.Player.SendSuccessMessage("[fishshop]鱼店配置已重载");
                        LoadConfig(true);
                    }
                    break;


                // 供测试用的指令
                case "special":
                case "spe":
                    if( !op.HasPermission(PermissionSpecial) ){
                        op.SendErrorMessage("你无权执行此指令！");
                    } else {
                        op.SendInfoMessage("/fish docs，生成参考文档");
                        op.SendInfoMessage("/fish relive，复活NPC");
                        op.SendInfoMessage("/fish tpall，集合");
                        op.SendInfoMessage("/fish jump，集体庆祝");
                        op.SendInfoMessage("/fish firework，烟花");
                    }
                    break;
                // 生成参考文档
                case "docs":
                    if( !op.HasPermission(PermissionSpecial) ){
                        op.SendErrorMessage("你无权执行此指令！");
                    } else {
                        DocsHelper.GenDocs(op, savedir);
                    }
                    break;
                    
                case "jump":
                    if( !op.HasPermission(PermissionSpecial) ){
                        op.SendErrorMessage("你无权执行此指令！");
                    } else {
                        CmdHelper.Jump(op);
                    }
                    break;

                case "firework":
                case "fw":
                    if( !op.HasPermission(PermissionSpecial) )
                        op.SendErrorMessage("你无权执行此指令！");
                    else
                        CmdHelper.FireworkRocket(op);
                    break;

                case "relive":
                    if( !op.HasPermission(PermissionSpecial) )
                        op.SendErrorMessage("你无权执行此指令！");
                    else
                        NPCHelper.ReliveNPC(op);
                    break;

                case "tpall":
                    if( !op.HasPermission(PermissionSpecial) )
                        op.SendErrorMessage("你无权执行此指令！");
                    else
                        CmdHelper.TPHereAll(op);
                    break;

                case "sb":
                    if( !op.HasPermission(PermissionSpecial) ){
                        op.SendErrorMessage("你无权执行此指令！");
                        return;
                    }
                    if( args.Parameters.Count<2 ){
                        op.SendErrorMessage("语法错误，需要输入 boss的npcid，例如 /fish sb 396");
                        return;
                    }
                    int npcID = 0;
                    if( int.TryParse(args.Parameters[1].ToLowerInvariant(), out npcID) )
                        NPCHelper.SpawnNPC(op, npcID);
                    else
                        op.SendErrorMessage("npcid输入不正确，例如 /fish sb 396");
                    break;
            }
        }


        // 查看商店
        private void ListGoods(CommandArgs args)
        {
            LoadConfig();

            // 商店是否解锁
            if( !CheckShopUnlock(args.Player) )
                return;

            // 更新货架
            List<ShopItem> founds = UpdateShelf(args.Player);

            float num = (float)founds.Count / _config.pageSlots;
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
            string msg = "";
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

                if( rowCount!=1 && rowCount==_config.rowSlots )
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


        // 询价
        private void AskGoods(CommandArgs args)
        {
            if( args.Parameters.Count<2 ){
                args.Player.SendErrorMessage("需要输入商品编号，例如: /fish ask 1");
                return;
            }

            LoadConfig();

            // 商店是否解锁
            if( !CheckShopUnlock(args.Player) )
                return;

            String itemNameOrId = args.Parameters[1];
            List<ShopItem> founds = UpdateShelf(args.Player);
            List<ShopItem2> goods = new List<ShopItem2>();
            ShopItem2 item2;

            int goodsSerial = 0;
            if( int.TryParse(itemNameOrId, out goodsSerial) )
            {
                // 编号有效性
                int count = founds.Count;
                if( goodsSerial<=0 || goodsSerial>count ){
                    args.Player.SendErrorMessage($"最大编号为: {count}，请使用 /fish list 查看货架.");
                    return;
                }
                item2 = new ShopItem2();
                item2.serial = goodsSerial;
                item2.item = founds[goodsSerial-1];
                goods.Add( item2 );
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
                        List<ShopItem2> finds = FindGoods(item.netID);
                        foreach( ShopItem2 sitem in finds ){
                            goods.Add( sitem );
                        }
                    }
                }
            }

            foreach (ShopItem2 shopItem in goods)
            {
                string shopDesc = shopItem.item.GetItemDesc();
                string costDesc = shopItem.item.GetCostDesc();
                string unlockDesc = shopItem.item.GetUnlockDesc();
                string s = $"{shopItem.serial}.{shopDesc} = {costDesc}";
                if( unlockDesc!="" )
                    s += $" | 需 {unlockDesc}";
                args.Player.SendInfoMessage( s );
            }

            if( goods.Count==0 ){
                args.Player.SendErrorMessage( $"没卖过 {itemNameOrId}!" );
            } else{
                if( args.Player.RealPlayer )
                    args.Player.SendInfoMessage($"你的余额: {InventoryHelper.GetCoinsCountDesc(args.Player)}");
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

            // 商店是否解锁
            if( !CheckShopUnlock(args.Player) )
                return;

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
            if( InventoryHelper.CheckCost(args.Player, shopItem, goodsAmount, out msg) )
            {
                // 检查扣除
                InventoryHelper.DeductCost(args.Player, shopItem, goodsAmount);
                // 提供商品/服务
                ProvideGoods(args.Player, shopItem, goodsAmount);

                s = "";
                if( args.Player.Group.Name=="builder" || args.Player.Group.Name=="architect" ){
                    float discountMoney = shopItem.GetCostMoney(goodsAmount) * 0.1f;
                    int discountMoneyInt = (int)Math.Ceiling( discountMoney );
                    string s2 = MyUtils.GetMoneyDesc(discountMoneyInt);
                    s =$"（你是建筑师，享1折优惠，钱币只收 {s2}）";
                }

                msg = $"你购买了 {goodsAmount}件 {shopItem.GetItemDesc()} | 花费: {shopItem.GetCostDesc(goodsAmount)}{s} | 余额: {InventoryHelper.GetCoinsCountDesc(args.Player)}";
                args.Player.SendSuccessMessage( msg  );
                Log.info( $"{args.Player.Name} 买了 {shopItem.GetItemDesc()}" );
            } else {
                args.Player.SendInfoMessage($"没买成功，因为: {msg}，请输入 /fish ask {goodsSerial} 查询购买条件");
            }

        }


        private bool CheckShopUnlock(TSPlayer op)
        {
            string msg = "";
            string s = "";
            foreach( ItemData d in _config.unlock )
            {
                if( !UnlockID.CheckUnlock(d, op, out s) )
                    msg += " " + s;
            }
            if( msg!="" ){
                if( op!=null )
                    op.SendInfoMessage( $"【{_config.name}】已打烊，因为: {msg}" );
                else
                   Log.info( $"【{_config.name}】已打烊，因为: {msg}" );
                return false;
            }
            return true;
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
            return _config.shop;
        }

        private List<ShopItem2> FindGoods(int _id, string _prefix="")
        {
            List<ShopItem2> items = new List<ShopItem2>();
            ShopItem2 item;
            for (int i = 0; i < _config.shop.Count; i++)
            {
                ShopItem data = _config.shop[i];
                if( data.id != _id )
                    continue;

                if( _prefix=="" ){
                    item = new ShopItem2();
                    item.serial = i+1;
                    item.item = data;
                    items.Add(item);
                } else {
                    if(data.prefix==_prefix )
                    {
                        item = new ShopItem2();
                        item.serial = i+1;
                        item.item = data;
                        items.Add(item);
                    }
                }
            }
            return items;
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

                    // 白天 中午 晚上 午夜
                    case ShopItemID.TimeToDay: CmdHelper.SwitchTime(player, "day"); return;
                    case ShopItemID.TimeToNoon: CmdHelper.SwitchTime(player, "noon"); return;
                    case ShopItemID.TimeToNight: CmdHelper.SwitchTime(player, "night"); return;
                    case ShopItemID.TimeToMidNight: CmdHelper.SwitchTime(player, "midnight"); return;
                    
                    // 好运来
                    case ShopItemID.GoodLucky: CmdHelper.GoodLucky(player); return;

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

                    //  集体庆祝
                    case ShopItemID.CelebrateAll: CmdHelper.CelebrateAll(player); return;


                    default:
                        break;
                }

                // 召唤NPC类
                int id2 = ShopItemID.GetRealSpawnID(id);
                if( id2!=0 )
                {
                    NPCHelper.SpawnNPC(player, id2, amount);
                    return;
                }
                
                // 清除NPC类
                id2 = ShopItemID.GetRealClearNPCID(id);
                if( id2!=0 )
                {
                    NPCHelper.ClearNPC(player, id2, amount);
                    return;
                }

                // 获得buff类
                id2 = ShopItemID.GetRealBuffID(id);
                if( id2!=0 )
                {
                    BuffHelper.SetPlayerBuff(player, id2, shopItem.stack*amount);
                    return;
                }

            } else {
                // 下发物品
                player.GiveItem( shopItem.id, shopItem.stack*amount, shopItem.GetPrefixInt() );
            }
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
