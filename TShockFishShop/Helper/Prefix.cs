using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace FishShop.Helper;

public class Prefix
{
    static readonly Dictionary<int, string> _prefixs = new()
    {
        {1, "大"},
        {2, "巨大"},
        {3, "危险"},
        {4, "凶残"},
        {5, "锋利"},
        {6, "尖锐"},
        {7, "微小"},
        {8, "可怕"},
        {9, "小"},
        {10, "钝"},
        {11, "倒霉"},
        {12, "笨重"},
        {13, "可耻"},
        {14, "重"},
        {15, "轻"}, //莱特
        {16, "精准"},
        {17, "迅速"},
        {18, "急速"},
        {19, "恐怖"},
        {20, "致命"},
        {21, "可靠"},
        {22, "可畏"},
        {23, "无力"},
        {24, "粗笨"},
        {25, "强大"},
        {26, "神秘"},
        {27, "精巧"},
        {28, "精湛"},
        {29, "笨拙"},
        {30, "无知"},
        {31, "错乱"},
        {32, "威猛"},
        {33, "禁忌"},
        {34, "天界"},
        {35, "狂怒"},
        {36, "锐利"},
        {37, "高端"},
        {38, "强力"},
        {39, "碎裂"},
        {40, "破损"},
        {41, "粗劣"},
        {42, "迅捷"},
        {43, "致命2"},
        {44, "灵活"},
        {45, "灵巧"},
        {46, "残暴"},
        {47, "缓慢"},
        {48, "迟钝"},
        {49, "呆滞"},
        {50, "惹恼"},
        {51, "凶险"},
        {52, "狂躁"},
        {53, "致伤"},
        {54, "强劲"},
        {55, "粗鲁"},
        {56, "虚弱"}, // 软弱
        {57, "无情"},
        {58, "暴怒"},
        {59, "神级"},
        {60, "恶魔"},
        {61, "狂热"},
        {62, "坚硬"},
        {63, "守护"},
        {64, "装甲"},
        {65, "护佑"},
        {66, "奥秘"},
        {67, "精确"},
        {68, "幸运"},
        {69, "锯齿"},
        {70, "尖刺"},
        {71, "愤怒"},
        {72, "险恶"},
        {73, "轻快"},
        {74, "快速"},
        {75, "急速2"},
        {76, "迅捷2"},
        {77, "狂野"},
        {78, "鲁莽"},
        {79, "勇猛"},
        {80, "暴力"},
        {81, "传奇"},
        {82, "虚幻"},
        {83, "神话"},
        {84, "传奇2"}, // 泰拉悠悠球
    };


    public static int GetPrefix(string idOrName)
    {
        if (int.TryParse(idOrName, out int num))
        {
            if (num <= 84 && num > 0)
            {
                return num;
            }
            else
            {
                return 0;
            }
        }
        switch (idOrName)
        {
            case "莱特": return 15;
            case "软弱": return 56;
        }
        var li = _prefixs.Where(obj => obj.Value == idOrName);
        if (li.Any())
        {
            return li.First().Key;
        }
        return 0;
    }


    public static string GetName(int prefix)
    {
        if (_prefixs.ContainsKey(prefix))
        {
            return _prefixs[prefix];
        }
        return "";
    }


    public static bool CanHavePrefixes(Item item)
    {
        if (item.type != 0)
        {
            if (item.damage <= 0)
            {
                return item.accessory;
            }
            return true;
        }
        return false;
    }
}
