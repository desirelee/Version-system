using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Class_Motion;
using Camera_Vision_HHiat;

namespace ClassificationMachine
{
    public partial class FormSafetyWatting : Form
    {
        //public ClassMotion m_ClassMotion;
        //string strPath;
        //CIni IniSetting;
        public FormSafetyWatting()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            //m_ClassMotion = new ClassMotion(true);

            //strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            //if (!Directory.Exists(strPath))
            //    Directory.CreateDirectory(strPath);
            //strPath += "\\Setting.ini";
            //if (!File.Exists(strPath))
            //{
            //    MessageBox.Show("不存在Setting.ini文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    var fs = File.Create(strPath);
            //    fs.Dispose();
            //    string strTemp = "2";
            //    IniSetting.IniWriteValue("IO", "IN5", strTemp);
            //}
            
            //IniSetting = new CIni(strPath);
            //timer1.Enabled = true;
        }

        private void FormSafetyWatting_Load(object sender, EventArgs e)
        {       
            
            
               

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //string strTemp = "";
            //strTemp = IniSetting.IniReadValue("IO", "IN5");
            //if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)) && !m_ClassMotion.CHXMotorEmg && !m_ClassMotion.CHYMotorEmg && !m_ClassMotion.CHCYMotorEmg && !m_ClassMotion.CHCZMotorEmg)//safety   等于0时为安全状态
            //{
            //    timer1.Enabled = false;
            //    PublicVar.ForceClose = false;
            //    this.Dispose() ;
            //}
            

        }

        private void FormSafetyWatting_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("真的要强制退出程序吗?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                PublicVar.ForceClose = true;
                //timer1.Enabled = false;
                this.Dispose();
                //System.Diagnostics.Process tt = System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id);
                //tt.Kill();//强制结束所有程序
            }
            else
            {
                return;

            }
        }


    }
}
