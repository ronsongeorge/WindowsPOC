using DataLayer;
using EntitiesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsPOC
{
    public partial class AccountCreation : Form
    {
        public AccountCreation()
        {
            InitializeComponent();
            FillAccountGrid();
        }

        private void FillAccountGrid()
        {
            dgvAccount.DataSource = null;
            AccountModel a = new AccountModel();
            var accList =a.GetAccountsList();
            dgvAccount.DataSource = accList;
            dgvAccount.Show();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            bool accountCreated = false;
            if (!string.IsNullOrEmpty(txtAccountName.Text))
            {
                AccountModel a = new AccountModel();
                if (btnCreate.Text == "Update")
                {
                    accountCreated = a.UpdateAccount(lblAccountID.Text,txtAccountName.Text, txtAccountName.Text);
                }
                else
                {
                    accountCreated = a.CreateAccount(txtAccountName.Text, txtAccountName.Text);
                }

                if (accountCreated)
                {
                    FillAccountGrid();
                    MessageBox.Show("Account Created successfully");
                    txtAccountName.Text = string.Empty;
                }
                else
                    MessageBox.Show("Issue while creating the account.\nTry after some time");
            }
            else
            {
                MessageBox.Show("Account Name cannot be blank");
            }
            btnCreate.Text = "Create";
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                lblAccountID.Text = senderGrid.Rows[e.RowIndex].Cells["AccountID"].Value.ToString();
                txtAccountName.Text = senderGrid.Rows[e.RowIndex].Cells["AccountName"].Value.ToString();
                btnCreate.Text = "Update";
            }
        }
    }
}
