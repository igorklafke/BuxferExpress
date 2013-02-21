using BuxferExpress.BuxferApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuxferExpress
{
    public partial class FormAccountsConfig : Form
    {
        public List<Account> Accounts { get; set; }

        public FormAccountsConfig()
        {
            InitializeComponent();
        }

        private void FormAccountsConfig_Load(object sender, EventArgs e)
        {
            DataGridViewComboBoxColumn colAccount = new DataGridViewComboBoxColumn();
            colAccount.HeaderText = "Account";
            colAccount.DataSource = Accounts.Select(a => a.key_account).ToList();
            colAccount.DisplayMember = "name";
            colAccount.ValueMember = "id";
            this.dataGridAccounts.Columns.Add(colAccount);

            DataGridViewTextBoxColumn colClosingDay = new DataGridViewTextBoxColumn();
            colClosingDay.HeaderText = "Closing Day";
            this.dataGridAccounts.Columns.Add(colClosingDay);

            DataGridViewTextBoxColumn colDueDay = new DataGridViewTextBoxColumn();
            colDueDay.HeaderText = "Due Day";
            this.dataGridAccounts.Columns.Add(colDueDay);

            List<ConfigAccountItem> acctSettings = Properties.Settings.Default.AccountsSettings;
            foreach (var item in acctSettings)
            {
                this.dataGridAccounts.Rows.Add(item.Id, item.ClosingDay, item.DueDay);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            List<ConfigAccountItem> acctSettings = new List<ConfigAccountItem>();

            foreach (DataGridViewRow row in this.dataGridAccounts.Rows)
            {
                DataGridViewCell cellAccount = row.Cells[0];
                DataGridViewCell cellClosing = row.Cells[1];
                DataGridViewCell cellDue = row.Cells[2];
                if (cellAccount.Value != null && cellClosing.Value != null && cellDue.Value != null)
                    acctSettings.Add(new ConfigAccountItem { Id = cellAccount.Value.ToString(), ClosingDay = Convert.ToInt32(cellClosing.Value), DueDay = Convert.ToInt32(cellDue.Value) });
            }

            Properties.Settings.Default.AccountsSettings = acctSettings;
            Properties.Settings.Default.Save();
        }

        private void dataGridAccounts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(ColDay_KeyPress);
            int colIndex = this.dataGridAccounts.CurrentCell.ColumnIndex;
            if (colIndex == 1 || colIndex == 2)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                    tb.KeyPress += new KeyPressEventHandler(ColDay_KeyPress);
            }
        }

        private void ColDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
