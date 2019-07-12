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
    public partial class FormAskNum : Form
    {
        public static string strName;

        public FormAskNum()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public FormAskNum(string strCmbProductName)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            strName = strCmbProductName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rb_4.Checked)
            {
                buildFile(4);
                this.Close();
            }
            else if (rb_4WithReference.Checked)
            {
                buildFile(6);
                this.Close();
            }
            else if (rb_9.Checked)
            {
                buildFile(9);
                this.Close();
            }
            else
            {
                MessageBox.Show("请至少选择其中一个选项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void buildFile(int num)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog\\" + strName + ".ini";

            CIni IniProg = new CIni(strPath);
            string strTemp = "";
            strTemp = Convert.ToString(num);
            IniProg.IniWriteValue("Total", "Count", strTemp);

            for (int i = 1; i <= num; i++)
            {
                strTemp = i.ToString();
                IniProg.IniWriteValue(i.ToString(), "NO.", strTemp);

                strTemp = "10";
                IniProg.IniWriteValue(i.ToString(), "CenterX", strTemp);

                strTemp = "10";
                IniProg.IniWriteValue(i.ToString(), "CenterY", strTemp);

                strTemp = "200";
                IniProg.IniWriteValue(i.ToString(), "InnerR", strTemp);

                strTemp = "400";
                IniProg.IniWriteValue(i.ToString(), "OuterR", strTemp);

                strTemp = "0";
                IniProg.IniWriteValue(i.ToString(), "StartA", strTemp);

                strTemp = "0";
                IniProg.IniWriteValue(i.ToString(), "EndA", strTemp);

                strTemp = "10";
                IniProg.IniWriteValue(i.ToString(), "MotorX", strTemp);

                strTemp = "10";
                IniProg.IniWriteValue(i.ToString(), "MotorY", strTemp);

                strTemp = "0.5";
                IniProg.IniWriteValue(i.ToString(), "R", strTemp);

                strTemp = "1";
                IniProg.IniWriteValue(i.ToString(), "Color", strTemp);

                strTemp = "1";
                IniProg.IniWriteValue(i.ToString(), "Type", strTemp);

            }
            //图像信息
            strTemp = "20";
            IniProg.IniWriteValue("Image", "Light0", strTemp);
            strTemp = "150";
            IniProg.IniWriteValue("Image", "Threshold0", strTemp);

            strTemp = "200";
            IniProg.IniWriteValue("Image", "Light1", strTemp);
            strTemp = "40";
            IniProg.IniWriteValue("Image", "Threshold1", strTemp);

            strTemp = "200";
            IniProg.IniWriteValue("Image", "Light2", strTemp);
            strTemp = "20";
            IniProg.IniWriteValue("Image", "Threshold2", strTemp);

            //参数信息
            strTemp = "0";
            IniProg.IniWriteValue("Param", "L1Set", strTemp);

            strTemp = "0";
            IniProg.IniWriteValue("Param", "L2Set", strTemp);

            strTemp = "0";
            IniProg.IniWriteValue("Param", "W1Set", strTemp);

            strTemp = "0";
            IniProg.IniWriteValue("Param", "W2Set", strTemp);

            strTemp = "0";
            IniProg.IniWriteValue("Param", "Y3Set", strTemp);

            strTemp = "0";
            IniProg.IniWriteValue("Param", "No1", strTemp);

            strTemp = "0";
            IniProg.IniWriteValue("Param", "No2", strTemp);

            strTemp = "1";
            IniProg.IniWriteValue("Param", "Dir", strTemp);//搜寻方向

            strTemp = "0";
            IniProg.IniWriteValue("Param", "Parity", strTemp);//搜寻极性

            if (num == 6)
            {
                if (MessageBox.Show("请问有无参考线?\n选“是”为四点测量(参考线)\n选“否”为六点测量", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    strTemp = "1";
                    IniProg.IniWriteValue("Param", "MasterLine", strTemp);//有参考线

                    strTemp = "0";
                    IniProg.IniWriteValue("CheckBox6Point", "CheckBox", strTemp);//6点测量CheckBox
                }
                else
                {
                    strTemp = "0";
                    IniProg.IniWriteValue("Param", "MasterLine", strTemp);//无参考线

                    strTemp = "1";
                    IniProg.IniWriteValue("CheckBox6Point", "CheckBox", strTemp);//6点测量CheckBox
                }
            }
            else
            {
                //if (num == 4 || num == 9) { strTemp = "0"; }
                //else strTemp = "1";//当圆点数为6时，默认勾选参考线
                strTemp = "0";
                IniProg.IniWriteValue("Param", "MasterLine", strTemp); //默认都无参考线
            }
            strTemp = "1";
            IniProg.IniWriteValue("Param", "MarkColor", strTemp);

            strTemp = "2";
            IniProg.IniWriteValue("Param", "ClassTray", strTemp);//分堆方式

            strTemp = "1";
            IniProg.IniWriteValue("Param", "AverageSort", strTemp);//是否按平均值分堆

            strTemp = "1.0004";
            IniProg.IniWriteValue("Param", "XStand", strTemp);//涨缩值标准值

            strTemp = "1.0016";
            IniProg.IniWriteValue("Param", "YStand", strTemp);

            //参考角度
            strTemp = "0";
            IniProg.IniWriteValue("RefAngle", "L1Set", strTemp);
            strTemp = "0";
            IniProg.IniWriteValue("RefAngle", "L2Set", strTemp);
            strTemp = "0";
            IniProg.IniWriteValue("RefAngle", "W1Set", strTemp);
            strTemp = "0";
            IniProg.IniWriteValue("RefAngle", "W2Set", strTemp);
            strTemp = "0";
            IniProg.IniWriteValue("RefAngle", "Y3Set", strTemp);
            strTemp = "1";
            IniProg.IniWriteValue("CircleDia", "Mark", strTemp);
            strTemp = "1";
            IniProg.IniWriteValue("CircleDia", "Ref", strTemp);

            //相机定位设置
            strTemp = "0";
            IniProg.IniWriteValue("PosCameraSet", "X", strTemp);
            strTemp = "0";
            IniProg.IniWriteValue("PosCameraSet", "Y", strTemp);
            strTemp = "-1000";
            IniProg.IniWriteValue("PosCameraSet", "Angle", strTemp);

            //选择LED灯光
            strTemp = "0";
            IniProg.IniWriteValue("Select", "Led", strTemp);
            strTemp = "0";
            IniProg.IniWriteValue("Select", "PosUsing", strTemp);//定位相机

            
            //默认9点测量的12条边为0
            if (num == 9||num==6)
            {
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LA", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LB", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LC", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LD", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LE", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LF", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LG", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LH", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LI", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LJ", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LK", strTemp);
                strTemp = "0";
                IniProg.IniWriteValue("TwelveLineStandard", "LL", strTemp);
            }
        }

        private void FormAskNum_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rb_9.Checked)
            {
                if (MessageBox.Show("请问需要设置12条边的标准值吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FormSet12DistanceValue fs12dv = new FormSet12DistanceValue(strName);
                    fs12dv.ShowDialog();
                }

            }
        }

        private void rb_4WithReference_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
