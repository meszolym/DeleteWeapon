using Rage;
using Rage.Attributes;
using Rage.ConsoleCommands.AutoCompleters;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("DeleteWeapon", Description = "Do you hate using trainers? I know I do! That's why I created this plugin! Delete weapons and vehicles with ease!", Author = "meszolym", PrefersSingleInstance =true)]


namespace DeleteWeapon
{
    public static class Plugin
    {
        internal static IniModel settings;

        public static void Main()
        {

            settings = IniModel.Load("Plugins\\DeleteWeapon.ini");

            InfoDisplay.Notify($"Plugin loaded!\n" +
                "You can now use the hotkeys specified in the ini, and also the commands (they start with Delete)");

            CheckForUpdate();

            GameFiber.StartNew(HotkeyListener.Listen);
        }

        private static void CheckForUpdate()
        {
            Version newVersion = GetLatestVersion();
            if (newVersion > System.Reflection.Assembly.GetExecutingAssembly().GetName().Version)
            {
                InfoDisplay.Notify($"New version (v{newVersion}) available on LCPDFR.com! Make sure to update the plugin to get the best experience!");
            }

            
        }

        private static Version GetLatestVersion()
        {
            string latestVersion = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    latestVersion = client.GetStringAsync(
                        "https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=44733&beta=false&textOnly=true"
                        ).Result;
                }
            }
            catch (Exception e)
            {
                InfoDisplay.Log($"Couldn't get latest version from LCPDFR.com! (Message of exception thrown: {e.Message})");
            }


            if (latestVersion.Any(x => !char.IsNumber(x)))
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in latestVersion)
                {
                    if (char.IsNumber(c) || c == '.')
                    {
                        sb.Append(c);
                    }
                }
                latestVersion = sb.ToString();
            }
            Version lv;
            if (Version.TryParse(latestVersion, out lv))
            {
                return lv;
            }
            InfoDisplay.Log("Couldn't parse latest version from LCPDFR.com!");
            return null;
        }

        [ConsoleCommand]
        public static void ReloadDWSettings()
        {
            settings = IniModel.Load("Plugins\\DeleteWeapon.ini");
        }
    }
}