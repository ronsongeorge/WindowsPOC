using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsPOC
{
    public partial class AccountForm : Form
    {
        public AccountForm()
        {
            InitializeComponent();
            FillAccountCombo();
        }

        public DirectoryInfo getdirDetails;

        private void FillAccountCombo(){
            foreach (DirectoryInfo dIn in getdirDetails.GetDirectories())
            {
                cmbAccountName.Items.Add(dIn.Name);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbAccountName.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Please select an account to continue..");
            }
            else
            {
                var parentForm = (GenerateReport)this.Parent.FindForm();
                parentForm.selectedAccountName = cmbAccountName.SelectedItem.ToString();
                Form.ActiveForm.Close();
            }
        }
    }
}
