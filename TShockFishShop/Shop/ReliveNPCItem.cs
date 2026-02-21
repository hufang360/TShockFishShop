using Terraria;
using TShockAPI;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;

namespace FishShop.Shop;


/// <summary>
/// 复活NPC
/// </summary>
public class ReliveNPCItem(ShopItemData si) : ShopItem(si)
{
    public override string CanBuy()
    {
        var msg = base.CanBuy();
        if (msg != "") return msg;

        if (!NeedBuyReliveNPC())
        {
            return "入住过的NPC都活着，无需购买!";
        }

        return "";
    }

    public override void ProvideGoods()
    {
        ReliveNPC(op);
    }

    public static void ReliveNPC(TSPlayer player)
    {
        List<int> found = GetRelive();

        for (int i = 0; i < Main.maxNPCs; i++)
        {
            if (!Main.npc[i].active || !Main.npc[i].townNPC || !found.Contains(Main.npc[i].type)) continue;
            found.Remove(Main.npc[i].type);
        }

        // 生成npc
        List<string> names = [];
        foreach (int npcID in found)
        {
            NPC npc = new();
            npc.SetDefaults(npcID);
            TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, 1, player.TileX, player.TileY, 5, 2);

            if (names.Count != 0 && names.Count % 10 == 0)
                names.Add("\n" + npc.FullName);
            else
                names.Add(npc.FullName);
        }

        if (found.Count > 0)
        {
            TSPlayer.All.SendInfoMessage($"{player.Name} 复活了 {found.Count}个 NPC:");
            TSPlayer.All.SendInfoMessage($"{string.Join("、", names)}");
        }
        else
        {
            player.SendInfoMessage("入住过的NPC都活着");
        }
    }


    private static bool NeedBuyReliveNPC()
    {
        List<int> found = GetRelive();

        for (int i = 0; i < Main.maxNPCs; i++)
        {
            if (!Main.npc[i].active || !Main.npc[i].townNPC || !found.Contains(Main.npc[i].type)) continue;
            found.Remove(Main.npc[i].type);
        }

        if (found.Count == 0)
        {
            return false;
        }
        return true;
    }

    private static List<int> GetRelive()
    {
        List<int> found = [];

        // 怪物图鉴解锁情况
        List<int> remains = [
            17, // 商人
            18, // 护士
            19, // 军火商
            20, // 树妖
            22, // 向导
            38, // 爆破专家
            54, // 服装商 
            107, // 哥布林工匠
            108, // 巫师
            124, // 机械师
            160, // 松露人
            178, // 蒸汽朋克人
            207, // 染料商
            208, // 派对女孩
            209, // 机器侠
            227, // 油漆工
            228, // 巫医
            229, // 海盗
            353, // 发型师
            369, // 渔夫
            441, // 税收官
            550, // 酒馆老板
            588, // 高尔夫球手
            633, // 动物学家
            663, // 公主
            637, // 猫咪
            638, // 狗狗
            656, // 兔兔
            670, // 呆瓜史莱姆
            678, // 冷酷史莱姆
            679, // 年长史莱姆
            680, // 笨拙史莱姆
            681, // 唱将史莱姆
            682, // 粗暴史莱姆
            683, // 神秘史莱姆
            684, // 侍卫史莱姆
            // 453, //骷髅商人
            // 368, //旅商
            // 37, // 老人
        ];
        if (Main.xMas) remains.Add(142); // 142, //圣诞老人

        foreach (int id in remains)
        {
            if (DidDiscoverBestiaryEntry(id)) found.Add(id);
        }
        return found;
    }

    private static bool DidDiscoverBestiaryEntry(int npcId)
    {
        return Main.BestiaryDB.FindEntryByNPCID(npcId).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
    }

}