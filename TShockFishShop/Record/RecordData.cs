using System.ComponentModel;

namespace FishShop
{
    public class RecordData
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [DefaultValue("")]
        public string name = "";
        
        /// <summary>
        /// 商品id
        /// </summary>
        public int id = 0;
        
        /// <summary>
        /// 购买次数
        /// </summary>
        public int count = 0;


        public RecordData(string _name, int _id, int _count)
        {
            id = _id;
            count = _count;

            name = _name;
        }

        public void Record(int num)
        {
            count += num;
        }
    }
}