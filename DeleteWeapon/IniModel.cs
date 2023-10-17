using Rage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeleteWeapon
{
    public class IniModel
    {

        #region Keys
        private Keys deleteEquippedWeaponKey;
        internal Keys? DeleteEquippedWeaponKey 
        {
            get
            {
                if (deleteEquippedWeaponKey != Keys.None)
                {
                    return deleteEquippedWeaponKey;
                }
                return null;
            }
        }

        private Keys deleteEquippedWeaponModifierKey;

        internal Keys? DeleteEquippedWeaponModifierKey
        {
            get
            {
                if (deleteEquippedWeaponModifierKey != Keys.None)
                {
                    return deleteEquippedWeaponModifierKey;
                }
                return null;
            }
        }

        private Keys deleteNearestVehicleKey;
        internal Keys? DeleteNearestVehicleKey 
        {
            get
            {
                if (deleteNearestVehicleKey != Keys.None)
                {
                    return deleteNearestVehicleKey;
                }
                return null;
            }
        }

        private Keys deleteNearestVehicleModifierKey;
        internal Keys? DeleteNearestVehicleModifierModifierKey
        {
            get
            {
                if (deleteNearestVehicleModifierKey != Keys.None)
                {
                    return deleteNearestVehicleModifierKey;
                }
                return null;
            }
        }

        private Keys yesKey;

        internal Keys? YesKey
        {
            get
            {
                if (yesKey != Keys.None)
                {
                    return yesKey;
                }
                return null;
            }
        }

        private Keys yesModifierKey;

        internal Keys? YesModifierKey
        {
            get
            {
                if (yesModifierKey != Keys.None)
                {
                    return yesModifierKey;
                }
                return null;
            }
        }

        private Keys noKey;

        internal Keys? NoKey
        {
            get
            {
                if (noKey != Keys.None)
                {
                    return noKey;
                }
                return null;
            }
        }

        private Keys noModifierKey;
        internal Keys? NoModifierKey
        {
            get
            {
                if (noModifierKey != Keys.None)
                {
                    return noModifierKey;
                }
                return null;
            }
        }

        #endregion

        #region Behaviour

        private bool confirmWeaponDeletion;

        internal bool ConfirmWeaponDeletion
        {
            get
            {
                if (this.YesKey == null || this.NoKey == null || this.NoKey == this.YesKey)
                {
                    InfoDisplay.Log("No key for YesKey or NoKey, or both are the same, behaviour defaults to false");
                    return false;
                }
                return confirmWeaponDeletion;
            }
        }

        private bool confirmVehicleDeletion;

        internal bool ConfirmVehicleDeletion
        {
            get
            {
                if (this.YesKey == null || this.NoKey == null || this.NoKey == this.YesKey)
                {
                    InfoDisplay.Log("No key for YesKey or NoKey, or both are the same, behaviour defaults to false");
                    return false;
                }
                return confirmVehicleDeletion;
            }
        }

        private bool confirmPlayerVehicleDeletion;

        internal bool ConfirmPlayerVehicleDeletion
        {
            get
            {
                if (this.YesKey == null || this.NoKey == null || this.NoKey == this.YesKey)
                {
                    InfoDisplay.Log("No key for YesKey or NoKey, or both are the same, behaviour defaults to false");
                    return false;
                }
                return confirmPlayerVehicleDeletion;
            }
        }

        #endregion

        IniModel() { }

        internal static IniModel Load(string path)
        {
            IniModel loadedModel = new IniModel();

            InitializationFile ini = new InitializationFile(path);
            ini.Create();


            #region Keys
            if (ini.DoesKeyExist("Keys","DeleteEquippedWeaponKey"))
            {
                loadedModel.deleteEquippedWeaponKey = ini.ReadEnum<Keys>("Keys", "DeleteEquippedWeaponKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "DeleteEquippedWeaponKey", "None");
                InfoDisplay.Log("No key for DeleteEquippedWeapon");
                loadedModel.deleteEquippedWeaponKey = Keys.None;


            }

            if (ini.DoesKeyExist("Keys", "DeleteEquippedWeaponModifierKey"))
            {
                loadedModel.deleteEquippedWeaponModifierKey = ini.ReadEnum<Keys>("Keys", "DeleteEquippedWeaponModifierKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "DeleteEquippedWeaponModifierKey", "None");
                InfoDisplay.Log("No key for DeleteEquippedWeaponModifierKey");
                loadedModel.deleteEquippedWeaponModifierKey= Keys.None;

            }

            if (ini.DoesKeyExist("Keys", "DeleteNearestVehicleKey"))
            {
                loadedModel.deleteNearestVehicleKey = ini.ReadEnum<Keys>("Keys", "DeleteNearestVehicleKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "DeleteNearestVehicleKey", "None");
                InfoDisplay.Log("No key for DeleteNearestVehicle");
                loadedModel.deleteNearestVehicleKey= Keys.None;
            }


            if (ini.DoesKeyExist("Keys", "DeleteNearestVehicleModifierKey"))
            {
                loadedModel.deleteNearestVehicleModifierKey = ini.ReadEnum<Keys>("Keys", "DeleteNearestVehicleModifierKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "DeleteNearestVehicleModifierKey", "None");
                InfoDisplay.Log("No key for DeleteNearestVehicle");
                loadedModel.deleteNearestVehicleModifierKey= Keys.None;
            }

            if (ini.DoesKeyExist("Keys", "YesKey"))
            {
                loadedModel.yesKey = ini.ReadEnum<Keys>("Keys", "YesKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "YesKey", "None");
                InfoDisplay.Log("No key for YesKey");
                loadedModel.yesKey= Keys.None;
            }

            if (ini.DoesKeyExist("Keys", "YesModifierKey"))
            {
                loadedModel.yesModifierKey = ini.ReadEnum<Keys>("Keys", "YesModifierKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "YesModifierKey", "None");
                InfoDisplay.Log("No key for YesModifierKey");
                loadedModel.yesModifierKey= Keys.None;
            }

            if (ini.DoesKeyExist("Keys", "NoKey"))
            {
                loadedModel.noKey = ini.ReadEnum<Keys>("Keys", "NoKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "NoKey", "None");
                InfoDisplay.Log("No key for NoKey");
                loadedModel.noKey= Keys.None;
            }

            if (ini.DoesKeyExist("Keys", "NoModifierKey"))
            {
                loadedModel.noModifierKey = ini.ReadEnum<Keys>("Keys", "NoModifierKey", Keys.None);
            }
            else
            {
                ini.Write("Keys", "NoModifierKey", "None");
                InfoDisplay.Log("No key for NoModifierKey");
                loadedModel.noModifierKey= Keys.None;
            }
            #endregion


            if (ini.DoesKeyExist("Behaviour", "ConfirmWeaponDeletion"))
            {
                loadedModel.confirmWeaponDeletion = bool.Parse(ini.ReadString("Behaviour", "ConfirmWeaponDeletion"));
            }
            else
            {
                ini.Write("Behaviour", "ConfirmWeaponDeletion", "False");
                InfoDisplay.Log("No value for ConfirmWeaponDeletion");
                loadedModel.confirmWeaponDeletion = false;
            }

            if (ini.DoesKeyExist("Behaviour", "ConfirmVehicleDeletion"))
            {
                loadedModel.confirmVehicleDeletion = bool.Parse(ini.ReadString("Behaviour", "ConfirmVehicleDeletion"));
            }
            else
            {
                ini.Write("Behaviour", "ConfirmVehicleDeletion", "False");
                InfoDisplay.Log("No value for ConfirmVehicleDeletion");
                loadedModel.confirmVehicleDeletion = false;
            }

            if (ini.DoesKeyExist("Behaviour", "ConfirmPlayerVehicleDeletion"))
            {
                loadedModel.confirmPlayerVehicleDeletion = bool.Parse(ini.ReadString("Behaviour", "ConfirmPlayerVehicleDeletion"));
            }
            else
            {
                ini.Write("Behaviour", "ConfirmPlayerVehicleDeletion", "False");
                InfoDisplay.Log("No value for ConfirmPlayerVehicleDeletion");
                loadedModel.confirmPlayerVehicleDeletion = false;
            }
            

            return loadedModel;
        }
    }
}
