using Terraria;
using TShockAPI;
using Microsoft.Xna.Framework;

namespace FishShop.Shop;


/// <summary>
/// 灵犀飞鱼
/// </summary>
public class OneDamageItem(ShopItemData si) : ShopItem(si)
{
    public override string CanBuy()
    {
        var msg = base.CanBuy();
        if (msg != "") return msg;

        if (!NPCHelper.AnyBoss())
        {
            return "当前无任何boss存在！";
        }

        return "";
    }

    public override void ProvideGoods()
    {
        for (int i = 0; i < Main.npc.Length; i++)
        {
            NPC npc = Main.npc[i];
            if (npc != null && npc.active && npc.boss)
            {
                AttackBoss(op, npc);
                TSPlayer.All.SendInfoMessage($"{op.Name} 购买了{shopItemData.GetItemDesc()}，对boss造成了1点点伤害！");
            }
        }
    }


    /// <summary>
    /// 模拟玩家攻击boss
    /// </summary>
    /// <param name="op">玩家对象</param>
    /// <param name="npc">boss的npc对象</param>
    public static void AttackBoss(TSPlayer op, NPC npc)
    {
        float rawX = op.TPlayer.position.X;
        float rawY = op.TPlayer.position.X;
        // 传送到boss身边，不然射弹不会生成
        op.Teleport(npc.position.X, npc.position.Y);

        // itemid projectileid
        // 279 48 投刀
        // 3197 520 灵犀飞鱼
        int itemid = 3197;
        int pjid = 520;

        Item item = new();
        item.SetDefaults(itemid);
        Vector2 pos = new(npc.position.X + (float)(npc.width * 0.5), npc.position.Y + (float)(npc.height * 0.5));
        Vector2 vel = new(20, 0);
        int pIndex = Projectile.NewProjectile(op.TPlayer.GetProjectileSource_Item(item), pos, vel, pjid, 1000, 0f, op.Index);
        Main.projectile[pIndex].ai[0] = 2f;
        Main.projectile[pIndex].timeLeft = 10;
        Main.projectile[pIndex].friendly = true;
        Main.projectile[pIndex].penetrate = 3;
        Main.projectile[pIndex].ranged = true;
        Main.projectile[pIndex].coldDamage = true;
        NetMessage.SendData(27, -1, -1, null, pIndex);


        // 回到原坐标
        // op.Teleport(rawX, rawY);

    }


}