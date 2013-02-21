using BuxferExpress.BuxferApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BuxferExpress
{
    public partial class FormLogin : Form
    {
        Buxfer buxferApi;
        public FormLogin(Buxfer api)
        {
            InitializeComponent();
            buxferApi = api;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string uName = textUsername.Text;
            string pass = textPassword.Text;
            if (uName.Length > 0 && pass.Length > 0)
            {
                if (buxferApi.Login(uName, pass) == true)
                {
                    //save username and password for future logins
                    Properties.Settings.Default.UserName = uName;
                    Properties.Settings.Default.Password = pass;
                    Properties.Settings.Default.Save();

                    this.Close();
                }
            }

            //todo: show error message if login fails
        }
    }
}
