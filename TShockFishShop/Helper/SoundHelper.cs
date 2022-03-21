using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using TShockAPI;

namespace FishShop
{
    public class SoundHelper
    {
        public static void PlayChat(TSPlayer op)
        {
            // MessageBuffer.cs
            // NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(player.TPlayer.position, 1, 0, 10, -16));
            // Main.musicVolume
            // Main.soundVolume
            // Main.ambientVolume
            // logSoundInfo();
            // item 59 232 59  
            // item166 179, 166
            // item130 140, 130
            // NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(op.TPlayer.position, 0, SoundID.Chat));
        }
        public static void PlayCoins(TSPlayer op)
        {
            // NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(op.TPlayer.position, 4, 42, 10, 2));
            // NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(op.TPlayer.position, 179 , 166));
        }

        private static void logSoundInfo()
        {
            // track：SoundID  public static readonly LegacySoundStyle Item1 = new LegacySoundStyle(2, 1);
            // Dictionary<ushort, LegacySoundStyle> ret3 = new Dictionary<ushort, LegacySoundStyle>();
			// ushort nextIndex = 0;
			// List<FieldInfo> list = (from f in typeof(SoundID).GetFields(BindingFlags.Static | BindingFlags.Public)
			// 	where f.FieldType == typeof(LegacySoundStyle)
			// 	select f).ToList();
			// list.Sort((FieldInfo a, FieldInfo b) => string.Compare(a.Name, b.Name));
			// list.ForEach(delegate(FieldInfo field)
			// {
			// 	ret3[nextIndex] = (LegacySoundStyle)field.GetValue(null);
            //     LegacySoundStyle style = ret3[nextIndex];
			// 	nextIndex++;
            //     Console.WriteLine($"{style.SoundId},{style._style} = {nextIndex}, {style._style} soudid:{style.SoundId} style:{style._style} type:{style._type}");
			// });
        }
    }
}