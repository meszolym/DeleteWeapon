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
    internal static class WeaponActions
    {

        internal static void DeleteWeaponByHotkey()
        {
            var w = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
            if (!Plugin.settings.ConfirmWeaponDeletion || HotkeyListener.ConfirmationTask("weapon deletion"))
            {
                DeleteWeapon(w.Hash);
            }
        }

        [ConsoleCommand]
        public static void DeleteEquippedWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null)
            {
                var wh = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
                DeleteWeapon(wh);
            }
        }

        [ConsoleCommand]
        public static void DeleteAllWeapons()
        {
            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        }

        //Looking for a better solution on this!
        [ConsoleCommand]
        public static void DeleteWeaponMods()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null)
            {
                var w = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Asset;
                var a = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Ammo;
                DeleteEquippedWeapon();
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(w, a, true);
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
            if (!Game.LocalPlayer.Character.Inventory.Weapons.Contains(w))
            {
                InfoDisplay.Log("Weapon not found in inventory!");
                return;
            }
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null
                && Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == w)
            {

                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(new WeaponAsset("WEAPON_UNARMED"), 0, true);
            }

            Game.LocalPlayer.Character.Inventory.Weapons.Remove(w);

        }

    }
}
