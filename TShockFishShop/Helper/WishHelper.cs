using System.Collections.Generic;
using Terraria;
using TShockAPI;


namespace FishShop
{
    /// <summary>
    /// 许愿池
    /// </summary>
    public class WishHelper
    {

        public static void Manage(CommandArgs args)
        {
            TSPlayer op = args.Player;
            void ShowHelpText()
            {
                op.SendInfoMessage("/wish list，查看货架");
            }

            if (TShock.ServerSideCharacterConfig.Settings.Enabled && op.Group.Name == TShock.Config.Settings.DefaultGuestGroupName)
            {
                op.SendErrorMessage("游客无法使用许愿池");
                return;
            }


            if (args.Parameters.Count == 0)
            {
                op.SendErrorMessage("语法错误，输入 /wish help 查询用法");
                return;
            }

            string kw = args.Parameters[0].ToLowerInvariant();
            switch (kw)
            {
                // 帮助
                case "h":
                case "help":
                    ShowHelpText();
                    return;
            }


            List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[0]);
            if (items.Count > 1)
            {
                args.Player.SendInfoMessage("匹配到多个物品");
                string text = "";
                for (int i = 0; i < items.Count; i++)
                {
                    text = text + Lang.GetItemNameValue(items[i].type) + ",";
                    if ((i + 1) % 5 == 0)
                    {
                        text += "\n";
                    }
                }
                text = text.Trim(',');
                args.Player.SendInfoMessage(text);
                return;
            }
            if (items.Count < 1)
            {
                args.Player.SendErrorMessage("此物品不存在");
                return;
            }
            utils.Log($"{items[0].Name} prefix:{items[0].prefix}  stack:{items[0].stack}");
        }
    }

}