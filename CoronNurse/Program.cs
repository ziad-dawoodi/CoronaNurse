using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoronNurse
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
            // Application.Run(new InfoForm());
            //   Application.Run(new DoctorsForm());
            //  Application.Run(new ProfileForm());
            // Application.Run(new LoginForm());
            //  Application.Run(new NewAccForm());
        }
    } 
}
