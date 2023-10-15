using Rage;
using Rage.Attributes;
using Rage.ConsoleCommands.AutoCompleters;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
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

        private static void Notify(string text)
        {
            Game.DisplayNotification("commonmenu","shop_gunclub_icon_b", $"DeleteWeapon v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}", "by meszolym", text);
        }

        public static void Main()
        {

            settings = IniModel.Load("Plugins\\DeleteWeapon.ini");

            Notify($"Plugin loaded!\n" +
                "You can now use the hotkeys specified in the ini, and also the commands (they start with Delete)");
            Notify("Please note that in the current version, confirmation behaviours are not implemented!\n" +
                "This means that when using hotkeys, you will <u>not</u> get asked to confirm your actions!");

            

            GameFiber.StartNew(Thread);
        }

       public static void Thread()
       {
            while (true)
            {
                Rage.GameFiber.Yield();

                if (settings.DeleteEquippedWeaponKey != null 
                    && Game.IsKeyDown((Keys)settings.DeleteEquippedWeaponKey)
                    && (settings.DeleteEquippedWeaponModifierKey == null
                        || Game.IsKeyDownRightNow((Keys)settings.DeleteEquippedWeaponModifierKey)))
                {
                    if (!settings.ConfirmWeaponDeletion /*|| await GetConfirmation()*/ )
                    {
                        DeleteEquippedWeapon();
                    }
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
            foreach (var w  in Player.Inventory.Weapons)
            {
                DeleteWeapon(w.Hash);
            }

        }

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

        [ConsoleCommand]
        public static void ReloadDWSettings()
        {
            settings = IniModel.Load("Plugins\\DeleteWeapon.ini");
        }

        private static void DeleteVehicleByHotkey()
        {
            if ((Player.IsInAnyVehicle(true)
                    && Player.CurrentVehicle == ClosestVeh
                    && (!settings.ConfirmPlayerVehicleDeletion
                        /*|| await GetConfirmation()*/))
                || (!settings.ConfirmVehicleDeletion)
                    /*|| await GetConfirmation()*/)
            {
                DeleteVehicle();
                return;
            }
        }

        /*public static async Task<bool> GetConfirmation()
        {
            To be implemented... When I figure it out...
        }*/
    }
}