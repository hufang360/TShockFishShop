using System.ComponentModel;

namespace FishShop
{
    public class LimitData
    {
        [DefaultValue("")]
        public string name = "";
        public int id = 0;
        public int count = 0;
        public int total = 0;

        public LimitData(string _name, int _id, int _count)
        {
            name = _name;
            id = _id;
            count = _count;
            total = _count;

        }

        public void Record(int num)
        {
            count += num;
            total += num;
        }
    }
}