using BuxferExpress.BuxferApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BuxferExpress
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Buxfer api = new Buxfer();

            bool logged = false;

            //check if username and password are saved
            if (Properties.Settings.Default.UserName.Length > 0 && Properties.Settings.Default.Password.Length > 0)
            {
                //try to login
                logged = api.Login(Properties.Settings.Default.UserName, Properties.Settings.Default.Password);
            }

            //show the login dialog if it's not logged yet
            if (!logged)
            {
                FormLogin formLogin = new FormLogin(api);
                formLogin.ShowDialog();
            }

            //run the add transaction form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormAddTransaction(api));

        }
    }
}
