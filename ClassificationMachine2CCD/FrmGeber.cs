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
    public partial class FrmGeber : Form
    {
        ClassGeber m_Geber = new ClassGeber();
        ClassGeber.structData[,] GeberData;
        public  double[] Markdist;
        public  double[] IntervalDist;
        public string strPath;
        public FrmGeber()
        {
            InitializeComponent();
        }

        private void FrmGeber_Load(object sender, EventArgs e)
        {
            GeberData = m_Geber.GeberReader(strPath);

            int m = GeberData.GetUpperBound(0);
            int n = GeberData.GetUpperBound(1);
            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.Red, 1);
            int x1, y1,x2,y2;
            double w = (this.Width - 200) / (GeberData[m, 0].X - GeberData[0, 0].X);
            double h = (this.Height   - 200) / (GeberData[0, n].Y - GeberData[0, 0].Y);
            x1 = 0;// Convert.ToInt16(GeberData[0, 0].X * w);
            y1 = 0;// Convert.ToInt16(GeberData[0, 0].Y * h);
            x2 = 0;// Convert.ToInt16(GeberData[0, 0].X * w);
            y2 = Convert.ToInt16((GeberData[0, n].Y - GeberData[0, 0].Y ) * h);
            TextBox txt1 = new TextBox();
            txt1.Name = "txtL" ;
            txt1.Size = new Size(50, 25);
            txt1.Location = new Point((x1 + x2) / 2 + 60, (y1 + y2) / 2 + 60);
            txt1.Visible = true;
            txt1.Enabled = false;
            txt1.Text = Math.Sqrt((GeberData[0, 0].X - GeberData[0, 0].X) * (GeberData[0, 0].X - GeberData[0, 0].X) + (GeberData[0, 0].Y - GeberData[0, n].Y) * (GeberData[0, 0].Y - GeberData[0, n].Y)).ToString("f1");
            this.Controls.Add(txt1);
            x1 = Convert.ToInt16((GeberData[m, 0].X - GeberData[0, 0] .X )* w);
            y1 = Convert.ToInt16((GeberData[m, 0].Y - GeberData[0, 0].Y ) * h);
            x2 = Convert.ToInt16((GeberData[m, 0].X - GeberData[0, 0].X ) * w);
            y2 = Convert.ToInt16((GeberData[m, n].Y - GeberData[0, 0].Y ) * h);
            TextBox txt2 = new TextBox();
            txt2.Name = "txtL";
            txt2.Size = new Size(50, 25);
            txt2.Location = new Point((x1 + x2) / 2 + 60, (y1 + y2) / 2 + 60);
            txt2.Visible = true;
            txt2.Enabled = false;
            txt2.Text = Math.Sqrt((GeberData[m, 0].X - GeberData[m, 0].X) * (GeberData[m, 0].X - GeberData[m, 0].X) + (GeberData[m, 0].Y - GeberData[m, n].Y) * (GeberData[m, 0].Y - GeberData[m, n].Y)).ToString("f1");
            this.Controls.Add(txt2);
            for (int j = 0; j <= GeberData.GetUpperBound(1); j++)
            {
                for (int i = 0; i < GeberData.GetUpperBound(0); i++)
                {
                    x1 = Convert.ToInt16((GeberData[i, j].X - GeberData[0, 0].X) * w);
                    y1 = Convert.ToInt16((GeberData[i, j].Y - GeberData[0, 0].Y) * h);
                    x2 = Convert.ToInt16((GeberData[i + 1, j].X - GeberData[0, 0].X) * w);
                    y2 = Convert.ToInt16((GeberData[i + 1, j].Y - GeberData[0, 0].Y) * h);
                    TextBox txt = new TextBox();
                    txt.Name = "txt" + (i * j + i).ToString();
                    txt.Size = new Size(50, 25);
                    txt.Location = new Point((x1 +x2)/2+ 60, (y1+y2)/2 + 60);
                    txt.Visible = true;
                    txt.Enabled = false;
                    txt.Text = Math.Sqrt((GeberData[i, j].X - GeberData[i + 1, j].X) * (GeberData[i, j].X - GeberData[i + 1, j].X) + (GeberData[i, j].Y - GeberData[i + 1, j].Y) * (GeberData[i, j].Y - GeberData[i + 1, j].Y)).ToString("f1");
                    this.Controls.Add(txt);
                }
            }
            for (int i = 0; i <= GeberData.GetUpperBound(0); i++)
            {
                x1 = Convert.ToInt16((GeberData[i, 0].X - GeberData[0, 0].X) * w);
                y1 = Convert.ToInt16((GeberData[i, 0].Y - GeberData[0, 0].Y) * h);
                CheckBox chk = new CheckBox();
                chk.Name = "chk" + i.ToString();
                chk.Size = new Size(25, 25);
                chk.Location = new Point(x1 + 60, y1 + 20);
                chk.Visible = true;
                this.Controls.Add(chk);
            }
        }

 
        private void FrmGeber_Paint(object sender, PaintEventArgs e)
        {
            int m = GeberData.GetUpperBound(0);
            int n = GeberData.GetUpperBound(1);
            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.Red, 1);
            int x1, y1, x2, y2;
            double w = (this.Width - 200) / (GeberData[m, 0].X - GeberData[0, 0].X);
            double h = (this.Height - 200) / (GeberData[0, n].Y - GeberData[0, 0].Y);
            x1 = 0;// Convert.ToInt16(GeberData[0, 0].X * w);
            y1 = 0;// Convert.ToInt16(GeberData[0, 0].Y * h);
            x2 = 0;// Convert.ToInt16(GeberData[0, 0].X * w);
            y2 = Convert.ToInt16((GeberData[0, n].Y - GeberData[0, 0].Y) * h);
            g.DrawLine(p, x1 + 50, y1 + 50, x2 + 50, y2 + 50);
            x1 = Convert.ToInt16((GeberData[m, 0].X - GeberData[0, 0].X )* w);
            y1 = Convert.ToInt16((GeberData[m, 0].Y - GeberData[0, 0].Y) * h);
            x2 = Convert.ToInt16((GeberData[m, 0].X - GeberData[0, 0].X) * w);
            y2 = Convert.ToInt16((GeberData[m, n].Y - GeberData[0, 0].Y )* h);
            g.DrawLine(p, x1 + 50, y1 + 50, x2 + 50, y2 + 50);
            for (int j = 0; j <= n; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    x1 = Convert.ToInt16((GeberData[i, j].X - GeberData[0, 0].X) * w);
                    y1 = Convert.ToInt16((GeberData[i, j].Y - GeberData[0, 0].Y ) * h);
                    x2 = Convert.ToInt16((GeberData[i + 1, j].X-GeberData[0, 0].X ) * w);
                    y2 = Convert.ToInt16((GeberData[i + 1, j].Y-GeberData[0, 0].Y ) * h);
                    g.DrawLine(p, x1 + 50, y1 + 50, x2 + 50, y2 + 50);
                }
            }
            for (int j = 0; j <= n; j++)
            {
                for (int i = 0; i <= m; i++)
                {
                    x1 = Convert.ToInt16((GeberData[i, j].X - GeberData[0, 0].X ) * w);
                    y1 = Convert.ToInt16((GeberData[i, j].Y-GeberData[0, 0].Y ) * h);
                    g.DrawEllipse(p, x1 + 40, y1 + 40, 20, 20);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int m = GeberData.GetUpperBound(0)+1;
            double[] Markdist = new double[m / 2];
            double[] IntervalDist = new double[m / 2-1 ];
            int n1 = 0,n2=0;
            int j1 = 0,j2=0;
            int k1 = 0, k2 = 0;
            int i = 0;
            Control temp = new Control();
                foreach (Control c in Controls)
                {
                    temp = c as CheckBox;
                    if (temp != null)
                    {
                        CheckBox chk = (CheckBox)temp;
                        if (chk.Checked == true)
                        {
                            if (n1 > 0 && (n1 % 2) == 1)
                            {
                                Markdist[k1] = GeberData[i, 0].X - GeberData[j1, 0].X;
                                k1++;
                                n1 = 0;
                            }
                            else
                            {
                                j1 = i;
                                n1++;
                            }
                            if (n2 > 1 && (n2 % 2) == 1)
                            {
                                IntervalDist[k2] = GeberData[i, 0].X - GeberData[j2, 0].X;
                                k2++;
                                n2 = 0;
                            }
                            else
                            {
                                j2 = i;
                                n2++;
                            }
                        }
                        i++;
                    }
                }
                Markdist = new double[k1];
                for (int p = 0; p < k1; p++)
                    Markdist[p] = Markdist[p];
                IntervalDist = new double[k2];
                for (int q = 0; q < k2; q++)
                    IntervalDist[q] = IntervalDist[q];
                if (n1 != 0 )
                    MessageBox.Show("选择错误，重新选择");
                else
                {
                    if (MessageBox.Show("确定吗？","一张基材贴" + (k2 + 1).ToString() + "张菲林", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        Close();
                }
        }
    }
}
