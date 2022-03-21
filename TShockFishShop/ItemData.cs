namespace FishShop
{
    public class ItemData
    {
        public string name = "";
        public int id = 0;
        public int stack = 1;

        public ItemData(string _name="", int _id = 0, int _stack=1 )
        {
            name = _name;
            id = _id;
            stack = _stack;
        }

        public void fixIDByName()
        {
            if( id==0 && name != "" )
                id = IDSet.GetIDByName(name);
        }

        public string GetItemDesc(){
            if( id==0 )
                fixIDByName();
            return MyUtils.GetItemDesc( "", id, stack );
        }

    }
}