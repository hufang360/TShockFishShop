using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using TerrariaApi.Server;
using TShockAPI;


namespace FishShop
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
        public static readonly string PermissionSpecial = "fishshop.special";
        public static readonly string PermissionGroupIgnore = "fishshop.ignore.allowgroup";

        public static readonly string savedir = Path.Combine(TShock.SavePath, "FishShop");
        public static readonly string settingsFile = Path.Combine(savedir, "settings.json");
        public static readonly string configFile = Path.Combine(savedir, "config.json");
        public static readonly string recordFile = Path.Combine(savedir, "record.json");


        // 配置文件
        public static Config _config;


        private static bool isLoaded = false;

        public Plugin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(new List<string>() { }, FishShop, "fishshop", "fish", "fs") { HelpText = "鱼店" });

            if (!Directory.Exists(savedir)) Directory.CreateDirectory(savedir);
            Settings.Load(settingsFile);
            Config.GenConfig(configFile);
            LimitHelper.recodFile = recordFile;
            utils.init();
        }

        #region command
        private void FishShop(CommandArgs args)
        {
            TSPlayer op = args.Player;
            #region help
            void ShowHelpText()
            {
                op.SendInfoMessage("/fish list，查看货架");
                op.SendInfoMessage("/fish ask <编号>，问价格");
                op.SendInfoMessage("/fish buy <编号>，购买");
                op.SendInfoMessage("/fish info，显示钓鱼信息");

                if (op.HasPermission(PermissionFinish))
                    op.SendInfoMessage("/fish finish <次数>，修改自己的渔夫任务完成次数");
                if (op.HasPermission(PermissionChange))
                    op.SendInfoMessage("/fish change，更换今天的任务鱼");
                if (op.HasPermission(PermissionChangeSuper))
                    op.SendInfoMessage("/fish changesuper <物品id|物品名>，指定今天的任务鱼");
                if (op.HasPermission(PermissionReload))
                {
                    op.SendInfoMessage("/fish reload，重载配置");
                    op.SendInfoMessage("/fish reset, 重置限购数量");
                }

                if (op.HasPermission(PermissionSpecial))
                {
                    op.SendInfoMessage("/fish special, 查看特别指令");
                }
            }
            #endregion

            if (TShock.ServerSideCharacterConfig.Settings.Enabled && op.Group.Name == TShock.Config.Settings.DefaultGuestGroupName)
            {
                op.SendErrorMessage("游客无法访问鱼店");
                return;
            }


            if (args.Parameters.Count == 0)
            {
                op.SendErrorMessage("语法错误，输入 /fish help 查询用法");
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

                #region admin command
                // 修改钓鱼次数
                case "f":
                case "finish":
                    if (!op.RealPlayer)
                    {
                        op.SendErrorMessage("此指令需要在游戏内才能执行！");
                        break;
                    }
                    if (!op.HasPermission(PermissionFinish))
                    {
                        op.SendErrorMessage("你无权更改钓鱼次数！");
                        break;
                    }
                    if (args.Parameters.Count < 2)
                    {
                        op.SendErrorMessage("需要输入完成次数，例如: /fish finish 10");
                        break;
                    }
                    if (int.TryParse(args.Parameters[1], out int finished))
                    {
                        op.TPlayer.anglerQuestsFinished = finished;
                        NetMessage.SendData(76, op.Index, -1, NetworkText.Empty, op.Index);
                        NetMessage.SendData(76, -1, -1, NetworkText.Empty, op.Index);
                        op.SendSuccessMessage($"你的渔夫任务完成次数已改成 {finished} 次");
                    }
                    else
                    {
                        op.SendErrorMessage("次数输入错误，例如: /fish finish 10");
                    }
                    break;


                // 切换钓鱼任务
                case "change":
                case "swap":
                case "next":
                case "pass":
                    if (!op.HasPermission(PermissionChange))
                        op.SendErrorMessage("你无权切换钓鱼任务！");
                    else
                        FishHelper.AnglerQuestSwap(op);
                    break;


                // 指定今天的任务鱼
                case "cs":
                case "changesuper":
                    if (!op.HasPermission(PermissionChangeSuper))
                    {
                        op.SendErrorMessage("你无权指定今天的任务鱼！");
                    }
                    else
                    {
                        if (args.Parameters.Count < 2)
                        {
                            op.SendErrorMessage("需输入任务鱼的 名称/物品id！，例如: /fish cs 向导巫毒鱼");
                            break;
                        }
                        FishHelper.FishQuestSwap(op, args.Parameters[1]);
                    }
                    break;

                //重载
                case "reload":
                case "r":
                    if (!op.HasPermission(PermissionReload))
                    {
                        op.SendErrorMessage("你无权执行重载操作！");
                    }
                    else
                    {
                        double t1 = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                        Settings.Load(settingsFile);
                        LoadConfig(true);
                        args.Player.SendSuccessMessage("[fishshop]鱼店配置已重载");
                        double t2 = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                        op.SendInfoMessage($"耗时：{t2 - t1} 毫秒");
                    }
                    break;

                // 重置限购数据
                case "reset":
                    if (!op.HasPermission(PermissionReload))
                    {
                        op.SendErrorMessage("你无权执行该操作！");
                    }
                    else
                    {
                        LimitHelper.ResetRecord();
                        args.Player.SendSuccessMessage("[fishshop]已重置限购数据");
                    }
                    break;
                #endregion


                #region special
                // 生成参考文档
                case "docs":
                    if (!op.HasPermission(PermissionSpecial))
                    {
                        op.SendErrorMessage("你无权执行此指令！");
                    }
                    else
                    {
                        DocsHelper.GenDocs(op, savedir);
                    }
                    break;

                // 供测试用的指令
                case "special":
                case "spe":
                    if (!op.HasPermission(PermissionSpecial))
                    {
                        op.SendErrorMessage("你无权执行此指令！");
                    }
                    else
                    {
                        op.SendInfoMessage("/fish docs，生成参考文档");
                        op.SendInfoMessage("/fish relive，复活NPC");
                        op.SendInfoMessage("/fish tpall，集合");
                        op.SendInfoMessage("/fish jump，集体庆祝");
                        op.SendInfoMessage("/fish firework，烟花");
                    }
                    break;

                case "jump":
                    if (!op.HasPermission(PermissionSpecial))
                        op.SendErrorMessage("你无权执行此指令！");
                    else
                        CmdHelper.Jump(op);
                    break;

                case "firework":
                case "fw":
                    if (!op.HasPermission(PermissionSpecial))
                        op.SendErrorMessage("你无权执行此指令！");
                    else
                        CmdHelper.FireworkRocket(op);
                    break;

                case "relive":
                    if (!op.HasPermission(PermissionSpecial))
                        op.SendErrorMessage("你无权执行此指令！");
                    else
                        NPCHelper.ReliveNPC(op);
                    break;

                case "tpall":
                    if (!op.HasPermission(PermissionSpecial))
                        op.SendErrorMessage("你无权执行此指令！");
                    else
                        CmdHelper.TPHereAll(op);
                    break;
                    #endregion
            }
        }
        #endregion

        #region ListGoods
        // 查看商店
        private void ListGoods(CommandArgs args)
        {
            LoadConfig();

            // 商店是否解锁
            if (!CheckShopUnlock(args.Player))
                return;

            // 更新货架
            List<ShopItem> founds = UpdateShelf(args.Player);

            float num = (float)founds.Count / _config.pageSlots;
            int totalPage = (int)Math.Ceiling(num);

            int pageNum = 1;
            if (args.Parameters.Count > 1)
                int.TryParse(args.Parameters[1], out pageNum);
            if (pageNum > totalPage)
                pageNum = totalPage;
            else if (pageNum <= 0)
                pageNum = 1;

            int totalSlots = _config.pageSlots * pageNum;


            // 显示指定页的内容
            string msg = "";
            int rowCount = 0;
            int pageCount = 0;
            int totalCount = 0;
            int startSlot = _config.pageSlots * (pageNum - 1);
            for (int i = 0; i < founds.Count; i++)
            {
                if (i < startSlot)
                    continue;

                rowCount++;
                pageCount++;

                ShopItem item = founds[i];
                msg += $"{i + 1}.{item.GetIcon()}{item.GetItemDesc()}  ";

                totalCount = i + 1;
                if (i >= (totalSlots - 1))
                {
                    break;
                }

                if (rowCount != 1 && rowCount == _config.rowSlots)
                {
                    rowCount = 0;
                    msg += "\n";
                }
            }

            if (founds.Count > (totalCount))
                msg += $"\n[c/96FF0A:输入 /fish list {pageNum + 1} 查看更多.]";

            if (msg == "")
                msg = "今天卖萌，不卖货！ɜː";
            else
                msg = $"[c/96FF0A:欢迎光临【{_config.name}】,货架 ({pageNum}/{totalPage}): ]\n" + msg;
            if (args.Player != null)
                args.Player.SendInfoMessage(msg);
            else
                utils.Log(msg);
        }
        #endregion

        #region AskGoods
        // 询价
        private void AskGoods(CommandArgs args)
        {
            if (args.Parameters.Count < 2)
            {
                args.Player.SendErrorMessage("需要输入商品编号，例如: /fish ask 1");
                return;
            }

            LoadConfig();

            TSPlayer op = args.Player;
            // 商店是否解锁
            if (!CheckShopUnlock(args.Player))
                return;

            String itemNameOrId = args.Parameters[1];
            List<ShopItem> founds = UpdateShelf(op);
            List<ShopItem2> goods = new List<ShopItem2>();
            ShopItem2 item2;

            if (int.TryParse(itemNameOrId, out int goodsSerial))
            {
                // 编号有效性
                int count = founds.Count;
                if (goodsSerial <= 0 || goodsSerial > count)
                {
                    op.SendErrorMessage($"最大编号为: {count}，请输入 /fish list 查看货架.");
                    return;
                }
                item2 = new ShopItem2();
                item2.serial = goodsSerial;
                item2.item = founds[goodsSerial - 1];
                goods.Add(item2);
            }
            else
            {
                // 通过名字 匹配编号
                int customID = IDSet.GetIDByName(itemNameOrId);
                if (customID != 0)
                {
                    goods = FindGoods(customID);
                }
                else
                {
                    List<Item> matchedItems = TShock.Utils.GetItemByIdOrName(itemNameOrId);
                    if (matchedItems.Count == 0)
                    {
                        op.SendErrorMessage($"物品名/物品id: {itemNameOrId} 不正确");
                        return;
                    }
                    utils.Log(matchedItems.Count.ToString());
                    foreach (Item item in matchedItems)
                    {
                        List<ShopItem2> finds = FindGoods(item.netID);
                        foreach (ShopItem2 sitem in finds)
                        {
                            goods.Add(sitem);
                        }
                    }
                }
            }

            foreach (ShopItem2 shopItem in goods)
            {
                string iconDesc = shopItem.item.GetIcon();
                string shopDesc = shopItem.item.GetItemDesc();
                string costDesc = shopItem.item.GetCostDesc();
                string unlockDesc = shopItem.item.GetUnlockDesc();
                string allowGroupDesc = shopItem.item.GetAllowGroupDesc();
                string comment = shopItem.item.GetComment();
                string limitDesc = shopItem.item.GetLimitDesc(op.Name);
                string s = $"{shopItem.serial}.{iconDesc}{shopDesc} = {costDesc}";
                if (unlockDesc != "")
                    s += $"\n解锁条件：需 {unlockDesc}";
                if (comment != "")
                    s += $"\n商品备注：{comment}";
                if (limitDesc != "")
                    s += $"\n商品限购：{limitDesc}";
                if (allowGroupDesc != "")
                    s += $"\n用户组限制：{allowGroupDesc}";

                op.SendInfoMessage(s);
            }

            if (goods.Count == 0)
            {
                op.SendErrorMessage($"没卖过 {itemNameOrId}!");
            }
            else
            {
                if (op.RealPlayer)
                    op.SendInfoMessage($"你的余额：{InventoryHelper.GetCoinsCountDesc(op)}");
            }
        }
        #endregion


        #region BuyGoods
        // 购买
        private void BuyGoods(CommandArgs args)
        {
            TSPlayer op = args.Player;
            if (!op.RealPlayer)
            {
                op.SendErrorMessage("此指令需要在游戏内执行！");
                return;
            }
            if (!op.InventorySlotAvailable)
            {
                op.SendErrorMessage("背包已满，不能购买！");
                return;
            }
            if (args.Parameters.Count < 2)
            {
                op.SendErrorMessage("需输入 物品名 / 商品编号，例如: /fish buy 1，/fish buy 生命水晶");
                return;
            }

            // 更新货架
            LoadConfig();

            // 商店是否解锁
            if (!CheckShopUnlock(op))
                return;

            List<ShopItem> goods = UpdateShelf(op);

            if (int.TryParse(args.Parameters[1], out int goodsSerial))
            {
                // 编号有效性
                int count = goods.Count;
                if (goodsSerial <= 0 || goodsSerial > count)
                {
                    op.SendErrorMessage($"最大编号为: {count}，请输入 /fish list 查看货架");
                    return;
                }
            }
            else
            {
                // 通过名字 匹配编号
                goodsSerial = 0;
                int goodsID = IDSet.GetIDByName(args.Parameters[1]);
                if (goodsID != 0)
                {
                    for (int i = 0; i < goods.Count; i++)
                    {
                        if (goods[i].id == goodsID)
                        {
                            goodsSerial = i + 1;
                            break;
                        }
                    }
                }

                if (goodsSerial == 0)
                {
                    op.SendErrorMessage($"没有名为 {args.Parameters[1]} 的 物品");
                    return;
                }
            }


            int goodsAmount = 1;
            if (args.Parameters.Count > 2)
                int.TryParse(args.Parameters[2], out goodsAmount);
            if (goodsAmount < 1)
                goodsAmount = 1;


            // 找到对应商品
            ShopItem shopItem = goods[goodsSerial - 1];

            // [日志记录]
            utils.Log(string.Format("{0} 要买{1}个 {2}", op.Name, goodsAmount, shopItem.GetItemDesc()));
            utils.Log($"item: {shopItem.name} {shopItem.id} {shopItem.stack} {shopItem.prefix}");
            foreach (ItemData _d in shopItem.unlock)
                utils.Log($"unlock: {_d.name} {_d.id} {_d.stack}");
            foreach (ItemData _d in shopItem.cost)
                utils.Log($"cost: {_d.name} {_d.id} {_d.stack}");
            utils.Log($"余额: {InventoryHelper.GetCoinsCountDesc(op, false)}");


            // 检查解锁条件
            string msg = "";
            string s = "";
            foreach (ItemData d in shopItem.unlock)
            {
                if (!UnlockID.CheckUnlock(d, op, out s))
                    msg += " " + s;
            }

            // 检查用户组
            bool groupPass = op.HasPermission(PermissionGroupIgnore) || shopItem.allowGroup.Count == 0;
            if (!groupPass) groupPass = shopItem.allowGroup.Contains(op.Group.Name);
            if (!groupPass)
            {
                msg += $" 你不是 {string.Join("、", shopItem.allowGroup)} 用户组的玩家";
            }

            if (msg != "")
            {
                op.SendInfoMessage($"暂时不能购买，因为: {msg}");
                return;
            }

            // buff类死亡时不能买
            if (op.Dead && !shopItem.DeadCanBuyItem())
            {
                op.SendInfoMessage($"请待复活后再购买此物品！");
                return;
            }

            // 坠落之星白天卖会消失
            if (shopItem.id == 75 && Main.dayTime)
            {
                op.SendInfoMessage($"坠落之星 只能在晚上购买！");
                return;
            }

            // 仅晚上可以购买的商品
            if (!shopItem.DayCanBuyItem() && Main.dayTime)
            {
                op.SendInfoMessage($"只能在晚上购买！");
                return;
            }

            // 仅白天可以购买的商品
            if (!shopItem.NightCanBuyItem() && !Main.dayTime)
            {
                op.SendInfoMessage($"只能在白天购买！");
                return;
            }

            // 检查是否需要购买
            if (!ShopItemID.CanBuy(op, shopItem, goodsAmount))
                return;

            // 单次至多买一件的物品
            goodsAmount = Math.Min(goodsAmount, shopItem.BuyMax());
            if (goodsAmount < 1) goodsAmount = 1;



            if (shopItem.id > 0)
            {
                // 检查物品堆叠上线
                Item itemnet = new Item();
                itemnet.SetDefaults(shopItem.id);
                if (shopItem.stack * goodsAmount > itemnet.maxStack)
                {
                    float num = itemnet.maxStack / shopItem.stack;
                    goodsAmount = (int)Math.Floor(num);
                    if (goodsAmount == 0)
                    {
                        op.SendErrorMessage($"[鱼店]此商品的堆叠数量配置错误,name={shopItem.name},id={shopItem.id},stack={shopItem.stack}");
                        return;
                    }
                }
            }

            // 限购
            if (!shopItem.CheckLimitCanBuy(op.Name))
            {
                op.SendErrorMessage("噢噢，这件商品太抢手了，已经卖完了");
                return;
            }

            // 最脏的块
            if (shopItem.id == ShopItemID.DirtiestBlock)
            {
                if (!shopItem.CheckDirtiestMatrix(op))
                {
                    op.SendInfoMessage($"在你的周围未能找到臭臭矩阵！(7x7中空)");
                    return;
                }
            }

            // 询价
            if (InventoryHelper.CheckCost(op, shopItem, goodsAmount, out msg))
            {
                // 检查扣除
                InventoryHelper.DeductCost(op, shopItem, goodsAmount);
                // 提供商品/服务
                ProvideGoods(op, shopItem, goodsAmount);

                s = "";
                if (op.Group.Name == "builder" || op.Group.Name == "architect")
                {
                    float discountMoney = shopItem.GetCostMoney(goodsAmount) * 0.1f;
                    int discountMoneyInt = (int)Math.Ceiling(discountMoney);
                    string s2 = utils.GetMoneyDesc(discountMoneyInt);
                    s = $"（你是建筑师，享1折优惠，钱币只收 {s2}）";
                }

                msg = $"你购买了 {goodsAmount}件 {shopItem.GetItemDesc()} | 花费: {shopItem.GetCostDesc(goodsAmount)}{s} | 余额: {InventoryHelper.GetCoinsCountDesc(op)}";
                op.SendSuccessMessage(msg);
                utils.Log($"{op.Name} 买了 {shopItem.GetItemDesc()}");
                LimitHelper.Record(op.Name, shopItem.name, shopItem.id, goodsAmount);
            }
            else
            {
                op.SendInfoMessage($"没买成功，因为: {msg}，请输入 /fish ask {goodsSerial} 查询购买条件");
            }

        }
        #endregion


        private bool CheckShopUnlock(TSPlayer op)
        {
            string msg = "";
            string s;
            foreach (ItemData d in _config.unlock)
            {
                if (!UnlockID.CheckUnlock(d, op, out s))
                    msg += " " + s;
            }
            if (msg != "")
            {
                if (op != null)
                    op.SendInfoMessage($"【{_config.name}】已打烊，因为: {msg}");
                else
                    utils.Log($"【{_config.name}】已打烊，因为: {msg}");
                return false;
            }
            return true;
        }

        // 加载配置
        private void LoadConfig(bool forceLoad = false)
        {
            if (!isLoaded || forceLoad)
            {
                _config = Config.Load(configFile);

                foreach (ItemData d in _config.unlock)
                {
                    d.FixIDByName();
                }

                foreach (ShopItem item in _config.shop)
                {
                    item.Filling();
                    foreach (ItemData d in item.cost)
                    {
                        d.FixIDByName();
                    }
                    foreach (ItemData d in item.unlock)
                    {
                        d.FixIDByName();
                    }
                }
                isLoaded = true;
            }
            LimitHelper.Load(forceLoad);
        }

        /// 更新货架
        private List<ShopItem> UpdateShelf(TSPlayer player)
        {
            return _config.shop;
        }

        private List<ShopItem2> FindGoods(int _id, string _prefix = "")
        {
            List<ShopItem2> items = new List<ShopItem2>();
            ShopItem2 item;
            for (int i = 0; i < _config.shop.Count; i++)
            {
                ShopItem data = _config.shop[i];
                if (data.id != _id)
                    continue;

                if (_prefix == "")
                {
                    item = new ShopItem2();
                    item.serial = i + 1;
                    item.item = data;
                    items.Add(item);
                }
                else
                {
                    if (data.prefix == _prefix)
                    {
                        item = new ShopItem2();
                        item.serial = i + 1;
                        item.item = data;
                        items.Add(item);
                    }
                }
            }
            return items;
        }


        // 提供商品/服务
        private void ProvideGoods(TSPlayer player, ShopItem shopItem, int amount = 1)
        {
            if (shopItem.id < -24)
                ShopItemID.ProvideGoods(player, shopItem, amount);  // 自定义商品
            else
                player.GiveItem(shopItem.id, shopItem.stack * amount, shopItem.GetPrefixInt()); // 下发物品
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }

}
