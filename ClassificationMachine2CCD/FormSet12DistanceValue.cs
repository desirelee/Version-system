using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassificationMachine;

namespace ClassificationMachine
{
    public partial class FormSet12DistanceValue : Form
    {
        public static string strName;
        public FormSet12DistanceValue()
        {
            InitializeComponent();
        }
        public FormSet12DistanceValue(string strCmbProductName)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            strName = strCmbProductName;
        }

        #region 选中所有字
        private void tb_LA_Click(object sender, EventArgs e)
        {
            tb_LA.SelectAll();
        }

        private void tb_LB_Click(object sender, EventArgs e)
        {
            tb_LB.SelectAll();
        }

        private void tb_LK_Click(object sender, EventArgs e)
        {
            tb_LK.SelectAll();
        }

        private void tb_LI_Click(object sender, EventArgs e)
        {
            tb_LI.SelectAll();
        }

        private void tb_LG_Click(object sender, EventArgs e)
        {
            tb_LG.SelectAll();
        }

        private void tb_LC_Click(object sender, EventArgs e)
        {
            tb_LC.SelectAll();
        }

        private void tb_LD_Click(object sender, EventArgs e)
        {
            tb_LD.SelectAll();
        }

        private void tb_LL_Click(object sender, EventArgs e)
        {
            tb_LL.SelectAll();
        }

        private void tb_LJ_Click(object sender, EventArgs e)
        {
            tb_LJ.SelectAll();
        }

        private void tb_LH_Click(object sender, EventArgs e)
        {
            tb_LH.SelectAll();
        }

        private void tb_LE_Click(object sender, EventArgs e)
        {
            tb_LE.SelectAll();
        }

        private void tb_LF_Click(object sender, EventArgs e)
        {
            tb_LF.SelectAll();
        }

        #endregion

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath;
                strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog\\" + strName + ".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                strTemp = tb_LA.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LA", strTemp);
                strTemp = tb_LB.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LB", strTemp);
                strTemp = tb_LC.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LC", strTemp);
                strTemp = tb_LD.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LD", strTemp);
                strTemp = tb_LE.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LE", strTemp);
                strTemp = tb_LF.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LF", strTemp);
                strTemp = tb_LG.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LG", strTemp);
                strTemp = tb_LH.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LH", strTemp);
                strTemp = tb_LI.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LI", strTemp);
                strTemp = tb_LJ.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LJ", strTemp);
                strTemp = tb_LK.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LK", strTemp);
                strTemp = tb_LL.Text.Trim();
                IniProg.IniWriteValue("TwelveLineStandard", "LL", strTemp);

                this.Close();
            }
            catch
            {
                MessageBox.Show("储存时发生错误\n请检查输入的值是否有误!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormSet12DistanceValue_Load(object sender, EventArgs e)
        {
            
        }


    }
}
