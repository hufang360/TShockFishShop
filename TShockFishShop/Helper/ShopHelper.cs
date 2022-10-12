using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using TShockAPI;

namespace FishShop
{
    public class ShopItem
    {
        public string name = "";    // 物品名
        public int id = 0;          // 物品id
        public int stack = 1;       // 堆叠
        public string prefix = "";  // 词缀

        public int limit = 0;       // 玩家限量
        public int serverLimit = 0;      // 全服限量

        public string comment = "";      // 商品备注

        public List<string> allowGroup = new List<string>();   // 允许某个组可购买

        public List<ItemData> unlock = new List<ItemData>();  // 解锁条件
        public List<ItemData> cost = new List<ItemData>();    // 支付条件

        public List<string> cmds = new List<string>();        // 指令清单
        public List<int> buffs = new List<int>();             // buff 清单
        public List<int> seconds = new List<int>();           // buff 持续时间


        [JsonIgnore]
        private SetShopData _settings = new SetShopData();   // 预设置

        [JsonIgnore]
        public Point posDirt = new Point();
        [JsonIgnore]
        public Point posPoop = new Point();

        //public ShopItem(string _name = "", int _id = 0, int _stack = 1, string _prefixOrName = "")
        //{
        //    name = _name;
        //    id = _id;
        //    stack = _stack;
        //    prefix = _prefixOrName;
        //    FillingSettings();
        //}


        public void Filling()
        {
            if (id == 0) id = IDSet.GetIDByName(name);

            if (stack == 0) stack = 1;

            FillingSettings();

            if (id == ShopItemID.RawCmd)
            {
                if (cmds.Count > 0)
                {
                    if (string.IsNullOrEmpty(name))
                        name = "指令";
                }
                else if (!string.IsNullOrEmpty(prefix))
                    name = string.Format($"{_settings.name}{prefix}");
            }
            else if (id == ShopItemID.Buff)
            {
                if (string.IsNullOrEmpty(name))
                    name = "增益";
            }

            // 执行指令
            if (id != ShopItemID.RawCmd)
                prefix = utils.AffixNameToPrefix(prefix).ToString();
        }

        private void FillingSettings()
        {
            if (Settings.shopItem.ContainsKey(id))
                _settings = Settings.shopItem[id];
        }

        //public void AddUnlock(string _name = "", int _id = 0, int _stack = 1)
        //{
        //    unlock.Add(new ItemData(_name, _id, _stack));
        //}

        //public void AddCost(string _name = "", int _id = 0, int _stack = 1)
        //{
        //    cost.Add(new ItemData(_name, _id, _stack));
        //}

        public string GetItemDesc()
        {
            if (id == 0) Filling();
            if (id == ShopItemID.RawCmd)
            {
                if (cmds.Count > 0)
                {
                    // 如果名字是空的才取第一条指令做名字
                    if (string.IsNullOrEmpty(name))
                        return "指令";
                    else
                        return name;
                }
                else if (!string.IsNullOrEmpty(prefix))
                    return string.Format($"{_settings.name}{prefix}");
            }
            else if (id == ShopItemID.Buff)
            {
                if (string.IsNullOrEmpty(name))
                    return "增益";
                else
                    return name;
            }
            return utils.GetItemDesc(id, stack, prefix, this);
        }

        public string GetIcon()
        {
            return _settings.icon == 0 ? "" : $"[i:{_settings.icon}]";
        }

        public int BuyMax()
        {
            if (ShopItemID.GetRealClearNPCID(id) != 0) return 1;
            return _settings.buyMax;
        }

        public bool DeadCanBuyItem()
        {
            if (ShopItemID.GetRealBuffID(id) != 0) return false;
            return !_settings.needAlive;
        }

        public bool DayCanBuyItem()
        {
            return !_settings.needNight;
        }

        public bool NightCanBuyItem()
        {
            return !_settings.needDay;
        }

        public List<int> GetBuff()
        {
            if (id == ShopItemID.Buff) return buffs;
            return _settings.buffs;
        }

        public List<int> GetBuffSecond()
        {
            if (id == ShopItemID.Buff) return seconds;
            return _settings.seconds;
        }

        // 商品说明
        public string GetComment()
        {
            if (!string.IsNullOrEmpty(_settings.comment))
            {
                if (!string.IsNullOrEmpty(comment))
                    return $"{_settings.comment}, {comment}";
                else
                    return _settings.comment;
            }
            else
                return comment;
        }

        // 指令商品的 指令内容
        public List<string> GetCMD()
        {
            if (id == ShopItemID.RawCmd) return cmds;
            return _settings.cmds;
        }

        // 汇总要减扣的物品
        // 货币不计算在内
        // 任意类的物品不计算在内
        public List<ItemData> GetCostItem(int amount)
        {
            // 取出要扣除的物品id
            List<ItemData> _items = new List<ItemData>();
            foreach (ItemData d in cost)
            {
                int _stack = d.stack * amount;
                int _id = d.id;
                if (_id == 0 || _id > Main.maxItemTypes)
                {
                    utils.Log($"[CheckCost]物品id{_id}无效");
                    continue;
                }

                // 跳过 铜、银、金、铂金
                if (_id == 71 || _id == 72 || _id == 73 || _id == 74)
                    continue;

                // 任务鱼
                if (_id == UnlockID.ItemIDQuestFish)
                {
                    _id = Main.anglerQuestItemNetIDs[Main.anglerQuest];
                }

                _items.Add(new ItemData("", _id, _stack));
            }
            return _items;
        }

