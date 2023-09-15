using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using TShockAPI;

namespace FishShop.Shop;

public class DirtiestItem : ShopItem
{
    public Point posDirt = new();
    public Point posPoop = new();

    public DirtiestItem(ShopItemData si) : base(si)
    {
    }

    public override string CanBuy()
    {
        var msg = base.CanBuy();
        if (msg != "") return msg;

        if (!CheckDirtiestMatrix(op)) return "在你的周围未能找到臭臭矩阵！(7x7中空)";
        return "";
    }

    public override void ProvideGoods()
    {
        bool flag = FindDirtest();
        MoveDirtest(flag);
        TSPlayer.All.SendInfoMessage($"{op.Name} 正在举行 [i:5395]臭臭仪式[i:5395]");
        if (flag)
            op.SendSuccessMessage("臭臭仪式完成，[i:5400]最脏的块 已生成(σﾟ∀ﾟ)σ");
        else
            op.SendErrorMessage("糟糕，找遍整个世界都没有发现 [i:5400]最脏的块 o(´^｀)o");
    }


    /// <summary>
    /// 玩家附近是否有 臭臭矩阵
    /// </summary>
    /// <param name="op"></param>
    /// <returns></returns>
    bool CheckDirtiestMatrix(TSPlayer op)
    {
        Rectangle rect = utils.GetScreen(op);
        for (int x = rect.X; x < rect.Right; x++)
        {
            for (int y = rect.Y; y < rect.Bottom; y++)
            {
                ITile tile = Main.tile[x, y];
                if (!tile.active()) continue;
                if (tile.type == TileID.PoopBlock)
                {
                    if (FindDirtestMatrix(x, y))
                    {
                        posPoop = new Point(x, y);
                        return true;
                    }
                }
            }
        }
        return false;
    }


    /// <summary>
    /// 查找 臭臭矩阵
    /// </summary>
    bool FindDirtestMatrix(int tileX, int tileY)
    {
        int x;
        int y;
        for (int i = 0; i < 49; i++)
        {
            x = tileX + i % 7;
            y = tileY + i / 7;

            ITile tile = Main.tile[x, y];
            if (i == 24)
            {
                if (tile.active()) return false;
            }
            else
            {
                if (!tile.active()) return false;
                if (tile.type != TileID.PoopBlock) return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 查找 是否有 最脏的块
    /// </summary>
    bool FindDirtest()
    {
        for (int x = 0; x < Main.maxTilesX; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                ITile tile = Main.tile[x, y];
                if (!tile.active()) continue;
                if (tile.type == TileID.DirtiestBlock)
                {
                    posDirt = new Point(x, y);
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// 转移 最脏的块 到臭臭矩阵中心
    /// </summary>
    void MoveDirtest(bool needSuccess)
    {
        int x;
        int y;
        // 清理图格
        for (int i = 0; i < 49; i++)
        {
            x = posPoop.X + i % 7;
            y = posPoop.Y + i / 7;


            if (i == 24)
            {
                ITile tile = Main.tile[x, y];
                tile.type = needSuccess ? TileID.DirtiestBlock : TileID.Dirt;
                tile.active(true);
                tile.slope(0);
                tile.halfBrick(false);
                NetMessage.SendTileSquare(-1, x, y);
            }
            else
            {
                ClearTile(x, y);
            }
        }

        utils.Log($"true: {posDirt.X} {posDirt.Y}");
        // 清除最脏块
        if (needSuccess)
        {
            ClearTile(posDirt.X, posDirt.Y);
            utils.Log($"{posDirt.X} {posDirt.Y}");
        }
    }

    static void ClearTile(int x, int y)
    {
        Main.tile[x, y].ClearTile();
        NetMessage.SendTileSquare(-1, x, y);
    }


}