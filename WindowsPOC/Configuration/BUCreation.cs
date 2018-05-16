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
    public partial class BUCreation : Form
    {
        public BUCreation()
        {
            InitializeComponent();
            FillAccountList();
            FillAccountGrid();
        }

        private void FillAccountList()
        {
            lstAccounts.DataSource = null;
            AccountModel bu = new AccountModel();
            List<Account> accList = bu.GetAccountsList();
            lstAccounts.DataSource = accList;
            lstAccounts.ValueMember = "AccountID";
            lstAccounts.DisplayMember = "AccountName";
        }

        private void FillAccountGrid()
        {
            dgvAccount.DataSource = null;
            BUModel a = new BUModel();
            var accList =a.GetBUList();
            dgvAccount.DataSource = accList;
            dgvAccount.Show();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            bool accountCreated = false;
            if (!string.IsNullOrEmpty(txtAccountName.Text))
            {
                BUModel a = new BUModel();
                if (btnCreate.Text == "Update")
                {
                    accountCreated = a.UpdateBU(lblAccountID.Text, txtAccountName.Text, txtAccountName.Text, lstAccounts.SelectedItems.Cast<Account>().ToList());
                }
                else
                {

                    accountCreated = a.CreateBU(txtAccountName.Text, txtAccountName.Text, lstAccounts.SelectedItems.Cast<Account>().ToList());

                }

                if (accountCreated)
                {
                    FillAccountGrid();
                    MessageBox.Show("BU Created successfully");
                    txtAccountName.Text = string.Empty;
                }
                else
                    MessageBox.Show("Issue while creating the account.\nTry after some time");
            }
            else
            {
                MessageBox.Show("BU Name cannot be blank");
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

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
