using DataLayer;
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
    public partial class Dashboard : Form
    {
        private int childFormNumber = 0;

        public Dashboard()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }        

        private void InitiateFormCall(string formName)
        {
            bool formFound = false;
            this.Hide();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == formName)
                {
                    formFound = true;
                    frm.Show();
                    frm.BringToFront();
                }
            }
            if (!formFound)
            {
                Form rptForm=null;
                switch (formName)
                {
                    case "ManagerReport":
                        rptForm = new ManagerReport();
                        break;
                    case "GroupReport":
                        rptForm = new GroupReport();
                        break;
                    case "AccountReport":
                        rptForm = new AccountReport();
                        break;
                    case "MarginPerPersonReport":
                        rptForm = new MarginPerPersonReport();
                        break;
                    case "PersonReport":
                        rptForm = new PersonReport();
                        break;
                    case "SalaryReport":
                        rptForm = new SalaryReport();
                        break;
                    case "MonthlyAccountUpload":
                        rptForm = new MonthlyAccountUpload();
                        break;
                    case "AccountCreation":
                        rptForm = new AccountCreation();
                        break;
                    case "BUCreation":
                        rptForm = new BUCreation();
                        break;
                    case "GenerateReport":
                        rptForm = new GenerateReport();
                        break;
                    default:
                        break;
                }
                rptForm.MdiParent = this.MdiParent;
                rptForm.Show();
                rptForm.BringToFront();
            }
        }

        private void managerWiseRevenueReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("ManagerReport");
        }

        private void groupWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("GroupReport");
        }

        private void accountWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("AccountReport");           
        }

        private void marginPerPersonYTDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("MarginPerPersonReport"); 
        }

        private void personMarginReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("PersonReport");           
        }

        private void salaryBasedReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("SalaryReport");            
        }

        private void addMonthlyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("GenerateReport");
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("MonthlyAccountUpload");
        }

        private void createAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("AccountCreation");
        }

        private void createBusinessUnitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("BUCreation");
        }

        private void mapAccountsToBusinessUnitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitiateFormCall("MapBUAccount");
        }        

    }
}
