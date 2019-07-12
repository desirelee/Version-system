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
    public partial class fRmLogin : Form
    {
        protected string UserName;
        protected string PassWord;
        CIni ini = new CIni(Application.StartupPath + "\\Doc\\User.ini");
        public bool bPassword = false;
        public fRmLogin()
        {
            InitializeComponent();
        }

        //private void btnOK_Click(object sender, EventArgs e)
        //{
        //    UserName = comboBox1.Text;
        //    PassWord = ini.IniReadValue(UserName, "PassWord");
        //    if (textBox1.Text == PassWord)
        //    {
        //        if (UserName == "Operator")
        //            PublicVar.bPasswordOperator = true;
        //        else
        //            PublicVar.bPasswordOperator = false;
        //        if (UserName == "Engineer")
        //            PublicVar.bPasswordEngineer = true;
        //        else
        //            PublicVar.bPasswordEngineer = false;
        //        if (UserName == "Administrator")
        //            PublicVar.bPasswordManager = true;
        //        else
        //            PublicVar.bPasswordManager = false;
        //        bPassword = true;
        //        this.Dispose();
        //    }
        //    else
        //    {
        //        MessageBox.Show("密码或用户名称错误，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        textBox1.Text = "";
        //        textBox1.Focus();
        //    }
        //}

        private void btnOK_Click(object sender, EventArgs e)
        {
            UserName = comboBox1.Text;
            PassWord = ini.IniReadValue(UserName, "PassWord");
            string PassWord_Operator = "";
            string PassWord_Engineer = "1214";
            string PassWord_Manager = "2428";
            
            if (UserName == "Operator")
            {
                if (textBox1.Text == PassWord_Operator)
                {
                    PublicVar.bPasswordOperator = true;
                    bPassword = true;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("密码错误，请重新输入！");
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
            else if (UserName == "Engineer")
            {
                if (textBox1.Text == PassWord_Engineer)
                {
                    PublicVar.bPasswordEngineer = true;
                    bPassword = true;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("密码错误，请重新输入！");
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
            else if (UserName == "Administrator")
            {
                if (textBox1.Text == PassWord_Manager)
                { 
                    PublicVar.bPasswordManager = true;
                    bPassword = true;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("密码错误，请重新输入！");
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
            
        }

        private void fRmLogin_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            //textBox2.Enabled = false;
            //textBox3.Enabled = false;
            textBox1.TabIndex = 0;
            PublicVar.bPasswordEngineer = false;
            PublicVar.bPasswordManager = false;
            PublicVar.bPasswordOperator = false;
            bPassword = false;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            UserName = comboBox1.Text;
            PassWord = ini.IniReadValue(UserName, "PassWord");
            if (textBox1.Text == PassWord)
            {
                //textBox2.Enabled = true;
                //textBox2.Focus();
            }
            else
            {
                MessageBox.Show("密码或用户名称错误，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                UserName = comboBox1.Text;
                PassWord = ini.IniReadValue(UserName, "PassWord");
                if (textBox1.Text == PassWord)
                {
                    if (UserName == "Operator")
                        PublicVar.bPasswordOperator = true;
                    else
                        PublicVar.bPasswordOperator = false;
                    if (UserName == "Engineer")
                        PublicVar.bPasswordEngineer = true;
                    else
                        PublicVar.bPasswordEngineer = false;
                    if (UserName == "Administrator")
                        PublicVar.bPasswordManager = true;
                    else
                        PublicVar.bPasswordManager = false;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("密码或用户名称错误，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                //textBox2.Enabled = false;
                //textBox3.Enabled = true;
                //textBox3.Focus();
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 13)
            //{
            //    if (textBox2.Text == textBox3.Text)
            //    {
            //        UserName = comboBox1.Text;
            //        ini.IniWriteValue(UserName, "PassWord", textBox2.Text);
            //        MessageBox.Show("密码修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    else
            //    {
            //        MessageBox.Show("两次输入的密码不一致，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        textBox2.Text = "";
            //        textBox3.Text = "";
            //        textBox2.Focus();
            //    }
            //}
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
