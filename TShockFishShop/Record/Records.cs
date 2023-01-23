using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TShockAPI;

namespace FishShop.Record
{
    /// <summary>
    /// 购买记录
    /// </summary>
    public class Records
    {
        static int lastSave = 0;
        static bool isLoaded;
        static RecordFile _config;

        public static string RecodFile = "";

        /// <summary>
        /// 加载
        /// </summary>
        public static void Load(bool forceLoad = false)
        {
            if (!isLoaded || forceLoad)
            {
                _config = RecordFile.Load(RecodFile);
                isLoaded = true;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private static void Save()
        {
            if (!isLoaded) return;

            // 保存间隔在2s以上
            if (utils.GetUnixTimestamp - lastSave > 2)
            {
                lastSave = utils.GetUnixTimestamp;
                File.WriteAllText(RecodFile, JsonConvert.SerializeObject(_config, Formatting.Indented));
            }
        }

        /// <summary>
        /// 记录购买情况
        /// </summary>
        public static void Record(TSPlayer op, ShopItem shopItem, int amount, int costMoney, int costFish)
        {
            // 加载
            Load();

            int index = -1;
            for (int i = 0; i < _config.player.Count; i++)
            {
                if (_config.player[i].name == op.Name)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                _config.player[index].costMoney += costMoney;
                _config.player[index].costFish += costFish;
                List<RecordData> datas = _config.player[index].datas;

                index = -1;
                for (int i = 0; i < datas.Count; i++)
                {
                    if (datas[i].id == shopItem.id)
                    {
                        index = i;
                        break;
                    }
                }


                if (index != -1)
                {
                    datas[index].Record(amount);
                }
                else
                {
                    datas.Add(new RecordData(shopItem.name, shopItem.id, amount));
                }

            }
            else
            {
                PlayerRecordData pd = new(op.Name);
                pd.costMoney += costMoney;
                pd.costFish += costFish;
                pd.datas.Add(new RecordData(shopItem.name, shopItem.id, amount));
                _config.player.Add(pd);
            }

            // 保存
            Save();
        }

        /// <summary>
        /// 获得某个玩家的购买某件商品的次数
        /// </summary>
        /// <param name="op"></param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetPlayerRecord(TSPlayer op, int goodsID)
        {
            Load();
            foreach (PlayerRecordData pd in _config.player)
            {
                if (pd.name == op.Name)
                {
                    foreach (RecordData d in pd.datas)
                    {
                        if (d.id == goodsID)
                            return d.count;
                    }
                    break;
                }
            }
            return -1;
        }

        /// <summary>
        /// 计算单件商品的售出数量
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int CountShopItemRecord(int goodsID)
        {
            Load();

            int count = -1;
            foreach (PlayerRecordData pd in _config.player)
            {
                foreach (RecordData d in pd.datas)
                {
                    if (d.id == goodsID) count += d.count;
                }
            }
            return count;
        }

        /// <summary>
        /// 重置记录
        /// </summary>
        public static void ResetRecord()
        {
            Load();
            _config.player.Clear();
            Save();
        }

        /// <summary>
        /// 显示消费排行
        /// </summary>
        /// <param name="op"></param>
        public static void ShowRank(CommandArgs args)
        {
            Load();
            var lists = _config.player.Where(obj => obj.costMoney > 0).OrderByDescending(obj => obj.costMoney).ToList();
            TSPlayer op = args.Player;

            if (lists.Count == 0)
            {
                op.SendInfoMessage("暂无 消费榜 数据");
                return;
            }

            if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, op, out int pageNumber))
                return;

            List<string> lines = new();
            for (int i = 0; i < lists.Count; i++)
            {
                lines.Add($"第{i + 1}名, {lists[i].name}, {utils.GetMoneyDesc(lists[i].costMoney)}");
            }

            PaginationTools.SendPage(op, pageNumber, lines, new PaginationTools.Settings
            {
                HeaderFormat = "[c/96FF0A:『消费榜』({0}/{1})：]",
                FooterFormat = "输入 /fish rank {{0}} 查看更多".SFormat(Commands.Specifier)
            });
        }

        /// <summary>
        /// 鱼篓
        /// </summary>
        public static void ShowBasket(CommandArgs args)
        {
            Load();
            var lists = _config.player.Where(obj => obj.costFish > 0).OrderByDescending(obj => obj.costFish).ToList();
            TSPlayer op = args.Player;

            if (lists.Count == 0)
            {
                op.SendInfoMessage("暂无 鱼篓榜 数据");
                return;
            }


            if (!PaginationTools.TryParsePageNumber(args.Parameters, 2, op, out int pageNumber))
                return;

            List<string> lines = new();
            for (int i = 0; i < lists.Count; i++)
            {
                lines.Add($"第{i + 1}名, {lists[i].name}, {lists[i].costFish}条");
            }

            PaginationTools.SendPage(op, pageNumber, lines, new PaginationTools.Settings
            {
                HeaderFormat = "[c/96FF0A:『鱼篓榜』({0}/{1})：]",
                FooterFormat = "输入 /fish basket {{0}} 查看更多".SFormat(Commands.Specifier)
            });
        }
    }
}