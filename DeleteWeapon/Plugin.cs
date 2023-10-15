using Rage;
using Rage.Attributes;
using Rage.ConsoleCommands.AutoCompleters;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("DeleteWeapon", Description = "This plugin lets you delete weapons", Author = "meszolym")]


namespace DeleteWeapon
{
    public static class Plugin
    {
        public static Ped Player
        {
            get
            {
                return Game.LocalPlayer.Character;
            }
        }

        public static Vehicle ClosestVeh
        {
            get
            {
                return (Vehicle)World.GetClosestEntity(Player.Position, 6.0f, GetEntitiesFlags.ConsiderAllVehicles);
            }
        }

        private static IniModel settings;

        private static uint Notify(string text)
        {
            var msg = Game.DisplayNotification("commonmenu","shop_gunclub_icon_b", $"DeleteWeapon v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}", "by meszolym", text);
            return msg;
        }

        public static void Main()
        {

            settings = IniModel.Load("Plugins\\DeleteWeapon.ini");

            Notify($"Plugin loaded!\n" +
                "You can now use the hotkeys specified in the ini, and also the commands (they start with Delete)");

            

            GameFiber.StartNew(MainThread);
        }

        public static void MainThread()
        {
            while (true)
            {
                Rage.GameFiber.Yield();

                if (settings.DeleteEquippedWeaponKey != null
                    && Game.IsKeyDown((Keys)settings.DeleteEquippedWeaponKey)
                    && (settings.DeleteEquippedWeaponModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)settings.DeleteEquippedWeaponModifierKey)))
                {
                    DeleteWeaponByHotkey();
                }

                if (settings.DeleteNearestVehicleKey != null
                    && Game.IsKeyDown((Keys)settings.DeleteNearestVehicleKey)
                    && (settings.DeleteNearestVehicleModifierModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)settings.DeleteNearestVehicleModifierModifierKey)))
                {
                    DeleteVehicleByHotkey();
                }
            }
        }

        [ConsoleCommand]
        public static void DeleteEquippedWeapon()
        {
            if (Player.Inventory.EquippedWeapon != null)
            {
                var wh = Player.Inventory.EquippedWeapon.Hash;
                DeleteWeapon(wh);
            }
        }

        [ConsoleCommand]
        public static void DeleteAllWeapons()
        {
            Player.Inventory.Weapons.Clear();

        }

        //Looking for a better solution on this!
        [ConsoleCommand]
        public static void DeleteWeaponMods()
        {
            if (Player.Inventory.EquippedWeapon != null)
            {
                var w = Player.Inventory.EquippedWeapon.Asset;
                var a = Player.Inventory.EquippedWeapon.Ammo;
                DeleteEquippedWeapon();
                Player.Inventory.GiveNewWeapon(w, a, true);
            }
                
        }

        [ConsoleCommand]
        public static void DeleteFlashLight()
        {
            DeleteWeapon(WeaponHash.Flashlight);
        }

        [ConsoleCommand]
        public static void DeleteBaton()
        {
            DeleteWeapon(WeaponHash.Nightstick);
        }

        [ConsoleCommand]
        public static void DeleteWeapon(WeaponHash w)
        {
            if (!Player.Inventory.Weapons.Contains(w))
            {
                Game.LogTrivial("Weapon not found in inventory!");
                return;
            }
            if (Player.Inventory.EquippedWeapon != null
                && Player.Inventory.EquippedWeapon.Hash == w)
            {
                
                Player.Inventory.GiveNewWeapon(new WeaponAsset("WEAPON_UNARMED"), 0, true);

                #region legacy/incorrect
                //Player.Inventory.EquippedWeapon = new WeaponAsset("WEAPON_UNARMED");
                //For some reason it does not work, throws exception that the player does not have that weapon.
                #endregion
            }

            #region legacy/incorrect
            //Old way of doing it
            //Player.Inventory.Weapons.Remove(new WeaponDescriptor(new WeaponAsset((uint) w)));
            #endregion

            Player.Inventory.Weapons.Remove(w);

        }

        [ConsoleCommand]
        public static void DeleteVehicle()
        {
            try
            {
                if (ClosestVeh.HasDriver && ClosestVeh.Driver != Player)
                {
                    ClosestVeh.Driver.Delete();
                }

                ClosestVeh.Delete();
            }
            catch
            {
                Notify("Can't get/delete closest vehicle, try again!");
            }
        }

        private static void DeleteVehicle(Vehicle vehicle)
        {
            try
            {
                if (vehicle.HasDriver && vehicle.Driver != Player)
                {
                    vehicle.Driver.Delete();
                }

                vehicle.Delete();
            }
            catch
            {
                Notify("Can't get/delete vehicle, try again!");
            }
        }

        [ConsoleCommand]
        public static void ReloadDWSettings()
        {
            settings = IniModel.Load("Plugins\\DeleteWeapon.ini");
        }

        private static bool ConfirmationTask(string starter)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            var msg = Notify($"Awaiting confirmation for {starter}. You have 10 seconds to confirm this action.");

            while (s.Elapsed <= TimeSpan.FromSeconds(10))
            {
                Rage.GameFiber.Yield();

                if (Game.IsKeyDown((Keys)settings.YesKey)
                    && (settings.YesModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)settings.YesModifierKey)))
                {
                    Game.RemoveNotification(msg);
                    return true;
                }

                if (Game.IsKeyDown((Keys)settings.NoKey)
                    && (settings.NoModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)settings.NoModifierKey)))
                {
                    Game.RemoveNotification(msg);
                    return false;
                }
            }
            Game.RemoveNotification(msg);
            return false;
        }

        private static void DeleteWeaponByHotkey()
        {
            var w = Player.Inventory.EquippedWeapon;
            if (!settings.ConfirmWeaponDeletion || ConfirmationTask("weapon deletion"))
            {
                DeleteWeapon(w.Hash);
            }
        }

        private static void DeleteVehicleByHotkey()
        {
            var v = ClosestVeh;
            if ((Player.IsInAnyVehicle(true)
                    && Player.CurrentVehicle == ClosestVeh
                    && (!settings.ConfirmPlayerVehicleDeletion
                        || ConfirmationTask("own vehicle deletion")))
                || (!settings.ConfirmVehicleDeletion
                        || ConfirmationTask("vehicle deletion")))
            {
                DeleteVehicle(v);
            }
        }
    }
}