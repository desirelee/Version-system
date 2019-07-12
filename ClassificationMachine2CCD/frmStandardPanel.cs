using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClassificationMachine
{
    public partial class frmStandardPanel : Form
    {
        public bool bFlag = false;
        public frmStandardPanel()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtIntervalX.Text.Trim() == "") return;
            if (txtIntervalY.Text.Trim() == "") return;
            if (txtLengthX.Text.Trim() == "") return;
            if (txtWidthY.Text.Trim() == "") return;
            bFlag = true;
            PublicVar.RepairIntervalX = Convert.ToDouble(txtIntervalX.Text.Trim());
            PublicVar.RepairIntervalY = Convert.ToDouble(txtIntervalY.Text.Trim());
            PublicVar.StandardPanelLength = Convert.ToDouble(txtLengthX.Text.Trim());
            PublicVar.StandardPanelWidth = Convert.ToDouble(txtWidthY.Text.Trim());
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bFlag = false;
            Close();
        }

        private void frmStandardPanel_Load(object sender, EventArgs e)
        {
            txtIntervalX.Text = PublicVar.RepairIntervalX.ToString("f3");
            txtIntervalY.Text = PublicVar.RepairIntervalY.ToString("f3");
        }
    }
}
