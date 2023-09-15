using FishShop.Record;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using TerrariaApi.Server;
using TShockAPI;


namespace FishShop;

[ApiVersion(2, 1)]
public partial class Plugin : TerrariaPlugin
{
    public override string Name => "FishShop";
    public override string Author => "hufang360";
    public override string Description => "鱼店";
    public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;



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
        Commands.ChatCommands.Add(new Command(Manage, "fishshop", "fish", "fs", "鱼店") { HelpText = "鱼店" });
        //Commands.ChatCommands.Add(new Command(WishHelper.Manage, "wish") { HelpText = "许愿池" });

        if (!Directory.Exists(savedir))
        {
            Directory.CreateDirectory(savedir);
        }
        Settings.Load(settingsFile);
        Config.GenConfig(configFile);
        Records.RecodFile = recordFile;
        utils.init();
    }

    private void Manage(CommandArgs args)
    {
        TSPlayer op = args.Player;
        #region help
        void ShowHelpText()
        {
            op.SendInfoMessage("/fish list，查看货架");
            op.SendInfoMessage("/fish ask <编号>，问价格");
            op.SendInfoMessage("/fish buy <编号>，购买");
            op.SendInfoMessage("/fish info，显示钓鱼信息");
            op.SendInfoMessage("/fish rank，消费榜");
            op.SendInfoMessage("/fish basket，鱼篓榜");

            if (op.HasPermission(Permissions.Finish)) op.SendInfoMessage("/fish finish <次数>，修改自己的渔夫任务完成次数");
            if (op.HasPermission(Permissions.Change)) op.SendInfoMessage("/fish change，更换今天的任务鱼");
            if (op.HasPermission(Permissions.ChangeSuper)) op.SendInfoMessage("/fish changesuper <物品id|物品名>，指定今天的任务鱼");

            if (op.HasPermission(Permissions.Reload))
            {
                op.SendInfoMessage("/fish reload，重载配置");
                op.SendInfoMessage("/fish reset, 重置限购数量");
            }

            if (op.HasPermission(Permissions.Special))
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
            default:
                ListGoods(args);
                op.SendInfoMessage("请输入 /fish help 查询用法");
                break;

            case "h": case "help": case "帮助": ShowHelpText(); return;  // 帮助

            case "l": case "list": case "货架": case "逛店": ListGoods(args); break;  // 浏览
            case "a": case "ask": case "询价": AskGoods(args); break;    // 询价
            case "b": case "buy": case "购买": case "买": BuyGoods(args); break;    // 购买

            case "i": case "info": case "信息": FishHelper.FishInfo(op); break;  // 钓鱼信息
            case "rank": case "消费榜": Records.ShowRank(args); break;    // 消费榜
            case "basket": case "鱼篓榜": Records.ShowBasket(args); break;    // 鱼篓榜（用鱼消费的排行）

            case "forge": ForgeHelper.Manage(args); break; // 10连敲

            #region admin command
            // 修改钓鱼次数
            case "f":
            case "finish":
                if (!op.RealPlayer)
                {
                    op.SendErrorMessage("此指令需要在游戏内才能执行！");
                    break;
                }
                if (!op.HasPermission(Permissions.Finish))
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
                if (!op.HasPermission(Permissions.Change))
                    op.SendErrorMessage("你无权切换钓鱼任务！");
                else
                    FishHelper.AnglerQuestSwap(op);
                break;


            // 指定今天的任务鱼
            case "cs":
            case "changesuper":
                if (!op.HasPermission(Permissions.ChangeSuper))
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
                if (!op.HasPermission(Permissions.Reload))
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
                if (!op.HasPermission(Permissions.Reload))
                {
                    op.SendErrorMessage("你无权执行该操作！");
                }
                else
                {
                    Records.ResetRecord();
                    args.Player.SendSuccessMessage("[fishshop]已重置限购数据");
                }
                break;
            #endregion


            #region special
            // 生成参考文档
            case "docs":
                if (!op.HasPermission(Permissions.Special))
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
                if (!op.HasPermission(Permissions.Special))
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
                if (!op.HasPermission(Permissions.Special))
                    op.SendErrorMessage("你无权执行此指令！");
                else
                    CmdHelper.Jump(op);
                break;

            case "firework":
            case "fw":
                if (!op.HasPermission(Permissions.Special))
                    op.SendErrorMessage("你无权执行此指令！");
                else
                    CmdHelper.FireworkRocket(op);
                break;

            case "relive":
                if (!op.HasPermission(Permissions.Special))
                    op.SendErrorMessage("你无权执行此指令！");
                else
                    NPCHelper.ReliveNPC(op);
                break;

            case "tpall":
                if (!op.HasPermission(Permissions.Special))
                    op.SendErrorMessage("你无权执行此指令！");
                else
                    CmdHelper.TPHereAll(op);
                break;
                #endregion
        }
    }


    /// <summary>
    /// 商店是否解锁
    /// </summary>
    static bool ShopIsReady(TSPlayer op)
    {
        // 更新货架
        LoadConfig();


        string msg = "";
        foreach (ItemData d in _config.unlock)
        {
            if (!UnlockID.CheckUnlock(d, op, out string s))
            {
                msg += " " + s;
            }
        }
        if (msg != "")
        {
            if (op != null)
            {
                op.SendInfoMessage($"【{_config.name}】已打烊，因为: {msg}");
            }
            else
            {
                utils.Log($"【{_config.name}】已打烊，因为: {msg}");
            }
            return false;
        }
        return true;
    }

    /// <summary>
    /// 加载配置
    /// </summary>
    /// <param name="forceLoad">强制加载</param>
    static void LoadConfig(bool forceLoad = false)
    {
        if (!isLoaded || forceLoad)
        {
            _config = Config.Load(configFile);

            foreach (ItemData d in _config.unlock)
            {
                d.FixIDByName(true);
            }

            var i = 0;
            foreach (ShopItemData siData in _config.shop)
            {
                siData.serial = i + 1;
                i++;
                siData.Filling();
                foreach (ItemData d in siData.unlock)
                {
                    d.FixIDByName(true);
                }
                foreach (ItemData d in siData.cost)
                {
                    d.FixIDByName(false);
                }
            }
            isLoaded = true;

        }
        Records.Load(forceLoad);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
        base.Dispose(disposing);
    }
}
