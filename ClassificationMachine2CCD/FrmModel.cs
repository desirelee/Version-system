using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using Camera_Vision_HHiat;
using System.Runtime.InteropServices;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using System.Collections.ObjectModel;
using System.Collections;
using ImageProcessHHiat;
namespace ClassificationMachine
{
    public partial class FrmModel : Form
    {
        public FrmModel()
        {
            InitializeComponent();
        }

        private void FrmModel_Load(object sender, EventArgs e)
        {
              btnSaveModel.Enabled = false;
              btnOK.Enabled = false;
              string strFile = "c:\\1.bmp";
              if (File.Exists(strFile))
              {
                  C.Image.ReadFile(strFile);
              }
        }

         private void btnSaveModel_Click(object sender, EventArgs e)
        {
            if (imageViewerModel.Image == null)
            {
                MessageBox.Show("先确认模板");
                return;
            }
            string strFile = "";
            strFile = System.Windows.Forms.Application.StartupPath + "\\ModelPos.png";
            MachineTool.LearnPattern(imageViewerModel,3,0);
            imageViewerModel.Image.WriteVisionFile(strFile);
            btnSaveModel.Enabled = false;
        }
 

        private void btnMatch_Click(object sender, EventArgs e)
        {
            btnSaveModel.Enabled = false;
            btnOK.Enabled = false;
            RectangleContour rect = new RectangleContour();
            rect.Left =0;
            rect.Top = 0;
            rect.Width = C.Image.Width;
            rect.Height = C.Image.Height;
            PointContour p2 = new PointContour();
            string TemplateFile="";
            TemplateFile = System.Windows.Forms.Application.StartupPath + "\\ModelPos.png";
            if (File.Exists(TemplateFile) && C.Image !=null)
            {
                imageViewerModel.Image.ReadFile(TemplateFile);
                CIni PosIni = new CIni(System.Windows.Forms.Application.StartupPath + "\\Doc\\Pos.ini");
                float dScore = Convert.ToSingle(PosIni.IniReadValue("PatternPos", "Score"));//匹配度
                p2 = MachineTool.MatchPattern(C.Image , rect, TemplateFile, 1, dScore);
                MessageBox.Show("X:" + p2.X.ToString("f3") + ", Y:" + p2.Y.ToString("f3"));
            }
           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (C.Roi.Count  >0 )
            {
                imageViewerModel.Image.Type = C.Image.Type;
                PixelValue2D Data2D = C.Image.ImageToArray(C.Roi);
                imageViewerModel.Image.ArrayToImage(Data2D);
                C.Roi.Clear();
                C.Image.Overlays.Default.Clear();
                C.RefreshImage();
                btnSaveModel.Enabled = true;
                btnOK.Enabled = false;
            }
        }

        private void C_RoiChanged(object sender, ContoursChangedEventArgs e)
        {
            if (C.Roi.Count > 0)
                btnOK.Enabled = true;
            else
                btnOK.Enabled = false;

        }

    }
}
