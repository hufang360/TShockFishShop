using Terraria;
using TShockAPI;
using System;
using System.Reflection;


namespace Plugin
{
    class Compatible
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.missingmethodexception?view=netframework-4.7.2
        // https://docs.microsoft.com/zh-cn/dotnet/api/system.type.invokemember?view=netframework-4.7.2
        public static string CheckUnlockDownedDeerclops()
        {
            string s="";
            try
            {
                Object result = typeof(NPC).InvokeMember("downedDeerclops", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, null, null);
                if( !(bool)result ) s = "未击败 鹿角怪";
            }
            catch (MissingMethodException e)
            {
                Console.WriteLine("Unable to call the DoSomething method: {0}", e.Message);
            }
            return s;
        }

    }
}