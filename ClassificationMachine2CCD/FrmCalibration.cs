using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace ClassificationMachine
{
    public partial class FrmCalibration : Form
    {
        public double nHeight;
        public double nWidth;
        public FrmCalibration()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double OffsetX, OffsetY;
            double LeftTopMX = Convert.ToDouble(txtLeftTopMX.Text.Trim()) * PublicVar.CHXCaliCorr;
            double LeftTopMY = Convert.ToDouble(txtLeftTopMX.Text.Trim()) * PublicVar.CHYCaliCorr;
            double LeftTopCX = Convert.ToDouble(txtLeftTopCX.Text.Trim());
            double LeftTopCY = Convert.ToDouble(txtLeftTopCX.Text.Trim());
            double LeftTopSX = Convert.ToDouble(txtLeftTopSX.Text.Trim());
            double LeftTopSY = Convert.ToDouble(txtLeftTopSX.Text.Trim());
            OffsetX = -(LeftTopCX - nWidth / 2) * PublicVar.CameraX_Unit;
            OffsetY = -(LeftTopCY - nHeight / 2) * PublicVar.CameraY_Unit;
            LeftTopMX += OffsetX;
            LeftTopMY += OffsetY;
            double LeftBottomMX = Convert.ToDouble(txtLeftBottomMX.Text.Trim()) * PublicVar.CHXCaliCorr;
            double LeftBottomMY = Convert.ToDouble(txtLeftBottomMX.Text.Trim()) * PublicVar.CHYCaliCorr;
            double LeftBottomCX = Convert.ToDouble(txtLeftBottomCX.Text.Trim());
            double LeftBottomCY = Convert.ToDouble(txtLeftBottomCX.Text.Trim());
            double LeftBottomSX = Convert.ToDouble(txtLeftBottomSX.Text.Trim());
            double LeftBottomSY = Convert.ToDouble(txtLeftBottomSX.Text.Trim());
            OffsetX = -(LeftBottomCX - nWidth / 2) * PublicVar.CameraX_Unit;
            OffsetY = -(LeftBottomCY - nHeight / 2) * PublicVar.CameraY_Unit;
            LeftBottomMX += OffsetX;
            LeftBottomMY += OffsetY;
            double RightTopMX = Convert.ToDouble(txtLeftTopMX.Text.Trim())*PublicVar.CHXCaliCorr ;
            double RightTopMY = Convert.ToDouble(txtLeftTopMX.Text.Trim()) * PublicVar.CHYCaliCorr;
            double RightTopCX = Convert.ToDouble(txtLeftTopCX.Text.Trim());
            double RightTopCY = Convert.ToDouble(txtLeftTopCX.Text.Trim());
            double RightTopSX = Convert.ToDouble(txtLeftTopSX.Text.Trim());
            double RightTopSY = Convert.ToDouble(txtLeftTopSX.Text.Trim());
            OffsetX = -(RightTopCX - nWidth / 2) * PublicVar.CameraX_Unit;
            OffsetY = -(RightTopCY - nHeight / 2) * PublicVar.CameraY_Unit;
            RightTopMX += OffsetX;
            RightTopMY += OffsetY;
            double RightBottomMX = Convert.ToDouble(txtLeftBottomMX.Text.Trim()) * PublicVar.CHXCaliCorr;
            double RightBottomMY = Convert.ToDouble(txtLeftBottomMX.Text.Trim()) * PublicVar.CHYCaliCorr;
            double RightBottomCX = Convert.ToDouble(txtLeftBottomCX.Text.Trim());
            double RightBottomCY = Convert.ToDouble(txtLeftBottomCX.Text.Trim());
            double RightBottomSX = Convert.ToDouble(txtLeftBottomSX.Text.Trim());
            double RightBottomSY = Convert.ToDouble(txtLeftBottomSX.Text.Trim());
            OffsetX = -(RightBottomCX - nWidth / 2) * PublicVar.CameraX_Unit;
            OffsetY = -(RightBottomCY - nHeight / 2) * PublicVar.CameraY_Unit;
            RightBottomMX += OffsetX;
            RightBottomMY += OffsetY;

            double x1, y1, x2, y2, a1, b1,a2,b2,Angle1,Angle2;
            x1 = LeftTopSX-LeftBottomSX ;
            y1 = LeftTopSY-LeftBottomSY ;
            x2 = RightBottomSX-LeftBottomSX ;
            y2 = RightBottomSY-LeftBottomSY ;
            a1 = LeftTopMX - LeftBottomMX;
            b1 = LeftTopMY - LeftBottomMY;
            a2 = RightBottomMX - LeftBottomMX;
            b2 = RightBottomMY - LeftBottomMY;
            Angle1 = (y1 * b2 - y2 * b1) / (a1 * b2 - a2 * b1);
            Angle1 = Math.Asin(Angle1)*180/3.14159;
            Angle2 = (x1 * a2 - x2 * a1) / (b1 * a2 - b2 * a1);
            Angle2 = Math.Asin(Angle2);
            double x3, y3;
            double angleX,angleY;
            angleX=Angle1*3.14159/180;
            angleY=Angle2 *3.14159/180;
            x3 = RightTopMX * Math.Cos(angleX)+RightTopMY *Math.Sin (angleY );
            y3 = RightTopMY * Math.Cos(angleY) + RightTopMX * Math.Sin(angleX);
            labelX.Text  = x3.ToString("f6");
            labelY.Text  = y3.ToString("f6");
            if (MessageBox.Show("确认保存吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strPath;
                strPath = Application.StartupPath + "\\DOC";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                strPath += "\\Setting.ini";
                CIni IniSetting = new CIni(strPath);
                IniSetting.IniWriteValue("CHX", "RepairAngle", Angle1.ToString("f6"));
                IniSetting.IniWriteValue("CHY", "RepairAngle", Angle1.ToString("f6"));
                PublicVar.CHXRepairAngle = Angle1;
                PublicVar.CHYRepairAngle = Angle2;
            }
        }

        private void FrmCalibration_Load(object sender, EventArgs e)
        {

        }

 
   }
}
