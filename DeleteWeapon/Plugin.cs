using Rage;
using Rage.Attributes;
using System;
using System.Configuration;
using System.Security.Permissions;
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

        public static void Main()
        {

            settings = IniModel.Load("Plugins\\DeleteWeapon.ini");

            Game.DisplayNotification("DeleteWeapon by meszolym loaded!\n" +
                "You can now use these commands:\n" +
                "\"DeleteEquippedWeapon\",\n" +
                "\"DeleteFlashLight\",\n" +
                "\"DeleteBaton\"," +
                "\"DeleteWeapon\"," +
                "\"DeleteVehicle\"");

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
                    if (!settings.ConfirmWeaponDeletion || GetConfirmation().Result)
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
                Game.Console.Print("Weapon not found in inventory!");
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
            
            if (ClosestVeh.HasDriver && ClosestVeh.Driver != Player)
            {
                ClosestVeh.Driver.Delete();
            }

            ClosestVeh.Delete();
        }

        private static void DeleteVehicleByHotkey()
        {
            if ((Player.IsInAnyVehicle(true)
                    && Player.CurrentVehicle == ClosestVeh
                    && (!settings.ConfirmPlayerVehicleDeletion
                        || GetConfirmation().Result))
                || (!settings.ConfirmVehicleDeletion)
                    || GetConfirmation().Result)
            {
                DeleteVehicle();
                return;
            }
        }

        public static async Task<bool> GetConfirmation()
        {
            //Code
        }
    }
}