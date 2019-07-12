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
    public partial class FormWaitting : Form
    {
        public FormWaitting()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 等待窗口
        /// </summary>
        /// <param name="time">等待时间（毫秒）</param>
        public FormWaitting(int time)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            
            timer1.Interval = time;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Dispose();
        
        }

        private void FormWaitting_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void FormWaitting_Load(object sender, EventArgs e)
        {

        }
    }
}
