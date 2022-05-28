namespace FishShop
{
    public class ItemData
    {
        public string name = "";

        public int id = 0;

        public int stack = 1;

        public string cmd = "";

        public ItemData(string _name = "", int _id = 0, int _stack = 1)
        {
            name = _name;
            id = _id;
            stack = _stack;
        }

        public void FixIDByName()
        {
            if (id == 0 && name != "")
                id = IDSet.GetIDByName(name);
            if (stack == 0)
                stack = 1;
        }

        public string GetItemDesc()
        {
            if (id == 0)
                FixIDByName();
            return utils.GetItemDesc(id, stack, cmd);
        }
    }
}