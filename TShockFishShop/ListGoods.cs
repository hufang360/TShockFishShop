using System;
using TShockAPI;

namespace FishShop;

public partial class Plugin
{
    // 查看商店
    static void ListGoods(CommandArgs args)
    {
        if (!ShopIsReady(args.Player))
        {
            return;
        }

        // 更新货架
        float num = (float)_config.shop.Count / _config.pageSlots;
        int totalPage = (int)Math.Ceiling(num);

        // 输入的页码
        if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out int pageNum))
        {
            if (pageNum > totalPage)
            {
                pageNum = totalPage;
            }
            else if (pageNum <= 0)
            {
                pageNum = 1;
            }
        }
        else
        {
            pageNum = 1;
        }


        int totalSlots = _config.pageSlots * pageNum;


        // 显示指定页的内容
        string msg = "";
        int rowCount = 0;
        int pageCount = 0;
        int totalCount = 0;
        int startSlot = _config.pageSlots * (pageNum - 1);
        for (int i = 0; i < _config.shop.Count; i++)
        {
            if (i < startSlot)
            {
                continue;
            }

            rowCount++;
            pageCount++;

            msg += $"{_config.shop[i].GoodsName()}  ";

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

        if (totalCount < _config.shop.Count)
        {
            msg += $"\n[c/96FF0A:输入 /fish list {pageNum + 1} 查看更多.]";
        }

        if (msg == "")
        {
            msg = "今天卖萌，不卖货！ɜː";
        }
        else
        {
            msg = $"[c/96FF0A:欢迎光临【{_config.name}】,货架 ({pageNum}/{totalPage}): ]\n" + msg;
        }

        if (args.Player != null)
        {
            args.Player.SendInfoMessage(msg);
        }
        else
        {
            utils.Log(msg);
        }
    }

}