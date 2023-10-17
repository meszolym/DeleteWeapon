using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteWeapon
{
    internal static class InfoDisplay
    {
        internal static uint Notify(string text)
        {
            var msg = Game.DisplayNotification("commonmenu", "shop_gunclub_icon_b", $"DeleteWeapon v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}", "by meszolym", text);
            return msg;
        }

        internal static void Log(string text)
        {
            Game.LogTrivial(text);
            Game.Console.Print(text);
        }
    }
}
