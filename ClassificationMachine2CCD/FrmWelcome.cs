using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Class_Motion;
using System.Threading;

namespace ClassificationMachine
{
    public partial class FrmWelcome : Form
    {
        
        public FrmWelcome()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }



        private void btn_OK_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmWelcome_Load(object sender, EventArgs e)
        {

        }

        public void KillMe(object o, EventArgs e)
        {
            this.Close();
        }

        public static void LoadAndRun(Form form)
        {//订阅主窗体的句柄创建事件
            form.HandleCreated += delegate
            {
                //启动新线程来显示Welcome窗体
                new Thread(new ThreadStart(delegate
                {
                    FrmWelcome frmWelcome = new FrmWelcome();
                    //订阅主窗体的show事件
                    form.Shown += delegate
                    {
                        frmWelcome.Invoke(new EventHandler(frmWelcome.KillMe));
                        frmWelcome.Dispose();
                    };
                    Application.Run(frmWelcome);
                })).Start(); 

            };
            Application.Run(form);
        }



    }
}
