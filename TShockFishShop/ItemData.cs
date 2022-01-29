namespace Plugin
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

            if( _id==0 && _name != "" ){
                id = IDSet.GetIDByName(_name);
            }
                
        }

        public void fixIDByName()
        {
            if( id==0 && name != "" ){
                id = IDSet.GetIDByName(name);
            }
        }

        public string GetItemDesc(){
            return MyUtils.GetItemDesc( "", id, stack );
        }

    }
}