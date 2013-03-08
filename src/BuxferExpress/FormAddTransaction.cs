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
    public partial class FormAddTransaction : Form
    {
        Buxfer buxferApi;
        List<Account> accounts;
        public FormAddTransaction(Buxfer api)
        {
            InitializeComponent();
            buxferApi = api;
        }

        private void FormAddTransaction_Load(object sender, EventArgs e)
        {
            //populate the accounts combo box
            accounts = buxferApi.GetAccounts();
            comboAccounts.DataSource = accounts.Select(a=>a.key_account).ToList();
            comboAccounts.DisplayMember = "name";
            comboAccounts.ValueMember = "id";
        }

        private void buttonAddExpense_Click(object sender, EventArgs e)
        {
            if (textDescription.Text != "" && textAmount.Text != "")
            {
                buxferApi.AddTransaction(
                    textDescription.Text, 
                    textAmount.Text, 
                    dateTransaction.Value,
                    comboAccounts.Text);

                if (MessageBox.Show("Transaction added. Add another one?", "Buxfer Express", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.textDescription.Clear();
                    this.textAmount.Clear();
                    this.textDescription.Focus();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void comboAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            int dueDateDay = 0;
            int closingDay = 0;

            //get the account configuration from settings
            List<ConfigAccountItem> acctsConfig = Properties.Settings.Default.AccountsSettings;
            ConfigAccountItem acctConfig = acctsConfig.FirstOrDefault(a => a.Id == cb.SelectedValue.ToString());

            if (acctConfig == null)
            {
                dateTransaction.Value = DateTime.Today;
                return;
            }

            dueDateDay = acctConfig.DueDay;
            closingDay = acctConfig.ClosingDay;

            if (dueDateDay > 0 && closingDay > 0)
            {
                //base date to estimate the month for the next due date
                DateTime baseDate = DateTime.Today;

                //if the due day comes before the closing day, the bill will due next month or later
                if (dueDateDay < closingDay)
                    baseDate = baseDate.AddMonths(1);

                //if it already passed the closing day this month, adds one month to the base date
                if (closingDay <= DateTime.Today.Day)
                    baseDate = baseDate.AddMonths(1);

                //sets the next due date from the base date
                DateTime nextDueDate = new DateTime(baseDate.Year, baseDate.Month, dueDateDay);

                dateTransaction.Value = nextDueDate;

                //add the current day and month to the transaction description
                textDescription.Text = DateTime.Today.Day.ToString("00") + "/" + DateTime.Today.Month.ToString("00") + " " + textDescription.Text;
            }
            else
                dateTransaction.Value = DateTime.Today;

        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            FormAccountsConfig formConfig = new FormAccountsConfig();
            formConfig.Accounts = accounts;
            formConfig.Show();
        }
    }
}
