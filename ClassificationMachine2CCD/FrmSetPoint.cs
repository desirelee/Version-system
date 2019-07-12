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
    public partial class FrmSetPoint : Form
    {
        public FrmSetPoint()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ClassPublicTool.ManyPointNum =(int) numericUpDownSetPoint.Value;
            Close();
        }
    }
}
