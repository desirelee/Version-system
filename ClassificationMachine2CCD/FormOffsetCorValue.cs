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
    public partial class FormOffsetCorValue : Form
    {
        public FormOffsetCorValue()
        {
            InitializeComponent();
        }
        public static int NowCount;
        double X = 0, Y = 0;
        double[] MotorXValue, MotorYValue;
        public FormOffsetCorValue(int nowCount,double[] motorX,double[] motorY)//获取当前Mark点的数值
        {
            InitializeComponent();
            NowCount = nowCount;
            MotorXValue=new double[motorX.GetUpperBound(0)+1];
            MotorYValue = new double[motorY.GetUpperBound(0) + 1];
            for (int i = 0; i <= motorX.GetUpperBound(0); i++)
            {
                MotorXValue[i] = motorX[i];
            }
            for (int i = 0; i <= motorY.GetUpperBound(0); i++)
            {
                MotorYValue[i] = motorY[i];
            }
        }
        private void FormOffsetCorValue_Load(object sender, EventArgs e)
        {
            int iNowCount=NowCount+1;
            label2.Text = iNowCount.ToString();
        }

        
        
        private void btn_up_Click(object sender, EventArgs e)
        {
            double temp = double.Parse(textBox1.Text.Trim());
            Y = Y - temp;
            label8.Text = Y.ToString();
            
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            double temp = double.Parse(textBox1.Text.Trim());
            Y = Y + temp;
            label8.Text = Y.ToString();
            
        }

        private void btn_left_Click(object sender, EventArgs e)
        {
            double temp = double.Parse(textBox1.Text.Trim());
            X = X - temp;
            label7.Text = X.ToString();
        }

        private void btn_right_Click(object sender, EventArgs e)
        {
            double temp = double.Parse(textBox1.Text.Trim());
            X = X + temp;
            label7.Text = X.ToString();
        }

        bool frmClose = false;
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                frmClose = true;
                MotorXValue[NowCount] = MotorXValue[NowCount] + double.Parse(label7.Text);
                MotorYValue[NowCount] = MotorYValue[NowCount] + double.Parse(label8.Text);

                string strPath;
                strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\FormOffsetCorValue.ini";

                CIni IniProg = new CIni(strPath);
                string strTemp = "";

                for (int i = 0; i <= MotorXValue.GetUpperBound(0); i++)
                {
                    strTemp = MotorXValue[i].ToString();
                    IniProg.IniWriteValue("MotorX", i.ToString(), strTemp);

                }
                for (int i = 0; i <= MotorYValue.GetUpperBound(0); i++)
                {
                    strTemp = MotorYValue[i].ToString();
                    IniProg.IniWriteValue("MotorY", i.ToString(), strTemp);

                }
            }
            catch
            {
                MessageBox.Show("失败,请检查格式是否正确");
            }
            //label7.Text = "0";
            //label8.Text = "0";
            this.Dispose();
        }

        private void FormOffsetCorValue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!frmClose)
            {
                MessageBox.Show("请保存再退出");
                MotorXValue[NowCount] = MotorXValue[NowCount] + double.Parse(label7.Text);
                MotorYValue[NowCount] = MotorYValue[NowCount] + double.Parse(label8.Text);

                string strPath;
                strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\FormOffsetCorValue.ini";

                CIni IniProg = new CIni(strPath);
                string strTemp = "0";

                for (int i = 0; i <= MotorXValue.GetUpperBound(0); i++)
                {
                    strTemp = MotorXValue[i].ToString();
                    IniProg.IniWriteValue("MotorX", i.ToString(), strTemp);

                }
                for (int i = 0; i <= MotorYValue.GetUpperBound(0); i++)
                {
                    strTemp = MotorYValue[i].ToString();
                    IniProg.IniWriteValue("MotorY", i.ToString(), strTemp);

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "1";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "2";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "3";
        }
    }
}
