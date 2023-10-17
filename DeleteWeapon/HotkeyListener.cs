using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeleteWeapon.Actions;

namespace DeleteWeapon
{
    internal class HotkeyListener
    {
        public static void Listen()
        {
            while (true)
            {
                Rage.GameFiber.Yield();
                if (Plugin.settings.DeleteEquippedWeaponKey != null
                && Game.IsKeyDown((Keys)Plugin.settings.DeleteEquippedWeaponKey)
                && (Plugin.settings.DeleteEquippedWeaponModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)Plugin.settings.DeleteEquippedWeaponModifierKey)))
                {
                    WeaponActions.DeleteWeaponByHotkey();
                }
                if (Plugin.settings.DeleteNearestVehicleKey != null
                    && Game.IsKeyDown((Keys)Plugin.settings.DeleteNearestVehicleKey)
                    && (Plugin.settings.DeleteNearestVehicleModifierModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)Plugin.settings.DeleteNearestVehicleModifierModifierKey)))
                {
                    VehicleActions.DeleteVehicleByHotkey();
                }
            }
        }

        internal static bool ConfirmationTask(string starter)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            var msg = InfoDisplay.Notify($"Awaiting confirmation for {starter}. You have 10 seconds to confirm this action.");

            while (s.Elapsed <= TimeSpan.FromSeconds(10))
            {
                Rage.GameFiber.Yield();

                if (Game.IsKeyDown((Keys)Plugin.settings.YesKey)
                    && (Plugin.settings.YesModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)Plugin.settings.YesModifierKey)))
                {
                    Game.RemoveNotification(msg);
                    return true;
                }

                if (Game.IsKeyDown((Keys)Plugin.settings.NoKey)
                    && (Plugin.settings.NoModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)Plugin.settings.NoModifierKey)))
                {
                    Game.RemoveNotification(msg);
                    return false;
                }
            }
            Game.RemoveNotification(msg);
            return false;
        }

    }
}