        public string GetAnyItemDesc()
        {
            // 取出要扣除的物品id
            List<ItemData> _costItems = new List<ItemData>();
            string msg = "";
            foreach (ItemData data in cost)
            {
                if (data.id == 0)
                {
                    switch (data.name)
                    {
                        // 任务鱼可以确定一个id
                        // 其它物品可能得到多个id
                        case "任务鱼":
                            _costItems.Add(new ItemData("", Main.anglerQuestItemNetIDs[Main.anglerQuest], 1));
                            break;

                        case "宝匣":
                        case "任意宝匣":
                            msg += "任意宝匣[i:2334]";
                            break;

                        case "金色小动物":
                        case "任意金色小动物":
                            msg += "任意金色小动物[i:2889]";
                            break;

                        case "任意墓碑":
                            msg += "任意墓碑[i:321]";
                            break;
                    }
                }
            }
            return msg;
        }


        public ItemData GetOneCostItem(List<ItemData> _data, int _id)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].id == _id)
                    return _data[i];
            }
            return new ItemData();
        }

        // 计算金钱消耗
        public int GetCostMoney(int goodsAmount)
        {
            int amount = 0;
            foreach (ItemData data in cost)
            {
                if (data.id == 0)
                {
                    switch (data.name)
                    {
                        case "铜币":
                        case "铜":
                            id = 71;
                            break;

                        case "银币":
                        case "银":
                            id = 72;
                            break;

                        case "金币":
                        case "金":
                            id = 73;
                            break;

                        case "铂金币":
                        case "铂金":
                        case "铂":
                            id = 74;
                            break;
                    }
                }
                switch (data.id)
                {
                    case 71: amount += data.stack; break;
                    case 72: amount += data.stack * 100; break;
                    case 73: amount += data.stack * 10000; break;
                    case 74: amount += data.stack * 1000000; break;
                }
            }
            return amount * goodsAmount;
        }

        public List<string> GetCostCMD()
        {
            // 列出要执行的指令
            List<string> cmds = new List<string>();
            foreach (ItemData d in cost)
            {
                if (d.id == ShopItemID.RawCmd && !string.IsNullOrEmpty(d.cmd))
                {
                    cmds.Add(d.cmd);
                }
            }
            return cmds;
        }

        public int GetPrefixInt()
        {
            if (int.TryParse(prefix, out int num))
                return num;
            else
                return utils.AffixNameToPrefix(prefix);
        }


        public string GetCostDesc(int amount = 1)
        {
            // 钱
            string msg = utils.GetMoneyDesc(GetCostMoney(amount));

            // 物品
            List<ItemData> costItems = GetCostItem(amount);
            List<string> msgs = new List<string>();
            if (!string.IsNullOrEmpty(msg))
                msgs.Add(msg);

            foreach (ItemData _d in costItems)
            {
                msgs.Add(_d.GetItemDesc());
            }
            msg = string.Join("", msgs);

            // 执行指令
            List<string> cmds = GetCostCMD();
            if (cmds.Count > 0)
                msg += " 执行" + string.Join(",", GetCostCMD());

            // 任意类型的物品
            // string s = GetAnyItemDesc();
            // if( s!="" )
            //     msg += $"{s} ";
            if( id==ShopItemID.DirtiestBlock )
            {
                msg += ", 臭臭仪式";
            }
            return msg;
        }

        public string GetUnlockDesc()
        {
            string msg = "";
            string s = "";
            foreach (ItemData d in unlock)
            {
                if (msg != "")
                    s = "、";
                msg += $"{s}{GetOneUnlockDesc(d)}";
            }

            return msg;
        }

        public string GetAllowGroupDesc()
        {
            if (allowGroup.Count > 0)
                return $"仅 {string.Join("、", allowGroup)} 用户组";
            else
                return "";
        }


        // 显示解锁条件
        private string GetOneUnlockDesc(ItemData re)
        {
            string s = IDSet.GetNameByID(re.id);

            // 完成钓鱼任务
            if (re.id == UnlockID.FishQuestCompleted)
                s = string.Format(s, re.stack);

            return s;
        }


        // 获得限购信息
        public string GetLimitDesc(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
                return "";

            int count1 = Math.Max(0, LimitHelper.GetPlayerRecord(playerName, id));
            int count2 = Math.Max(0, LimitHelper.GetServerRecord(id));
            List<string> msgs = new List<string>();
            if (limit > 0)
                msgs.Add($"个人 {count1}/{limit}");
            if (serverLimit > 0)
                msgs.Add($"全服 {count2}/{serverLimit}");

            return string.Join(", ", msgs);
        }

        // 限购检查
        public bool CheckLimitCanBuy(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
                return true;

            int count1 = LimitHelper.GetPlayerRecord(playerName, id);
            if (limit > 0)
            {
                if (limit - count1 > 0)
                    return true;
                else
                    return false;
            }

            int count2 = LimitHelper.GetServerRecord(id);
            if (serverLimit > 0)
            {
                if (serverLimit - count2 > 0)
                    return true;
                else
                    return false;
            }

            return true;
        }


        #region 最脏的块

        /// <summary>
        /// 玩家附近是否有 臭臭矩阵
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public bool CheckDirtiestMatrix(TSPlayer op)
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
        private bool FindDirtestMatrix(int tileX, int tileY)
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
        /// <returns></returns>
        public bool FindDirtest()
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
        public void MoveDirtest(bool needSuccess)
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

        private static void ClearTile(int x, int y)
        {
            Main.tile[x, y].ClearTile();
            NetMessage.SendTileSquare(-1, x, y);
        }
        #endregion
    }


    public class ShopItem2
    {
        public int serial = 0;

        public ShopItem item;
    }

}