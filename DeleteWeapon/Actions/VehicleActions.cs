using Rage.Attributes;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace DeleteWeapon.Actions
{
    internal static class VehicleActions
    {
        private static Vehicle ClosestVeh
        {
            get
            {
                return (Vehicle)World.GetClosestEntity(Game.LocalPlayer.Character.Position, 6.0f, GetEntitiesFlags.ConsiderAllVehicles);
            }
        }

        internal static void DeleteVehicleByHotkey()
        {
            var v = ClosestVeh;
            if ((Game.LocalPlayer.Character.IsInAnyVehicle(true)
                    && Game.LocalPlayer.Character.CurrentVehicle == ClosestVeh
                    && (!Plugin.settings.ConfirmPlayerVehicleDeletion
                        || HotkeyListener.ConfirmationTask("own vehicle deletion")))
                || (!Plugin.settings.ConfirmVehicleDeletion
                        || HotkeyListener.ConfirmationTask("vehicle deletion")))
            {
                DeleteVehicle(v);
            }
        }

        [ConsoleCommand]
        public static void DeleteVehicle()
        {
            try
            {
                if (ClosestVeh.HasDriver && ClosestVeh.Driver != Game.LocalPlayer.Character)
                {
                    ClosestVeh.Driver.Delete();
                }

                ClosestVeh.Delete();
            }
            catch
            {
                InfoDisplay.Notify("Can't get/delete closest vehicle, try again!");
            }
        }

        private static void DeleteVehicle(Vehicle vehicle)
        {
            try
            {
                if (vehicle.HasDriver && vehicle.Driver != Game.LocalPlayer.Character)
                {
                    vehicle.Driver.Delete();
                }

                vehicle.Delete();
            }
            catch
            {
                InfoDisplay.Notify("Can't get/delete vehicle, try again!");
            }
        }

    }
}
