using Microsoft.Win32;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Forms;

namespace VanguardToggler
{
    internal class Vanguard
    {
        static ServiceController[] scServices;
        
        internal static void Toggle() 
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            if (GetStatus())
            {
                scServices = ServiceController.GetServices();
                foreach (ServiceController service in scServices)
                {
                    if (service.ServiceName == "vgc") service.Stop();
                    else if (service.ServiceName == "vgk") service.Stop();
                }

                var tray = Process.GetProcessesByName("vgtray.exe");
                if (tray.Length > 0)
                    foreach (var process in tray)
                    {
                        process.Kill();
                    }
                else MessageBox.Show("Couldn't kill vgtray. You should kill this manually in your taskbar.");

                key.DeleteSubKey("Riot Vanguard"); // remove startup key
                MessageBox.Show("Vanguard disabled.");
            }
                
            else
            {
                if (!((string)key.GetValue("Riot Vanguard") == @"C:\Program Files\Riot Vanguard\vgtray.exe"))
                    key.CreateSubKey("Riot Vanguard").SetValue("Riot Vanguard", @"C:\Program Files\Riot Vanguard\vgtray.exe");
                key.Close();
                Process.Start("shutdown", "/r /t 0"); // restart
            }
        }

        public static bool GetStatus()
        {
            var tray = Process.GetProcessesByName("vgtray.exe");
            if (tray.Length > 0) return true;

            scServices = ServiceController.GetServices();
            foreach (ServiceController service in scServices)
            {
                if (service.ServiceName == "vgc") // vanguard main
                {
                    if (!(service.Status == ServiceControllerStatus.Stopped))
                        return true;
                }
                else if (service.ServiceName == "vgk") // vanguard kernel
                {
                    if (!(service.Status == ServiceControllerStatus.Stopped))
                        return true;
                }
                

            }
            // appease the compiler gods
            return false;
        }
    }
}
