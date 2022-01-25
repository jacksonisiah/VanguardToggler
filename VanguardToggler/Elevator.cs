using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace VanguardToggler
{
    internal class Elevator
    {
        internal static void Elevate()
        {
            var SelfProc = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Application.ExecutablePath,
                Verb = "runas"
            };
            try
            {
                Process.Start(SelfProc);
                Environment.Exit(0);
            }
            catch
            {
                MessageBox.Show("Could not elevate to administrator");
            }
        }

        internal static bool IsElevated()
        {
            var app = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return app.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
