using Rage;
using Rage.Attributes;
using System;

[assembly: Rage.Attributes.Plugin("DeleteWeapon", Description = "This plugin lets you delete weapons", Author = "meszolym")]


namespace DeleteWeapon
{
    public static class Plugin
    {
        
        public static void Main()
        {
            Game.DisplayNotification("DeleteWeapon by meszolym loaded!\n" +
                "You can now use these commands:\n" +
                "\"DeleteEquippedWeapon\",\n" +
                "\"DeleteFlashLight\",\n" +
                "\"DeleteBaton\"," +
                "and \"DeleteWeapon\".");
            while (true)
            {
                Rage.GameFiber.Yield();
            }
        }

        [ConsoleCommand]
        public static void DeleteEquippedWeapon()
        {
            if (Rage.Game.LocalPlayer.Character.Inventory.EquippedWeapon != null)
            {
                var wh = Rage.Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
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
            if (!Rage.Game.LocalPlayer.Character.Inventory.Weapons.Contains(w))
            {
                Game.Console.Print("Weapon not found in inventory!");
                return;
            }
            if (Rage.Game.LocalPlayer.Character.Inventory.EquippedWeapon != null
                && Rage.Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == w)
            {
                Rage.Game.LocalPlayer.Character.Inventory.GiveNewWeapon(new WeaponAsset("WEAPON_UNARMED"), 0, true);
            }
            Rage.Game.LocalPlayer.Character.Inventory.Weapons.Remove(new WeaponDescriptor(new WeaponAsset((uint) w)));

        }
    }
}