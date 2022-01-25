using System;
using System.Data;
using System.Security.Permissions;
using System.Windows.Forms;

namespace VanguardToggler
{
internal class Program
{
        static void Main(string[] args)
        {
            if (!Elevator.IsElevated()) Elevator.Elevate(); // to modify registry
            if (Vanguard.GetStatus())
            {
                if (MessageBox.Show("Vanguard is currently running. Disable Vanguard?", "RATIRL", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    Vanguard.Toggle();
                else Environment.Exit(0);
            }
            else
            {
                if (MessageBox.Show("Vanguard is not running. Enable Vanguard and restart PC?", "RATIRL", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    Vanguard.Toggle();
                else Environment.Exit(0);
            }
        }
    }
}
