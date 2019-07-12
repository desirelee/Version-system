using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;

using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using System.Threading;
using Class_Motion;
using Class_Com;
using Camera_Vision_HHiat;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using ImageProcessHHiat;
using System.Threading.Tasks;
//using System.Runtime.InteropServices;

namespace ClassificationMachine
{
    public partial class FrmMain : Form
    {
        public struct CircleParam
        {
           public double X;
           public double Y;
           public double R;
        };
        CircleParam ParamCircle;
        public struct ResultData
        {
            public  double W1;
            public  double W2;
            public double L1;
            public double L2;
            public double L13;
            public double L24;
            //6点数据
            public double X1, X2, Y1, Y2, Y3;
            //9点数据
            public double LA, LB, LC, LD, LE, LF, LG, LH, LI, LJ, LK, LL;
        }
        public struct LineCenterAngle
        {
            public double X;
            public double Y;
            public double Angle;
        };
        int TakePhotoDelay = 30;
        ResultData[] m_ResultData;
        double x1, y1, x2, y2;
        PointContour[] ManyFitPoint;
        int iManyFitPointCount = 0;
        int iRow = 0;
        OvalContour oval ;
        LineContour l1 ;
        LineContour l2 ;
        int iSelectRow = 0;
        int iSelect1, iSelect2;
        public ClassMotion m_ClassMotion;
        public ClassCom m_ClassCom;
        ClassCameraNet hhiatalign;
        string sDebug;
        string sPwd;
        string sUSB;
        int[] Sequence;
        double[] CenterX ;
        double[] CenterY;
        double[] InnerR;
        double[] OuterR ;
        double[] StartA ;
        double[] EndA ;
        double[] MotorX ;
        double[] MotorY ;
        double[] R;
        int[] Colors;
        int[] Types;
        int iStep;
        int iMasterStep;
        string strSerialC1, strSerialC2;
        private ClassCameraNet m_Camera;
        bool bAuto = false;
        double[] TestX ;
        double[] TestY ;
        double[] TestOffsetX;
        double[] TestOffsetY;
        int iCount = 0;
        int iXCount = 0;
        int iYCount = 0;
        int iXCountSet = 0;
        int iYCountSet = 0;
        int iCycleTime = 0;
        bool bFirst = false;
        bool bButton = false;
        FrmCalibration frm2 = new FrmCalibration();
        ClassRepairData Rdata = new ClassRepairData();
        double dXPreCurrent, dYPreCurrent;
        bool bMasterTest = false;
        double dx = 0, dy = 0, da = 0;
        bool bStop = false;
        int iAdjustTime = 0;
        Microsoft.Office.Interop.Excel.Application ExcelApp = null;
        Microsoft.Office.Interop.Excel.Workbook ExcelWb = null;
        Sheets sheets = null;
        Worksheet ExcelWs = null;
        Microsoft.Office.Interop.Excel.Range ExcelRange = null;
        bool bSaved = false;
        #region imageprocess
        public CircleParam SearchAreaCenter(VisionImage image1, double AreaSet, double AreaRange)
        {
            CircleParam m_CircleParam;
            m_CircleParam.X = -1000;
            m_CircleParam.Y = -1000;
            m_CircleParam.R = 1000;
            try
            {

                ParticleMeasurementsReport vaParticleReport = new ParticleMeasurementsReport();
                ParticleMeasurementsReport vaParticleReportCalibrated = new ParticleMeasurementsReport();
                // Algorithms.AutoThreshold(image1, img, 2, ThresholdMethod.Metric);
                Collection<MeasurementType> vaPixelMeasurements = new Collection<MeasurementType>(new MeasurementType[] { MeasurementType.CenterOfMassX, MeasurementType.CenterOfMassY, MeasurementType.FirstPixelX, MeasurementType.FirstPixelY, MeasurementType.BoundingRectLeft, MeasurementType.BoundingRectTop, MeasurementType.BoundingRectRight, MeasurementType.BoundingRectBottom, MeasurementType.MaxFeretDiameterStartX, MeasurementType.MaxFeretDiameterStartY, MeasurementType.MaxFeretDiameterEndX, MeasurementType.MaxFeretDiameterEndY, MeasurementType.MaxHorizontalSegmentLengthLeft, MeasurementType.MaxHorizontalSegmentLengthRight, MeasurementType.MaxHorizontalSegmentLengthRow, MeasurementType.BoundingRectWidth, MeasurementType.BoundingRectHeight, MeasurementType.BoundingRectDiagonal, MeasurementType.Perimeter, MeasurementType.ConvexHullPerimeter, MeasurementType.HolesPerimeter, MeasurementType.MaxFeretDiameter, MeasurementType.EquivalentEllipseMajorAxis, MeasurementType.EquivalentEllipseMinorAxis, MeasurementType.EquivalentEllipseMinorAxisFeret, MeasurementType.EquivalentRectLongSide, MeasurementType.EquivalentRectShortSide, MeasurementType.EquivalentRectDiagonal, MeasurementType.EquivalentRectShortSideFeret, MeasurementType.AverageHorizontalSegmentLength, MeasurementType.AverageVerticalSegmentLength, MeasurementType.HydraulicRadius, MeasurementType.WaddelDiskDiameter, MeasurementType.Area, MeasurementType.HolesArea, MeasurementType.ParticleAndHolesArea, MeasurementType.ConvexHullArea, MeasurementType.ImageArea, MeasurementType.NumberOfHoles, MeasurementType.NumberOfHorizontalSegments, MeasurementType.NumberOfVerticalSegments, MeasurementType.Orientation, MeasurementType.MaxFeretDiameterOrientation, MeasurementType.AreaByImageArea, MeasurementType.AreaByParticleAndHolesArea, MeasurementType.RatioOfEquivalentEllipseAxes, MeasurementType.RatioOfEquivalentRectSides, MeasurementType.ElongationFactor, MeasurementType.CompactnessFactor, MeasurementType.HeywoodCircularityFactor, MeasurementType.TypeFactor, MeasurementType.SumX, MeasurementType.SumY, MeasurementType.SumXX, MeasurementType.SumXY, MeasurementType.SumYY, MeasurementType.SumXXX, MeasurementType.SumXXY, MeasurementType.SumXYY, MeasurementType.SumYYY, MeasurementType.MomentOfInertiaXX, MeasurementType.MomentOfInertiaXY, MeasurementType.MomentOfInertiaYY, MeasurementType.MomentOfInertiaXXX, MeasurementType.MomentOfInertiaXXY, MeasurementType.MomentOfInertiaXYY, MeasurementType.MomentOfInertiaYYY, MeasurementType.NormalizedMomentOfInertiaXX, MeasurementType.NormalizedMomentOfInertiaXY, MeasurementType.NormalizedMomentOfInertiaYY, MeasurementType.NormalizedMomentOfInertiaXXX, MeasurementType.NormalizedMomentOfInertiaXXY, MeasurementType.NormalizedMomentOfInertiaXYY, MeasurementType.NormalizedMomentOfInertiaYYY, MeasurementType.HuMoment1, MeasurementType.HuMoment2, MeasurementType.HuMoment3, MeasurementType.HuMoment4, MeasurementType.HuMoment5, MeasurementType.HuMoment6, MeasurementType.HuMoment7 });
                Collection<MeasurementType> vaCalibratedMeasurements = new Collection<MeasurementType>(new MeasurementType[] { });
                Connectivity connectivity = Connectivity.Connectivity8;

                // Computes the requested pixel measurements.
                if (vaPixelMeasurements.Count != 0)
                {
                    vaParticleReport = Algorithms.ParticleMeasurements(image1, vaPixelMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Pixel);
                }
                else
                {
                    vaParticleReport = new ParticleMeasurementsReport();
                }

                // Computes the requested calibrated measurements.
                if (vaCalibratedMeasurements.Count != 0)
                {
                    vaParticleReportCalibrated = Algorithms.ParticleMeasurements(image1, vaCalibratedMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Calibrated);
                }
                else
                {
                    vaParticleReportCalibrated = new ParticleMeasurementsReport();
                }

                // Computes the center of mass of each particle to log as results.
                ParticleMeasurementsReport centerOfMass;
                Collection<MeasurementType> centerOfMassMeasurements = new Collection<MeasurementType>();
                centerOfMassMeasurements.Add(MeasurementType.CenterOfMassX);
                centerOfMassMeasurements.Add(MeasurementType.CenterOfMassY);
                centerOfMassMeasurements.Add(MeasurementType.Area);
                centerOfMassMeasurements.Add(MeasurementType.BoundingRectHeight);
                centerOfMassMeasurements.Add(MeasurementType.BoundingRectWidth);
                if ((image1.InfoTypes & InfoTypes.Calibration) != 0)
                {
                    centerOfMass = Algorithms.ParticleMeasurements(image1, centerOfMassMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Both);
                }
                else
                {
                    centerOfMass = Algorithms.ParticleMeasurements(image1, centerOfMassMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Pixel);
                }
                int iIndex = -1;
                for (int i = 0; i < centerOfMass.PixelMeasurements.GetLength(0); i++)
                {
                    double a = centerOfMass.PixelMeasurements[i, 2];
                    double h = centerOfMass.PixelMeasurements[i, 3];
                    double w = centerOfMass.PixelMeasurements[i, 4];
                    double ratio = 1;
                    if (h > w)
                        ratio = w / h;
                    else
                        ratio = w / h;
                    if (Math.Abs(a - AreaSet) <= AreaRange && ratio > 0.75 && h > 10 && (image1.Height - h) > 50 && w > 10 && (image1.Width - w) > 50)
                    {
                        iIndex = i;
                    }
                }

                if (iIndex >= 0)
                {
                    double x, y;
                    x = centerOfMass.PixelMeasurements[iIndex, 0];
                    y = centerOfMass.PixelMeasurements[iIndex, 1];

                    if (x > image1.Width) x = image1.Width - 10;
                    if (x < 0) x = 10;
                    if (y > image1.Height) y = image1.Height - 10;
                    if (y < 0) y = 10;
                    m_CircleParam.X = x;
                    m_CircleParam.Y = y;

                }
            }
            catch { }
            return m_CircleParam;
        }
        private CircleParam DetectCircleCenter(VisionImage SourceImage, RectangleContour rectangle, double bigcircle, double smallcircle,
                                                    double CUnit, double dCr, double dRange, bool bBlack,int iThreshold=75)
            {
                CircleParam m_CircleParam ;
                m_CircleParam.X = -1000;
                m_CircleParam.Y = -1000;
                m_CircleParam.R = -1000;
                try
                {
                    PointContour GrayPoint = new PointContour();
                    Roi roi = new Roi();
                    roi.Add(rectangle);
                    CircleDescriptor vaCircleDescriptor = new CircleDescriptor(smallcircle, bigcircle);
                    CurveOptions vaCurveOptions = new CurveOptions();
                    vaCurveOptions.ColumnStepSize = 15;
                    vaCurveOptions.ExtractionMode = ExtractionMode.NormalImage;
                    vaCurveOptions.FilterSize = EdgeFilterSize.Normal;
                    vaCurveOptions.MaximumEndPointGap = 10;
                    vaCurveOptions.MinimumLength = 25;
                    vaCurveOptions.RowStepSize = 15;

                    vaCurveOptions.SubpixelAccuracy = true;
                    vaCurveOptions.Threshold = iThreshold;

                    ShapeDetectionOptions vaShapeOptions = new ShapeDetectionOptions();
                    vaShapeOptions.MinimumMatchScore = 600;
                    vaShapeOptions.Mode = (GeometricMatchModes)5;
                    double[] vaRangesMin = { 0, 0, 50 };
                    double[] vaRangesMax = { 360, 0, 100 };
                    vaShapeOptions.RotationAngleRanges.Add(new  NationalInstruments.Vision.Range(vaRangesMin[0], vaRangesMax[0]));
                    vaShapeOptions.RotationAngleRanges.Add(new NationalInstruments.Vision.Range(vaRangesMin[1], vaRangesMax[1]));
                    vaShapeOptions.ScaleRange = new NationalInstruments.Vision.Range(vaRangesMin[2], vaRangesMax[2]);
                    // Detect Circles
                    Collection<CircleMatch> circles = Algorithms.DetectCircles(SourceImage, vaCircleDescriptor, roi, vaCurveOptions, vaShapeOptions);
                    roi.Dispose();
                    if (circles.Count > 0)
                    {
                        CircleMatch tempCircleMatch = null;
                        if (circles.Count >= 1)
                        {
                            var p1 = from cc in circles
                                     where
                                             Math.Abs( 2*cc.Radius * CUnit - 2*dCr) <= dRange
                                     select cc;
                            if (p1.Count() > 0)
                            {
                                tempCircleMatch = (CircleMatch)p1.First();
                                double dRmin = 10000;
                                int iMin = -1;
                                for (int im = 0; im < p1.Count(); im++)
                                {
                                    tempCircleMatch = (CircleMatch)p1.ElementAt(im);
                                    GrayPoint.X = tempCircleMatch.Center.X + tempCircleMatch.Radius - 10;
                                    if (GrayPoint.X > rectangle.Width) GrayPoint.X = rectangle.Width - 10;
                                    if (GrayPoint.X < rectangle.Left) GrayPoint.X = rectangle.Left + 10;
                                    GrayPoint.Y = tempCircleMatch.Center.Y;
                                    if (GrayPoint.Y > rectangle.Height) GrayPoint.Y = rectangle.Height - 10;
                                    if (GrayPoint.Y < rectangle.Top) GrayPoint.Y = rectangle.Top + 10;
                                    float d = SourceImage.GetPixel(GrayPoint).Grayscale;
                                    if (bBlack == true && SourceImage.GetPixel(GrayPoint).Grayscale < 50)
                                    {
                                        if (Math.Abs(tempCircleMatch.Radius * CUnit - dCr) < dRmin)
                                        {
                                            dRmin = Math.Abs(tempCircleMatch.Radius - dCr);
                                            iMin = im;
                                        }
                                    }
                                    else if (bBlack == false && SourceImage.GetPixel(GrayPoint).Grayscale > 150)
                                    {
                                        if (Math.Abs(tempCircleMatch.Radius * CUnit - dCr) < dRmin)
                                        {
                                            dRmin = Math.Abs(tempCircleMatch.Radius * CUnit - dCr);
                                            iMin = im;
                                        }
                                    }
                                }
                                if (iMin >= 0)
                                {
                                    tempCircleMatch = (CircleMatch)p1.ElementAt(iMin);
                                    GrayPoint = tempCircleMatch.Center;
                                    SourceImage.Overlays.Default.AddRectangle(new RectangleContour(GrayPoint.X - tempCircleMatch.Radius - 3, GrayPoint.Y - tempCircleMatch.Radius - 3, 2 * (tempCircleMatch.Radius + 3), 2 * (tempCircleMatch.Radius + 3)), Rgb32Value.GreenColor);
                                }
                                else
                                {
                                    tempCircleMatch = null;
                                }
                            }
                        }
                        circles.Clear();

                        if (tempCircleMatch != null)
                        {
                            circles.Add(tempCircleMatch);
                            m_CircleParam.X = tempCircleMatch.Center.X;
                            m_CircleParam.Y = tempCircleMatch.Center.Y;
                            m_CircleParam.R = tempCircleMatch.Radius;
                        }
                    }
                }
                catch { }
                return m_CircleParam;
            }
            VisionImage img = new VisionImage();
            private VisionImage ThresholdImage(int iThreshold, VisionImage image,bool bInverse)
            {
                try
                {
                    if (bInverse == true)
                    {
                        Algorithms.Inverse(image, img);
                    }
                    else
                    {
                        Algorithms.Copy(image, img);
                    }
                    if (iThreshold > 0)
                        Algorithms.Threshold(img, img, new NationalInstruments.Vision.Range(iThreshold, 255), true, 255);
                    else
                        Algorithms.Copy(image, img);
                }
                catch { }
                return img;
            }
            private CircleParam FindCircluarEdge(VisionImage image, double vRadius, double CenterX, double CenterY, double rRange,int iDir,int iParity)
            {
                CircleParam m_CircleParam;
                m_CircleParam.X = -1000;
                m_CircleParam.Y = -1000;
                m_CircleParam.R = -1000;
                try
                {
                    double InnerRadius;
                    double OuterRadius;
                    InnerRadius = vRadius - rRange;
                    OuterRadius = vRadius + rRange;
                    // Creates a new, empty region of interest.
                    Roi roi = new Roi();
                    // Creates a new AnnulusContour using the given values.
                    PointContour vaCenter = new PointContour(CenterX, CenterY);
                    AnnulusContour vaOval = new AnnulusContour(vaCenter, InnerRadius, OuterRadius, 0, 360);
                    roi.Add(vaOval);
                    // Find Circular Edge
                    EdgeOptions vaOptions = new EdgeOptions();
                    vaOptions.ColumnProcessingMode = ColumnProcessingMode.Average;
                    vaOptions.InterpolationType = InterpolationMethod.Bilinear;
                    vaOptions.KernelSize = 3;
                    vaOptions.MinimumThreshold = 164;
                    vaOptions.Polarity = EdgePolaritySearchMode.All;
                    switch (iParity)
                    {
                        case 0:
                            vaOptions.Polarity = EdgePolaritySearchMode.All;
                            break;
                        case 1:
                            vaOptions.Polarity = EdgePolaritySearchMode.Falling;
                            break;
                        case 2:
                            vaOptions.Polarity = EdgePolaritySearchMode.Rising;
                            break;
                    }
                    vaOptions.Width = 3;
                    CircularEdgeFitOptions vaFitOptions = new CircularEdgeFitOptions();
                    vaFitOptions.ProcessType = RakeProcessType.GetFirstEdges;
                    vaFitOptions.StepSize = 2;
                    vaFitOptions.MaxPixelRadius = 3;
                    SpokeDirection direction;
                    if (iDir == 0)
                    {
                        direction = SpokeDirection.OutsideToInside ;
                    }
                    else
                    {
                        direction = SpokeDirection.InsideToOutside;
                    }
                    FindCircularEdgeOptions circleOptions = new FindCircularEdgeOptions(direction);
                    circleOptions.EdgeOptions = vaOptions;
                    FindCircularEdgeReport circleReport = new FindCircularEdgeReport();
                    circleReport = Algorithms.FindCircularEdge(image, roi, circleOptions, vaFitOptions);
                    if (circleReport.CircleFound)
                    {
                        oval = null;
                        oval = new OvalContour();
                        oval.Left = circleReport.Center.X - circleReport.Radius;
                        oval.Top = circleReport.Center.Y - circleReport.Radius;
                        oval.Height = circleReport.Radius * 2;
                        oval.Width = circleReport.Radius * 2;
                        ImgView.Image.Overlays.Default.AddOval(oval, Rgb32Value.RedColor);
                        string strInfo = "X=" + circleReport.Center.X.ToString("f2") + "\nY=" + circleReport.Center.Y.ToString("f2") + "\nR=" + (circleReport.Radius * PublicVar.CameraX_Unit).ToString("f2");
                        //ImgView.Image.Overlays.Default.AddText(strInfo, new PointContour(20, 10));
                        lblDisplayPoints.Visible = true;
                        lblDisplayPoints.Text = strInfo;
                        m_CircleParam.X = circleReport.Center.X;
                        m_CircleParam.Y = circleReport.Center.Y;
                        m_CircleParam.R = circleReport.Radius;
                    }
                    roi.Dispose();
                }
                catch { }
                return m_CircleParam;
            }
            private CircleParam FindCircluarEdge(VisionImage image, AnnulusContour vaOval,int iDir,int iParity,int iThreshold=164)
            {
                CircleParam m_CircleParam;
                m_CircleParam.X = -1000;
                m_CircleParam.Y = -1000;
                m_CircleParam.R = -1000;
                try
                {
                    // Creates a new, empty region of interest.
                    Roi roi = new Roi();
                    roi.Add(vaOval);
                    // Find Circular Edge
                    EdgeOptions vaOptions = new EdgeOptions();
                    vaOptions.ColumnProcessingMode = ColumnProcessingMode.Average;
                    vaOptions.InterpolationType = InterpolationMethod.Bilinear;
                    vaOptions.KernelSize = 3;
                    vaOptions.MinimumThreshold = iThreshold;
                    vaOptions.Polarity = EdgePolaritySearchMode.All;
                    switch (iParity)
                    { 
                        case 0:
                            vaOptions.Polarity = EdgePolaritySearchMode.All;
                           break;
                        case 1:
                           vaOptions.Polarity = EdgePolaritySearchMode.Falling ;
                           break;
                        case 2:
                           vaOptions.Polarity = EdgePolaritySearchMode.Rising ;
                           break;
                    }
                    vaOptions.Width = 3;
                    CircularEdgeFitOptions vaFitOptions = new CircularEdgeFitOptions();
                    vaFitOptions.ProcessType = RakeProcessType.GetFirstEdges;
                    vaFitOptions.StepSize = 2;
                    vaFitOptions.MaxPixelRadius = 3;
                    SpokeDirection direction;
                    if (iDir ==0)
                    {
                            direction = SpokeDirection.InsideToOutside;
                    }
                    else
                    {
                            direction = SpokeDirection.InsideToOutside ;
                    }
                    FindCircularEdgeOptions circleOptions = new FindCircularEdgeOptions(direction);
                    circleOptions.EdgeOptions = vaOptions;
                    FindCircularEdgeReport circleReport = new FindCircularEdgeReport();
                    circleReport = Algorithms.FindCircularEdge(image, roi, circleOptions, vaFitOptions);
                    if (circleReport.CircleFound)
                    {
                        oval = null;
                        oval = new OvalContour();
                        oval.Left = circleReport.Center.X - circleReport.Radius;
                        oval.Top = circleReport.Center.Y - circleReport.Radius;
                        oval.Height = circleReport.Radius * 2;
                        oval.Width = circleReport.Radius * 2;
                        ImgView.Image.Overlays.Default.AddOval(oval, Rgb32Value.RedColor);
                        string strInfo = "X=" + circleReport.Center.X.ToString("f2") + "\nY=" + circleReport.Center.Y.ToString("f2") + "\nR=" + (circleReport.Radius * PublicVar.CameraX_Unit).ToString("f2");
                        //ImgView.Image.Overlays.Default.AddText(strInfo , new PointContour(20, 10));
                        lblDisplayPoints.Visible = true;
                        lblDisplayPoints.Text = strInfo ;
                        m_CircleParam.X = circleReport.Center.X;
                        m_CircleParam.Y = circleReport.Center.Y;
                        m_CircleParam.R = circleReport.Radius;
                    }
                    roi.Dispose();
                }
                catch { }
                return m_CircleParam;
            }
            private CircleParam ImageProcess1(VisionImage ImgC, int iThreshold, bool bThreshold, double dCr, bool bBlack)
            {
                CircleParam m_CircleParam;
                m_CircleParam.X = -1000;
                m_CircleParam.Y = -1000;
                m_CircleParam.R = -1000;
                try
                {
                    double dRange = 0.1 ;
                    string strPath;
                    strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    strPath += "\\Setting.ini";
                    CIni IniSetting = new CIni(strPath);
                    string strTemp;
                    strTemp = IniSetting.IniReadValue("Unit", "CameraX");
                    double CUnit = Convert.ToDouble(strTemp);
                    RectangleContour rect = new RectangleContour();
                    strTemp = IniSetting.IniReadValue("Param", "Range");
                    dRange = Convert.ToDouble(strTemp);
                    rect.Left = 0;
                    rect.Top = 0;
                    rect.Width = ImgC.Width;
                    rect.Height = ImgC.Height;
                    double s = 3.14159 * (dCr / PublicVar.CameraX_Unit) * (dCr / PublicVar.CameraX_Unit);
                    m_CircleParam = SearchAreaCenter(ThresholdImage(iThreshold, ImgC, bBlack), s, s * dRange);
                    ImgC.Overlays.Default.AddPoint(new PointContour(m_CircleParam.X, m_CircleParam.Y), Rgb32Value.RedColor);
                    if (m_CircleParam.X > 0 && m_CircleParam.Y > 0 && m_CircleParam.R > 0)
                    {
                        m_CircleParam = FindCircluarEdge(ThresholdImage(iThreshold, ImgC, false), dCr / PublicVar.CameraX_Unit, m_CircleParam.X, m_CircleParam.Y, 100, listBoxDirectory.SelectedIndex, listBoxParity.SelectedIndex);
                    }
                    /*
                    if((m_CircleParam.X <0 || m_CircleParam.Y <0)|| bBlack ==true)
                    {
                        m_CircleParam = DetectCircleCenter(ThresholdImage(iThreshold, ImgC, bBlack), rect, 1000, 50, CUnit, dCr, dRange, bBlack);
                        if (m_CircleParam.X > 0 && m_CircleParam.Y > 0 && m_CircleParam.R > 0)
                        {
                            m_CircleParam = FindCircluarEdge(ThresholdImage(iThreshold, ImgC, bBlack), m_CircleParam.R, m_CircleParam.X, m_CircleParam.Y, 20, listBoxDirectory.SelectedIndex, listBoxParity.SelectedIndex);
                        }
                    }*/
                    lblDisplayPoints.Text += "(" + (m_CircleParam.R * PublicVar.CameraX_Unit).ToString("f2") + "mm)";
                }
                catch { }
                return m_CircleParam;
            }
            private CircleParam ImageProcessPos(VisionImage ImgC)
            {
                CircleParam m_CircleParam;
                m_CircleParam.X = -1000;
                m_CircleParam.Y = -1000;
                m_CircleParam.R = -1000;
                try
                {
                     CIni PosIni = new CIni(System.Windows.Forms.Application.StartupPath + "\\Doc\\Pos.ini");
                   string  sWay = PosIni.IniReadValue("PatternPos", "Way");
                   if (sWay == "0")
                   {
                       double dLeftP, dTopP, dWidth, dHeight;
                       dLeftP = Convert.ToDouble(PosIni.IniReadValue("H", "Left"));
                       dTopP = Convert.ToDouble(PosIni.IniReadValue("H", "Top"));
                       dWidth = Convert.ToDouble(PosIni.IniReadValue("H", "Width"));
                       dHeight = Convert.ToDouble(PosIni.IniReadValue("H", "Height"));
                       int iThreshold = Convert.ToInt16(PosIni.IniReadValue("H", "Threshold"));
                       ImgView.Image.Overlays.Default.AddRectangle(new RectangleContour(dLeftP, dTopP, dWidth, dHeight), Rgb32Value.YellowColor);
                       LineCenterAngle caH = DetectEdge(ImgC, dLeftP + dWidth / 2, dTopP + dHeight / 2, dWidth, dHeight, RakeDirection.RightToLeft, EdgePolaritySearchMode.Rising, iThreshold);
                       dLeftP = Convert.ToDouble(PosIni.IniReadValue("V", "Left"));
                       dTopP = Convert.ToDouble(PosIni.IniReadValue("V", "Top"));
                       dWidth = Convert.ToDouble(PosIni.IniReadValue("V", "Width"));
                       dHeight = Convert.ToDouble(PosIni.IniReadValue("V", "Height"));
                       iThreshold = Convert.ToInt16(PosIni.IniReadValue("V", "Threshold"));
                       ImgView.Image.Overlays.Default.AddRectangle(new RectangleContour(dLeftP, dTopP, dWidth, dHeight), Rgb32Value.GreenColor);
                       LineCenterAngle caV = DetectEdge(ImgC, dLeftP + dWidth / 2, dTopP + dHeight / 2, dWidth, dHeight, RakeDirection.TopToBottom, EdgePolaritySearchMode.Falling, iThreshold);
                       Algorithms.Copy(ImgView.Image, ImgViewPos.Image);
                       m_CircleParam.X = caH.X;
                       m_CircleParam.Y = caV.Y;
                       m_CircleParam.R = (caH.Angle + caV.Angle) / 2 * Math.PI / 180;

                   }
                   else
                   {

                       RectangleContour rect = new RectangleContour();
                       rect.Left = 0;
                       rect.Top = 0;
                       rect.Width = ImgC.Width;
                       rect.Height = ImgC.Height;
                       PointContour p2 = new PointContour();
                       string TemplateFile = "";
                       TemplateFile = System.Windows.Forms.Application.StartupPath + "\\ModelPos.png";
                       if (File.Exists(TemplateFile) && ImgC != null)
                       {
                           float dScore = Convert.ToSingle(PosIni.IniReadValue("PatternPos", "Score"));
                           p2 = MachineTool.MatchPattern(ImgC, rect, TemplateFile, 1, dScore);
                           m_CircleParam.X = p2.X ;
                           m_CircleParam.Y = p2.Y;
                       }
                   }
                }
                catch { }
                Algorithms.Copy(ImgView.Image, ImgViewPos.Image);
                return m_CircleParam;
            }
            private LineCenterAngle DetectEdge(VisionImage SourceImage, double cx, double cy, double width, double height, RakeDirection direction, EdgePolaritySearchMode Polarity,int iThreshold=0)
            {
                img=ThresholdImage(iThreshold, SourceImage, false);
               // Creates a new, empty region of interest.
                Roi roi = new Roi();
                // Creates a new RotatedRectangleContour using the given values.
                PointContour vaCenter = new PointContour(cx, cy);
                RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, width, height, 0);
                roi.Add(vaRotatedRect);
                // Find Straight Edge
                EdgeOptions vaOptions = new EdgeOptions();
                vaOptions.ColumnProcessingMode = ColumnProcessingMode.Average;
                vaOptions.InterpolationType = InterpolationMethod.Bilinear;
                vaOptions.KernelSize = 13;
                vaOptions.MinimumThreshold = 45;
                vaOptions.Polarity = Polarity;// EdgePolaritySearchMode.All;
                vaOptions.Width = 11;
                StraightEdgeOptions vaStraightEdgeOptions = new StraightEdgeOptions();
                vaStraightEdgeOptions.AngleRange = 45;
                vaStraightEdgeOptions.AngleTolerance = 1;
                vaStraightEdgeOptions.HoughIterations = 5;
                vaStraightEdgeOptions.MinimumCoverage = 25;
                vaStraightEdgeOptions.MinimumSignalToNoiseRatio = 0;
                vaStraightEdgeOptions.NumberOfLines = 1;
                vaStraightEdgeOptions.Orientation = 0;
                NationalInstruments.Vision.Range vaRange = new NationalInstruments.Vision.Range(0, 1000);
                vaStraightEdgeOptions.ScoreRange = vaRange;
                vaStraightEdgeOptions.StepSize = 33;
                vaStraightEdgeOptions.SearchMode = StraightEdgeSearchMode.FirstRakeEdges;

                // Find the Edge
                FindEdgeOptions edgeOptions = new FindEdgeOptions(direction);//RakeDirection.LeftToRight
                edgeOptions.EdgeOptions = vaOptions;
                edgeOptions.StraightEdgeOptions = vaStraightEdgeOptions;
                FindEdgeReport lineReport = new FindEdgeReport();
                lineReport = Algorithms.FindEdge(img, roi, edgeOptions);
                roi.Dispose();
                LineCenterAngle cA;
                double x11, y11, x12, y12;
                if (lineReport.StraightEdges.Count >= 1)
                {
                    x11 = lineReport.StraightEdges[0].StraightEdge.Start.X;
                    y11 = lineReport.StraightEdges[0].StraightEdge.Start.Y;
                    x12 = lineReport.StraightEdges[0].StraightEdge.End.X;
                    y12 = lineReport.StraightEdges[0].StraightEdge.End.Y;
                    cA.X = (x11 + x12) / 2;
                    cA.Y = (y11 + y12) / 2;
                    cA.Angle = lineReport.StraightEdges[0].Angle;
                    OverlayTextOptions option = new OverlayTextOptions();
                    option.FontSize = 50;
                    PointContour p = new PointContour(10, cA.Y+20);
                    SourceImage.Overlays.Default.AddText("(" + cA.X.ToString("f2") + "," + cA.Y.ToString("f2")
                        + "):A" + cA.Angle.ToString("f3"), p, Rgb32Value.RedColor, option);
                    LineContour l11 = new LineContour();
                    l11.Start.X = x11;
                    l11.Start.Y = y11;
                    l11.End.X = x12;
                    l11.End.Y = y12;
                    SourceImage.Overlays.Default.AddLine(l11, Rgb32Value.RedColor);
                }
                else
                {
                    cA.X = -10000;
                    cA.Y = -10000;
                    cA.Angle = -10000;
                }
                return cA;
            }
        #endregion
        private bool ReadParam()
            {
                    string strTemp;
                    string strPath;
                    strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    strPath += "\\Setting.ini";
                    CIni IniSetting = new CIni(strPath);
                    strTemp = IniSetting.IniReadValue("Unit", "CameraX");
                    PublicVar.CameraX_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "CameraY");
                    PublicVar.CameraY_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "CameraPos");
                    PublicVar.CameraPos_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "X");
                    PublicVar.CHXMotor_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "Y");
                    PublicVar.CHYMotor_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "CY");
                    PublicVar.CHCYMotor_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "CZ");
                    PublicVar.CHCZMotor_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "EncoderX");
                    PublicVar.CHXEncoder_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Unit", "EncoderY");
                    PublicVar.CHYEncoder_Unit = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "InitVel");
                    PublicVar.CHXMotorInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "ACC");
                    PublicVar.CHXMotorACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "DEC");
                    PublicVar.CHXMotorDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "Speed");
                    PublicVar.CHXMotorSpeed = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "InitPos");
                    PublicVar.CHXMotorInitPos = Convert.ToDouble(strTemp);

                    strTemp = IniSetting.IniReadValue("CHY", "InitVel");
                    PublicVar.CHYMotorInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "ACC");
                    PublicVar.CHYMotorACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "DEC");
                    PublicVar.CHYMotorDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "Speed");
                    PublicVar.CHYMotorSpeed = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "InitPos");
                    PublicVar.CHYMotorInitPos = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "InitVel");
                    PublicVar.CHCYMotorInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "ACC");
                    PublicVar.CHCYMotorACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "DEC");
                    PublicVar.CHCYMotorDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "Speed");
                    PublicVar.CHCYMotorSpeed = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "InitVel");
                    PublicVar.CHCZMotorInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "ACC");
                    PublicVar.CHCZMotorACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "DEC");
                    PublicVar.CHCZMotorDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "Speed");
                    PublicVar.CHCZMotorSpeed = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "InitPos");
                    PublicVar.CHCYMotorInitPos = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "InitPos");
                    PublicVar.CHCZMotorInitPos = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "WorkPos");
                    PublicVar.CHCYMotorWorkPos = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "WorkPos");
                    PublicVar.CHCZMotorWorkPos = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "WorkPos1");
                    PublicVar.CHCZMotorWorkPos1 = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "LowVel");
                    PublicVar.CHCZMotorLowVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "InitPos");
                    PublicVar.CHXMotorInitPos = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "HomeInitVel");
                    PublicVar.CHXMotorHomeInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "HomeMaxVel");
                    PublicVar.CHXMotorHomeMaxVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "HomeACC");
                    PublicVar.CHXMotorHomeACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "HomeDEC");
                    PublicVar.CHXMotorHomeDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "HomeInitVel");
                    PublicVar.CHYMotorHomeInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "HomeMaxVel");
                    PublicVar.CHYMotorHomeMaxVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "HomeACC");
                    PublicVar.CHYMotorHomeACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "HomeDEC");
                    PublicVar.CHYMotorHomeDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "HomeInitVel");
                    PublicVar.CHCYMotorHomeInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "HomeMaxVel");
                    PublicVar.CHCYMotorHomeMaxVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "HomeACC");
                    PublicVar.CHCYMotorHomeACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCY", "HomeDEC");
                    PublicVar.CHCYMotorHomeDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "HomeInitVel");
                    PublicVar.CHCZMotorHomeInitVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "HomeMaxVel");
                    PublicVar.CHCZMotorHomeMaxVel = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "HomeACC");
                    PublicVar.CHCZMotorHomeACC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHCZ", "HomeDEC");
                    PublicVar.CHCZMotorHomeDEC = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("JogSpeed", "High");
                    PublicVar.HighSpeed = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("JogSpeed", "Mid");
                    PublicVar.MidSpeed = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("JogSpeed", "Low");
                    PublicVar.LowSpeed = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "CaliCorr");
                    PublicVar.CHYCaliCorr = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "CaliCorr");
                    PublicVar.CHXCaliCorr = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHY", "RepairAngle");
                    PublicVar.CHYRepairAngle = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("CHX", "RepairAngle");
                    PublicVar.CHXRepairAngle = Convert.ToDouble(strTemp);
                
                
                try
                {
                    strTemp = IniSetting.IniReadValue("RepairInterval", "X");
                    PublicVar.RepairIntervalX = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("RepairInterval", "Y");
                    PublicVar.RepairIntervalY = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("RepairStart", "X");
                    PublicVar.RepairStartX  = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("RepairStart", "Y");
                    PublicVar.RepairStartY = Convert.ToDouble(strTemp);
                }
                catch 
                {
                    MessageBox.Show("读取Setting配置文件补偿值错误！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                try
                {
                    strTemp = IniSetting.IniReadValue("Master", "PosX1");
                    PublicVar.MasterPos1X  = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "PosY1");
                    PublicVar.MasterPos1Y = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "PosX2");
                    PublicVar.MasterPos2X = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "PosY2");
                    PublicVar.MasterPos2Y = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "PosX3");
                    PublicVar.MasterPos3X = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "PosY3");
                    PublicVar.MasterPos3Y = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "PosX4");
                    PublicVar.MasterPos4X = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "PosY4");
                    PublicVar.MasterPos4Y = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "NumHZ");
                    PublicVar.MasterNumHZ  = Convert.ToInt16 (strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "L1");
                    PublicVar.MasterL1  = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "L2");
                    PublicVar.MasterL2 = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "W1");
                    PublicVar.MasterW1 = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Master", "W2");
                    PublicVar.MasterW2 = Convert.ToDouble(strTemp);

                    strTemp = IniSetting.IniReadValue("Corr", "X1K");
                    PublicVar.CorrX1K = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Corr", "X1Offset");
                    PublicVar.CorrX1Offset = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Corr", "X2K");
                    PublicVar.CorrX2K = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Corr", "X2Offset");
                    PublicVar.CorrX2Offset = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Corr", "Y1K");
                    PublicVar.CorrY1K = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Corr", "Y1Offset");
                    PublicVar.CorrY1Offset = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Corr", "Y2K");
                    PublicVar.CorrY2K = Convert.ToDouble(strTemp);
                    strTemp = IniSetting.IniReadValue("Corr", "Y2Offset");
                    PublicVar.CorrY2Offset = Convert.ToDouble(strTemp);
                }
                catch { MessageBox.Show("读取Setting文件错误\n(ReadParam错误)","提示",MessageBoxButtons.OK,MessageBoxIcon.Error); return false; }
            //定位相机是否使用由该产品名称决定,无需重复读取
                //try
                //{
                //    if ("1" == IniSetting.IniReadValue("Select", "PosUsing"))
                //        chkPosUsing.Checked = true;
                //    else
                //        chkPosUsing.Checked = false;
                //}
                //catch { MessageBox.Show("ReadParam错误！"); return false; }
                /////////////////////////////////////////////////
                try
                {
                    radioRingLed.Checked = false;
                    radioPosLed.Checked = false;
                    radioBackLed.Checked = false;
                    strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                    strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                    CIni IniProg = new CIni(strPath);
                    strTemp = IniProg.IniReadValue("Image", "Light" + PublicVar.iLedSel.ToString());
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold" + PublicVar.iLedSel.ToString());
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                    radioRingLed.Checked = true;
                }
                catch
                {
                    MessageBox.Show("请检查产品配置文件中[Image]灯光或二值化数值是否错误或遗漏！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    
                }
                return true;
            }
        private bool MachinReplaceAll()
        {
            m_ClassMotion.E_Stop();
            Thread.Sleep(50);
            m_ClassMotion.Write_Out_Bit(0, 3, 1);//A
            m_ClassMotion.Write_Out_Bit(0, 4, 1);//B
            m_ClassMotion.Write_Out_Bit(0, 31, 1);//C
            m_ClassMotion.Write_Out_Bit(0, 32, 1);//D
            m_ClassMotion.Write_Out_Bit(0, 1, 1);//alarm
            ///////////////////////////////////////
            bool bHomeFlag = true;
            if ( m_ClassMotion.ReplaceCHCZMotor(new double[] { PublicVar.CHCZMotorHomeInitVel / PublicVar.CHCZMotor_Unit, 
                                PublicVar.CHCZMotorHomeMaxVel/ PublicVar.CHCZMotor_Unit, PublicVar.CHCZMotorHomeACC, PublicVar.CHCZMotorHomeDEC, PublicVar.CHCZMotorHomeMaxVel/ PublicVar.CHCZMotor_Unit }, Convert.ToInt32(PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit)))
            {
                if (m_ClassMotion.CHCZMotorORG)
                {
                    Func<double[], int, bool> CHLoadXReplace = m_ClassMotion.ReplaceCHXMotor;
                    Func<double[], int, bool> CHLoadYReplace = m_ClassMotion.ReplaceCHYMotor;
                    Func<double[], int, bool> CHLoadCYReplace = m_ClassMotion.ReplaceCHCYMotor;

                    IAsyncResult RCHCYMotorReplace = CHLoadCYReplace.BeginInvoke(new double[] { PublicVar.CHCYMotorHomeInitVel / PublicVar.CHCYMotor_Unit, 
                                PublicVar.CHCYMotorHomeMaxVel/ PublicVar.CHCYMotor_Unit, PublicVar.CHCYMotorHomeACC, PublicVar.CHCYMotorHomeDEC, PublicVar.CHCYMotorHomeMaxVel/ PublicVar.CHCYMotor_Unit }, Convert.ToInt32(PublicVar.CHCYMotorInitPos / PublicVar.CHCYMotor_Unit), null, null);
                    IAsyncResult RCHXMotorReplace = CHLoadXReplace.BeginInvoke(new double[] { PublicVar.CHXMotorHomeInitVel / PublicVar.CHXMotor_Unit, 
                                PublicVar.CHXMotorHomeMaxVel/ PublicVar.CHXMotor_Unit, PublicVar.CHXMotorHomeACC, PublicVar.CHXMotorHomeDEC, PublicVar.CHXMotorHomeMaxVel/ PublicVar.CHXMotor_Unit }, Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit), null, null);
                    IAsyncResult RCHYMotorReplace = CHLoadYReplace.BeginInvoke(new double[] { PublicVar.CHYMotorHomeInitVel / PublicVar.CHYMotor_Unit, 
                                PublicVar.CHYMotorHomeMaxVel/ PublicVar.CHYMotor_Unit, PublicVar.CHYMotorHomeACC, PublicVar.CHYMotorHomeDEC, PublicVar.CHYMotorHomeMaxVel/ PublicVar.CHYMotor_Unit }, Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit), null, null);
                    do
                    {
                        DateTime CurrentTime = DateTime.Now;
                        if (!m_ClassMotion.IsOutTime(CurrentTime, 100000))
                        {
                            break;
                        }
                        if (RCHXMotorReplace.IsCompleted && RCHYMotorReplace.IsCompleted && RCHCYMotorReplace.IsCompleted)
                        {
                            break;
                        }
                        Thread.Sleep(20);
                    } while (true);

                    if (CHLoadXReplace.EndInvoke(RCHXMotorReplace))
                    {
                        bHomeFlag &= true;
                    }
                    else
                    {
                        m_ClassMotion.StopAxis(m_ClassMotion.CHXMotor);
                        MessageBox.Show("相机马达X回位失败");
                        bHomeFlag &= false;
                    }
                    if (CHLoadYReplace.EndInvoke(RCHYMotorReplace))
                    {
                        bHomeFlag &= true;
                    }
                    else
                    {
                        m_ClassMotion.StopAxis(m_ClassMotion.CHYMotor);
                        MessageBox.Show("相机马达Y回位失败");
                        bHomeFlag &= false;
                    }
                    if (CHLoadCYReplace.EndInvoke(RCHCYMotorReplace))
                    {
                        bHomeFlag &= true;
                    }
                    else
                    {
                        m_ClassMotion.StopAxis(m_ClassMotion.CHCYMotor);
                        MessageBox.Show("相机马达Y回位失败");
                        bHomeFlag &= false;
                    }
                }
                else
                {
                    MessageBox.Show("光源马达Z没在原点安全位置");
                    bHomeFlag &= false;
                }
            }
            else
            {

                MessageBox.Show("光源马达Z回位失败");
                bHomeFlag &= false;
            }

            return bHomeFlag;
         }
        public void LoadFileNametoCombo(ComboBox cmb)
        {
            string strPath;
            strPath = Directory.GetCurrentDirectory();
            string str1 = strPath + "\\Doc\\Prog";
            if (Directory.Exists(str1))
            {
                int iUpperBound = Directory.GetFiles(str1).GetLength(0);
                string[] strFile = new string[iUpperBound];
                strFile = Directory.GetFiles(str1);
                cmb.Items.Clear();
                for (int i = 0; i < iUpperBound; i++)
                {
                    char[] delimiterChars = { '\\', '.', '\t' };
                    string[] words = strFile[i].Split(delimiterChars);
                    int iwords = words.GetUpperBound(0);
                    cmb.Items.Add(words[iwords - 1]);
                }
            }
        }
        private void  SaveDatatoDisk(int iTrayNo)
        {
            string strTemp = "";
            string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            strPath += "\\prog\\" + cmbProductName.Text.Trim() + ".ini";
            CIni IniProg = new CIni(strPath);
            strTemp = IniProg.IniReadValue("Param", "No1");//lower 
            double SetNo1 = Convert.ToDouble(strTemp);
            strTemp = IniProg.IniReadValue("Param", "No2");//upper
            double SetNo2 = Convert.ToDouble(strTemp);
            strTemp = IniProg.IniReadValue("Param", "L1Set");
            double L1Set = Convert.ToDouble(strTemp);
            strTemp = IniProg.IniReadValue("Param", "L2Set");
            double L2Set = Convert.ToDouble(strTemp);
            strTemp = IniProg.IniReadValue("Param", "W1Set");
            double W1Set = Convert.ToDouble(strTemp);
            strTemp = IniProg.IniReadValue("Param", "W2Set");
            double W2Set = Convert.ToDouble(strTemp);

            double w1 = 0, w2 = 0, l1 = 0, l2 = 0;
            l1=Convert.ToDouble(lblTop.Text) ;
            l2=Convert.ToDouble(lblBottom.Text);
            w1=Convert.ToDouble(lblLeft.Text);
            w2=Convert.ToDouble(lblRight.Text);
            double dL1, dL2, dW1, dW2;
            dL1 = (l1 - L1Set) / L1Set * 10000;
            dL2 = (l2 - L2Set) / L2Set * 10000;
            dW1 = (w1 - W1Set) / W1Set * 10000;
            dW2 = (w2 - W2Set) / W2Set * 10000;
            strPath = "d:\\data";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            iTrayNo++;
            strPath = "d:\\data\\第"+iTrayNo.ToString()+"盘";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            DateTime t = DateTime.Now;
            string strFile = strPath + "\\" + t.ToString("yyyy-MM-dd") + "_Tray" + iTrayNo.ToString() + ".csv";
            string s = "";
            if (!File.Exists(strFile ))
            {
                StreamWriter sw1 = new StreamWriter(strFile);
                s ="Name"+","+ "T"+","+"Y1" + "," + "Y2" + "," + "X1" + "," + "X2" + ",";
                s += "Y1_P" + "," + "Y2_P" + "," + "X1_P" + "," + "X2_P" + ",";
                s += "X1_S" + "," + "X2_S" + "," + "X1_S" + "," + "X2_S" + ",";
                s +=  "Result";
                for (int i = 1; i <= Sequence.GetUpperBound (0)+1; i++)//坐标
                {
                    s += "," + "pX"+i.ToString () + "," + "pY"+i.ToString ();
                }
                //
                s += "," + "C_1X" + "," + "C_1Y" + "," + "C_2X" + "," + "C_2Y" + "," + "C_3X" + "," + "C_3Y" + "," + "C_4X" + "," + "C_4Y";
                s += "," + "E_1X" + "," + "E_1Y" + "," + "E_2X" + "," + "E_2Y" + "," + "E_3X" + "," + "E_3Y" + "," + "E_4X" + "," + "E_4Y";
                sw1.WriteLine(s);
                sw1.Close();
            }
            s = cmbProductName .Text .Trim () + "," + DateTime.Now.ToShortTimeString() + ",";
            s += lblLeft.Text + ","+lblRight.Text + ","+lblTop.Text + ","+lblBottom.Text + ",";
            s += dW1.ToString ("f3") + "," + dW2.ToString ("f3") + "," + dL1.ToString ("f3") + "," + dL2.ToString("f3") + ",";
            s += W1Set.ToString("f3") + "," + W2Set.ToString("f3") + "," + L1Set.ToString("f3") + "," + L2Set.ToString("f3") + ",";
            s +=  txtResult.Text;
            for (int i = 0; i < Sequence.GetUpperBound(0) + 1; i++)//坐标
            {
                s += "," + TestX[i].ToString("f4") + "," + TestY[i].ToString("f4") ;
            }
            
            //
            for (int i = 0; i < Sequence.GetUpperBound(0) + 1; i++)
            {
                s += "," + SaveCameraX[i].ToString("f3") + ","+ SaveCameraY[i].ToString("f3");
            }
            for (int i = 0; i < Sequence.GetUpperBound(0) + 1; i++)
            {
                s += "," + SaveCurrentEncoderX[i].ToString("f4") + "," + SaveCurrentEncoderY[i].ToString("f4");
            }


            StreamWriter sw = new StreamWriter(strFile, true);
            sw.WriteLine(s);
            sw.Close();
            /////////////////////////////如果Excel数据写入失败,跳转到报警
            WriteExcel();
        }

        Task task;
        private void   ExcelInit()
        {
            #region 注释内容
            //DateTime t = DateTime.Now;
                //int year = t.Year;
                //int month = t.Month;
                //int day = t.Day;
                //string strPath = "d:\\data";
                //if (!Directory.Exists(strPath))
                //    Directory.CreateDirectory(strPath);
                //strPath = "d:\\data\\Report\\" + year.ToString() + "\\" + month.ToString() + "月\\" + day.ToString()+"日" ;
                //if (!Directory.Exists(strPath))
                //    Directory.CreateDirectory(strPath);
                
                /////////////////////////
                //string strFile = "";
                ////string productName = cmbProductName.Text;
                ////if (productName == "") { productName = "未知产品名称"; }
                //string path = strPath + "\\" + t.ToString("yyyy-MM-dd") + ".xlsx";
                ////string path = strPath + "\\" + productName + ".xlsx";
                //strFile = System.Windows.Forms.Application.StartupPath + "\\Doc\\CPKReport.xlsx";
                //if (!File.Exists(path))
                //    File.Copy(strFile, path);
                //ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                //ExcelWb = ExcelApp.Workbooks.Open(path);
                //sheets = ExcelWb.Worksheets;
                //ExcelWs = (Worksheet)sheets.get_Item(1);
                //ExcelRange = ExcelWs.Cells;
            ////ExcelApp.Visible = true;
            #endregion

            string productName = cmbProductName.Text;
            task = new Task(() =>
            {
                DateTime t = DateTime.Now;
                int year = t.Year;
                int month = t.Month;
                int day = t.Day;
                int hour = t.Hour;
                int minute = t.Minute;
                try
                {
                    string[] ProName = productName.Split('-');
                    string strPath = "E:\\data\\Report\\" + year.ToString() + "\\" + month.ToString() + "月\\" + day.ToString() + "日" + "\\" + ProName[0] + "\\" + ProName[1];
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);

                    ///////////////////////
                    string strFile = "";
                    string path = strPath + "\\" + productName + "-" + hour.ToString() + minute.ToString() + ".xlsx";
                    strFile = System.Windows.Forms.Application.StartupPath + "\\Doc\\CPKReport.xlsx";
                    if (!File.Exists(path))
                        File.Copy(strFile, path);
                
                ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ExcelWb = ExcelApp.Workbooks.Open(path);
                sheets = ExcelWb.Worksheets;
                ExcelWs = (Worksheet)sheets.get_Item(1);
                ExcelRange = ExcelWs.Cells;
                //ExcelApp.Visible = true;
                }
                catch
                {
                    return;
                }
            });
            task.Start();


        }

        private void ExcelUnInit()
        {
            #region 注释内容
            //if (ExcelApp != null)
            //{
            //    DateTime t = DateTime.Now;
            //    int year = t.Year;
            //    int month = t.Month;
            //    int day = t.Day;
            //    int hour = t.Hour;
            //    int minute = t.Minute;
            //    string productName = cmbProductName.Text;
            //    if (productName == "") { productName = "未知产品名称"; }
            //    string strPath = "d:\\data\\Report\\" + year.ToString() + "\\" + month.ToString() + "月\\" + day.ToString() + "日" + "\\" + productName;
            //    if (!Directory.Exists(strPath))
            //        Directory.CreateDirectory(strPath);

            //    string path = strPath + "\\" + productName + "-"+hour.ToString()+minute.ToString()+".xlsx";
            //    ExcelApp.DisplayAlerts = false;
            //    if (bSaved == true)
            //        ExcelWb.SaveAs(path);
            //    ExcelApp.DisplayAlerts = true;
            //    try
            //    {
            //        ExcelWb.Close();
            //        ExcelApp = null;
            //    }
            //    catch
            //    {

            //    }
            //}
            #endregion
            //防止点了运行之后马上点击停止按钮
            while (!task.IsCompleted)
            {
                MessageBox.Show("Excel正在响应中,请稍等...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Thread.Sleep(2000);
                if (task.IsCompleted)
                {
                    break;
                }
                
            }

            if (ExcelApp != null)
            {

                DateTime t = DateTime.Now;
                int year = t.Year;
                int month = t.Month;
                int day = t.Day;
                int hour = t.Hour;
                int minute = t.Minute;
                string productName = cmbProductName.Text;
                string[] ProName = productName.Split('-');
                if (productName == "") { productName = "未知产品名称"; }
                string strPath = "E:\\data\\Report\\" + year.ToString() + "\\" + month.ToString() + "月\\" + day.ToString() + "日" + "\\" + ProName[0] + "\\" + ProName[1];
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

                string path = strPath + "\\" + productName + "-" + hour.ToString() + minute.ToString() + ".xlsx";
                ExcelApp.DisplayAlerts = false;
                try
                {
                    if (bSaved == true)
                    {
                        ExcelWb.SaveAs(path);
                        ExcelApp.DisplayAlerts = true;
                        ExcelWb.Close();
                    }
                    KillExcel();
                    ExcelApp = null;
                }
                catch
                {
                    MessageBox.Show("Excel保存失败\n解决方法:请关闭正在运行的所有Excel程序");
                    KillExcel();
                    ExcelApp = null;
                }
            }
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out   int ID);
        protected void KillExcel()
        {
            IntPtr t = new IntPtr(ExcelApp.Hwnd);
            int k = 0;
            GetWindowThreadProcessId(t, out   k);
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
            p.Kill();
        }

        private void WriteExcel()
        {
            bSaved = false;
            //等待线程初始化完成
            while (!task.IsCompleted)
            {
                if (timer1.Enabled == true)
                {
                    timer1.Enabled = false;
                    FormWaitting fw = new FormWaitting(2000);
                    fw.ShowDialog();
                    
                }
                if (task.IsCompleted)
                {
                    timer1.Enabled = true;
                    break;
                }
                Thread.Sleep(2000);




            }
            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;      //线程初始化完成后重新打开定时器

            }


            try
            {
                string strTemp = "";
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                strPath += "\\prog\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                strTemp = IniProg.IniReadValue("Param", "No1");//lower 
                double SetNo1 = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "No2");//upper
                double SetNo2 = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "L1Set");
                double L1Set = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "L2Set");
                double L2Set = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "W1Set");
                double W1Set = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "W2Set");
                double W2Set = Convert.ToDouble(strTemp);
                double w1 = 0, w2 = 0, l1 = 0, l2 = 0;
                l1 = Convert.ToDouble(lblTop.Text);
                l2 = Convert.ToDouble(lblBottom.Text);
                w1 = Convert.ToDouble(lblLeft.Text);
                w2 = Convert.ToDouble(lblRight.Text);
                double L13, L24;
                L13 = Convert.ToDouble(lblL13.Text);
                L24 = Convert.ToDouble(lblL24.Text);
                int j = 13;
                while (true)
                {
                    if ((ExcelRange.Cells[2, j]).Text == "")
                        break;
                    else
                        j++;
                }
                //填充时间
                ExcelRange.Cells.set_Item(2, j, string.Format("{0:G}", DateTime.Now));
                //第n个样本数据表头
                ExcelRange.Cells.set_Item(3, j, j - 12);


                ExcelRange.Cells.set_Item(4, 2, "X1");
                ExcelRange.Cells.set_Item(4, 6, L1Set);
                ExcelRange.Cells.set_Item(4, j, l1);

                ExcelRange.Cells.set_Item(5, 2, "X2");
                ExcelRange.Cells.set_Item(5, 6, L2Set);
                ExcelRange.Cells.set_Item(5, j, l2);

                ExcelRange.Cells.set_Item(6, 2, "Y1");
                ExcelRange.Cells.set_Item(6, 6, W1Set);
                ExcelRange.Cells.set_Item(6, j, w1);

                ExcelRange.Cells.set_Item(7, 2, "Y2");
                ExcelRange.Cells.set_Item(7, 6, W2Set);
                ExcelRange.Cells.set_Item(7, j, w2);

                 
                double d = Math.Sqrt(W1Set * W1Set + L1Set * L1Set);
                ExcelRange.Cells.set_Item(8, 2, "L13");
                ExcelRange.Cells.set_Item(8, 6, d.ToString ("f3"));
                ExcelRange.Cells.set_Item(8, j, L13);

                ExcelRange.Cells.set_Item(9, 2, "L24");
                ExcelRange.Cells.set_Item(9, 6, d.ToString("f3"));
                ExcelRange.Cells.set_Item(9, j, L24);
                //
                ExcelRange.Cells.set_Item(10, 2, "X_Average");
                //double temp = GetAverage(Convert.ToDouble(txtLowerLimit.Text), Convert.ToDouble(txtUpperLimit.Text));
                ExcelRange.Cells.set_Item(10, 6, tB_XStand.Text);
                ExcelRange.Cells.set_Item(10, j, PublicVar.L_Average);

                ExcelRange.Cells.set_Item(11, 2, "Y_Average");
                //temp = GetAverage(Convert.ToDouble(txtLowerLimit.Text), Convert.ToDouble(txtUpperLimit.Text));
                ExcelRange.Cells.set_Item(11, 6, tB_YStand.Text);
                ExcelRange.Cells.set_Item(11, j, PublicVar.W_Average);

                
                //ExcelRange.Cells.set_Item(12, 2, "L13/L");
                //ExcelRange.Cells.set_Item(12, 6, "1");//标准值为1
                //ExcelRange.Cells.set_Item(12, j, PublicVar.bL13.ToString("f5"));

                //ExcelRange.Cells.set_Item(13, 2, "L24/L");
                //ExcelRange.Cells.set_Item(13, 6, "1");//标准值为1
                //ExcelRange.Cells.set_Item(13, j, PublicVar.bL24.ToString("f5"));

                bSaved = true;
            }
            catch 
            {
                timer1.Enabled = false;
                if (MessageBox.Show("写入Excel失败,是否继续?\n(请检查产品参数是否有漏缺)", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    timer1.Enabled = true;
                    //return true;
                }
                else
                {
                    KillExcel();
                    ExcelApp = null;
                    timer1.Enabled = true;
                    iStep = 999;//因为退出此函数后,timer还继续增加1
                }
               
            }
            //return true;
            
        }
        private void WriteExcel_6Point()
        {

            bSaved = false;
            //等待线程初始化完成
            while (!task.IsCompleted)
            {
                if (timer1.Enabled == true)
                {
                    timer1.Enabled = false;
                    FormWaitting fw = new FormWaitting(2000);
                    fw.ShowDialog();

                }
                if (task.IsCompleted)
                {
                    timer1.Enabled = true;
                    break;
                }
                Thread.Sleep(2000);
            }
            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;      //线程初始化完成后重新打开定时器

            }
            try
            {
                string strTemp = "";
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                strPath += "\\prog\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                strTemp = IniProg.IniReadValue("Param", "L1Set");  //X1标准值
                double LAStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "L2Set");  //X2
                double LBStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "W1Set");  //Y1
                double LCStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "W2Set");  //Y2
                double LDStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("Param", "Y3Set");  //Y3
                double LEStandard = Convert.ToDouble(strTemp);
                              

                int j = 13;
                while (true)
                {
                    if ((ExcelRange.Cells[2, j]).Text == "")
                        break;
                    else
                        j++;
                }
                //填充时间
                ExcelRange.Cells.set_Item(2, j, string.Format("{0:G}", DateTime.Now));
                //第n个样本数据表头
                ExcelRange.Cells.set_Item(3, j, j - 12);

                ExcelRange.Cells.set_Item(4, 2, "X1");
                ExcelRange.Cells.set_Item(4, 6, LAStandard);
                ExcelRange.Cells.set_Item(4, j, m_ResultData[0].X1);

                ExcelRange.Cells.set_Item(5, 2, "X2");
                ExcelRange.Cells.set_Item(5, 6, LBStandard);
                ExcelRange.Cells.set_Item(5, j, m_ResultData[0].X2);

                ExcelRange.Cells.set_Item(6, 2, "Y1");
                ExcelRange.Cells.set_Item(6, 6, LCStandard);
                ExcelRange.Cells.set_Item(6, j, m_ResultData[0].Y1);

                ExcelRange.Cells.set_Item(7, 2, "Y2");
                ExcelRange.Cells.set_Item(7, 6, LDStandard);
                ExcelRange.Cells.set_Item(7, j, m_ResultData[0].Y2);

                ExcelRange.Cells.set_Item(8, 2, "Y3");
                ExcelRange.Cells.set_Item(8, 6, LEStandard);
                ExcelRange.Cells.set_Item(8, j, m_ResultData[0].Y3);
                
                double CalculateX1Percent=(m_ResultData[0].X1-LAStandard)/LAStandard*10000;
                double CalculateX2Percent = (m_ResultData[0].X2 - LBStandard) / LBStandard * 10000;
                double X_Average = (CalculateX1Percent + CalculateX2Percent) / 2;
                ExcelRange.Cells.set_Item(9, 2, "X_Average");//X方向平均值万分比
                ExcelRange.Cells.set_Item(9, 6, tB_XStand.Text);//X方向标准值
                ExcelRange.Cells.set_Item(9, j, X_Average.ToString("f3"));

                double CalculateY1Percent = (m_ResultData[0].Y1 - LCStandard) / LCStandard * 10000;
                ExcelRange.Cells.set_Item(10, 2, "Y1_Per");//Y1方向万分比
                ExcelRange.Cells.set_Item(10, 6, tB_YStand.Text);//Y方向标准值
                ExcelRange.Cells.set_Item(10, j, CalculateY1Percent.ToString("f3"));

                double CalculateY2Percent = (m_ResultData[0].Y2 - LDStandard) / LDStandard * 10000;
                ExcelRange.Cells.set_Item(11, 2, "Y2_Per");//Y2方向万分比
                ExcelRange.Cells.set_Item(11, 6, tB_YStand.Text);//Y方向标准值
                ExcelRange.Cells.set_Item(11, j, CalculateY2Percent.ToString("f3"));

                double CalculateY3Percent = (m_ResultData[0].Y3 - LEStandard) / LEStandard * 10000;
                ExcelRange.Cells.set_Item(12, 2, "Y3_Per");//Y2方向万分比
                ExcelRange.Cells.set_Item(12, 6, tB_YStand.Text);//Y方向标准值
                ExcelRange.Cells.set_Item(12, j, CalculateY3Percent.ToString("f3"));

                bSaved = true;

                //展示结果
                dataGridView1.Rows.Add(1);
                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.RowCount - 1];
                dataGridView1.FirstDisplayedCell = dataGridView1[0, dataGridView1.RowCount - 1];
                dataGridView1[0, dataGridView1.RowCount - 1].Value = dataGridView1.RowCount - 1;//序号
                //dataGridView1[1, dataGridView1.RowCount - 1].Value = m_ResultData[0].X1;

                
                
            }
            catch
            {
                timer1.Enabled = false;
                if (MessageBox.Show("写入Excel失败,是否继续?\n(请检查产品参数是否有漏缺)", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    timer1.Enabled = true;
                    //return true;
                }
                else
                {
                    KillExcel();
                    ExcelApp = null;
                    timer1.Enabled = true;
                    iStep = 999;//因为退出此函数后,timer还继续增加1
                }

            }


        }
        private void WriteExcel_9Point()
        {

            bSaved = false;
            //等待线程初始化完成
            while (!task.IsCompleted)
            {
                if (timer1.Enabled == true)
                {
                    timer1.Enabled = false;
                    FormWaitting fw = new FormWaitting(2000);
                    fw.ShowDialog();

                }
                if (task.IsCompleted)
                {
                    timer1.Enabled = true;
                    break;
                }
                Thread.Sleep(2000);                
            }
            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;      //线程初始化完成后重新打开定时器

            }
            try
            {
                string strTemp = "";
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                strPath += "\\prog\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LA");  //LA-LL标准值
                double LAStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LB");  
                double LBStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LC");
                double LCStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LD");
                double LDStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LE");
                double LEStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LF");
                double LFStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LG");
                double LGStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LH");
                double LHStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LI");
                double LIStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LJ");
                double LJStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LK");
                double LKStandard = Convert.ToDouble(strTemp);
                strTemp = IniProg.IniReadValue("TwelveLineStandard", "LL");
                double LLStandard = Convert.ToDouble(strTemp);
                                
                int j = 13;
                while (true)
                {
                    if ((ExcelRange.Cells[2, j]).Text == "")
                        break;
                    else
                        j++;
                }
              //填充时间
                ExcelRange.Cells.set_Item(2, j, string.Format("{0:G}", DateTime.Now));
              //第n个样本数据表头
                ExcelRange.Cells.set_Item(3, j, j - 12);
                
                ExcelRange.Cells.set_Item(4, 2, "L1");
                ExcelRange.Cells.set_Item(4, 6, LAStandard);
                ExcelRange.Cells.set_Item(4, j, m_ResultData[0].LA);

                ExcelRange.Cells.set_Item(5, 2, "L2");
                ExcelRange.Cells.set_Item(5, 6, LBStandard);
                ExcelRange.Cells.set_Item(5, j, m_ResultData[0].LB);

                ExcelRange.Cells.set_Item(6, 2, "L3");
                ExcelRange.Cells.set_Item(6, 6, LCStandard);
                ExcelRange.Cells.set_Item(6, j, m_ResultData[0].LC);

                ExcelRange.Cells.set_Item(7, 2, "L4");
                ExcelRange.Cells.set_Item(7, 6, LDStandard);
                ExcelRange.Cells.set_Item(7, j, m_ResultData[0].LD);

                ExcelRange.Cells.set_Item(8, 2, "L5");
                ExcelRange.Cells.set_Item(8, 6, LEStandard);
                ExcelRange.Cells.set_Item(8, j, m_ResultData[0].LE);

                ExcelRange.Cells.set_Item(9, 2, "L6");
                ExcelRange.Cells.set_Item(9, 6, LFStandard);
                ExcelRange.Cells.set_Item(9, j, m_ResultData[0].LF);

                ExcelRange.Cells.set_Item(10, 2, "L7");
                ExcelRange.Cells.set_Item(10, 6, LGStandard);
                ExcelRange.Cells.set_Item(10, j, m_ResultData[0].LG);

                ExcelRange.Cells.set_Item(11, 2, "L8");
                ExcelRange.Cells.set_Item(11, 6, LHStandard);
                ExcelRange.Cells.set_Item(11, j, m_ResultData[0].LH);

                ExcelRange.Cells.set_Item(12, 2, "L9");
                ExcelRange.Cells.set_Item(12, 6, LIStandard);
                ExcelRange.Cells.set_Item(12, j, m_ResultData[0].LI);

                ExcelRange.Cells.set_Item(13, 2, "L10");
                ExcelRange.Cells.set_Item(13, 6, LJStandard);
                ExcelRange.Cells.set_Item(13, j, m_ResultData[0].LJ);

                ExcelRange.Cells.set_Item(14, 2, "L11");
                ExcelRange.Cells.set_Item(14, 6, LKStandard);
                ExcelRange.Cells.set_Item(14, j, m_ResultData[0].LK);

                ExcelRange.Cells.set_Item(15, 2, "L12");
                ExcelRange.Cells.set_Item(15, 6, LLStandard);
                ExcelRange.Cells.set_Item(15, j, m_ResultData[0].LL);

                
                bSaved = true;

              //展示结果
                dataGridView1.Rows.Add(1);
                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.RowCount - 1];
                dataGridView1.FirstDisplayedCell = dataGridView1[0, dataGridView1.RowCount - 1];
                dataGridView1[0, dataGridView1.RowCount - 1].Value = dataGridView1.RowCount - 1;//序号
                double dMax;
              //LA
                dataGridView1[1, dataGridView1.RowCount - 1].Value = m_ResultData[0].LA;
                if (LAStandard != 0)
                {
                    dMax = (m_ResultData[0].LA - LAStandard) / LAStandard * 10000;
                    dataGridView1[1, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[1, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[1, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[1, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LB
                dataGridView1[2, dataGridView1.RowCount - 1].Value = m_ResultData[0].LB;
                if (LBStandard != 0)
                {
                    dMax = (m_ResultData[0].LB - LBStandard) / LBStandard * 10000;
                    dataGridView1[2, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[2, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[2, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[2, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LC
                dataGridView1[3, dataGridView1.RowCount - 1].Value = m_ResultData[0].LC;
                if (LCStandard != 0)
                {
                    dMax = (m_ResultData[0].LC - LCStandard) / LCStandard * 10000;
                    dataGridView1[3, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[3, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[3, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[3, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LD
                dataGridView1[4, dataGridView1.RowCount - 1].Value = m_ResultData[0].LD;
                if (LDStandard != 0)
                {
                    dMax = (m_ResultData[0].LD - LDStandard) / LDStandard * 10000;
                    dataGridView1[4, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[4, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[4, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[4, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LE
                dataGridView1[5, dataGridView1.RowCount - 1].Value = m_ResultData[0].LE;
                if (LEStandard != 0)
                {
                    dMax = (m_ResultData[0].LE - LEStandard) / LEStandard * 10000;
                    dataGridView1[5, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LF
                dataGridView1[6, dataGridView1.RowCount - 1].Value = m_ResultData[0].LF;
                if (LFStandard != 0)
                {
                    dMax = (m_ResultData[0].LF - LFStandard) / LFStandard * 10000;
                    dataGridView1[6, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
            //LG
                dataGridView1[7, dataGridView1.RowCount - 1].Value = m_ResultData[0].LG;
                if (LGStandard != 0)
                {
                    dMax = (m_ResultData[0].LG - LGStandard) / LGStandard * 10000;
                    dataGridView1[7, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LH
                dataGridView1[8, dataGridView1.RowCount - 1].Value = m_ResultData[0].LH;
                if (LHStandard != 0)
                {
                    dMax = (m_ResultData[0].LH - LHStandard) / LHStandard * 10000;
                    dataGridView1[8, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LI
                dataGridView1[9, dataGridView1.RowCount - 1].Value = m_ResultData[0].LI;
                if (LIStandard != 0)
                {
                    dMax = (m_ResultData[0].LI - LIStandard) / LIStandard * 10000;
                    dataGridView1[9, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[9, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[9, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[9, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LJ
                dataGridView1[10, dataGridView1.RowCount - 1].Value = m_ResultData[0].LJ;
                if (LJStandard != 0)
                {
                    dMax = (m_ResultData[0].LJ - LJStandard) / LJStandard * 10000;
                    dataGridView1[10, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[10, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[10, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[10, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LK
                dataGridView1[11, dataGridView1.RowCount - 1].Value = m_ResultData[0].LK;
                if (LKStandard != 0)
                {
                    dMax = (m_ResultData[0].LK - LKStandard) / LKStandard * 10000;
                    dataGridView1[11, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[11, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[11, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[11, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
              //LL
                dataGridView1[12, dataGridView1.RowCount - 1].Value = m_ResultData[0].LL;
                if (LLStandard != 0)
                {
                    dMax = (m_ResultData[0].LL - LLStandard) / LLStandard * 10000;
                    dataGridView1[12, dataGridView1.RowCount - 1].Value = dMax;
                    if (dMax < double.Parse(txtLowerLimit.Text)) dataGridView1[12, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                    else if (dMax > double.Parse(txtUpperLimit.Text)) dataGridView1[12, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                    else dataGridView1[12, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                }
            }
            catch
            {
                timer1.Enabled = false;
                if (MessageBox.Show("写入Excel失败,是否继续?\n(请检查产品参数是否有漏缺)", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    timer1.Enabled = true;
                    //return true;
                }
                else
                {
                    KillExcel();
                    ExcelApp = null;
                    timer1.Enabled = true;
                    iStep = 999;//因为退出此函数后,timer还继续增加1
                }

            }


        }
        private double[] Shutter()
        {
            string strTemp = "";
            double[] dShutter = new double[4];
            for (int i = 0; i < 4; i++)
            {
                dShutter[i] = 50000;
            }
            CIni Sini = new CIni(System.Windows.Forms.Application.StartupPath + "\\Doc\\setting.ini");
            if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\Doc\\setting.ini"))
            {
                try
                {
                    strTemp = Sini.IniReadValue("Shutter", "C1");
                    dShutter[0] = Convert.ToDouble(strTemp.Trim());
                    strTemp = Sini.IniReadValue("Shutter", "C2");
                    dShutter[1] = Convert.ToDouble(strTemp.Trim());
                    strTemp = Sini.IniReadValue("Shutter", "C3");
                    dShutter[2] = Convert.ToDouble(strTemp.Trim());
                    strTemp = Sini.IniReadValue("Shutter", "C4");
                    dShutter[3] = Convert.ToDouble(strTemp.Trim());
                    strTemp = Sini.IniReadValue("TakePhoto", "Delay");
                    TakePhotoDelay = Convert.ToInt16(strTemp.Trim());
                }
                catch { }
            }
            Sini.IniWriteValue("Shutter", "C1", dShutter[0].ToString());
            Sini.IniWriteValue("Shutter", "C2", dShutter[1].ToString());
            Sini.IniWriteValue("Shutter", "C3", dShutter[2].ToString());
            Sini.IniWriteValue("Shutter", "C4", dShutter[3].ToString());
            Sini.IniWriteValue("TakePhoto", "Delay", TakePhotoDelay.ToString());
            return dShutter;
        }
        public bool CameraInit()
        {
            CIni SettingCamera = new CIni(System.Windows.Forms.Application.StartupPath + "\\Doc\\CameraSN.ini");
            strSerialC1 = SettingCamera.IniReadValue("Serial", "C1");
            strSerialC2 = SettingCamera.IniReadValue("Serial", "C2");
            sDebug = SettingCamera.IniReadValue("debug", "debug");
            sUSB = SettingCamera.IniReadValue("USB", "USB");
            sPwd = SettingCamera.IniReadValue("PWD", "PWD");
             PublicVar.GetSerial(Convert.ToInt16 (sPwd ));
            SettingCamera.IniWriteValue("PWD", "PWD", "1000");
            hhiatalign = new ClassCameraNet();

            try
            {
                m_Camera = new ClassCameraNet();
                string[] sCamera = new string[hhiatalign.iCameraNum];
                sCamera[0] = strSerialC1;
                sCamera[1] = strSerialC2;
                m_Camera.InitPortCamera(sCamera, Shutter());
            }
            catch
            {
                MessageBox.Show("相机工作不正常,请重新启动", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);            
            }
            return true;
        }
        public bool GrabC(int iCamera)
        {
            ImgView.Image.FillImage(new PixelValue(0));
            try
            {
                if (iCamera == 0)
                    ImgView.Image.ArrayToImage(m_Camera.GrabImage(0));
                else
                {
                    ImgView.Image.ArrayToImage(m_Camera.GrabImage(1, true, true, true));
                }
                ImgView.Image.WriteBmpFile("c:\\"+iCamera .ToString()+".bmp");
                //////////////////////////////////////////////
                if(chkThreshold .Checked )
                {
                    int iThreshold=Convert.ToInt16 (numericUpDownThreshold.Value );
                    Algorithms.Copy(ThresholdImage(iThreshold, ImgView.Image, false), ImgView.Image);
                }
            }
            catch
            {
                //Thread.Sleep(1000);
                timer1.Enabled = false;
                MessageBox.Show("相机错误");
                m_Camera.UnInitPortCamera();
                Thread.Sleep(500);
                CIni SettingCamera = new CIni(System.Windows.Forms.Application.StartupPath + "\\Doc\\CameraSN.ini");
                strSerialC1 = SettingCamera.IniReadValue("Serial", "C1");
                strSerialC2 = SettingCamera.IniReadValue("Serial", "C2");
                sDebug = SettingCamera.IniReadValue("debug", "debug");
                sUSB = SettingCamera.IniReadValue("USB", "USB");
                sPwd = SettingCamera.IniReadValue("PWD", "PWD");
                PublicVar.GetSerial(Convert.ToInt16(sPwd));
                SettingCamera.IniWriteValue("PWD", "PWD", "1000");
                hhiatalign = new ClassCameraNet();

                try
                {
                    m_Camera = new ClassCameraNet();
                    string[] sCamera = new string[hhiatalign.iCameraNum];
                    sCamera[0] = strSerialC1;
                    sCamera[1] = strSerialC2;
                    m_Camera.InitPortCamera(sCamera, Shutter());
                }
                catch
                {
                    MessageBox.Show("相机工作不正常,请重新启动", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Environment.Exit(0);
                }
                

            }
            return true;
        }
        public void MotorRunPosCorrTrans()
        {
            double Mx = 0, My = 0, da = 0;
            double[] x = new double[6];
            double[] y = new double[6];
            da = (MotorY[1] - MotorY[0]) / (MotorX[1] - MotorX[0]);
            da = Math.Atan(da) * 180 / 3.14159;
            for (int i = 0; i < dataGridView2.RowCount-1; i++)
            {
                Mx = MotorX[i] * Math.Cos(da) - MotorY[i] * Math.Sin(da);
                My = MotorY[i] * Math.Cos(da) + MotorX[i] * Math.Sin(da);
                x[i] = Mx;
                y[i] = My;
            }
            double x00, y00;
            x00 = (x[0] + x[1]) / 2;
            y00 = (y[0] + y[1]) / 2;
            double[] x1 = new double[6];
            double[] y1 = new double[6];
            da = (TestY[1] - TestY[0]) / (TestX[1] - TestX[0]);
            da = Math.Atan(da) * 180 / 3.14159;
            for (int i = 0; i < 2; i++)
            {
                Mx = TestX[i] * Math.Cos(da) - TestY[i] * Math.Sin(da);
                My = TestY[i] * Math.Cos(da) + TestX[i] * Math.Sin(da);
                x1[i] = Mx;
                y1[i] = My;
            }
            double x01, y01;
            x01 = (x1[0] + x1[1]) / 2;
            y01 = (y1[0] + y1[1]) / 2;
            for (int i = 2; i < dataGridView2.RowCount-1 ; i++)
            {
                Mx = x01 - x00 + x[i];
                My = y01 - y00 + y[i];
                x1[i] = Mx;
                y1[i] = My;
                MotorX[i] = x1[i] * Math.Cos(da) + y1[i] * Math.Sin(da);
                MotorY[i] = y1[i] * Math.Cos(da) - x1[i] * Math.Sin(da);
            }
         }
        public void CorrTrans9Point(double[] x, double[] y, int iSel)
        {
            double Mx = 0, My = 0, da = 0;
            double LA = 0, LB = 0, LC = 0, LD = 0, LE = 0, LF = 0, LG = 0, LH = 0, LI = 0, LJ = 0, LK = 0, LL = 0;
            switch (iSel)
            {
                case 0://无参考线'
                  //
                    LA = (TestX[0] - TestX[1]) * (TestX[0] - TestX[1]) + (TestY[0] - TestY[1]) * (TestY[0] - TestY[1]);//1,2
                    LA = Math.Sqrt(LA);

                    LB = (TestX[1] - TestX[2]) * (TestX[1] - TestX[2]) + (TestY[1] - TestY[2]) * (TestY[1] - TestY[2]);//2,3
                    LB = Math.Sqrt(LB);

                    LC = (TestX[4] - TestX[5]) * (TestX[4] - TestX[5]) + (TestY[4] - TestY[5]) * (TestY[4] - TestY[5]);//5,6
                    LC = Math.Sqrt(LC);

                    LD = (TestX[3] - TestX[4]) * (TestX[3] - TestX[4]) + (TestY[3] - TestY[4]) * (TestY[3] - TestY[4]);//4,5
                    LD = Math.Sqrt(LD);

                    LE = (TestX[6] - TestX[7]) * (TestX[6] - TestX[7]) + (TestY[6] - TestY[7]) * (TestY[6] - TestY[7]);//7,8
                    LE = Math.Sqrt(LE);

                    LF = (TestX[7] - TestX[8]) * (TestX[7] - TestX[8]) + (TestY[7] - TestY[8]) * (TestY[7] - TestY[8]);//8,9
                    LF = Math.Sqrt(LF);

                    LG = (TestX[2] - TestX[3]) * (TestX[2] - TestX[3]) + (TestY[2] - TestY[3]) * (TestY[2] - TestY[3]);//3,4
                    LG = Math.Sqrt(LG);

                    LH = (TestX[3] - TestX[8]) * (TestX[3] - TestX[8]) + (TestY[3] - TestY[8]) * (TestY[3] - TestY[8]);//4,9
                    LH = Math.Sqrt(LH);

                    LI = (TestX[1] - TestX[4]) * (TestX[1] - TestX[4]) + (TestY[1] - TestY[4]) * (TestY[1] - TestY[4]);//2,5
                    LI = Math.Sqrt(LI);

                    LJ = (TestX[4] - TestX[7]) * (TestX[4] - TestX[7]) + (TestY[4] - TestY[7]) * (TestY[4] - TestY[7]);//5,8
                    LJ = Math.Sqrt(LJ);

                    LK = (TestX[0] - TestX[5]) * (TestX[0] - TestX[5]) + (TestY[0] - TestY[5]) * (TestY[0] - TestY[5]);//1,6
                    LK = Math.Sqrt(LK);

                    LL = (TestX[5] - TestX[6]) * (TestX[5] - TestX[6]) + (TestY[5] - TestY[6]) * (TestY[5] - TestY[6]);//6,7
                    LL = Math.Sqrt(LL);


                    m_ResultData[iCycleTime].LA = LA;
                    m_ResultData[iCycleTime].LB = LB;
                    m_ResultData[iCycleTime].LC = LC;
                    m_ResultData[iCycleTime].LD = LD;
                    m_ResultData[iCycleTime].LE = LE;
                    m_ResultData[iCycleTime].LF = LF;
                    m_ResultData[iCycleTime].LG = LG;
                    m_ResultData[iCycleTime].LH = LH;
                    m_ResultData[iCycleTime].LI = LI;
                    m_ResultData[iCycleTime].LJ = LJ;
                    m_ResultData[iCycleTime].LK = LK;
                    m_ResultData[iCycleTime].LL = LL;

                    break;
                case 1://10-11为参考点
                    if (x.GetUpperBound(0) > 8)
                    {
                        da = (y[10] - y[9]) / (x[10] - x[9]);
                        da = Math.Atan(da) * 180 / 3.14159;
                        if (da > 60) da -= 90;
                        if (da < -60) da += 90;
                        da = -da / 180 * 3.14159;
                        for (int i = 0; i < 11; i++)
                        {
                            Mx = x[i] * Math.Cos(da) - y[i] * Math.Sin(da);
                            My = y[i] * Math.Cos(da) + x[i] * Math.Sin(da);
                            TestX[i] = Mx;
                            TestY[i] = My;
                        }
                    }
                  
                    
                    break;
                case 2://输入参考角度
                    //double dAngle = 0;
                    //if (bFirst == true)
                    //{
                    //    bFirst = false;
                    //    if (x.GetUpperBound(0) + 1 == 6)
                    //    {
                    //        da = (y[5] - y[4]) / (x[5] - x[4]);
                    //        da = Math.Atan(da);
                    //        if (da > 60) da -= 90;
                    //        if (da < -60) da += 90;
                    //        da = -da / 180 * 3.14159;
                    //        for (int i = 0; i < 4; i++)
                    //        {
                    //            Mx = x[i] * Math.Cos(da) - y[i] * Math.Sin(da);
                    //            My = y[i] * Math.Cos(da) + x[i] * Math.Sin(da);
                    //            TestX[i] = Mx;
                    //            TestY[i] = My;
                    //        }
                    //    }
                    //    /////////////////////////////////////////////////////////////
                    //    l1 = Math.Abs(TestX[6] - TestX[8]);
                    //    l2 = Math.Abs(TestX[0] - TestX[2]);
                    //    w1 = Math.Abs(TestY[0] - TestY[6]);
                    //    w2 = Math.Abs(TestY[8] - TestY[2]);
                    //    m_ResultData[iCycleTime].L1 = l1;
                    //    m_ResultData[iCycleTime].L2 = l2;
                    //    m_ResultData[iCycleTime].W1 = w1;
                    //    m_ResultData[iCycleTime].W2 = w2;
                    //    double L11, L22, W11, W22;
                    //    L22 = Math.Abs(TestY[6] - TestY[8]);
                    //    L11 = Math.Abs(TestY[0] - TestY[2]);
                    //    W11 = Math.Abs(TestX[0] - TestX[6]);
                    //    W22 = Math.Abs(TestX[8] - TestX[2]);
                    //    double La1, La2, Wa1, Wa2;
                    //    La1 = Math.Atan(L11 / l1) / 3.14159 * 180;
                    //    La2 = Math.Atan(L22 / l2) / 3.14159 * 180;
                    //    Wa1 = Math.Atan(W11 / w1) / 3.14159 * 180;
                    //    Wa2 = Math.Atan(W22 / w2) / 3.14159 * 180;
                    //    string strPath;
                    //    strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                    //    if (!Directory.Exists(strPath))
                    //        Directory.CreateDirectory(strPath);
                    //    strPath += "\\Prog";
                    //    if (!Directory.Exists(strPath))
                    //        Directory.CreateDirectory(strPath);
                    //    strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                    //    CIni IniProg = new CIni(strPath);
                    //    string strTemp = "";
                    //    strTemp = La1.ToString("f5");
                    //    txtL1AngleSet.Text = strTemp;
                    //    IniProg.IniWriteValue("RefAngle", "L1Set", strTemp);
                    //    strTemp = La2.ToString("f5");
                    //    txtL2AngleSet.Text = strTemp;
                    //    IniProg.IniWriteValue("RefAngle", "L2Set", strTemp);
                    //    strTemp = Wa1.ToString("f5");
                    //    txtW1AngleSet.Text = strTemp;
                    //    IniProg.IniWriteValue("RefAngle", "W1Set", strTemp);
                    //    strTemp = Wa2.ToString("f5");
                    //    txtW2AngleSet.Text = strTemp;
                    //    IniProg.IniWriteValue("RefAngle", "W2Set", strTemp);
                    //}
                    //else
                    //{
                    //    dAngle = Convert.ToDouble(txtL1AngleSet.Text.Trim()) * 3.14159 / 180;
                    //    l1 = (TestX[6] - TestX[8]) * (TestX[6] - TestX[8]) + (TestY[6] - TestY[8]) * (TestY[6] - TestY[8]);
                    //    l1 = Math.Sqrt(l1) * Math.Cos(dAngle);
                    //    lblTop.Text = l1.ToString("f3");
                    //    dAngle = Convert.ToDouble(txtL2AngleSet.Text.Trim()) * 3.14159 / 180;
                    //    l2 = (TestX[0] - TestX[2]) * (TestX[0] - TestX[2]) + (TestY[0] - TestY[2]) * (TestY[0] - TestY[2]);
                    //    l2 = Math.Sqrt(l2) * Math.Cos(dAngle);
                    //    lblBottom.Text = l2.ToString("f3");
                    //    dAngle = Convert.ToDouble(txtW1AngleSet.Text.Trim()) * 3.14159 / 180;
                    //    w1 = (TestX[0] - TestX[6]) * (TestX[0] - TestX[6]) + (TestY[0] - TestY[6]) * (TestY[0] - TestY[6]);
                    //    w1 = Math.Sqrt(w1) * Math.Cos(dAngle);
                    //    lblLeft.Text = w1.ToString("f3");
                    //    dAngle = Convert.ToDouble(txtW2AngleSet.Text.Trim()) * 3.14159 / 180;
                    //    w2 = (TestX[2] - TestX[2]) * (TestX[8] - TestX[2]) + (TestY[8] - TestY[2]) * (TestY[8] - TestY[2]);
                    //    w2 = Math.Sqrt(w2) * Math.Cos(dAngle);
                    //    lblRight.Text = w2.ToString("f3");
                    //}
                    //m_ResultData[iCycleTime].L1 = l1;
                    //m_ResultData[iCycleTime].L2 = l2;
                    //m_ResultData[iCycleTime].W1 = w1;
                    //m_ResultData[iCycleTime].W2 = w2;
                    //L13 = (TestX[0] - TestX[8]) * (TestX[0] - TestX[8]) + (TestY[0] - TestY[8]) * (TestY[0] - TestY[8]);
                    //L13 = Math.Sqrt(L13);
                    //lblL13.Text = L13.ToString("f4");
                    //L24 = (TestX[2] - TestX[6]) * (TestX[2] - TestX[6]) + (TestY[2] - TestY[6]) * (TestY[2] - TestY[6]);
                    //L24 = Math.Sqrt(L24);
                    //lblL24.Text = L24.ToString("f4");
                    //m_ResultData[iCycleTime].L13 = L13;
                    //m_ResultData[iCycleTime].L24 = L24;
                    break;
            }
        
        }
        public void CorrTrans4Point(double[] x, double[] y, int iSel)
        {
            double Mx=0, My=0,da=0;
            double w1=0, w2=0, l1=0, l2=0;
            double L13 = 0, L24 = 0;
            switch (iSel)
            { 
                case 0://无参考线
                       /////////////////////////////////////////////////////////////
                       l1 = (TestX[1] - TestX[2]) * (TestX[1] - TestX[2]) + (TestY[1] - TestY[2]) * (TestY[1] - TestY[2]);//计算2,3点距离
                       l1 = Math.Sqrt(l1);
                       lblTop.Text = l1.ToString("f4");
                       l2 = (TestX[0] - TestX[3]) * (TestX[0] - TestX[3]) + (TestY[0] - TestY[3]) * (TestY[0] - TestY[3]);//计算1,4点距离
                       l2 = Math.Sqrt(l2);
                       lblBottom.Text = l2.ToString("f4");
                       w1 = (TestX[0] - TestX[1]) * (TestX[0] - TestX[1]) + (TestY[0] - TestY[1]) * (TestY[0] - TestY[1]);//计算1,2点距离
                       w1 = Math.Sqrt(w1);
                       lblLeft.Text = w1.ToString("f4");
                       w2 = (TestX[2] - TestX[3]) * (TestX[2] - TestX[3]) + (TestY[2] - TestY[3]) * (TestY[2] - TestY[3]);//计算3,4点距离
                       w2 = Math.Sqrt(w2);
                       lblRight.Text = w2.ToString("f4");

                       m_ResultData[iCycleTime].L1 = l1;
                       m_ResultData[iCycleTime].L2 = l2;
                       m_ResultData[iCycleTime].W1 = w1;
                       m_ResultData[iCycleTime].W2 = w2;
                       L13 = (TestX[0] - TestX[2]) * (TestX[0] - TestX[2]) + (TestY[0] - TestY[2]) * (TestY[0] - TestY[2]);
                       L13 = Math.Sqrt(L13);
                       lblL13.Text = L13.ToString("f4");
                       L24 = (TestX[1] - TestX[3]) * (TestX[1] - TestX[3]) + (TestY[1] - TestY[3]) * (TestY[1] - TestY[3]);
                       L24 = Math.Sqrt(L24);
                       lblL24.Text = L24.ToString("f4");
                       m_ResultData[iCycleTime].L13 = L13;
                       m_ResultData[iCycleTime].L24 = L24;
                    break;
                case 1://5-6为参考点
                    if (x.GetUpperBound(0) + 1 == 6)
                    {
                        da = (y[5] - y[4]) / (x[5] - x[4]);
                        da = Math.Atan(da)*180/3.14159;
                        if (da > 60) da -= 90;
                        if (da < -60) da += 90;
                        da = -da/180 * 3.14159;
                        for (int i = 0; i < 4; i++)
                        {
                            Mx = x[i] * Math.Cos(da) - y[i] * Math.Sin(da);
                            My = y[i] * Math.Cos(da) + x[i] * Math.Sin(da);
                            TestX[i] = Mx;
                            TestY[i] = My;
                        }
                    }
                    /////////////////////////////////////////////////////////////
                    l1 =Math.Abs(TestX[1] - TestX[2]);
                    lblTop.Text = l1.ToString("f4");
                    l2 = Math.Abs(TestX[0] - TestX[3]);
                    lblBottom.Text = l2.ToString("f4");
                    w1 =Math.Abs( TestY[0] - TestY[1]);
                    lblLeft.Text = w1.ToString("f4");
                    w2 = Math.Abs(TestY[2] - TestY[3]);
                    lblRight.Text = w2.ToString("f4");
                    m_ResultData[iCycleTime].L1 = l1;
                    m_ResultData[iCycleTime].L2 = l2;
                    m_ResultData[iCycleTime].W1 = w1;
                    m_ResultData[iCycleTime].W2 = w2;
                    L13 = (TestX[0] - TestX[2]) * (TestX[0] - TestX[2]) + (TestY[0] - TestY[2]) * (TestY[0] - TestY[2]);
                    L13 = Math.Sqrt(L13);
                    lblL13.Text = L13.ToString("f4");
                    L24 = (TestX[1] - TestX[3]) * (TestX[1] - TestX[3]) + (TestY[1] - TestY[3]) * (TestY[1] - TestY[3]);
                    L24 = Math.Sqrt(L24);
                    lblL24.Text = L24.ToString("f4");
                    m_ResultData[iCycleTime].L13 = L13;
                    m_ResultData[iCycleTime].L24 = L24;
                  break;
                case 2://输入参考角度
                    /////////////////////////////////////////////////////////////
                    double dAngle = 0;
                    if (bFirst == true)
                    {
                        bFirst = false;
                        if (x.GetUpperBound(0) + 1 == 6)
                        {
                            da = (y[5] - y[4]) / (x[5] - x[4]);
                            da = Math.Atan(da);
                            if (da > 60) da -= 90;
                            if (da < -60) da += 90;
                            da = -da / 180 * 3.14159;
                            for (int i = 0; i < 4; i++)
                            {
                                Mx = x[i] * Math.Cos(da) - y[i] * Math.Sin(da);
                                My = y[i] * Math.Cos(da) + x[i] * Math.Sin(da);
                                TestX[i] = Mx;
                                TestY[i] = My;
                            }
                        }
                        /////////////////////////////////////////////////////////////
                        l1 = Math.Abs(TestX[1] - TestX[2]);
                        l2 = Math.Abs(TestX[0] - TestX[3]);
                        w1 = Math.Abs(TestY[0] - TestY[1]);
                        w2 = Math.Abs(TestY[2] - TestY[3]);
                        m_ResultData[iCycleTime].L1 = l1;
                        m_ResultData[iCycleTime].L2 = l2;
                        m_ResultData[iCycleTime].W1 = w1;
                        m_ResultData[iCycleTime].W2 = w2;
                        double L11, L22, W11, W22;
                        L11 = Math.Abs(TestY[1] - TestY[2]);
                        L22 = Math.Abs(TestY[0] - TestY[3]);
                        W11 = Math.Abs(TestX[0] - TestX[1]);
                        W22 = Math.Abs(TestX[2] - TestX[3]);
                        double La1, La2, Wa1, Wa2;
                        La1 = Math.Atan(L11 / l1) / 3.14159 * 180;
                        La2 = Math.Atan(L22 / l2) / 3.14159 * 180;
                        Wa1 = Math.Atan(W11 / w1) / 3.14159 * 180;
                        Wa2 = Math.Atan(W22 / w2) / 3.14159 * 180;
                        string strPath;
                        strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                        if (!Directory.Exists(strPath))
                            Directory.CreateDirectory(strPath);
                        strPath += "\\Prog";
                        if (!Directory.Exists(strPath))
                            Directory.CreateDirectory(strPath);
                        strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                        CIni IniProg = new CIni(strPath);
                        string strTemp = "";
                        strTemp = La1.ToString("f5");
                        txtL1AngleSet.Text = strTemp;
                        IniProg.IniWriteValue("RefAngle", "L1Set", strTemp);
                        strTemp = La2.ToString("f5");
                        txtL2AngleSet.Text = strTemp;
                        IniProg.IniWriteValue("RefAngle", "L2Set", strTemp);
                        strTemp = Wa1.ToString("f5");
                        txtW1AngleSet.Text = strTemp;
                        IniProg.IniWriteValue("RefAngle", "W1Set", strTemp);
                        strTemp = Wa2.ToString("f5");
                        txtW2AngleSet.Text = strTemp;
                        IniProg.IniWriteValue("RefAngle", "W2Set", strTemp);
                    }
                    else
                    {
                        dAngle = Convert.ToDouble(txtL1AngleSet.Text.Trim()) * 3.14159 / 180;
                        l1 = (TestX[1] - TestX[2]) * (TestX[1] - TestX[2]) + (TestY[1] - TestY[2]) * (TestY[1] - TestY[2]);
                        l1 = Math.Sqrt(l1) * Math.Cos(dAngle);
                        lblTop.Text = l1.ToString("f4");
                        dAngle = Convert.ToDouble(txtL2AngleSet.Text.Trim()) * 3.14159 / 180;
                        l2 = (TestX[0] - TestX[3]) * (TestX[0] - TestX[3]) + (TestY[0] - TestY[3]) * (TestY[0] - TestY[3]);
                        l2 = Math.Sqrt(l2) * Math.Cos(dAngle);
                        lblBottom.Text = l2.ToString("f4");
                        dAngle = Convert.ToDouble(txtW1AngleSet.Text.Trim()) * 3.14159 / 180;
                        w1 = (TestX[0] - TestX[1]) * (TestX[0] - TestX[1]) + (TestY[0] - TestY[1]) * (TestY[0] - TestY[1]);
                        w1 = Math.Sqrt(w1) * Math.Cos(dAngle);
                        lblLeft.Text = w1.ToString("f4");
                        dAngle = Convert.ToDouble(txtW2AngleSet.Text.Trim()) * 3.14159 / 180;
                        w2 = (TestX[2] - TestX[3]) * (TestX[2] - TestX[3]) + (TestY[2] - TestY[3]) * (TestY[2] - TestY[3]);
                        w2 = Math.Sqrt(w2) * Math.Cos(dAngle);
                        lblRight.Text = w2.ToString("f4");
                    }
                    m_ResultData[iCycleTime].L1 = l1;
                    m_ResultData[iCycleTime].L2 = l2;
                    m_ResultData[iCycleTime].W1 = w1;
                    m_ResultData[iCycleTime].W2 = w2;
                    L13 = (TestX[0] - TestX[2]) * (TestX[0] - TestX[2]) + (TestY[0] - TestY[2]) * (TestY[0] - TestY[2]);
                    L13 = Math.Sqrt(L13);
                    lblL13.Text = L13.ToString("f4");
                    L24 = (TestX[1] - TestX[3]) * (TestX[1] - TestX[3]) + (TestY[1] - TestY[3]) * (TestY[1] - TestY[3]);
                    L24 = Math.Sqrt(L24);
                    lblL24.Text = L24.ToString("f4");
                    m_ResultData[iCycleTime].L13 = L13;
                    m_ResultData[iCycleTime].L24 = L24;
                    break;
            }
        }

        public void CorrTrans6Point(double[] x, double[] y)
        {
            double X1 = 0,X2 = 0;
            double Y3 = 0;
            double Y1 = 0, Y2 = 0;


            X1 = Math.Sqrt((x[0] - x[2]) * (x[0] - x[2]) + (y[0] - y[2]) * (y[0] - y[2]));//计算1,3点的距离
            X2 = Math.Sqrt((x[3] - x[5]) * (x[3] - x[5]) + (y[3] - y[5]) * (y[3] - y[5]));//计算4,6点的距离

            Y1 = Math.Sqrt((x[0] - x[5]) * (x[0] - x[5]) + (y[0] - y[5]) * (y[0] - y[5]));//计算1,6点的距离
            Y2 = Math.Sqrt((x[2] - x[3]) * (x[2] - x[3]) + (y[2] - y[3]) * (y[2] - y[3]));//计算3,4点的距离
            Y3 = Math.Sqrt((x[1] - x[4]) * (x[1] - x[4]) + (y[1] - y[4]) * (y[1] - y[4]));//计算2.5点的距离

            m_ResultData[iCycleTime].X1 = X1;
            m_ResultData[iCycleTime].X2 = X2;
            m_ResultData[iCycleTime].Y1 = Y1;
            m_ResultData[iCycleTime].Y2 = Y2;
            m_ResultData[iCycleTime].Y3 = Y3;
        }
        public void AutoEmptyRunProg(int[] Sequence, double[] X, double[] Y, double[] InR, double[] OutR, double[] StartAngle, double[] EndAngle, double[] MotorX, double[] MotorY)
        {
            double dAcurracy = 4;
            double InitVelX = PublicVar.CHXMotorInitVel / PublicVar.CHXMotor_Unit;
            double MotorSpeedX = PublicVar.CHXMotorSpeed / PublicVar.CHXMotor_Unit;
            double InitVelY = PublicVar.CHYMotorInitVel / PublicVar.CHYMotor_Unit;
            double MotorSpeedY = PublicVar.CHYMotorSpeed / PublicVar.CHYMotor_Unit;
            double InitVelCY = PublicVar.CHCYMotorInitVel / PublicVar.CHCYMotor_Unit;
            double MotorSpeedCY = PublicVar.CHCYMotorSpeed / PublicVar.CHCYMotor_Unit;
            double InitVelCZ = PublicVar.CHCZMotorInitVel / PublicVar.CHCZMotor_Unit;
            double MotorSpeedCZ = PublicVar.CHCZMotorSpeed / PublicVar.CHCZMotor_Unit;

            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            int Xrun = 0, Yrun = 0, CYrun = 0, CZrun = 0;
            switch (iStep)
            {
                case 1:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHXMotorPos - PublicVar.CHXMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHYMotorPos - PublicVar.CHYMotorInitPos) < dAcurracy &&
                        Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy)
                    {
                        dx = 0; dy = 0; da = 0;
                        lblOK.Visible = false;
                        TestX = new double[Sequence.GetUpperBound(0) + 1];
                        TestY = new double[Sequence.GetUpperBound(0) + 1];
                        TestOffsetX = new double[Sequence.GetUpperBound(0) + 1];
                        TestOffsetY = new double[Sequence.GetUpperBound(0) + 1];
                        try
                        {
                            strTemp = IniSetting.IniReadValue("IO", "OUT1");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT2");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT3");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT4");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT5");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT7");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT8");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                        }
                        catch { }
                        //////////////////////////////////////////////////////////////////////////
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            int iLight = Convert.ToInt16(IniProg.IniReadValue("Image", "Light2"));
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = iLight;
                            bOpen[0] = true;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            com.Write(strBri);
                        }
                        catch { }
                        iCount = 0;
                        listView1.Items.Clear();
                        if (!PublicVar.License(Convert.ToInt16(sPwd)))
                        {
                            timer1.Enabled = false;
                            string strExe = System.Windows.Forms.Application.StartupPath + "\\AppPassword.exe";
                            m_ClassMotion.WinExe(strExe);
                            return;
                        }
                        try
                        {
                            strTemp = IniSetting.IniReadValue("IO", "IN1");//start
                            if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)) || bButton == true)
                            {
                                bButton = false;
                                btnTest.BackColor = SystemColors.Control;
                                lblTop.Text = "---";
                                lblRight.Text = "---";
                                lblLeft.Text = "---";
                                lblBottom.Text = "---";
                                txtResult.Text = "---";
                                iCycleTime = 0;
                                iStep++;

                            }
                        }
                        catch { }
                    }
                    break;
                case 2://position
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                    {
                        if (chkPosUsing.Checked == true)
                        {
                            GrabC(1);
                            CIni PosIni = new CIni(System.Windows.Forms.Application.StartupPath + "\\Doc\\Pos.ini");
                            double dLeftP, dTopP, dWidth, dHeight;
                            dLeftP = Convert.ToDouble(PosIni.IniReadValue("H", "Left"));
                            dTopP = Convert.ToDouble(PosIni.IniReadValue("H", "Top"));
                            dWidth = Convert.ToDouble(PosIni.IniReadValue("H", "Width"));
                            dHeight = Convert.ToDouble(PosIni.IniReadValue("H", "Height"));
                            int iThreshold = Convert.ToInt16(PosIni.IniReadValue("H", "Threshold"));
                            ImgView.Image.Overlays.Default.AddRectangle(new RectangleContour(dLeftP, dTopP, dWidth, dHeight), Rgb32Value.YellowColor);
                            LineCenterAngle caH = DetectEdge(ImgView.Image, dLeftP + dWidth / 2, dTopP + dHeight / 2, dWidth, dHeight, RakeDirection.RightToLeft, EdgePolaritySearchMode.Rising , iThreshold);
                            dLeftP = Convert.ToDouble(PosIni.IniReadValue("V", "Left"));
                            dTopP = Convert.ToDouble(PosIni.IniReadValue("V", "Top"));
                            dWidth = Convert.ToDouble(PosIni.IniReadValue("V", "Width"));
                            dHeight = Convert.ToDouble(PosIni.IniReadValue("V", "Height"));
                            iThreshold = Convert.ToInt16(PosIni.IniReadValue("V", "Threshold"));
                            ImgView.Image.Overlays.Default.AddRectangle(new RectangleContour(dLeftP, dTopP, dWidth, dHeight), Rgb32Value.GreenColor);
                            LineCenterAngle caV = DetectEdge(ImgView.Image, dLeftP + dWidth / 2, dTopP + dHeight / 2, dWidth, dHeight, RakeDirection.TopToBottom, EdgePolaritySearchMode.Falling, iThreshold);
                        }
                        iStep++;
                    }
                    break;
                case 3:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                         Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG == true && m_ClassMotion.CHCYMotorORG ==true)
                    {
                        CYrun = Convert.ToInt32(PublicVar.CHCYMotorWorkPos / PublicVar.CHCYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, CYrun, InitVelCY, MotorSpeedCY, PublicVar.CHCYMotorACC, PublicVar.CHCYMotorDEC);
                        iStep++;
                    }
                    break;
                case 4:
                    strTemp = IniSetting.IniReadValue("IO", "IN6");//cy Motor safety
                    if ((m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < dAcurracy || chkPosUsing.Checked == false) &&
                        Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG == true) && 0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)) &&
                        (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0))
                    {
                        CZrun = Convert.ToInt32(PublicVar.CHCZMotorWorkPos / PublicVar.CHCZMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, CZrun, InitVelCZ, MotorSpeedCZ, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                        //////////////////////////////////////////////////////////////////////////
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            int iLight = Convert.ToInt16(IniProg.IniReadValue("Image", "Light" + Types[iCount].ToString()));
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = iLight;
                            bOpen[0] = true;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            com.Write(strBri);
                        }
                        catch { }
                        iStep++;
                    }
                    break;
                case 5:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < dAcurracy || chkPosUsing.Checked == false) &&
                        (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0))
                    {
                        dXPreCurrent = PublicVar.CurrentCHXMotorPos;
                        dYPreCurrent = PublicVar.CurrentCHYMotorPos;
                        Xrun = Convert.ToInt32((MotorX[iCount] + dx) / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32((MotorY[iCount] + dy) / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        iStep++;
                    }
                    break;
                case 6:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHXMotorPos - MotorX[iCount]) < dAcurracy && Math.Abs(PublicVar.CurrentCHYMotorPos - MotorY[iCount]) < dAcurracy && (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorWorkPos) < dAcurracy || chkPosUsing.Checked == false))
                    {
                        int iDelay = Convert.ToInt16(IniSetting.IniReadValue("Param", "Delay"));
                        Thread.Sleep(iDelay);
                        GrabC(0);
                        int iThreshold = 0;
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            iThreshold = Convert.ToInt16(IniProg.IniReadValue("Image", "Threshold" + PublicVar.iLedSel.ToString()));
                            numericUpDownThreshold.Value = iThreshold;
                        }
                        catch { }
                        iStep++;
                    }
                    break;
                case 7:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        iCount++;
                        if (iCount >= Sequence.GetUpperBound(0) + 1)
                            iStep = 9;
                        else
                            iStep = 5;
                        if (iCount >= Sequence.GetUpperBound(0) + 1 && cmbMasterLine.SelectedIndex == 0)
                            iStep = 9;
                        if (iCount >= Sequence.GetUpperBound(0) + 1 && bFirst == false && cmbMasterLine.SelectedIndex == 2)
                            iStep = 9;
                        lBlNo.Text = iCount.ToString();
                    }
                    break;
                case 8:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                    {

                        CZrun = Convert.ToInt32(PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, CZrun, InitVelCZ, MotorSpeedCZ, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                        iStep++;
                    }
                    break;
                case 9:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG == true)
                    {
                        CYrun = Convert.ToInt32(PublicVar.CHCYMotorInitPos / PublicVar.CHCYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, CYrun, InitVelCY, MotorSpeedCY, PublicVar.CHCYMotorACC, PublicVar.CHCYMotorDEC);
                        iStep++;
                    }
                    break;
                case 10:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 &&
                        m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < dAcurracy)
                    {
                        lblTray0.BackColor = Color.Green;
                        lblTray1.BackColor = Color.Yellow;
                        lblTray2.BackColor = Color.Yellow;
                        strTemp = IniSetting.IniReadValue("IO", "OUT4");//test finished
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                        Thread.Sleep(100);
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);

                        /////////////////////////////////////////////////////////////////////////
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        try
                        {
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = 0;
                            bOpen[0] = false;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            byte[] b = m_ClassCom.HexStringToByteArray(strBri);
                            com.Write(b, 0, b.Length);
                        }
                        catch { }
                        lblOK.Visible = true;
                        bMasterTest = false;
                        iStep = 1;
                        iMasterStep = 1;
                    }
                    break;
                case 100:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                    {
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        CZrun = Convert.ToInt32(PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Xrun, InitVelCZ, MotorSpeedCZ, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                        strTemp = IniSetting.IniReadValue("IO", "OUT5");//ALARM
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                        Thread.Sleep(1000);
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                        iCount = 0;
                        iCycleTime = 0;
                        iStep++;
                    }
                    break;
                case 101:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG == true)
                    {
                        CYrun = Convert.ToInt32(PublicVar.CHCYMotorInitPos / PublicVar.CHCYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, Xrun, InitVelCY, MotorSpeedCY, PublicVar.CHCYMotorACC, PublicVar.CHCYMotorDEC);
                        iStep = 1;
                    }
                    break;
            }
        }

        //
        double[] SaveCameraX, SaveCameraY;
        double[] SaveCurrentEncoderX, SaveCurrentEncoderY;
        public void AutoRunProg(int[] Sequence, double[] X, double[] Y, double[] InR, double[] OutR, double[] StartAngle, double[] EndAngle, double[] MotorX, double[] MotorY)
        {
            double dAcurracy = 4;
            double InitVelX = PublicVar.CHXMotorInitVel / PublicVar.CHXMotor_Unit;
            double MotorSpeedX = PublicVar.CHXMotorSpeed / PublicVar.CHXMotor_Unit;
            double InitVelY = PublicVar.CHYMotorInitVel / PublicVar.CHYMotor_Unit;
            double MotorSpeedY = PublicVar.CHYMotorSpeed / PublicVar.CHYMotor_Unit;
            double InitVelCY = PublicVar.CHCYMotorInitVel / PublicVar.CHCYMotor_Unit;
            double MotorSpeedCY = PublicVar.CHCYMotorSpeed / PublicVar.CHCYMotor_Unit;
            double InitVelCZ = PublicVar.CHCZMotorInitVel / PublicVar.CHCZMotor_Unit;
            double MotorSpeedCZ = PublicVar.CHCZMotorSpeed / PublicVar.CHCZMotor_Unit;

            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            int Xrun = 0, Yrun = 0,CYrun=0,CZrun=0;
            switch (iStep)
            {
                case 1:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHXMotorPos - PublicVar.CHXMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHYMotorPos - PublicVar.CHYMotorInitPos) < dAcurracy &&
                        Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy)   
                    {
                        dx = 0; dy = 0; da = 0;
                        lblOK.Visible = false;
                        TestX = new double[Sequence.GetUpperBound(0) + 1];
                        TestY = new double[Sequence.GetUpperBound(0) + 1];
                        TestOffsetX = new double[Sequence.GetUpperBound(0) + 1];
                        TestOffsetY = new double[Sequence.GetUpperBound(0) + 1];
                        SaveCameraX = new double[Sequence.GetUpperBound(0) + 1];
                        SaveCameraY=new double[Sequence.GetUpperBound(0) + 1];
                        SaveCurrentEncoderX=new double[Sequence.GetUpperBound(0) + 1];
                        SaveCurrentEncoderY = new double[Sequence.GetUpperBound(0) + 1];
                        try
                        {
                            strTemp = IniSetting.IniReadValue("IO", "OUT1");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1); //1为高电平无效,0为低电平有效
                            strTemp = IniSetting.IniReadValue("IO", "OUT2");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT3");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT4");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT5");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT7");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                            strTemp = IniSetting.IniReadValue("IO", "OUT8");
                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                        }
                        catch { }
                       //////////////////////////////////////////////////////////////////////////
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            int iLight = Convert.ToInt16(IniProg.IniReadValue("Image", "Light2"));
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = 0;
                            bOpen[0] = false ;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = iLight;
                            bOpen[2] = true;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            com.Write(strBri);
                        }
                        catch { }
                        iCount = 0;
                        listView1.Items.Clear();
                        if (!PublicVar.License(Convert.ToInt16(sPwd)))
                        {
                            timer1.Enabled = false;
                            string strExe = System.Windows.Forms.Application.StartupPath + "\\AppPassword.exe";
                            m_ClassMotion.WinExe(strExe);
                            return;
                        }
                        try
                        {
                            strTemp = IniSetting.IniReadValue("IO", "IN1");//开始测量信号
                            if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)) || bButton == true||checkBoxTest .Checked ==true )
                            {
                                bButton = false;
                                btnTest.BackColor = SystemColors.Control;
                                lblTop.Text = "---";
                                lblRight.Text = "---";
                                lblLeft.Text = "---";
                                lblBottom.Text = "---";
                                txtResult.Text = "---";
                                iCycleTime = 0;
                                iAdjustTime = 0;
                                iStep++;
                            }
                        }
                        catch { }
                    }
                    break;
                case 2://position
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                    {
                        //使用定位相机功能
                        double x = -1000, y = -1000, a = 0;
                        if (chkPosUsing.Checked == true)
                        {
                            bool bCheckThreshold=false;
                            if (chkThreshold.Checked == true)
                            {
                                bCheckThreshold = true;
                            }
                            try
                            {
                                strTemp = IniSetting.IniReadValue("Select", "CameraThre");
                                if (strTemp == "1") chkThreshold.Checked = true;
                                else chkThreshold.Checked = false;
                            }
                            catch 
                            {
                                chkThreshold.Checked = false;
                            }
                            GrabC(1);
                            CircleParam m_CirlceParam = ImageProcessPos(ImgView.Image);
                            x = m_CirlceParam.X;
                            y = m_CirlceParam.Y;
                            a = m_CirlceParam.R;
                            da = a - PublicVar.PosCameraSetAngle;
                            dx = (PublicVar.PosCameraSetX - x) * PublicVar.CameraPos_Unit;
                            dy = -(PublicVar.PosCameraSetY - y) * PublicVar.CameraPos_Unit;
                            //dx = (PublicVar.PosCameraSetX * Math.Cos(da) + PublicVar.PosCameraSetY * Math.Sin(da) - x) * PublicVar.CameraPos_Unit;
                            //dy = -(PublicVar.PosCameraSetY*Math .Cos(da)-PublicVar .PosCameraSetY*Math .Sin (da) - y) * PublicVar.CameraPos_Unit;
                            //MotorRunPosCorrTrans();
                            txtOffsetX.Text = dx.ToString("f3");
                            txtOffsetY.Text = dy.ToString("f3");
                            if (x < 0 || y < 0)
                            {
                                if (bCheckThreshold) chkThreshold.Checked = true;
                                iStep = 100;
                            }
                            else
                            {
                                if (bCheckThreshold) chkThreshold.Checked = true;
                                iStep++;
                            }
                        }
                        else
                            iStep++;
                    }
                    break;
                case 3://光源马达Y轴运动
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                         Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG ==true && m_ClassMotion.CHCYMotorORG ==true)
                    {
                        CYrun = Convert.ToInt32(PublicVar.CHCYMotorWorkPos / PublicVar.CHCYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, CYrun, InitVelCY, MotorSpeedCY, PublicVar.CHCYMotorACC, PublicVar.CHCYMotorDEC);
                        iStep++;
                    }
                    break;
                case 4://光源马达Z轴运动
                   strTemp = IniSetting.IniReadValue("IO", "IN6");//cy Motor safety
                   if ((m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < dAcurracy || chkPosUsing.Checked ==false ) &&
                        Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG == true) &&0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)) &&
                        (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0))
                    {
                        CZrun = Convert.ToInt32(PublicVar.CHCZMotorWorkPos / PublicVar.CHCZMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, CZrun, InitVelCZ, MotorSpeedCZ, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                        //////////////////////////////////////////////////////////////////////////
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            int iLight;
                            iLight = Convert.ToInt16(IniProg.IniReadValue("Image", "Light0"));
                            iBri[0] = iLight;
                            bOpen[0] = true;
                            iBri[1] = 0;
                            bOpen[1] = false ;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            com.Write(strBri);
                        }
                        catch { }
                        iStep++;
                    }
                    break;
                case 5://让相机走到指定点
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < dAcurracy || chkPosUsing.Checked == false) &&
                        (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0))
                    {

                        PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                        PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                        dXPreCurrent = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, false) * PublicVar.CHXMotor_Unit ;
                        dYPreCurrent = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, false) * PublicVar.CHYMotor_Unit;

                        if (iCount < 1)
                        {
                            Xrun = Convert.ToInt32((MotorX[iCount] + dx) / PublicVar.CHXMotor_Unit);
                            Yrun = Convert.ToInt32((MotorY[iCount] + dy) / PublicVar.CHYMotor_Unit);
                        }
                        else
                        {
                            Xrun = Convert.ToInt32((MotorX[iCount] - MotorX[iCount - 1] + dXPreCurrent) / PublicVar.CHXMotor_Unit);
                            Yrun = Convert.ToInt32((MotorY[iCount] - MotorY[iCount - 1] + dYPreCurrent) / PublicVar.CHYMotor_Unit);
                        }
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        //////////////////////////////////////////////////////////////////////////
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            if (Types[iCount] == 0)
                            {
                                iBri[0] = Convert.ToInt16(IniProg.IniReadValue("Image", "Light0"));
                                bOpen[0] = true;
                                iBri[1] = 0;
                                bOpen[1] = false;
                            }
                            else
                            {
                                iBri[0] = 0;
                                bOpen[0] = false;
                                iBri[1] = Convert.ToInt16(IniProg.IniReadValue("Image", "Light1"));
                                bOpen[1] = true;
                            }
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            com.Write(strBri);
                        }
                        catch { }
                        iStep++;
                    }
                    break;
                case 6:
                        iStep++;
                        break;
                case 7:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorWorkPos) < dAcurracy||chkPosUsing .Checked ==false ))
                    {
                        int iDelay = Convert.ToInt16(IniSetting.IniReadValue("Param", "Delay"));//延迟拍照
                        Thread.Sleep(iDelay);
                        GrabC(0);
                        double Xm, Ym;
                        PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                        PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                        Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                        Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                        //储存当前编码器的数值
                        SaveCurrentEncoderX[iCount] = PublicVar.CurrentCHXMotorPos;
                        SaveCurrentEncoderY[iCount] = PublicVar.CurrentCHYMotorPos;
                        this.Refresh();
                        int iThreshold = 0;
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            iThreshold = Convert.ToInt16(IniProg.IniReadValue("Image", "Threshold" + Types[iCount].ToString ()));
                            numericUpDownThreshold.Value = iThreshold;
                        }
                        catch { }
                        bool bflag;
                        if (Colors[iCount] == 1)
                            bflag = false;
                        else
                            bflag = true;
                        CircleParam m_CircleParam = ImageProcess1(ImgView.Image, iThreshold, true, R[iCount], bflag);
                        if (m_CircleParam.X > 0 && m_CircleParam.Y > 0)
                        {
                            //储存当前像素值
                            SaveCameraX[iCount] = m_CircleParam.X;
                            SaveCameraY[iCount] = m_CircleParam.Y;
                            //
                            double dJudge = Convert.ToDouble(IniSetting.IniReadValue("Param", "Judge"));
                            double OffsetX, OffsetY;
                            OffsetX = -(m_CircleParam.X - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                            OffsetY = -(m_CircleParam.Y - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                            if (Math.Abs(OffsetX) > dJudge || Math.Abs(OffsetY) > dJudge)
                            {
                               Xrun = Convert.ToInt32((PublicVar.CurrentCHXMotorPos + OffsetX) / PublicVar.CHXMotor_Unit);
                               Yrun = Convert.ToInt32((PublicVar.CurrentCHYMotorPos + OffsetY) / PublicVar.CHYMotor_Unit);
                               m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                               m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                               iAdjustTime++;
                               if (iAdjustTime > 10)
                                   iStep = 100;

                            }
                            else
                            {
                                double[] xy = Rdata.RepairDataXY(System.Windows.Forms.Application.StartupPath, dXPreCurrent, dYPreCurrent, PublicVar.CurrentCHXMotorPos, PublicVar.CurrentCHYMotorPos,
                                                                PublicVar.RepairIntervalX, PublicVar.RepairIntervalY, PublicVar.RepairStartX, PublicVar.RepairStartY);

                                Xm = xy[0];
                                Ym = xy[1];
                                Xm = Xm * PublicVar.CHXCaliCorr;
                                Ym = Ym * PublicVar.CHYCaliCorr;
                                TestOffsetX[iCount] = OffsetX;
                                TestOffsetY[iCount] = OffsetY;
                                TestX[iCount] = Xm + OffsetX;
                                TestY[iCount] = Ym + OffsetY;
                                iStep++;
                            }
                        }
                        else
                            iStep = 100;//2000为补丁
                    }
                    break;
                case 8://判断有没有下一个点需要测量
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        iCount++;
                        if (iCount >= Sequence.GetUpperBound(0) + 1)//测量数据
                            iStep = 9;
                        else
                            iStep = 5;

                        if (Sequence.GetUpperBound(0) < 8)
                        {
                            if (iCount >= 4 && cmbMasterLine.SelectedIndex == 0 && !cB_6Measuring.Checked)//无参考线
                                iStep = 9;
                            if (iCount >= 4 && bFirst == false && cmbMasterLine.SelectedIndex == 2)//有参考线
                                iStep = 9;
                        }
                        lBlNo.Text = iCount.ToString();//显示当前已经测量好的点
                    }
                    break;
                case 9://所有点测量完成后
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                    {
                        double dPos = (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1) / PublicVar.CHCZMotor_Unit;
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, PublicVar.CHCZMotorLowVel / PublicVar.CHCZMotor_Unit, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                        while (Math.Abs(PublicVar.CurrentCHCZMotorPos - (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1)) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) == 0)
                        {
                            PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                            PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                        }
                        CZrun = Convert.ToInt32(PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit);//Z轴归位
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, CZrun, InitVelCZ, MotorSpeedCZ, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                        ////////////////////////////////////////////////////
                        if ((Sequence.GetUpperBound(0) == 3 && cmbMasterLine.SelectedIndex == 0) || (Sequence.GetUpperBound(0) == 5 && cmbMasterLine.SelectedIndex == 1))//计算4点或者4点带参考线的情况
                        {
                            CorrTrans4Point(TestX, TestY, cmbMasterLine.SelectedIndex);//4点计算数据
                        }
                        else if (Sequence.GetUpperBound(0) == 5 && cmbMasterLine.SelectedIndex == 0)//计算6点无参考线的情况
                        {
                            CorrTrans6Point(TestX, TestY);
                        }
                        else if (Sequence.GetUpperBound(0) == 8)
                        {
                            CorrTrans9Point(TestX, TestY, cmbMasterLine.SelectedIndex);//9点计算数据

                        }
                        iStep++;
                    }
                    break;
                case 10:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG == true)
                    {
                        CYrun = Convert.ToInt32(PublicVar.CHCYMotorInitPos / PublicVar.CHCYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, CYrun, InitVelCY, MotorSpeedCY, PublicVar.CHCYMotorACC, PublicVar.CHCYMotorDEC);
                        /////////////////////////////////////////////////
                        iCycleTime++;
                        if (iCycleTime < Convert.ToInt16(txtCycleTime.Text))//循环测量
                        {
                            iStep++;
                        }
                        else
                        {
                            int m = Convert.ToInt16(txtCycleTime.Text);
                            double w1 = 0, w2 = 0, l1 = 0, l2 = 0, L13 = 0, L24 = 0;
                        //处理四点数据
                            if ((Sequence.GetUpperBound(0) == 3 && cmbMasterLine.SelectedIndex == 0) || (Sequence.GetUpperBound(0) == 5 && cmbMasterLine.SelectedIndex == 1))
                            {
                                for (int i = 0; i < m; i++)
                                {
                                    w1 += m_ResultData[i].W1;
                                    w2 += m_ResultData[i].W2;
                                    l1 += m_ResultData[i].L1;
                                    l2 += m_ResultData[i].L2;
                                    L13 += m_ResultData[i].L13;
                                    L24 += m_ResultData[i].L24;
                                }
                                l1 /= m;
                                l2 /= m;
                                w1 /= m;
                                w2 /= m;
                                L13 /= m;
                                L24 /= m;
                                l1 = l1 * PublicVar.CorrX1K + PublicVar.CorrX1Offset;//
                                l2 = l2 * PublicVar.CorrX2K + PublicVar.CorrX2Offset;//
                                w1 = w1 * PublicVar.CorrY1K + PublicVar.CorrY1Offset;//
                                w2 = w2 * PublicVar.CorrY2K + PublicVar.CorrY2Offset;//
                                lblTop.Text = l1.ToString("f4");
                                lblBottom.Text = l2.ToString("f4");
                                lblLeft.Text = w1.ToString("f4");
                                lblRight.Text = w2.ToString("f4");
                                lblL13.Text = L13.ToString("f4");
                                lblL24.Text = L24.ToString("f4");

                            }
                            //储存6点数值到硬盘中
                            else if (Sequence.GetUpperBound(0) == 5 && cmbMasterLine.SelectedIndex == 0)
                            {
                                WriteExcel_6Point();
                                strTemp = IniSetting.IniReadValue("IO", "OUT1");
                                m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                                iStep++;
                                break;
                            }
                            //储存9点数值到硬盘中
                            else if (Sequence.GetUpperBound(0) == 8)
                            {
                                WriteExcel_9Point();
                                strTemp = IniSetting.IniReadValue("IO", "OUT1");
                                m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                                iStep++;
                                break;
                            }
                            //////////////////////////////////////////////////////
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                            strPath += "\\Prog\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            strTemp = IniProg.IniReadValue("Param", "No1");//lower 
                            if (strTemp == "")
                            {
                                MessageBox.Show("检查参数");
                            }
                            double SetNo1 = Convert.ToDouble(strTemp);
                            strTemp = IniProg.IniReadValue("Param", "No2");//upper
                            if (strTemp == "")
                            {
                                MessageBox.Show("检查参数");
                            }
                            double SetNo2 = Convert.ToDouble(strTemp);
                            strTemp = IniProg.IniReadValue("Param", "L1Set");
                            if (strTemp == "")
                            {
                                MessageBox.Show("检查参数");
                            }
                            double L1Set = Convert.ToDouble(strTemp);
                            strTemp = IniProg.IniReadValue("Param", "L2Set");
                            if (strTemp == "")
                            {
                                MessageBox.Show("检查参数");
                            }
                            double L2Set = Convert.ToDouble(strTemp);
                            strTemp = IniProg.IniReadValue("Param", "W1Set");
                            if (strTemp == "")
                            {
                                MessageBox.Show("检查参数");
                            }
                            double W1Set = Convert.ToDouble(strTemp);
                            strTemp = IniProg.IniReadValue("Param", "W2Set");
                            if (strTemp == "")
                            {
                                MessageBox.Show("检查参数");
                            }
                            double W2Set = Convert.ToDouble(strTemp);

                            double dL1, dL2, dW1, dW2, dL13, dL24;
                            dL1 = (l1 - L1Set) / L1Set * 10000;
                            if (dL1 < SetNo1)
                                labelL1.BackColor = Color.PaleVioletRed;
                            else if (dL1 >= SetNo1 && dL1 <= SetNo2)
                                labelL1.BackColor = Color.Green;
                            else if (dL1 > SetNo2)
                                labelL1.BackColor = Color.Red;
                            dL2 = (l2 - L2Set) / L2Set * 10000;
                            if (dL2 < SetNo1)
                                labelL2.BackColor = Color.PaleVioletRed;
                            else if (dL2 >= SetNo1 && dL2 <= SetNo2)
                                labelL2.BackColor = Color.Green;
                            else if (dL2 > SetNo2)
                                labelL2.BackColor = Color.Red;
                            dW1 = (w1 - W1Set) / W1Set * 10000;
                            if (dW1 < SetNo1)
                                labelW1.BackColor = Color.PaleVioletRed;
                            else if (dW1 >= SetNo1 && dW1 <= SetNo2)
                                labelW1.BackColor = Color.Green;
                            else if (dW1 > SetNo2)
                                labelW1.BackColor = Color.Red;
                            dW2 = (w2 - W2Set) / W2Set * 10000;
                            if (dW2 < SetNo1)
                                labelW2.BackColor = Color.PaleVioletRed;
                            else if (dW2 >= SetNo1 && dW2 <= SetNo2)
                                labelW2.BackColor = Color.Green;
                            else if (dW2 > SetNo2)
                                labelW2.BackColor = Color.Red;
                            //////对角线////
                            double dL = Math.Sqrt(L1Set * L1Set + W1Set * W1Set);//对角线长度设置
                            dL13 = (L13 - dL) / dL * 10000;
                            //测量值除以标准值并保留5位小数
                            PublicVar.bL13 = L13 / dL;
                            PublicVar.bL24 = L24 / dL;
                            if (dL13 < SetNo1)
                                lblL13.BackColor = Color.PaleVioletRed;
                            else if (dL13 >= SetNo1 && dL13 <= SetNo2)
                                lblL13.BackColor = Color.Green;
                            else if (dL13 > SetNo2)
                                lblL13.BackColor = Color.Red;
                            dL24 = (L24 - dL) / dL * 10000;
                            if (dL24 < SetNo1)
                                lblL24.BackColor = Color.PaleVioletRed;
                            else if (dL24 >= SetNo1 && dL24 <= SetNo2)
                                lblL24.BackColor = Color.Green;
                            else if (dL24 > SetNo2)
                                lblL24.BackColor = Color.Red;

                            double dMax = 0;
                            PublicVar.W_Average = (dW1 + dW2) / 2;
                            PublicVar.L_Average = (dL1 + dL2) / 2;
                            switch (cmbClassTray.SelectedIndex)
                            {
                                case 0://X&Y
                                    //按万分比平均值分堆
                                    if (cB_AverageSort.Checked == true)
                                    {
                                        if (Math.Abs(PublicVar.W_Average) > Math.Abs(dMax)) dMax = PublicVar.W_Average;
                                        if (Math.Abs(PublicVar.L_Average) > Math.Abs(dMax)) dMax = PublicVar.L_Average;
                                    }
                                    else
                                    {
                                        if (Math.Abs(dL1) > Math.Abs(dMax)) dMax = dL1;
                                        if (Math.Abs(dL2) > Math.Abs(dMax)) dMax = dL2;
                                        if (Math.Abs(dW1) > Math.Abs(dMax)) dMax = dW1;
                                        if (Math.Abs(dW2) > Math.Abs(dMax)) dMax = dW2;
                                    }
                                    break;
                                case 1://x
                                    if (cB_AverageSort.Checked == true)
                                    {
                                        dMax = PublicVar.L_Average;
                                    }
                                    else
                                    {
                                        if (Math.Abs(dL1) > Math.Abs(dMax)) dMax = dL1;
                                        if (Math.Abs(dL2) > Math.Abs(dMax)) dMax = dL2;
                                    }
                                    break;
                                case 2://y
                                    if (cB_AverageSort.Checked == true)
                                    {
                                        dMax = PublicVar.W_Average;
                                    }
                                    else
                                    {
                                        if (Math.Abs(dW1) > Math.Abs(dMax)) dMax = dW1;
                                        if (Math.Abs(dW2) > Math.Abs(dMax)) dMax = dW2;
                                    }
                                    break;
                                case 3://XxY

                                    if (Math.Abs(dL13) > Math.Abs(dMax)) dMax = dL13;
                                    if (Math.Abs(dL24) > Math.Abs(dMax)) dMax = dL24;
                                    break;
                                case 4://X&Y&XxY
                                    if (cB_AverageSort.Checked == true)
                                    {
                                        if (Math.Abs(PublicVar.W_Average) > Math.Abs(dMax)) dMax = PublicVar.W_Average;
                                        if (Math.Abs(PublicVar.L_Average) > Math.Abs(dMax)) dMax = PublicVar.L_Average;
                                    }
                                    else
                                    {
                                        if (Math.Abs(dL1) > Math.Abs(dMax)) dMax = dL1;
                                        if (Math.Abs(dL2) > Math.Abs(dMax)) dMax = dL2;
                                        if (Math.Abs(dW1) > Math.Abs(dMax)) dMax = dW1;
                                        if (Math.Abs(dW2) > Math.Abs(dMax)) dMax = dW2;
                                    }
                                    if (Math.Abs(dL13) > Math.Abs(dMax)) dMax = dL13;
                                    if (Math.Abs(dL24) > Math.Abs(dMax)) dMax = dL24;
                                    break;
                                case 5://Y&XxY
                                    if (cB_AverageSort.Checked == true)
                                    {
                                        if (Math.Abs(PublicVar.W_Average) > Math.Abs(dMax)) dMax = PublicVar.W_Average;                                        
                                    }
                                    else
                                    {
                                        if (Math.Abs(dW1) > Math.Abs(dMax)) dMax = dW1;
                                        if (Math.Abs(dW2) > Math.Abs(dMax)) dMax = dW2;

                                    }
                                    if (Math.Abs(dL13) > Math.Abs(dMax)) dMax = dL13;
                                    if (Math.Abs(dL24) > Math.Abs(dMax)) dMax = dL24;
                                    break;
                            }
                            int iNo = 0;
                            double dXxYJudge = Convert.ToDouble(IniSetting.IniReadValue("Param", "XxYJudge"));
                            //如果对角线数值不合格，且打勾对角线检查在程序储存完数据后，跳转到报警
                            if (Math.Abs(L13 - L24) >= dXxYJudge && cB_LCheck.Checked)
                            {
                                iNo = 3;
                                txtResult.Text = iNo.ToString();
                                ////////////////////////////////////////////////////////////
                                dataGridView1.Rows.Add(1);
                                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.RowCount - 1];
                                dataGridView1.FirstDisplayedCell = dataGridView1[0, dataGridView1.RowCount - 1];
                                dataGridView1[0, dataGridView1.RowCount - 1].Value = dataGridView1.RowCount - 1;
                                dataGridView1[1, dataGridView1.RowCount - 1].Value = lblLeft.Text;
                                dataGridView1[2, dataGridView1.RowCount - 1].Value = lblRight.Text;
                                dataGridView1[3, dataGridView1.RowCount - 1].Value = lblTop.Text;
                                dataGridView1[4, dataGridView1.RowCount - 1].Value = lblBottom.Text;
                                //对边取平均值分堆
                                if (cB_AverageSort.Checked == true)
                                {
                                    dataGridView1[5, dataGridView1.RowCount - 1].Value = PublicVar.W_Average.ToString("f4");
                                    dataGridView1[6, dataGridView1.RowCount - 1].Value = PublicVar.L_Average.ToString("f4");
                                }
                                else
                                {
                                    dataGridView1[5, dataGridView1.RowCount - 1].Value = dW1.ToString("f4");
                                    dataGridView1[6, dataGridView1.RowCount - 1].Value = dW2.ToString("f4");
                                    dataGridView1[7, dataGridView1.RowCount - 1].Value = dL1.ToString("f4");
                                    dataGridView1[8, dataGridView1.RowCount - 1].Value = dL2.ToString("f4");
                                }
                                //
                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 1)&&cB_AverageSort.Checked==false)
                                {
                                    if (dL1 < SetNo1)
                                        dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dL1 >= SetNo1 && dL1 <= SetNo2)
                                        dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dL1 > SetNo2)
                                        dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                    dL2 = (l2 - L2Set) / L2Set * 10000;
                                    if (dL2 < SetNo1)
                                        dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dL2 >= SetNo1 && dL2 <= SetNo2)
                                        dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dL2 > SetNo2)
                                        dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }
                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 2)&&cB_AverageSort.Checked==false)
                                {
                                    dW1 = (w1 - W1Set) / W1Set * 10000;
                                    if (dW1 < SetNo1)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dW1 >= SetNo1 && dW1 <= SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dW1 > SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                    dW2 = (w2 - W2Set) / W2Set * 10000;
                                    if (dW2 < SetNo1)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dW2 >= SetNo1 && dW2 <= SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dW2 > SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }

                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 1) && cB_AverageSort.Checked == true)
                                {
                                    if (PublicVar.L_Average < SetNo1)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (PublicVar.L_Average >= SetNo1 && PublicVar.L_Average <= SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (PublicVar.L_Average > SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }
                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 2) && cB_AverageSort.Checked == true)
                                {
                                    if (PublicVar.W_Average < SetNo1)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (PublicVar.W_Average >= SetNo1 && PublicVar.W_Average <= SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (PublicVar.W_Average > SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }
                                ///////////////////////////////////////////////////////////////////
                                SaveDatatoDisk(iNo);
                                iStep = 1000;
                                break;
                            }
                            else
                            {
                                if (dMax < SetNo1)
                                {
                                    iNo = 0;
                                    try
                                    {
                                        strTemp = IniSetting.IniReadValue("IO", "OUT1");
                                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                                    }
                                    catch { }
                                    lblTray0.BackColor = Color.Green;
                                    lblTray1.BackColor = Color.Yellow;
                                    lblTray2.BackColor = Color.Yellow;
                                }
                                else if (dMax >= SetNo1 && dMax <= SetNo2)
                                {
                                    iNo = 1;
                                    try
                                    {
                                        strTemp = IniSetting.IniReadValue("IO", "OUT2");
                                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                                    }
                                    catch { }
                                    lblTray0.BackColor = Color.Yellow;
                                    lblTray1.BackColor = Color.Green;
                                    lblTray2.BackColor = Color.Yellow;
                                }
                                else if (dMax > SetNo2)
                                {
                                    iNo = 2;
                                    try
                                    {
                                        strTemp = IniSetting.IniReadValue("IO", "OUT3");
                                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                                    }
                                    catch { }
                                    lblTray0.BackColor = Color.Yellow;
                                    lblTray1.BackColor = Color.Yellow;
                                    lblTray2.BackColor = Color.Green;
                                }


                                if (cmbClassTray.SelectedIndex == 5)
                                {

                                    if (Math.Abs(L13 - L24) >= dXxYJudge)
                                    {
                                        iNo = 2;
                                        try
                                        {
                                            strTemp = IniSetting.IniReadValue("IO", "OUT9");//EXCEPTIION
                                            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                                        }
                                        catch { }
                                        lblTray0.BackColor = Color.Yellow;
                                        lblTray1.BackColor = Color.Yellow;
                                        lblTray2.BackColor = Color.Green;

                                    }
                                }
                                txtResult.Text = iNo.ToString();
                                ////////////////////////////////////////////////////////////
                                dataGridView1.Rows.Add(1);
                                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.RowCount - 1];
                                dataGridView1.FirstDisplayedCell = dataGridView1[0, dataGridView1.RowCount - 1];
                                dataGridView1[0, dataGridView1.RowCount - 1].Value = dataGridView1.RowCount - 1;
                                dataGridView1[1, dataGridView1.RowCount - 1].Value = lblLeft.Text;
                                dataGridView1[2, dataGridView1.RowCount - 1].Value = lblRight.Text;
                                dataGridView1[3, dataGridView1.RowCount - 1].Value = lblTop.Text;
                                dataGridView1[4, dataGridView1.RowCount - 1].Value = lblBottom.Text;
                                 //对边取平均值分堆
                                if (cB_AverageSort.Checked == true)
                                {
                                    dataGridView1[5, dataGridView1.RowCount - 1].Value = PublicVar.W_Average.ToString("f4");
                                    dataGridView1[6, dataGridView1.RowCount - 1].Value = PublicVar.L_Average.ToString("f4");
                                }
                                else
                                {
                                    dataGridView1[5, dataGridView1.RowCount - 1].Value = dW1.ToString("f4");
                                    dataGridView1[6, dataGridView1.RowCount - 1].Value = dW2.ToString("f4");
                                    dataGridView1[7, dataGridView1.RowCount - 1].Value = dL1.ToString("f4");
                                    dataGridView1[8, dataGridView1.RowCount - 1].Value = dL2.ToString("f4");
                                }
                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 1)&&cB_AverageSort.Checked==false)
                                {
                                    if (dL1 < SetNo1)
                                        dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dL1 >= SetNo1 && dL1 <= SetNo2)
                                        dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dL1 > SetNo2)
                                        dataGridView1[7, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                    dL2 = (l2 - L2Set) / L2Set * 10000;
                                    if (dL2 < SetNo1)
                                        dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dL2 >= SetNo1 && dL2 <= SetNo2)
                                        dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dL2 > SetNo2)
                                        dataGridView1[8, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }
                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 2)&&cB_AverageSort.Checked==false)
                                {
                                    dW1 = (w1 - W1Set) / W1Set * 10000;
                                    if (dW1 < SetNo1)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dW1 >= SetNo1 && dW1 <= SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dW1 > SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                    dW2 = (w2 - W2Set) / W2Set * 10000;
                                    if (dW2 < SetNo1)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (dW2 >= SetNo1 && dW2 <= SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (dW2 > SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }

                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 1) && cB_AverageSort.Checked == true)
                                {
                                    if (PublicVar.L_Average < SetNo1)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (PublicVar.L_Average >= SetNo1 && PublicVar.L_Average <= SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (PublicVar.L_Average > SetNo2)
                                        dataGridView1[6, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }
                                if ((cmbClassTray.SelectedIndex == 0 || cmbClassTray.SelectedIndex == 2) && cB_AverageSort.Checked == true)
                                {
                                    if (PublicVar.W_Average < SetNo1)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.PaleVioletRed;
                                    else if (PublicVar.W_Average >= SetNo1 && PublicVar.W_Average <= SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Green;
                                    else if (PublicVar.W_Average > SetNo2)
                                        dataGridView1[5, dataGridView1.RowCount - 1].Style.BackColor = Color.Red;
                                }
                                ///////////////////////////////////////////////////////////////////
                                SaveDatatoDisk(iNo);
                                iStep++;
                                break;
                            }
                        }

                    }
                    break;
                case 11:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 &&
                        m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < dAcurracy)
                   {
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        /////////////////////////////////////////////////////////////////////////
                        strTemp = IniSetting.IniReadValue("IO", "OUT4");//test finished
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                        Thread.Sleep(100);
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                        try
                        {
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = 0;
                            bOpen[0] = false;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            byte[] b = m_ClassCom.HexStringToByteArray(strBri);
                            com.Write(b, 0, b.Length);
                        }
                        catch { }
                        lblOK.Visible = true;
                        bMasterTest = false;
                        iStep = 1;
                        iMasterStep = 1;
                    }
                    break;
                case 100:
                    strTemp = IniSetting.IniReadValue("IO", "IN6");//cy Motor safety
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && 0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
                    {
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        
                        //添加找圆失败退回Z轴慢动作
                        double MaxSpeed = 0.1;
                        double Tacc = 0.1;
                        double Tdec = 0.1;
                        if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < 0.05)
                        {
                            MaxSpeed = PublicVar.CHCZMotorLowVel / PublicVar.CHCZMotor_Unit;
                            double dPos = (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1) / PublicVar.CHCZMotor_Unit;
                            m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                        }
                        while (Math.Abs(PublicVar.CurrentCHCZMotorPos - (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1)) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) == 0)
                        {
                            PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                            PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                        }

                        if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                              Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < 0.1)
                        {
                            MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCZMotor_Unit;
                            double dPos = PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit;
                            m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                        }
                        //
                        //CZrun = Convert.ToInt32(PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit);
                        //m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, CZrun, InitVelCZ, MotorSpeedCZ, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                        strTemp = IniSetting.IniReadValue("IO", "OUT5");//ALARM
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                        Thread.Sleep(1000);
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                        iCount = 0;
                        iCycleTime = 0;
                        iStep++;
                    }
                    break;
                case 101:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0&&
                        Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < dAcurracy && m_ClassMotion.CHCZMotorORG == true)
                    {
                        CYrun = Convert.ToInt32(PublicVar.CHCYMotorInitPos / PublicVar.CHCYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, CYrun, InitVelCY, MotorSpeedCY, PublicVar.CHCYMotorACC, PublicVar.CHCYMotorDEC);
                        iStep = 1;
                    }
                    break;
                case 1000:
                    //XY轴回初始位
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                     {
                    
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        iStep++;
                       
                     }

                    break;

                case 1001:
                    strTemp = IniSetting.IniReadValue("IO", "IN6");//cy Motor safety
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && 1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
                    
                    {
                        strTemp = IniSetting.IniReadValue("IO", "OUT5");//ALARM
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                        Thread.Sleep(1000);
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                        iCount = 0;
                        iCycleTime = 0;
                        iStep = 1;
                        timer1.Enabled = false;
                        if(MessageBox.Show("检测到该产品的对角线不合格!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information)==DialogResult.OK)
                            timer1.Enabled=true;
                        
                    }
                    break;
                case 2000://拍照失败补丁
                    
                    strTemp = IniSetting.IniReadValue("IO", "OUT5");//报警
                    m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                    Thread.Sleep(1000);
                    m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                    timer1.Enabled = false;
                    if (MessageBox.Show("是否尝试通过补偿坐标来找到Mark点?", "Mark点未找到", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        FormOffsetCorValue frmOffset = new FormOffsetCorValue(iCount, MotorX, MotorY);
                        frmOffset.ShowDialog();
                        string strPathOffset;
                        strPathOffset = System.Windows.Forms.Application.StartupPath + "\\DOC\\FormOffsetCorValue.ini";
                        CIni IniOffset = new CIni(strPathOffset);
                        string strTempOffset = "";
                        double value;
                        for (int i = 0; i <= MotorX.GetUpperBound(0); i++)
                        {
                            strTempOffset = IniOffset.IniReadValue("MotorX", i.ToString());
                            value = double.Parse(strTempOffset);
                            MotorX[i] = value;
                        }
                        for (int i = 0; i <= MotorY.GetUpperBound(0); i++)
                        {
                            strTempOffset = IniOffset.IniReadValue("MotorY", i.ToString());
                            value = double.Parse(strTempOffset);
                            MotorY[i] = value;
                        }
                        iStep = 5;
                        timer1.Enabled = true;
                        break;
                    }
                    else
                    {
                        iStep = 100;
                        timer1.Enabled = true;
                        break;
                    }
                    


            }

        }
        public double GetAverage(double a, double b)
        {
            double average = (a + b) / 2;
            return average;
        }
        public void AutoRunMasterProg()
        {
            double dAcurracy = 4;
            double InitVelX = PublicVar.CHXMotorInitVel / PublicVar.CHXMotor_Unit;
            double MotorSpeedX = PublicVar.CHXMotorSpeed / PublicVar.CHXMotor_Unit;
            double InitVelY = PublicVar.CHYMotorInitVel / PublicVar.CHYMotor_Unit;
            double MotorSpeedY = PublicVar.CHYMotorSpeed / PublicVar.CHYMotor_Unit;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            double[] MotorMasterX = new double[4];
            double[] MotorMasterY = new double[4];
            MotorMasterX[0] = PublicVar.MasterPos1X;
            MotorMasterY[0] = PublicVar.MasterPos1Y;
            MotorMasterX[1] = PublicVar.MasterPos2X;
            MotorMasterY[1] = PublicVar.MasterPos2Y;
            MotorMasterX[2] = PublicVar.MasterPos3X;
            MotorMasterY[2] = PublicVar.MasterPos3Y;
            MotorMasterX[3] = PublicVar.MasterPos4X;
            MotorMasterY[3] = PublicVar.MasterPos4Y;
            int Xrun = 0, Yrun = 0;
            switch (iMasterStep)
            {
                case 1:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHXMotorPos - PublicVar.CHXMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHYMotorPos - PublicVar.CHYMotorInitPos) < dAcurracy)
                    {
                        iStep = 0;
                        TestX = new double[4];
                        TestY = new double[4];
                        TestOffsetX = new double[4];
                        TestOffsetY = new double[4];
                        iCount = 0;
                        try
                        {
                            strTemp = IniSetting.IniReadValue("IO", "IN1");
                            if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)) || bButton == true)
                            {
                                bButton = false;
                                btnTest.BackColor = SystemColors.Control;
                                lblTop.Text = "---";
                                lblRight.Text = "---";
                                lblLeft.Text = "---";
                                lblBottom.Text = "---";
                                txtResult.Text = "---";
                                iCycleTime = 0;
                                iMasterStep++;
                            }
                        }
                        catch { }
                    }
                    break;
                case 2:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        dXPreCurrent = PublicVar.CurrentCHXMotorPos;
                        dYPreCurrent = PublicVar.CurrentCHYMotorPos;
                        Xrun = Convert.ToInt32(MotorMasterX[iCount] / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(MotorMasterY[iCount] / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        //////////////////////////////////////////////////////////////////////////
                        try
                        {
                            int iLight = Convert.ToInt16(IniSetting.IniReadValue("Master", "Light"));
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = iLight;
                            bOpen[0] = true;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            com.Write(strBri);
                        }
                        catch { }
                        iMasterStep++;
                    }
                    break;
                case 3:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHXMotorPos - MotorMasterX[iCount]) < dAcurracy && Math.Abs(PublicVar.CurrentCHYMotorPos - MotorMasterY[iCount]) < dAcurracy)
                    {
                        int iDelay = Convert.ToInt16(IniSetting.IniReadValue("Param", "Delay"));
                        Thread.Sleep(iDelay);
                        GrabC(0);
                        double Xm, Ym;
                        PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit ;
                        PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                        Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                        Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                        int iThreshold = 0;
                        try
                        {
                            iThreshold = Convert.ToInt16(IniSetting.IniReadValue("Master", "Threshold"));
                            numericUpDownThreshold.Value = iThreshold;
                        }
                        catch { }
                        double cR = Convert.ToDouble(IniSetting.IniReadValue("Master", "PointDia"))/2;
                        CircleParam m_CircleParam = FindCircluarEdge(ThresholdImage(iThreshold, ImgView.Image, false), cR / PublicVar.CameraX_Unit, ImgView.Image.Width / 2, ImgView.Image.Height / 2, 100, 1, 2);
                        if (m_CircleParam.X > 0 && m_CircleParam.Y > 0)
                        {
                            double dJudge = Convert.ToDouble(IniSetting.IniReadValue("Param", "Judge"));
                            double OffsetX, OffsetY;
                            OffsetX = -(m_CircleParam.X - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                            OffsetY = -(m_CircleParam.Y - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                            if (Math.Abs(OffsetX) > dJudge || Math.Abs(OffsetY) > dJudge)
                            {
                                Xrun = Convert.ToInt32((PublicVar.CurrentCHXMotorPos + OffsetX) / PublicVar.CHXMotor_Unit);
                                Yrun = Convert.ToInt32((PublicVar.CurrentCHYMotorPos + OffsetY) / PublicVar.CHYMotor_Unit);
                                m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                                m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                            }
                            else
                            {
                                double[] xy = Rdata.RepairDataXY(System.Windows.Forms.Application.StartupPath, dXPreCurrent, dYPreCurrent, PublicVar.CurrentCHXMotorPos, PublicVar.CurrentCHYMotorPos,
                                                                PublicVar.RepairIntervalX, PublicVar.RepairIntervalY, PublicVar.RepairStartX, PublicVar.RepairStartY);
                                Xm = xy[0];
                                Ym = xy[1];
                                TestOffsetX[iCount] = OffsetX;
                                TestOffsetY[iCount] = OffsetY;
                                TestX[iCount] = Xm + OffsetX;
                                TestY[iCount] = Ym + OffsetY;
                                iMasterStep++;
                            }
                        }
                        else
                            iMasterStep = 100;
                    }
                    break;
                case 4:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        iCount++;
                        if (iCount >= 4)
                            iMasterStep = 5;
                        else
                            iMasterStep = 2;
                        if (iCount >= 4 && cmbMasterLine.SelectedIndex == 0)
                            iMasterStep = 5;
                        if (iCount >= 4 && bFirst == false && cmbMasterLine.SelectedIndex == 2)
                            iMasterStep = 5;
                        lBlNo.Text = iCount.ToString();
                    }
                    break;
                case 5:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        ////////////////////////////////////////////////////
                        CorrTrans4Point(TestX, TestY, 0);
                        iMasterStep++;
                    }
                    break;
                case 6:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        int m = Convert.ToInt16(txtCycleTime.Text);
                        double w1 = 0, w2 = 0, l1 = 0, l2 = 0, L13 = 0, L24 = 0;
                        for (int i = 0; i < m; i++)
                        {
                            w1 += m_ResultData[i].W1;
                            w2 += m_ResultData[i].W2;
                            l1 += m_ResultData[i].L1;
                            l2 += m_ResultData[i].L2;
                            L13 += m_ResultData[i].L13;
                            L24 += m_ResultData[i].L24;
                        }
                        l1 /= m;
                        l2 /= m;
                        w1 /= m;
                        w2 /= m;
                        L13 /= m;
                        L24 /= m;
                        lblTop.Text = l1.ToString("f4");
                        lblBottom.Text = l2.ToString("f4");
                        lblLeft.Text = w1.ToString("f4");
                        lblRight.Text = w2.ToString("f4");
                        lblL13.Text = L13.ToString("f4");
                        lblL24.Text =L24.ToString("f4");
                        //////////////////////////////////////////////////////
                        PublicVar.CHXCaliCorr = (PublicVar.MasterL1 / l1 + PublicVar.MasterL2 / l2) / 2;
                        PublicVar.CHYCaliCorr = (PublicVar.MasterW1 / w1 + PublicVar.MasterW2 / w2) / 2;
                        IniSetting.IniWriteValue("CHX", "CaliCorr", PublicVar.CHXCaliCorr.ToString("f8"));
                        //IniSetting.IniWriteValue("CHX", "RepairAngle", PublicVar.CHXRepairAngle .ToString ("f5"));
                        IniSetting.IniWriteValue("CHY", "CaliCorr", PublicVar.CHYCaliCorr.ToString("f8"));
                        //IniSetting.IniWriteValue("CHY", "RepairAngle", PublicVar.CHYRepairAngle.ToString("f5"));
                        iMasterStep++;
                    }
                    break;
                case 7:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        try
                        {
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = 0;
                            bOpen[0] = false;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            byte[] b = m_ClassCom.HexStringToByteArray(strBri);
                            com.Write(b, 0, b.Length);
                        }
                        catch { }
                        bMasterTest = true;
                        lblOK.Visible = true;
                        bButton = true;
                        iStep = 1;
                    }
                    break;
                case 100:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        strTemp = IniSetting.IniReadValue("IO", "OUT5");//ALARM
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                        Thread.Sleep(1000);
                        m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                        iCount = 0;
                        iCycleTime = 0;
                        iMasterStep = 1;
                    }
                    break;
            }
        }
        public void AutoRepairRun()
        {
            double dAcurracy = 0.1;
            double InitVelX = PublicVar.CHXMotorInitVel / PublicVar.CHXMotor_Unit;
            double MotorSpeedX = PublicVar.CHXMotorSpeed / PublicVar.CHXMotor_Unit;
            double InitVelY = PublicVar.CHYMotorInitVel / PublicVar.CHYMotor_Unit;
            double MotorSpeedY = PublicVar.CHYMotorSpeed / PublicVar.CHYMotor_Unit;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";

            int Xrun = 0, Yrun = 0;
            switch (iStep)
            {
                case 1:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 &&
                        Math.Abs(PublicVar.CurrentCHXMotorPos - PublicVar.CHXMotorInitPos) < dAcurracy && Math.Abs(PublicVar.CurrentCHYMotorPos - PublicVar.CHYMotorInitPos) < dAcurracy)
                    {
                        lblOK.Visible = false;
                        iXCountSet =Convert .ToInt16 ( PublicVar.StandardPanelLength / PublicVar.RepairIntervalX)+1;
                        iYCountSet = Convert.ToInt16(PublicVar.StandardPanelWidth  / PublicVar.RepairIntervalY)+1;
                        TestX = new double[iXCountSet+1 ];
                        TestY = new double[iXCountSet +1];
                        TestOffsetX = new double[iXCountSet +1];
                        TestOffsetY = new double[iXCountSet +1];
                        iXCount = 0;
                        iYCount = 0;
                        try
                        {
                            strTemp = IniSetting.IniReadValue("IO", "IN1");
                            if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)) || bButton == true)
                            {
                                bButton = false;
                                btnTest.BackColor = SystemColors.Control;
                                /////////////////////////////////
                                strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                                CIni IniProg = new CIni(strPath);
                                int iLight = Convert.ToInt16(IniProg.IniReadValue("Image", "Light0"));
                                int[] iBri = new int[4];
                                bool[] bOpen = new bool[4];
                                iBri[0] = iLight;
                                bOpen[0] = true;
                                iBri[1] = 0;
                                bOpen[1] = false;
                                iBri[2] = 0;
                                bOpen[2] = false;
                                iBri[3] = 0;
                                bOpen[3] = false;
                                string strBri = "";
                                strBri = m_ClassCom.SendStr(iBri, bOpen);
                                com.Write(strBri);
                                iStep++;
                            }
                        }
                        catch { }
                    }
                    break;
                case 2:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        Xrun = Convert.ToInt32((PublicVar .RepairStartX +iXCount *PublicVar .RepairIntervalX ) / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32((PublicVar.RepairStartY + iYCount * PublicVar.RepairIntervalY) / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        iStep++;
                    }
                    break;
                case 3:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        int iDelay = Convert.ToInt16(IniSetting.IniReadValue("Param", "Delay"));
                        Thread.Sleep(iDelay);
                        GrabC(0);
                        int iThreshold = 0;
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            iThreshold = Convert.ToInt16(IniProg.IniReadValue("Image", "Threshold" + PublicVar.iLedSel.ToString()));
                            numericUpDownThreshold.Value = iThreshold;
                        }
                        catch { }
                        bool bflag;
                        if (Colors[iCount] == 1)
                            bflag = false;
                        else
                            bflag = true;
                        CircleParam m_CircleParam = ImageProcess1(ImgView.Image, iThreshold, true, Convert.ToDouble(txtCircleDia.Text.Trim()) / 2, bflag);
                        if (m_CircleParam.X > 0 && m_CircleParam.Y > 0)
                        {
                            double dJudge = Convert.ToDouble(IniSetting.IniReadValue("Param", "Judge"));
                            double OffsetX, OffsetY;
                            OffsetX = -(m_CircleParam.X - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                            OffsetY = -(m_CircleParam.Y - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                            if (Math.Abs(OffsetX) > dJudge || Math.Abs(OffsetY) > dJudge)
                            {

                                Xrun = Convert.ToInt32((PublicVar.CurrentCHXMotorPos + OffsetX) / PublicVar.CHXMotor_Unit);
                                Yrun = Convert.ToInt32((PublicVar.CurrentCHYMotorPos + OffsetY) / PublicVar.CHYMotor_Unit);
                                m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                                m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                            }
                            else
                            {
                                double Xm, Ym;
                                PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                                PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                                Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                                Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                                Xm = Xm * PublicVar.CHXCaliCorr;
                                Ym = Ym * PublicVar.CHYCaliCorr;
                                TestOffsetX[iXCount] = OffsetX;
                                TestOffsetY[iXCount] = OffsetY;
                                TestX[iXCount] = Xm + OffsetX;
                                TestY[iXCount] = Ym + OffsetY;
                                iStep++;
                            }
                        }
                        else
                        {
                            TestX[iXCount] =0;
                            TestY[iXCount] =0;
                            iStep++;
                        }
                    }
                    break;
                case 4:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        iXCount++;
                        lBlNo.Text = (iXCountSet * iYCount + iXCount).ToString();
                        if (iXCount >= iXCountSet)
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\RepairDataMachine";
                            if (!Directory.Exists(strPath))
                                Directory.CreateDirectory(strPath);
                            string strFile = strPath + "\\X.dat";
                            StreamWriter swX = new StreamWriter(strFile, (iYCount == 0 ? false : true));
                            string str1 = "";
                            try
                            {
                                for (int i = 0; i < iXCountSet; i++)
                                {
                                    str1 += TestX[i].ToString("f4") + ",";
                                }
                            }
                            catch { }
                            str1.Substring(0, str1.Length - 1);
                            swX.WriteLine(str1);
                            swX.Close();
                            strFile = strPath + "\\Y.dat";
                            StreamWriter swY = new StreamWriter(strFile, (iYCount == 0 ? false : true));
                            str1 = "";
                            try
                            {
                                for (int i = 0; i < iXCountSet; i++)
                                {
                                    str1 += TestY[i].ToString("f4") + ",";
                                }
                            }
                            catch { }
                            str1.Substring(0, str1.Length - 1);
                            swY.WriteLine(str1);
                            swY.Close();
                            /////////////////////////////////
                            iYCount++;
                            iXCount = 0;
                        }
                        if (iYCount < iYCountSet)
                            iStep = 2;
                        else
                            iStep++;
                    }
                    break;
                case 5:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        //timer1.Enabled = false;
                        //if (MessageBox.Show("继续吗？", "HHIAT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{//反向补偿
                        //    timer1.Enabled = true;
                        //    iXCount = 0;
                        //    iYCount = 0;
                        //    iStep++;
                        //}
                        //else
                            iStep = 10;
                    }
                    break;
                case 6:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        Xrun = Convert.ToInt32((PublicVar.RepairStartX +(iXCountSet - iXCount) * PublicVar.RepairIntervalX) / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32((PublicVar.RepairStartY +(iYCountSet - iYCount) * PublicVar.RepairIntervalY) / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        iStep++;
                    }
                    break;
                case 7:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0 )
                    {
                        int iDelay = Convert.ToInt16(IniSetting.IniReadValue("Param", "Delay"));
                        Thread.Sleep(iDelay);
                        GrabC(0);
                        int iThreshold = 0;
                        try
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            CIni IniProg = new CIni(strPath);
                            iThreshold = Convert.ToInt16(IniProg.IniReadValue("Image", "Threshold" + PublicVar.iLedSel.ToString()));
                            numericUpDownThreshold.Value = iThreshold;
                        }
                        catch { }
                        bool bflag;
                        if (Colors[iCount] == 1)
                            bflag = false;
                        else
                            bflag = true;
                        CircleParam m_CircleParam = ImageProcess1(ImgView.Image, iThreshold, true, R[iCount], bflag);
                        if (m_CircleParam.X > 0 && m_CircleParam.Y > 0)
                        {
                            double dJudge = Convert.ToDouble(IniSetting.IniReadValue("Param", "Judge"));
                            double OffsetX, OffsetY;
                            OffsetX = -(m_CircleParam.X - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                            OffsetY = -(m_CircleParam.Y - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                            if (Math.Abs(OffsetX) > dJudge || Math.Abs(OffsetY) > dJudge)
                            {

                                Xrun = Convert.ToInt32((PublicVar.CurrentCHXMotorPos + OffsetX) / PublicVar.CHXMotor_Unit);
                                Yrun = Convert.ToInt32((PublicVar.CurrentCHYMotorPos + OffsetY) / PublicVar.CHYMotor_Unit);
                                m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                                m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                            }
                            else
                            {
                                double Xm, Ym;
                                PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                                PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                                Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                                Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                                Xm = Xm * PublicVar.CHXCaliCorr;
                                Ym = Ym * PublicVar.CHYCaliCorr;
                                TestOffsetX[ iXCount] = OffsetX;
                                TestOffsetY[ iXCount] = OffsetY;
                                TestX[iXCount] = Xm + OffsetX;
                                TestY[iXCount] = Ym + OffsetY;
                                iStep++;
                            }
                        }
                        else
                            iStep ++;
                    }
                    break;
                case 8:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        iXCount++;
                        lBlNo.Text = (iXCountSet * iYCount + iXCount).ToString();
                        if (iXCount >= iXCountSet)
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\RepairDataMachine";
                            if (!Directory.Exists(strPath))
                                Directory.CreateDirectory(strPath);
                            string strFile = strPath + "\\_X.dat";
                            StreamWriter swX = new StreamWriter(strFile, (iYCount == 0 ? false : true));
                            string str1 = "";
                            try
                            {
                                for (int i = 0; i < iXCountSet; i++)
                                {
                                    str1 += TestX[i].ToString("f4") + ",";
                                }
                            }
                            catch { }
                            str1.Substring(0, str1.Length - 1);
                            swX.WriteLine(str1);
                            swX.Close();
                            strFile = strPath + "\\_Y.dat";
                            StreamWriter swY = new StreamWriter(strFile, (iYCount == 0 ? false : true));
                            str1 = "";
                            try
                            {
                                for (int i = 0; i < iXCountSet; i++)
                                {
                                    str1 += TestY[i].ToString("f4") + ",";
                                }
                            }
                            catch { }
                            str1.Substring(0, str1.Length - 1);
                            swY.WriteLine(str1);
                            swY.Close();
                            ///////
                            iYCount++;
                            iXCount = 0;
                        }
                        if (iYCount < iYCountSet)
                            iStep = 6;
                        else
                            iStep++;
                    }
                    break;
                case 9:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        iStep++;
                    }
                    break;
                case 10:
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                        Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                        try
                        {
                            int[] iBri = new int[4];
                            bool[] bOpen = new bool[4];
                            iBri[0] = 0;
                            bOpen[0] = false;
                            iBri[1] = 0;
                            bOpen[1] = false;
                            iBri[2] = 0;
                            bOpen[2] = false;
                            iBri[3] = 0;
                            bOpen[3] = false;
                            string strBri = "";
                            strBri = m_ClassCom.SendStr(iBri, bOpen);
                            byte[] b = m_ClassCom.HexStringToByteArray(strBri);
                            com.Write(b, 0, b.Length);
                        }
                        catch { }
                        iStep = 0;
                        ManyPointToolStripMenuItem.Checked = false;
                        MessageBox.Show("OK");
                    }
                    break;
            }
        }
        public FrmMain()
        {
            InitializeComponent();

        }

        private void InitFrmWelcome()
        {
            FrmWelcome frmwel = new FrmWelcome();
            frmwel.Show();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {


            //FrmWelcome frmwel = new FrmWelcome();
            //frmwel.ShowDialog();
            this.Text = "基材涨缩分堆专用分拣机---广东华恒智能科技有限公司  ";
            tabControl1.SelectedIndex = 2;
            ExcelInit();
            this.Top = 0;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - this.Width / 2;
            dataGridView1.RowCount = 10;
            dataGridView1.ColumnCount = 13;
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 20;
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            for (int i = 2; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Width = (dataGridView1.Width-100) / 4;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView2.RowCount = 1;
            dataGridView2.ColumnCount = 12;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].Visible = false;
            dataGridView2.Columns[3].Visible = false;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Visible = false;
            dataGridView2.Columns[6].Visible = false;
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                dataGridView2.Columns[i].Width = dataGridView2.Width / dataGridView2.ColumnCount;
            }
            //dataGridView2.Columns[0].HeaderText = "NO";
            //dataGridView2.Columns[1].HeaderText = "NO";
            dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable; 
            dataGridView2.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable; 

            dataGridView2[0, 0].Value = "NO.";
            dataGridView2.Columns[0].Width = 50;
            dataGridView2[1, 0].Value = "CenterX";
            dataGridView2[2, 0].Value = "CenterY";
            dataGridView2[3, 0].Value = "InnerR";
            dataGridView2[4, 0].Value = "OuterR";
            dataGridView2[5, 0].Value = "StartA";
            dataGridView2[6, 0].Value = "EndA";
            dataGridView2[7, 0].Value = "MotorX";
            dataGridView2.Columns[7].Width = 100;
            dataGridView2[8, 0].Value = "MotorY";
            dataGridView2.Columns[8].Width = 100;
            dataGridView2[9, 0].Value = "R";
            dataGridView2.Columns[9].Width = 100;
            dataGridView2[10, 0].Value = "Color";
            dataGridView2.Columns[10].Width = 50;
            dataGridView2[11, 0].Value = "Type";
            dataGridView2.Columns[11].Width = 50;
            LoadFileNametoCombo(cmbProductName);
            try
            {
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\CurrentProduct.ini";
                CIni IniCurrProd = new CIni(strPath);
                cmbProductName.Text = IniCurrProd.IniReadValue("CurrentProduct", "Name");
            }
            catch { }
            try
            {
                ReadParam();
                CameraInit();
                
            }
            catch { MessageBox.Show("初始化相机失败"); }
            m_ClassMotion = new ClassMotion(true);
            m_ClassCom = new ClassCom();
            m_ClassCom.sVendor = "0";
            com.BaudRate = 19200;
            com.PortName = "COM1";
            if (!com.IsOpen)
            {
                com.Open();
                Thread.Sleep(50);
            }
            numericUpDownLightC_ValueChanged(sender, e);
            try
            {
                 int[] iBri = new int[4];
                bool[] bOpen = new bool[4];
                iBri[0] = 50;
                bOpen[0] = true;
                iBri[1] = 50;
                bOpen[1] = true;
                iBri[2] = 50;
                bOpen[2] = true;
                iBri[3] = 0;
                bOpen[3] = false;
                string strBri = "";
                strBri = m_ClassCom.SendStr(iBri, bOpen);
                com.Write(strBri);
            }
            catch { }
            if (sDebug == "0")
            {
                MachinReplaceAll();
                Thread.Sleep(1000);
            }
            else
                MessageBox.Show("机器处于调试模式，不要自动运行！");
            bAuto = false;
            if (sDebug == "1" && m_ClassMotion.CHCZMotorEmg)
            {
                timer1.Enabled = false;
                ToolBar1.Items[4].Enabled = true;
                ToolBar1.Items[5].Enabled = false;
                chkZSafetysn.Enabled = true;
            }
            else
            {
                timer1.Enabled = true;
                ToolBar1.Items[4].Enabled = false;
                ToolBar1.Items[5].Enabled = true;
                chkZSafetysn.Enabled = false;
            }
            listBoxColor.SelectedIndex = 0;
            
            //屏蔽相关不需要的功能

            CheckWarranty();//检查保修期限

        }

        private void btnPtoP_Click(object sender, EventArgs e)
        {
            iSelectRow = 0;
            ClassPublicTool.m_CalcuTool = CalcuTool.PointToPoint;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = false ;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = true;
            btnLine.Enabled = true;
            btnPoint.Enabled = true;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = true;
        }

        private void btnLtoL_Click(object sender, EventArgs e)
        {
            iSelectRow = 0;
            ClassPublicTool.m_CalcuTool = CalcuTool.LineToLine;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = false;
            btnCtoC.Enabled = true;
            btnLine.Enabled = true;
            btnPoint.Enabled = true;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = true;
        }

        private void btnCtoC_Click(object sender, EventArgs e)
        {
            iSelectRow = 0;
            ClassPublicTool.m_CalcuTool = CalcuTool.CircleToCirlce;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = false;
            btnLine.Enabled = true;
            btnPoint.Enabled = true;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = true;
        }
        private void btnPtoL_Click(object sender, EventArgs e)
        {
            iSelectRow = 0;
            ClassPublicTool.m_CalcuTool = CalcuTool.PointToLine;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = false;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = true;
            btnLine.Enabled = true;
            btnPoint.Enabled = true;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = true;
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            ClassPublicTool.m_CalcuTool = CalcuTool.Line ;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = true;
            btnLine.Enabled = false;
            btnPoint.Enabled = true;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = true;
            x1 = 0;
            y1 = 0;
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            ClassPublicTool.m_CalcuTool = CalcuTool.Point ;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = true;
            btnLine.Enabled = true;
            btnPoint.Enabled = false;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = true;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            ClassPublicTool.m_CalcuTool = CalcuTool.Circle ;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = true;
            btnLine.Enabled = true;
            btnPoint.Enabled = true;
            btnCircle.Enabled = false;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = true;
            x1 = 0;
            y1 = 0;
        }

        private void btnFittingCircle_Click(object sender, EventArgs e)
        {
            iManyFitPointCount = 0;
            ClassPublicTool.m_CalcuTool = CalcuTool.ManyPointtoCircle;
            FrmSetPoint frm = new FrmSetPoint();
            frm.ShowDialog();
            ManyFitPoint = new PointContour[ClassPublicTool.ManyPointNum];
            lblDisplayPoints.Visible = true;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = true;
            btnLine.Enabled = true;
            btnPoint.Enabled = true;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = false;
            BtnSelect.Enabled = true;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            iManyFitPointCount = 0;
            ClassPublicTool.m_CalcuTool = CalcuTool.None ;
            lblDisplayPoints.Visible = false;
            btnPtoP.Enabled = true;
            btnPtoL.Enabled = true;
            btnLtoL.Enabled = true;
            btnCtoC.Enabled = true;
            btnLine.Enabled = true;
            btnPoint.Enabled = true;
            btnCircle.Enabled = true;
            btnFittingCircle.Enabled = true;
            BtnSelect.Enabled = false;
            ImgView.Image.Overlays.Default.Clear();
        }

        private void ToolBar1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double InitVelX = PublicVar.CHXMotorInitVel / PublicVar.CHXMotor_Unit;
            double MotorSpeedX = PublicVar.CHXMotorSpeed / PublicVar.CHXMotor_Unit;
            double InitVelY = PublicVar.CHYMotorInitVel / PublicVar.CHYMotor_Unit;
            double MotorSpeedY = PublicVar.CHYMotorSpeed / PublicVar.CHYMotor_Unit;
            int Xrun = 0, Yrun = 0;
            double InitVelCY = PublicVar.CHCYMotorInitVel / PublicVar.CHCYMotor_Unit;
            double MotorSpeedCY = PublicVar.CHCYMotorSpeed / PublicVar.CHCYMotor_Unit;
            double InitVelCZ = PublicVar.CHCZMotorInitVel / PublicVar.CHCZMotor_Unit;
            double MotorSpeedCZ = PublicVar.CHCZMotorSpeed / PublicVar.CHCZMotor_Unit;
            int CYrun = 0, CZrun = 0;
            double dCr;
            ImgView.Image.Overlays.Default.Clear();
            ImgView.Roi.Clear();
            string strFile = "";
            switch (e.ClickedItem.Text)
            {
                case "运 行":
                    if (cmbProductName.Text == "")
                    {
                        MessageBox.Show("请选择产品");
                        return;
                    }
                    //解决读取参数错误问题，如果读取参数有问题，那么就return;
                    if (!ReadParam())
                    {

                        return;

                    }                    
                    ToolCmbMode.SelectedIndex = 1;
                    strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    strPath += "\\Prog";
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    strPath += "\\" + cmbProductName.Text.Trim()+".ini";
                    CIni IniProg = new CIni(strPath);
                    string strTemp = "";
                    strTemp=IniProg.IniReadValue("Total", "Count");//初始化Mark点的个数
                    int iTotal = Convert.ToInt16(strTemp);
                    Sequence = new int[iTotal];
                    CenterX = new double[iTotal];
                    CenterY = new double[iTotal];
                    InnerR = new double[iTotal];
                    OuterR = new double[iTotal];
                    StartA = new double[iTotal];
                    EndA = new double[iTotal];
                    MotorX = new double[iTotal];
                    MotorY = new double[iTotal];
                    R = new double[iTotal];
                    Colors  = new int[iTotal];
                    Types  = new int[iTotal];
                   for (int i = 1; i <= iTotal; i++)
                    {
                        strTemp = IniProg.IniReadValue(i.ToString(), "NO.");
                        Sequence[i-1] = Convert.ToInt16(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "CenterX");
                        CenterX[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "CenterY");
                        CenterY[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "InnerR");
                        InnerR[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "OuterR");
                        OuterR[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "StartA");
                        StartA[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "EndA");
                        EndA[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "MotorX");
                        MotorX[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "MotorY");
                        MotorY[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "R");
                        R[i - 1] = Convert.ToDouble(strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "Color");
                        Colors[i - 1] = Convert.ToInt16 (strTemp);
                        strTemp = IniProg.IniReadValue(i.ToString(), "Type");
                        Types[i - 1] = Convert.ToInt16(strTemp);
                    }
                    strTemp = IniProg.IniReadValue("Param", "L1Set");
                    txtL1Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "L2Set");
                    txtL2Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "W1Set");
                    txtW1Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "W2Set");
                    txtW2Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "No1");
                    txtLowerLimit.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "No2");
                    txtUpperLimit.Text = strTemp;
                    try
                    {
                        strTemp = IniProg.IniReadValue("RefAngle", "L1Set");
                        txtL1AngleSet.Text = strTemp;
                        strTemp = IniProg.IniReadValue("RefAngle", "L2Set");
                        txtL2AngleSet.Text = strTemp;
                        strTemp = IniProg.IniReadValue("RefAngle", "W1Set");
                        txtW1AngleSet.Text = strTemp;
                        strTemp = IniProg.IniReadValue("RefAngle", "W2Set");
                        txtW2AngleSet.Text = strTemp;
                        strTemp = IniProg.IniReadValue("CircleDia", "Mark");
                        txtCircleDia.Text = strTemp;
                        strTemp = IniProg.IniReadValue("CircleDia", "Ref");
                        txtRefCircleDia.Text = strTemp;
                    }
                    catch { MessageBox.Show("运行错误！"); }
                try
                {
                    txtPosCameraSetX .Text  = IniProg.IniReadValue("PosCameraSet", "X");
                    txtPosCameraSetY .Text  = IniProg.IniReadValue("PosCameraSet", "Y");
                    txtPosCameraSetAngle .Text  = IniProg.IniReadValue("PosCameraSet", "Angle");
                }
                catch { MessageBox.Show("运行错误！"); }
                    //////////////////////////////////////////////
                if (iTotal == 9)//测量数为9点时显示
                {
                    dataGridView1.RowCount = 1;
                    dataGridView1.ColumnCount = 13;
                    dataGridView1.Columns[0].Width = 30;
                    for (int i = 1; i < dataGridView1.ColumnCount; i++)
                    {
                        dataGridView1.Columns[i].Width = (dataGridView1.Width-30)/12;
                    }
                    dataGridView1[0, 0].Value = "序号";
                    dataGridView1[1, 0].Value = "L1";
                    dataGridView1[2, 0].Value = "L2";
                    dataGridView1[3, 0].Value = "L3";
                    dataGridView1[4, 0].Value = "L4";
                    dataGridView1[5, 0].Value = "L5";
                    dataGridView1[6, 0].Value = "L6";
                    dataGridView1[7, 0].Value = "L7";
                    dataGridView1[8, 0].Value = "L8";
                    dataGridView1[9, 0].Value = "L9";
                    dataGridView1[10, 0].Value = "L10";
                    dataGridView1[11, 0].Value = "L11";
                    dataGridView1[12, 0].Value = "L12";

                }
                else if(iTotal ==4)//4点时显示
                {
                    
                    dataGridView1.RowCount = 1;
                    dataGridView1.ColumnCount = 9;
                    dataGridView1.Columns[0].Width = 30;
                    for (int i = 1; i < dataGridView1.ColumnCount; i++)
                    {
                        dataGridView1.Columns[i].Width = (dataGridView1.Width - 40) / 8;
                    }
                    dataGridView1[0, 0].Value = "序号";
                    dataGridView1[1, 0].Value = "Y1";
                    dataGridView1[2, 0].Value = "Y2";
                    dataGridView1[3, 0].Value = "X1";
                    dataGridView1[4, 0].Value = "X2";
                    if (cB_AverageSort.Checked == false)
                    {
                        dataGridView1[5, 0].Value = "Y1_P";
                        dataGridView1[6, 0].Value = "Y2_P";
                        dataGridView1[7, 0].Value = "X1_P";
                        dataGridView1[8, 0].Value = "X2_P";
                    }
                    else
                    {
                        dataGridView1[5, 0].Value = "Y_Average";
                        dataGridView1[6, 0].Value = "X_Average";
                    }

                }
                   //////////////////////////////////////////////
                   //ToolCmbMode.SelectedIndex = 1;
                   bAuto = true;
                   iStep = 1;
                   iMasterStep = 1;
#region Check Vaild Time
                   CIni ReadMac = new CIni("c:\\hhiatsn.ini");
                    string sTime = ReadMac.IniReadValue("Time", "T");
                    if (sTime != "-1")
                    {
                        DateTime datetime = DateTime.Now;
                        string[] d = new string[5];
                        d[0] = sTime.Substring(0, 2);//day
                        d[1] = sTime.Substring(2, 2);//month
                        d[2] = sTime.Substring(4, 2);//year_L
                        d[3] = sTime.Substring(6, 2);//len
                        d[4] = sTime.Substring(8, 2);//Year_R
                        DateTime datetimeStart = new DateTime(Convert.ToInt16(d[2] + d[4]), Convert.ToInt16(d[1]), Convert.ToInt16(d[0]), 0, 0, 0);
                        int iday = Convert.ToInt16(d[3]) * 30 - (datetime - datetimeStart).Days;

                        if (iday < 5)
                        {
                            string strExe = System.Windows.Forms.Application.StartupPath + "\\AppPassword.exe";
                            m_ClassMotion.WinExe(strExe);
                        }
                    }
#endregion
                    ///////////////////////////////////
                    Xrun = Convert.ToInt32(PublicVar.CHXMotorInitPos / PublicVar.CHXMotor_Unit);
                    Yrun = Convert.ToInt32(PublicVar.CHYMotorInitPos / PublicVar.CHYMotor_Unit);
                    m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                    m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);

                    CZrun = Convert.ToInt32(PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit);
                    m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, CZrun, 0, MotorSpeedCY, PublicVar.CHCZMotorACC, PublicVar.CHCZMotorDEC);
                    
                    while (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) == 0)
                    {
                        PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                        PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                    }
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        Math .Abs (PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos)<0.1&&m_ClassMotion .CHCZMotorORG ==true)
                    {
                        CYrun = Convert.ToInt32(PublicVar.CHCYMotorInitPos / PublicVar.CHCYMotor_Unit);
                        m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, CYrun, 0, MotorSpeedCY, PublicVar.CHCYMotorACC, PublicVar.CHCYMotorDEC);
                    }
                    while (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) == 0) 
                    {
                        PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                        PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                    }
                    if (ExcelApp == null)
                        ExcelInit();
                   ///////////////////////////////////////////
                    bStop = false;
                    tabControl1.SelectedIndex = 3;
                    iCount = 0;
                    iCycleTime = 0;
#region Hide Buttons
                    ToolBar1.Items[0].Enabled = false;
                    ToolBar1.Items[1].Enabled = true;
                    ToolBar1.Items[2].Enabled = false;
                    ToolBar1.Items[3].Enabled = false;
                    ToolBar1.Items[4].Enabled = false;
                    ToolBar1.Items[5].Enabled = false;
                    ToolBar1.Items[6].Enabled = false;
                    ToolBar1.Items[7].Enabled = false;
                    ToolBar1.Items[8].Enabled = false;
                    ToolBar1.Items[9].Enabled = false;
                    ToolBar1.Items[10].Enabled = false;
                    ToolBar1.Items[11].Enabled = false;
                    ToolBar1.Items[12].Enabled = false;
                    BtnLeft.Enabled = false;
                    BtnRight.Enabled = false;
                    BtnRst.Enabled = false;
                    BtnUp.Enabled = false;
                    BtnDown.Enabled = false;
                    chkThreshold.Enabled = false;
                    listBoxColor.Enabled = false;
                    chkCalibration.Enabled = false;
                    //numericUpDownLightC.Enabled = false;
                    //numericUpDownThreshold.Enabled = false;
                    cmbProductName.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox3.Enabled = false;
                    btnParamEdit.Enabled = false;
                    btnParamSave.Enabled = false;
                    btnDelete.Enabled = false;
#endregion
                    int m=Convert.ToInt16 (txtCycleTime .Text);//读取产品参数循环测量次数
                    m_ResultData=new ResultData[m];
                    timer1.Enabled = true;                   
                    
                    if (bFirst == false && cmbMasterLine.SelectedIndex == 2 && MessageBox.Show("测量基准点吗？", "输入角度方式", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        bFirst = true;
                    if (iTotal < 8)
                    {
                        lbNo1.Visible = true;
                        lbNo1.Text = "1";
                        lbNo2.Visible = false;
                        lbNo2.Text = "";
                        lbNo3.Visible = true;
                        lbNo3.Text = "4";
                        lbNo4.Visible = false;
                        lbNo4.Text = "";
                        lbNo5.Visible = true;
                        lbNo5.Text = "FPC";
                        lbNo6.Visible = false;
                        lbNo6.Text = "";
                        lbNo7.Visible = true;
                        lbNo7.Text = "2";
                        lbNo8.Visible = false;
                        lbNo8.Text = "";
                        lbNo9.Visible = true;
                        lbNo9.Text = "3";
                    }
                    else
                    {
                        lbNo1.Visible = true;
                        lbNo1.Text = "1";
                        lbNo2.Visible = true;
                        lbNo2.Text = "2";
                        lbNo3.Visible = true;
                        lbNo3.Text = "3";
                        lbNo4.Visible = true;
                        lbNo4.Text = "4";
                        lbNo5.Visible = true;
                        lbNo5.Text = "5(o)";
                        lbNo6.Visible = true;
                        lbNo6.Text = "6";
                        lbNo7.Visible = true;
                        lbNo7.Text = "7";
                        lbNo8.Visible = true;
                        lbNo8.Text = "8";
                        lbNo9.Visible = true;
                        lbNo9.Text = "9";
                        #region comment
                        /*
                        double  dCenterX, dCenterY, dInnerR, dOuterR, dStartA, dEndA, dMotorX, dMotorY, dR;
                        int iSequence, iColors, iType;
                        ///////////
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = i; j < 9; j++)
                            {
                                if (MotorY[i] > MotorY[j] && Math.Abs(MotorX[i] - MotorX[j]) < 10)
                                {
                                    iSequence = Sequence[i];
                                    dCenterX = CenterX[i];
                                    dCenterY = CenterY[i];
                                    dInnerR = InnerR[i];
                                    dOuterR = OuterR[i];
                                    dStartA = StartA[i];
                                    dEndA = EndA[i];
                                    dMotorX = MotorX[i];
                                    dMotorY = MotorY[i];
                                    dR = R[i];
                                    iColors = Colors[i];
                                    iType = Types[i];
                                    /////////
                                    Sequence[i] = Sequence[j];
                                    CenterX[i] = CenterX[j];
                                    CenterY[i] = CenterY[j];
                                    InnerR[i] = InnerR[j];
                                    OuterR[i] = OuterR[j];
                                    StartA[i] = StartA[j];
                                    EndA[i] = EndA[j];
                                    MotorX[i] = MotorX[j];
                                    MotorY[i] = MotorY[j];
                                    R[i] = R[j];
                                    Colors[i] = Colors[j];
                                    Types[i] = Types[j];
                                    ////////
                                    Sequence[j] = iSequence;
                                    CenterX[j] = dCenterX;
                                    CenterY[j] = dCenterY;
                                    InnerR[j] = dInnerR;
                                    OuterR[j] = dOuterR;
                                    StartA[j] = dStartA;
                                    EndA[j] = dEndA;
                                    MotorX[j] = dMotorX;
                                    MotorY[j] = dMotorY;
                                    R[j] = dR;
                                    Colors[j] = iColors;
                                    Types[j] = iType;
                                }
                            }
                        }
                        ///////////////
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = i; j < 9; j++)
                            {
                                if (MotorX[i] > MotorX[j] && Math.Abs(MotorY[i] - MotorY[j]) < 10)
                                {
                                    iSequence = Sequence[i];
                                    dCenterX = CenterX[i];
                                    dCenterY = CenterY[i];
                                    dInnerR = InnerR[i];
                                    dOuterR = OuterR[i];
                                    dStartA = StartA[i];
                                    dEndA = EndA[i];
                                    dMotorX = MotorX[i];
                                    dMotorY = MotorY[i];
                                    dR = R[i];
                                    iColors = Colors[i];
                                    iType = Types[i];
                                    /////////
                                    Sequence[i] = Sequence[j];
                                    CenterX[i] = CenterX[j];
                                    CenterY[i] = CenterY[j];
                                    InnerR[i] = InnerR[j];
                                    OuterR[i] = OuterR[j];
                                    StartA[i] = StartA[j];
                                    EndA[i] = EndA[j];
                                    MotorX[i] = MotorX[j];
                                    MotorY[i] = MotorY[j];
                                    R[i] = R[j];
                                    Colors[i] = Colors[j];
                                    Types[i] = Types[j];
                                    ////////
                                    Sequence[j] = iSequence;
                                    CenterX[j] = dCenterX;
                                    CenterY[j] = dCenterY;
                                    InnerR[j] = dInnerR;
                                    OuterR[j] = dOuterR;
                                    StartA[j] = dStartA;
                                    EndA[j] = dEndA;
                                    MotorX[j] = dMotorX;
                                    MotorY[j] = dMotorY;
                                    R[j] = dR;
                                    Colors[j] = iColors;
                                    Types[j] = iType;
                                }
                            }
                        }*/
#endregion
                        radioRingLed.Checked = false;
                        radioPosLed.Checked = false;
                        radioBackLed.Checked = false;
                    }
                          //////////
                   break;
                case "停 止":
                   bStop = true;
                    csDmc2410.Dmc2410.d2410_decel_stop(m_ClassMotion.CHXMotor, 0.1);//减速停止
                    csDmc2410.Dmc2410.d2410_decel_stop(m_ClassMotion.CHYMotor, 0.1);//减速停止
                   break;
                case "复 位":
                   timer1.Enabled = true;
                   MachinReplaceAll();
                   MessageBox.Show("ok");
                   break;
                case "机器参数设置":
                   timer1.Enabled = false;
                    fRmLogin frmLog = new fRmLogin();
                    frmLog.ShowDialog();
                    if (frmLog.bPassword == true)
                    {
                        FrmMachineParam frm = new FrmMachineParam();
                        frm.ShowDialog();
                        try
                        {
                            ReadParam();

                        }
                        catch { }
                    }
                    timer1.Enabled = true;
                   break;
                case "装载图片":
                   strFile = System.Windows.Forms.Application.StartupPath + "\\Image";
                    if (!Directory.Exists(strFile))
                        Directory.CreateDirectory(strFile);
                    openFileDialog1.Filter = "image(*.*)|*.bmp|All files(*.*)|*.*";
                    openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath + "\\Image";
                    openFileDialog1.RestoreDirectory = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        strFile = openFileDialog1.FileName;
                        FileInformation fileinfo = Algorithms.GetFileInformation(strFile);
                        ImgView.Image.Type = fileinfo.ImageType;
                        ImgView.Image.ReadFile(openFileDialog1.FileName);
                    }

                   break;
                case "保存图片":
                   if (ImgView.Image == null) return;
                   strFile = System.Windows.Forms.Application.StartupPath + "\\Image";
                    if (!Directory.Exists(strFile))
                        Directory.CreateDirectory(strFile);
                    saveFileDialog1.FileName = "";
                    saveFileDialog1.Filter = "image(*.*)|*.bmp|All files(*.*)|*.*";
                    saveFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath + "\\Image\\";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        strFile = saveFileDialog1.FileName;
                        ImgView.Image.WriteBmpFile(strFile);
                    }
                   break;
                case "摄像开始":
                   timer1.Enabled = true;
                   ToolBar1.Items[4].Enabled = false;
                   ToolBar1.Items[5].Enabled = true;
                   Thread.Sleep(500);
                   break;
                case "摄像停止":
                   timer1.Enabled = false;
                   ToolBar1.Items[4].Enabled = true;
                   ToolBar1.Items[5].Enabled = false ;
                   Thread.Sleep(500);
                   break;
                case "自动找圆对中":
                   timer1.Enabled = false;
                   ToolBar1.Items[4].Enabled = true;
                   ToolBar1.Items[5].Enabled = false;
                   Thread.Sleep(500);
                   int iThreshold = Convert.ToInt16(numericUpDownThreshold.Value);
                   if(chkRefCircle .Checked )
                        dCr = Convert.ToDouble(txtRefCircleDia.Text.Trim()) / 2;
                   else
                        dCr = Convert.ToDouble(txtCircleDia.Text.Trim()) / 2;
                   bool bflag = false;
                   if (listBoxColor.SelectedIndex == 0)
                       bflag = true;
                   else
                       bflag = false;

                   InitVelX = PublicVar.CHXMotorInitVel / PublicVar.CHXMotor_Unit;
                   MotorSpeedX = PublicVar.CHXMotorSpeed / PublicVar.CHXMotor_Unit;
                   InitVelY = PublicVar.CHYMotorInitVel / PublicVar.CHYMotor_Unit;
                   MotorSpeedY = PublicVar.CHYMotorSpeed / PublicVar.CHYMotor_Unit;
                   double Xm, Ym;
                   while (true)
                   {
                       Thread.Sleep(1000);
                       PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                       PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                       Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                       Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                       GrabC(0);
                       CircleParam m_CircleParam = ImageProcess1(ImgView.Image, iThreshold, true, dCr, bflag);
                       if (m_CircleParam.X > 0 && m_CircleParam.Y > 0)
                       {
                           double dJudge = 0.05;
                           double OffsetX, OffsetY;
                           OffsetX = -(m_CircleParam.X - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                           OffsetY = -(m_CircleParam.Y - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                           if (Math.Abs(OffsetX) > dJudge || Math.Abs(OffsetY) > dJudge)
                           {
                               Xrun = Convert.ToInt32((PublicVar.CurrentCHXMotorPos + OffsetX) / PublicVar.CHXMotor_Unit);
                               Yrun = Convert.ToInt32((PublicVar.CurrentCHYMotorPos + OffsetY) / PublicVar.CHYMotor_Unit);
                               m_ClassMotion.Absolute_Move(m_ClassMotion.CHXMotor, Xrun, InitVelX, MotorSpeedX, PublicVar.CHXMotorACC, PublicVar.CHXMotorDEC);
                               m_ClassMotion.Absolute_Move(m_ClassMotion.CHYMotor, Yrun, InitVelY, MotorSpeedY, PublicVar.CHYMotorACC, PublicVar.CHYMotorDEC);
                           }
                           else
                               break;
                       }
                       else
                       {
                           MessageBox.Show("error!");
                           break;
                       }
                   }
                   PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                   PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                   Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                   Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                   txtCurrentXpos.Text = Xm.ToString("f4");
                   txtCurrentYpos.Text = Ym.ToString("f4");
                   break;
                case "自动找圆":
                   timer1.Enabled = false;
                   ToolBar1.Items[4].Enabled = true;
                   ToolBar1.Items[5].Enabled = false ;
                   Thread.Sleep(500);
                   iThreshold = Convert.ToInt16(numericUpDownThreshold.Value);
                   if(chkRefCircle .Checked )
                        dCr = Convert.ToDouble(txtRefCircleDia.Text.Trim()) / 2;
                   else
                        dCr = Convert.ToDouble(txtCircleDia.Text.Trim()) / 2;
                   bflag = false;
                   if (listBoxColor.SelectedIndex == 0)
                       bflag = true;
                   else
                       bflag = false;
                    GrabC(0);
                    CircleParam m_CircleParam1 = ImageProcess1(ImgView.Image, iThreshold, true, dCr, bflag);
                    if (m_CircleParam1.X <= 0 && m_CircleParam1.Y <= 0)
                    {
                        MessageBox.Show("error!");
                    }
                    PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                    PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                    Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                    Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                    txtCurrentXpos.Text = Xm.ToString("f4");
                    txtCurrentYpos.Text = Ym.ToString("f4");
                  break;
                case "定位设置":
                    timer1.Enabled = false;
                    GrabC(1);
                    CircleParam m_CirlceParam = ImageProcessPos(ImgView.Image);
                    double x = 0, y = 0, a = 0;
                    x = m_CirlceParam.X;
                    y = m_CirlceParam.Y;
                    a = m_CirlceParam.R;
                    if (x > 0 && y > 0 )
                    {
                        string s = "x=" + x.ToString("f2") + " \ny=" + y.ToString("f2") + " \nAngle=" + a.ToString("f3");
                        if (MessageBox.Show("更新数据吗？(" + s + ")", "hello", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                            if (!Directory.Exists(strPath))
                                Directory.CreateDirectory(strPath);
                            strPath += "\\Prog";
                            if (!Directory.Exists(strPath))
                                Directory.CreateDirectory(strPath);
                            strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                            IniProg = new CIni(strPath);
                            txtPosCameraSetX.Text = x.ToString("f2");
                            txtPosCameraSetY.Text = y.ToString("f2");
                            txtPosCameraSetAngle.Text = a.ToString("f3");
                            strTemp = txtPosCameraSetX.Text.Trim();
                            PublicVar.PosCameraSetX = Convert.ToDouble(strTemp);
                            IniProg.IniWriteValue("PosCameraSet", "X", strTemp);
                            strTemp = txtPosCameraSetY.Text.Trim();
                            PublicVar.PosCameraSetY = Convert.ToDouble(strTemp);
                            IniProg.IniWriteValue("PosCameraSet", "Y", strTemp);
                            strTemp = txtPosCameraSetAngle.Text.Trim();
                            PublicVar.PosCameraSetAngle  = Convert.ToDouble(strTemp);
                            IniProg.IniWriteValue("PosCameraSet", "Angle", strTemp);

                        }
                    }
                    else
                        MessageBox.Show("error");
                    //更新摄像控件
                    
                    if (timer1.Enabled == false)
                    {
                        ToolBar1.Items[4].Enabled = false;
                        ToolBar1.Items[5].Enabled = true;
                        timer1.Enabled = true;
                    }
                    break ;
                case "激光笔开":
                  strTemp = IniSetting.IniReadValue("IO", "OUT8");//laser on
                   m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                   m_ClassMotion.Write_Out_Bit(0, 32, 1);
                    toolStripLaser.Text = "激光笔关";
                    break;
                case "激光笔关":
                   strTemp = IniSetting.IniReadValue("IO", "OUT8");//laser on
                   m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                   m_ClassMotion.Write_Out_Bit(0, 32, 0);
                    toolStripLaser.Text = "激光笔开";
                    break;
                case "退 出":
                   Close();
                   break;
            }
        }

        private void ToolSetting_Click(object sender, EventArgs e)
        {
            FrmMachineParam frm = new FrmMachineParam();
            frm.ShowDialog();
        }

        private void ImgView_ImageMouseUp(object sender, ImageMouseEventArgs e)
        {
            try
            {
                if (ClassPublicTool.m_ModeTool == ModeTool.Program)
                {
                    Roi roi = ImgView.Roi;
                    if (roi.Count > 0)
                    {
                        Contour ct = roi.GetContour(0);
                        AnnulusContour cp = null;
                        if (ct.Type.ToString() == "Annulus")
                        {
                            ImgView.Image.Overlays.Default.Clear();
                            cp = (AnnulusContour)ct.Shape;
                            int iThreshold =Convert.ToInt16 (numericUpDownThreshold.Value);
                            ParamCircle = FindCircluarEdge(ThresholdImage(iThreshold, ImgView.Image,false ), cp, 40, listBoxDirectory.SelectedIndex, listBoxParity.SelectedIndex);
                            lblDisplayPoints.Text +="("+ (ParamCircle.R * PublicVar.CameraX_Unit).ToString("f2")+"mm)";
                            ImgView.ShowToolbar = false;
                        }
                        else
                            roi.Clear();
                    }
                }
                if (ClassPublicTool.m_ModeTool == ModeTool.Navigate)
                {
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        double MaxSpeed = 0.1;
                        double Tacc = 0.1;
                        double Tdec = 0.1;
                        bool bFlag = false;
                        if (radioHighSpeed.Checked)
                            MaxSpeed = PublicVar.HighSpeed / PublicVar.CHXMotor_Unit;
                        else if (radioMidSpeed.Checked)
                            MaxSpeed = PublicVar.MidSpeed / PublicVar.CHXMotor_Unit;
                        else if (radioLowerSpeed.Checked)
                            MaxSpeed = PublicVar.LowSpeed / PublicVar.CHXMotor_Unit;
                        double dMotorX, dMotorY;
                        dMotorX = (PublicVar.CurrentCHXMotorPos - (e.Point.X - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit) / PublicVar.CHXMotor_Unit;
                        dMotorY = (PublicVar.CurrentCHYMotorPos - (e.Point.Y - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit) / PublicVar.CHYMotor_Unit;
                        bFlag = m_ClassMotion.CHXMotorNEL | m_ClassMotion.CHXMotorPEL | m_ClassMotion.CHXMotorALM;
                        ushort uAxis = 0;
                        if (bFlag == false)
                        {
                            uAxis = m_ClassMotion.CHXMotor;
                            m_ClassMotion.Absolute_Move(uAxis, Convert.ToInt32(dMotorX), 0, MaxSpeed, Tacc, Tdec);
                        }
                        bFlag = m_ClassMotion.CHYMotorNEL | m_ClassMotion.CHYMotorPEL | m_ClassMotion.CHYMotorALM;
                        if (bFlag == false)
                        {
                            uAxis = m_ClassMotion.CHYMotor;
                            m_ClassMotion.Absolute_Move(uAxis, Convert.ToInt32(dMotorY), 0, MaxSpeed, Tacc, Tdec);
                        }
                        timer1.Enabled = true;
                        ToolBar1.Items[4].Enabled = false;
                        ToolBar1.Items[5].Enabled = true;
                    }
                }
                if (e.Button != MouseButtons.Middle && ClassPublicTool.m_ModeTool == ModeTool.Manual)
                {
                    double Xm, Ym;
                    PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                    PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                    Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                    Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                    Xm = Xm * PublicVar.CHXCaliCorr;
                    Ym = Ym * PublicVar.CHYCaliCorr;
                    switch (ClassPublicTool.m_CalcuTool)
                    {
                        case CalcuTool.Point:
                            if (MessageBox.Show("确定吗？", "测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                double OffsetX, OffsetY;
                                OffsetX = -(x1 - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                                OffsetY = -(y1 - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                                dataGridView1[0, iRow].Value = iRow.ToString();
                                dataGridView1[1, iRow].Value = "P";
                                dataGridView1[2, iRow].Value =(PublicVar.CurrentCHXMotorPos+OffsetX).ToString("f3");
                                dataGridView1[3, iRow].Value = (PublicVar.CurrentCHYMotorPos + OffsetY).ToString("f3");
                                dataGridView1[6, iRow].Value = x1.ToString("f3");
                                dataGridView1[7, iRow].Value = y1.ToString("f3");
                                iRow++;
                            }
                            break;
                        case CalcuTool.Line:
                            if (MessageBox.Show("确定吗？", "测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                double OffsetX, OffsetY;
                                OffsetX = -(x1 - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                                OffsetY = -(y1 - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                                dataGridView1[0, iRow].Value = iRow.ToString();
                                dataGridView1[1, iRow].Value = "L";
                                dataGridView1[2, iRow].Value = (PublicVar.CurrentCHXMotorPos + OffsetX).ToString("f3");
                                dataGridView1[3, iRow].Value = (PublicVar.CurrentCHYMotorPos + OffsetY).ToString("f3");
                                OffsetX = -(x2 - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                                OffsetY = -(y2 - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                                dataGridView1[4, iRow].Value = (PublicVar.CurrentCHXMotorPos + OffsetX).ToString("f3");
                                dataGridView1[5, iRow].Value = (PublicVar.CurrentCHYMotorPos + OffsetY).ToString("f3");

                                dataGridView1[6, iRow].Value = x1.ToString("f3");
                                dataGridView1[7, iRow].Value = y1.ToString("f3");
                                dataGridView1[8, iRow].Value = x2.ToString("f3");
                                dataGridView1[9, iRow].Value = y2.ToString("f3");
                                iRow++;
                            }
                            break;
                        case CalcuTool.Circle:
                            if (MessageBox.Show("自动捕捉吗？", "测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                ImgView.Image.Overlays.Default.Clear();
                                double CenterX, CenterY, R;
                                CenterX = (x1 + x2) / 2;
                                CenterY = (y1 + y2) / 2;
                                R = (x2 - x1) / 2;
                                CircleParam cc = FindCircluarEdge(ThresholdImage(100, ImgView.Image,false), R, CenterX, CenterY, 20, listBoxDirectory.SelectedIndex, listBoxParity.SelectedIndex);
                                oval.Left = cc.X - cc.R;
                                oval.Top = cc.Y - cc.R;
                                oval.Height = cc.R * 2;
                                oval.Width = cc.R * 2;
                                ImgView.Image.Overlays.Default.AddOval(oval, Rgb32Value.RedColor);
                                x1 = cc.X - cc.R;
                                y1 = cc.Y - cc.R;
                                x2 = cc.X + cc.R;
                                y2 = cc.Y + cc.R;
                            }
                            if (MessageBox.Show("确定吗？", "测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                dataGridView1[0, iRow].Value = iRow.ToString();
                                dataGridView1[1, iRow].Value = "C";
                                dataGridView1[2, iRow].Value = ((x1 + x2) / 2).ToString("f3");
                                dataGridView1[3, iRow].Value = ((y1 + y2) / 2).ToString("f3");
                                dataGridView1[4, iRow].Value = Math.Abs(x1 - x2).ToString("f3");
                                dataGridView1[5, iRow].Value = Math.Abs(y1 - y2).ToString("f3");
                                iRow++;
                            }
                            break;
                    }
                }
                x1 = 0;
                y1 = 0;
                x2 = 0;
                y2 = 0;
            }
            catch { }
        }

        private void ImgView_ImageMouseMove(object sender, ImageMouseEventArgs e)
        {
            try
            {
                if (ClassPublicTool.m_ModeTool == ModeTool.Program)
                {
                    timer1.Enabled = false;
                    ToolBar1.Items[4].Enabled = true;
                    ToolBar1.Items[5].Enabled = false;
                }

                if (ClassPublicTool.m_ModeTool == ModeTool.Navigate)
                {
                    ImgView.Image.Overlays.Default.Clear();
                    l1 = null;
                    l2 = null;
                    l1 = new LineContour();
                    l1.Start.X = e.Point.X;
                    l1.Start.Y = 0;
                    l1.End.X = e.Point.X;
                    l1.End.Y = ImgView.Image.Height;
                    ImgView.Image.Overlays.Default.AddLine(l1, Rgb32Value.BlueColor);
                    l2 = new LineContour();
                    l2.Start.X = 0;
                    l2.Start.Y = e.Point.Y;
                    l2.End.X = ImgView.Image.Width;
                    l2.End.Y = e.Point.Y;
                    ImgView.Image.Overlays.Default.AddLine(l2, Rgb32Value.BlueColor);

                }
                if (e.Button != MouseButtons.Middle && ClassPublicTool.m_ModeTool == ModeTool.Manual)
                {
                    switch (ClassPublicTool.m_CalcuTool)
                    {
                        case CalcuTool.Circle:
                            if (x1 > 0 && y1 > 0)
                            {
                                ImgView.Image.Overlays.Default.Clear();
                                x2 = e.Point.X;
                                y2 = e.Point.Y;

                                l1 = null;
                                l2 = null;
                                oval = null;
                                oval = new OvalContour();
                                oval.Left = x1;
                                oval.Top = y1;
                                if (x2 > x1 && y2 > y1)
                                {
                                    oval.Width = x2 - x1;
                                    oval.Height = y2 - y1;
                                    ImgView.Image.Overlays.Default.AddOval(oval, Rgb32Value.RedColor);
                                }
                            }
                            break;
                        case CalcuTool.Line:
                            if (x1 > 0 && y1 > 0)
                            {
                                l1 = null;
                                l2 = null;
                                oval = null;
                                l1 = new LineContour();
                                ImgView.Image.Overlays.Default.Clear();
                                x2 = e.Point.X;
                                y2 = e.Point.Y;
                                l1.Start.X = x1;
                                l1.Start.Y = y1;
                                l1.End.X = x2;
                                l1.End.Y = y2;
                                ImgView.Image.Overlays.Default.AddLine(l1, Rgb32Value.RedColor);
                            }
                            break;
                    }
                }
                else
                {
                    if (ClassPublicTool.m_CalcuTool == CalcuTool.Circle)
                    {
                        ImgView.Image.Overlays.Default.Clear();
                        oval.Left = e.Point.X - oval.Width / 2;
                        oval.Top = e.Point.Y - oval.Height / 2;
                        ImgView.Image.Overlays.Default.AddOval(oval, Rgb32Value.RedColor);
                    }
                }
            }
            catch { }
        }

        private void ImgView_ImageMouseDown(object sender, ImageMouseEventArgs e)
        {
            try
            {
                if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt && ClassPublicTool.m_ModeTool == ModeTool.Program)
                {
                    ImgView.Roi.Clear();
                    ImgView.Image.Overlays.Default.Clear();
                }
                if (ClassPublicTool.m_ModeTool == ModeTool.Program)
                {
                    timer1.Enabled = false;
                    ToolBar1.Items[4].Enabled = true;
                    ToolBar1.Items[5].Enabled = false;
                }
                if (e.Button != MouseButtons.Middle && ClassPublicTool.m_ModeTool == ModeTool.Manual)
                {
                    switch (ClassPublicTool.m_CalcuTool)
                    {
                        case CalcuTool.Line:
                            x1 = e.Point.X;
                            y1 = e.Point.Y;
                            break;
                        case CalcuTool.Point:
                            if (e.Button == MouseButtons.Left)
                            {
                                l1 = null;
                                l2 = null;
                                oval = null;
                                l1 = new LineContour();
                                l2 = new LineContour();
                                x1 = e.Point.X;
                                y1 = e.Point.Y;
                                l1.Start.X = x1 - 20;
                                l1.Start.Y = y1;
                                l1.End.X = x1 + 20;
                                l1.End.Y = y1;
                                ImgView.Image.Overlays.Default.AddLine(l1, Rgb32Value.GreenColor);
                                l2.Start.X = x1;
                                l2.Start.Y = y1 - 20;
                                l2.End.X = x1;
                                l2.End.Y = y1 + 20;
                                ImgView.Image.Overlays.Default.AddLine(l2, Rgb32Value.GreenColor);
                            }
                            break;
                        case CalcuTool.Circle:
                            x1 = e.Point.X;
                            y1 = e.Point.Y;
                            break;
                        case CalcuTool.ManyPointtoCircle:
                            if (e.Button == MouseButtons.Right)
                            {
                                for (int i = 0; i < iManyFitPointCount; i++)
                                {
                                    ManyFitPoint[i].X = 0;
                                    ManyFitPoint[i].Y = 0;
                                }
                                ImgView.Image.Overlays.Default.Clear();
                                iManyFitPointCount = 0;
                                lblDisplayPoints.Text = "1点";
                            }
                            else
                            {
                                if (iManyFitPointCount == 0)
                                {
                                    ImgView.Image.Overlays.Default.Clear();
                                }
                                //////////////////////////////////////////
                                double Xm, Ym;
                                PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
                                PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit;
                                Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
                                Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
                                double OffsetX, OffsetY;
                                OffsetX = -(e.Point.X  - ImgView.Image.Width / 2) * PublicVar.CameraX_Unit;
                                OffsetY = -(e.Point.Y  - ImgView.Image.Height / 2) * PublicVar.CameraY_Unit;
                                ManyFitPoint[iManyFitPointCount] = e.Point;
                                //ManyFitPoint[iManyFitPointCount].X = PublicVar.CurrentCHXMotorPos + OffsetX;
                                //ManyFitPoint[iManyFitPointCount].Y  = PublicVar.CurrentCHYMotorPos + OffsetY;
                                l1 = null;
                                l2 = null;
                                oval = null;
                                oval = new OvalContour();
                                l1 = new LineContour();
                                l2 = new LineContour();
                                l1.Start.X = ManyFitPoint[iManyFitPointCount].X - 5;
                                l1.Start.Y = ManyFitPoint[iManyFitPointCount].Y;
                                l1.End.X = ManyFitPoint[iManyFitPointCount].X + 5;
                                l1.End.Y = ManyFitPoint[iManyFitPointCount].Y;
                                ImgView.Image.Overlays.Default.AddLine(l1, Rgb32Value.GreenColor);
                                l2.Start.X = ManyFitPoint[iManyFitPointCount].X;
                                l2.Start.Y = ManyFitPoint[iManyFitPointCount].Y - 5;
                                l2.End.X = ManyFitPoint[iManyFitPointCount].X;
                                l2.End.Y = ManyFitPoint[iManyFitPointCount].Y + 5;
                                ImgView.Image.Overlays.Default.AddLine(l2, Rgb32Value.GreenColor);
                                iManyFitPointCount++;
                                lblDisplayPoints.Text = iManyFitPointCount.ToString() + "/" + ClassPublicTool.ManyPointNum.ToString();
                                if (iManyFitPointCount >= ClassPublicTool.ManyPointNum)
                                {///
                                    double[] PointX = new double[ClassPublicTool.ManyPointNum];
                                    double[] PointY = new double[ClassPublicTool.ManyPointNum];
                                    for (int i = 0; i < ClassPublicTool.ManyPointNum; i++)
                                    {
                                        PointX[i] = ManyFitPoint[i].X;
                                        PointY[i] = ManyFitPoint[i].Y;
                                    }
                                    double[] CircleXYR = ClassPublicTool.LeastSquaresFitting(ClassPublicTool.ManyPointNum, PointX, PointY);
                                    if (MessageBox.Show("自动捕捉吗？", "测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        CircleParam cc = FindCircluarEdge(ThresholdImage(100, ImgView.Image,false ), CircleXYR[2], CircleXYR[0], CircleXYR[1], 10, listBoxDirectory.SelectedIndex, listBoxParity.SelectedIndex);
                                        CircleXYR[0] = cc.X;
                                        CircleXYR[1] = cc.Y;
                                        CircleXYR[2] = cc.R;
                                    }
                                    oval.Left = CircleXYR[0] - CircleXYR[2];
                                    oval.Top = CircleXYR[1] - CircleXYR[2];
                                    oval.Height = CircleXYR[2] * 2;
                                    oval.Width = CircleXYR[2] * 2;

                                    ImgView.Image.Overlays.Default.AddOval(oval, Rgb32Value.RedColor);
                                    iManyFitPointCount = 0;
                                    l1 = null;
                                    l2 = null;
                                    if (MessageBox.Show("确定吗？", "测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        dataGridView1[0, iRow].Value = iRow.ToString();
                                        dataGridView1[1, iRow].Value = "X";
                                        dataGridView1[2, iRow].Value = CircleXYR[0].ToString("f3");
                                        dataGridView1[3, iRow].Value = CircleXYR[1].ToString("f3");
                                        dataGridView1[4, iRow].Value = (CircleXYR[2]* PublicVar.CameraX_Unit).ToString("f3");
                                        iRow++;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch { }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            iSelectRow++;
            if (iSelectRow == 1)
            {
                iSelect1 = dataGridView1.CurrentRow.Index ;
            }
            else if (iSelectRow == 2)
            {

                iSelectRow = 0;
                iSelect2 = dataGridView1.CurrentRow.Index;
                double x11, y11, x12, y12,dist;
                switch (ClassPublicTool.m_CalcuTool)
                { 
                    case CalcuTool.PointToPoint:
                        x11 =Convert.ToDouble ( dataGridView1[2, iSelect1].Value) ;
                        y11 =Convert.ToDouble ( dataGridView1[3, iSelect1].Value) ;
                        x12 =Convert.ToDouble ( dataGridView1[2, iSelect2].Value) ;
                        y12 =Convert.ToDouble ( dataGridView1[3, iSelect2].Value) ;
                        dist = Math.Sqrt((x11 - x12) * (x11 - x12) + (y11 - y12) * (y11 - y12));
                        dataGridView1[0, iRow].Value  = "PP";
                        dataGridView1[1, iRow].Value  = iSelect1 ;
                        dataGridView1[2, iRow].Value = iSelect2;
                        dataGridView1[3, iRow].Value = dist.ToString("f3");
                        iRow++;
                        break;
                    case CalcuTool.PointToLine :
                        double x, y;
                        x =Convert.ToDouble ( dataGridView1[2, iSelect1].Value) ;
                        y =Convert.ToDouble ( dataGridView1[3, iSelect1].Value) ;
                        x11 =Convert.ToDouble ( dataGridView1[2, iSelect2].Value) ;
                        y11 =Convert.ToDouble ( dataGridView1[3, iSelect2].Value) ;
                        x12 =Convert.ToDouble ( dataGridView1[4, iSelect2].Value) ;
                        y12 =Convert.ToDouble ( dataGridView1[5, iSelect2].Value) ;
                        dist = Math.Abs((y12 - y11) * x - (y12 - y11) * x12 + y12*(x12 - x11) - y * (x12 - x11)) / Math.Sqrt((y12 - y11) * (y12 - y11) + (x12 - x11) * (x12 - x11));
                        dataGridView1[0, iRow].Value  = "PL";
                        dataGridView1[1, iRow].Value  = iSelect1 ;
                        dataGridView1[2, iRow].Value = iSelect2;
                        dataGridView1[3, iRow].Value = dist.ToString("f3");
                        iRow++;
                       break;
                    case CalcuTool.LineToLine :
                        x11 =Convert.ToDouble ( dataGridView1[2, iSelect1].Value) ;
                        x11 +=Convert.ToDouble ( dataGridView1[4, iSelect1].Value) ;
                        x11 /= 2.0;
                        y11 =Convert.ToDouble ( dataGridView1[3, iSelect1].Value) ;
                        y11+=Convert.ToDouble ( dataGridView1[5, iSelect1].Value) ;
                        y11 /= 2.0;
                        x12 =Convert.ToDouble ( dataGridView1[2, iSelect2].Value) ;
                        x12 +=Convert.ToDouble ( dataGridView1[4, iSelect2].Value) ;
                        x12 /= 2.0;
                        y12 =Convert.ToDouble ( dataGridView1[3, iSelect2].Value) ;
                        y12 +=Convert.ToDouble ( dataGridView1[5, iSelect2].Value) ;
                        y12 /= 2.0;
                        dist = Math.Sqrt((x11 - x12) * (x11 - x12) + (y11 - y12) * (y11 - y12));
                        dataGridView1[0, iRow].Value  = "PP";
                        dataGridView1[1, iRow].Value  = iSelect1 ;
                        dataGridView1[2, iRow].Value = iSelect2;
                        dataGridView1[3, iRow].Value = dist.ToString("f3");
                        iRow++;
                      break;
                    case CalcuTool.CircleToCirlce :
                        x11 =Convert.ToDouble ( dataGridView1[2, iSelect1].Value) ;
                        y11 =Convert.ToDouble ( dataGridView1[3, iSelect1].Value) ;
                        x12 =Convert.ToDouble ( dataGridView1[2, iSelect2].Value) ;
                        y12 =Convert.ToDouble ( dataGridView1[3, iSelect2].Value) ;
                        dist = Math.Sqrt((x11 - x12) * (x11 - x12) + (y11 - y12) * (y11 - y12));
                        dataGridView1[0, iRow].Value  = "CC";
                        dataGridView1[1, iRow].Value  = iSelect1 ;
                        dataGridView1[2, iRow].Value = iSelect2;
                        dataGridView1[3, iRow].Value = dist.ToString("f3");
                        iRow++;
                       break;
                }


            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridView1.RowCount = 0;
                dataGridView1.RowCount = 10;
                dataGridView1.ColumnCount = 6;
                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 20;
                for (int i = 2; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Columns[i].Width = (dataGridView1.Width - 60) / 4;
                }
            }

        }

        private void ToolCmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImgView.Image.Overlays.Default.Clear();
            ImgView.Roi.Clear();
            switch (ToolCmbMode.SelectedIndex)
            {
                case 0:
                    ClassPublicTool.m_ModeTool = ModeTool.Manual;
                    ImgView.ShowToolbar = false;
                    ToolBar1.Items[1].Enabled = true;
                    break;
                case 1:
                    ClassPublicTool.m_ModeTool = ModeTool.Auto;
                    ImgView.ShowToolbar = false;
                    break;
                case 2:
                    ClassPublicTool.m_ModeTool = ModeTool.Navigate;
                    ImgView.ShowToolbar = false;
                    l1 = null;
                    l2 = null;
                    l1 = new LineContour();
                    l1.Start.X = ImgView.Image.Width / 2;
                    l1.Start.Y = 0;
                    l1.End.X = ImgView.Image.Width / 2;
                    l1.End.Y = ImgView.Image.Height;
                    ImgView.Image.Overlays.Default.AddLine(l1, Rgb32Value.BlueColor);
                    l2 = new LineContour();
                    l2.Start.X = 0;
                    l2.Start.Y = ImgView.Image.Height / 2;
                    l2.End.X = ImgView.Image.Width;
                    l2.End.Y = ImgView.Image.Height / 2;
                    ImgView.Image.Overlays.Default.AddLine(l2, Rgb32Value.BlueColor);
                    tabControl1.SelectedIndex = 1;
                    timer1.Enabled = true;
                    ToolBar1.Items[4].Enabled = false ;
                    ToolBar1.Items[5].Enabled = true;
                   break;
                case 3:
                    ClassPublicTool.m_ModeTool = ModeTool.Program;
                    ImgView.ShowToolbar = true;
                    if (dataGridView2.RowCount >= 5)
                    {
                        if (MessageBox.Show("清除所有数据吗？", "hello", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            dataGridView2.RowCount = 1;
                        else
                            MessageBox.Show("必须先清除数据！");
                    }
                    tabControl1.SelectedIndex = 1;
                    break;
            }

            if (ToolCmbMode.SelectedIndex == 0)
            {
                groupBox2.Enabled = true;
            }
            else
            {
                groupBox2.Enabled = false;
            }
        }

        private void BtnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHXMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if ( PublicVar.CurrentCHXMotorPos > RightBottomX)
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHXMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHXMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHXMotor_Unit;
            bFlag = m_ClassMotion.CHXMotorNEL;
            PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() == "0")
                    m_ClassMotion.Jog(userAxis, 0, MaxSpeed, Tacc, Tdec, 1);
            }

         }

        private void BtnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            ushort userAxis = m_ClassMotion.CHXMotor;
            if (comboBoxDist.Text.Trim() == "0")
            {

                if (radioHighSpeed.Checked)
                {
                    csDmc2410.Dmc2410.d2410_decel_stop(userAxis, 0.1);//减速停止
                }
                else
                {
                    csDmc2410.Dmc2410.d2410_emg_stop();
                }

            }
            else
                comboBoxDist.Text = "0";//每次输入距离后清零
        }

        private void BtnUp_MouseDown(object sender, MouseEventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHYMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if (PublicVar.CurrentCHYMotorPos < LeftTopY )
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHYMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHYMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHYMotor_Unit;
            bFlag = m_ClassMotion.CHYMotorPEL;
            PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHXEncoder_Unit;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() == "0")
                    m_ClassMotion.Jog(userAxis, 0, MaxSpeed, Tacc, Tdec, 0);
            }

        }

        private void BtnUp_MouseUp(object sender, MouseEventArgs e)
        {
            ushort userAxis = m_ClassMotion.CHYMotor;
            if (comboBoxDist.Text.Trim() == "0")
            {

                if (radioHighSpeed.Checked)
                {
                    csDmc2410.Dmc2410.d2410_decel_stop(userAxis, 0.1);//减速停止
                }
                else
                {
                    csDmc2410.Dmc2410.d2410_emg_stop();
                }

            }
            else
                comboBoxDist.Text = "0";//每次输入距离后清零

        }

        private void BtnDown_MouseDown(object sender, MouseEventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHYMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if ( PublicVar.CurrentCHYMotorPos > RightBottomY)
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            if (radioHighSpeed.Checked)
                MaxSpeed =PublicVar .HighSpeed  / PublicVar.CHXMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHXMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHXMotor_Unit;
            bFlag = m_ClassMotion.CHYMotorNEL;
            PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHXEncoder_Unit;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() == "0")
                    m_ClassMotion.Jog(userAxis, 0, MaxSpeed, Tacc, Tdec, 1);
            }
        }

        private void BtnDown_MouseUp(object sender, MouseEventArgs e)
        {
            ushort userAxis = m_ClassMotion.CHYMotor;
            if (comboBoxDist.Text.Trim() == "0")
            {

                if (radioHighSpeed.Checked)
                {
                    csDmc2410.Dmc2410.d2410_decel_stop(userAxis, 0.1);//减速停止
                }
                else
                {
                    csDmc2410.Dmc2410.d2410_emg_stop();
                }

            }
            else
                comboBoxDist.Text = "0";//每次输入距离后清零
        }

        private void BtnRight_MouseDown(object sender, MouseEventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHXMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if (PublicVar.CurrentCHXMotorPos < LeftTopX )
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHXMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHXMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHXMotor_Unit;
            bFlag = m_ClassMotion.CHXMotorPEL;
            PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() == "0")
                    m_ClassMotion.Jog(userAxis, 0, MaxSpeed, Tacc, Tdec, 0);
            }
         }

        private void BtnRight_MouseUp(object sender, MouseEventArgs e)
        {
            ushort userAxis = m_ClassMotion.CHXMotor;
            if (comboBoxDist.Text.Trim() == "0")
            {

                if (radioHighSpeed.Checked)
                {
                    csDmc2410.Dmc2410.d2410_decel_stop(userAxis, 0.1);//减速停止
                }
                else
                {
                    csDmc2410.Dmc2410.d2410_emg_stop();
                }

            }
            else
                comboBoxDist.Text = "0";//每次输入距离后清零
        }

        private void BtnRst_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定复位吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //添加每次点击时自动打开摄像
                if (timer1.Enabled == false)
                {
                    timer1.Enabled = true;
                    ToolBar1.Items[4].Enabled = false;
                    ToolBar1.Items[5].Enabled = true;
                    Thread.Sleep(50);
                }
                //
                BtnRst.Enabled = false;
                MachinReplaceAll();
                BtnRst.Enabled = true;
                MessageBox.Show("OK");
                
            }

        }

        private void numericUpDownLightC_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int[] iBri = new int[4];
                bool[] bOpen = new bool[4];
                if (cmbProductName.Text == "") return;
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                if (radioRingLed.Checked)
                {
                    iBri[0] = Convert.ToInt16(numericUpDownLightC.Value);
                    bOpen[0] = true;
                    iBri[1] = 0;
                    bOpen[1] = false;
                    iBri[2] = 0;
                    bOpen[2] = false;
                    iBri[3] = 0;
                    bOpen[3] = false;
                    PublicVar.iLedSel = 0;
                    strTemp = numericUpDownLightC.Value.ToString();
                    IniProg.IniWriteValue("Image", "Light0" , strTemp);
                }
                else if (radioBackLed.Checked)
                {
                    iBri[0] = 0;
                    bOpen[0] = false;
                    iBri[1] = Convert.ToInt16(numericUpDownLightC.Value);
                    bOpen[1] = true;
                    iBri[2] = 0;
                    bOpen[2] = false;
                    iBri[3] = 0;
                    bOpen[3] = false;
                    PublicVar.iLedSel = 1;
                    strTemp = numericUpDownLightC.Value.ToString();
                    IniProg.IniWriteValue("Image", "Light1" , strTemp);
                }
                else if (radioPosLed.Checked)
                {
                    iBri[0] = 0;
                    bOpen[0] = false ;
                    iBri[1] = 0;
                    bOpen[1] = false;
                    iBri[2] = Convert.ToInt16(numericUpDownLightC.Value);
                    bOpen[2] = true;
                    iBri[3] = 0;
                    bOpen[3] = false;
                    strTemp = numericUpDownLightC.Value.ToString();
                    IniProg.IniWriteValue("Image", "Light2", strTemp);
                }
                string strBri = "";
                strBri = m_ClassCom.SendStr(iBri, bOpen);
                com.Write(strBri);
                Thread.Sleep(50);
            }
            catch { }

        }

        private void numericUpDownThreshold_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProductName.Text == "") return;
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                if (radioRingLed.Checked)
                {
                    PublicVar.iLedSel = 0;
                    strTemp = numericUpDownThreshold.Value.ToString();
                    IniProg.IniWriteValue("Image", "Threshold0", strTemp);
                }
                else if (radioBackLed.Checked)
                {
                    PublicVar.iLedSel = 1;
                    strTemp = numericUpDownThreshold.Value.ToString();
                    IniProg.IniWriteValue("Image", "Threshold1" , strTemp);
                }
                else if (radioPosLed.Checked)
                {
                    strTemp = numericUpDownThreshold.Value.ToString();
                    IniProg.IniWriteValue("Image", "Threshold2", strTemp);
                }

                Thread.Sleep(50);
            }
            catch { }

        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHYMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if (Convert.ToDouble(comboBoxDist.Text.Trim()) < LeftTopY || Convert.ToDouble(comboBoxDist.Text.Trim()) > RightBottomY)
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            //添加每次点击时自动打开摄像
            if (timer1.Enabled==false)
            {
                timer1.Enabled = true;
                ToolBar1.Items[4].Enabled = false;
                ToolBar1.Items[5].Enabled = true;
                Thread.Sleep(50);
            }
            //
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHYMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHYMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHYMotor_Unit;
            bFlag = m_ClassMotion.CHYMotorPEL;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() != "0")
                {
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        double dPos = Convert.ToDouble(comboBoxDist.Text.Trim()) / PublicVar.CHYMotor_Unit;
                        dPos *= PublicVar.CHXCaliCorr;
                        dPos *= PublicVar.CHYCaliCorr;
                        //m_ClassMotion.Absolute_Move(userAxis, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);


                        csDmc2410.Dmc2410.d2410_set_profile(userAxis, 0, MaxSpeed, Tacc, Tdec);   //设置速度、加速度
                        csDmc2410.Dmc2410.d2410_t_pmove(userAxis, -Convert.ToInt32(dPos), 0);//作相对t型运动
                    }
                }
            }
            ToolBar1.Items[1].Enabled = true;//打开停止按钮
            

        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHXMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);

            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if (Convert.ToDouble(comboBoxDist.Text.Trim()) < LeftTopX || Convert.ToDouble(comboBoxDist.Text.Trim()) > RightBottomX)
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            //添加每次点击时自动打开摄像
            if (timer1.Enabled==false)
            {
                timer1.Enabled = true;
                ToolBar1.Items[4].Enabled = false;
                ToolBar1.Items[5].Enabled = true;
                Thread.Sleep(50);
            }
            //
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHXMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHXMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHXMotor_Unit;
            bFlag = m_ClassMotion.CHXMotorPEL;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() != "0")
                {
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        double dPos = Convert.ToDouble(comboBoxDist.Text.Trim()) / PublicVar.CHXMotor_Unit;
                        dPos *= PublicVar.CHXCaliCorr;
                        //m_ClassMotion.Absolute_Move(userAxis, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                        csDmc2410.Dmc2410.d2410_set_profile(userAxis, 0, MaxSpeed, Tacc, Tdec);   //设置速度、加速度
                        csDmc2410.Dmc2410.d2410_t_pmove(userAxis, -Convert.ToInt32(dPos), 0);//作相对t型运动
                    }
                }
            }
            ToolBar1.Items[1].Enabled = true;//打开停止按钮
           
        }

        private void ImgView_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Roi roi = ImgView.Roi;
                Contour ct = roi.GetContour(0);
                AnnulusContour cp = null;
                if (ct.Type.ToString() == "Annulus")
                {
                    cp = (AnnulusContour)ct.Shape;
                    dataGridView2.Rows.Add();
                    int ix = dataGridView2.RowCount - 1;
                    dataGridView2[0, ix].Value = ix.ToString();
                    dataGridView2[1, ix].Value = ParamCircle.X.ToString("f3");
                    dataGridView2[2, ix].Value = ParamCircle.Y.ToString("f3");
                    dataGridView2[3, ix].Value = cp.InnerRadius;
                    dataGridView2[4, ix].Value = cp.OuterRadius;
                    dataGridView2[5, ix].Value = cp.StartAngle;
                    dataGridView2[6, ix].Value = cp.EndAngle;
                    dataGridView2[7, ix].Value = txtCurrentXpos.Text ;
                    dataGridView2[8, ix].Value = txtCurrentYpos.Text;
                    if(chkRefCircle .Checked )
                        dataGridView2[9, ix].Value = Convert.ToDouble(txtRefCircleDia.Text) / 2;
                    else
                        dataGridView2[9, ix].Value =Convert .ToDouble(txtCircleDia .Text)/2 ;
                    dataGridView2[10, ix].Value = listBoxColor.SelectedIndex;
                    if(radioRingLed .Checked )
                        dataGridView2[11, ix].Value = 0;
                    else
                        dataGridView2[11, ix].Value = 1;
                    ImgView.ShowToolbar = true;
                }
                if (ClassPublicTool.m_ModeTool == ModeTool.Program)
                {
                    ImgView.Image.Overlays.Default.Clear();
                    ImgView.Roi.Clear();
                    timer1.Enabled = true;
                    ToolBar1.Items[4].Enabled = false;
                    ToolBar1.Items[5].Enabled = true;
                }
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount < 2 || cmbProductName.Text.Trim() == "")
            {
                MessageBox.Show("没有产品名称或没有编程数据！");
                return;
            }
            if (MessageBox.Show("确定保存测量程式吗？", "自动测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strPath;
                strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                strPath += "\\Prog";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                strPath += "\\" + cmbProductName.Text.Trim()+".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                strTemp = (dataGridView2.RowCount-1).ToString();
                IniProg.IniWriteValue("Total", "Count", strTemp);
                for (int i = 1; i < dataGridView2.RowCount ; i++)
                {
                    strTemp = dataGridView2[0, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "NO.", strTemp);
                    strTemp = dataGridView2[1, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "CenterX", strTemp);
                    strTemp = dataGridView2[2, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "CenterY", strTemp);
                    strTemp = dataGridView2[3, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "InnerR", strTemp);
                    strTemp = dataGridView2[4, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "OuterR", strTemp);
                    strTemp = dataGridView2[5, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "StartA", strTemp);
                    strTemp = dataGridView2[6, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "EndA", strTemp);
                    strTemp = dataGridView2[7, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "MotorX", strTemp);
                    strTemp = dataGridView2[8, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "MotorY", strTemp);
                    strTemp = dataGridView2[9, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "R", strTemp);
                    strTemp = dataGridView2[10, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "Color", strTemp);
                    strTemp = dataGridView2[11, i].Value.ToString();
                    IniProg.IniWriteValue(i.ToString(), "Type", strTemp);
                }
                
            }

        }

        private void cmbProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\CurrentProduct.ini";
                CIni IniCurrProd = new CIni(strPath);
                IniCurrProd.IniWriteValue("CurrentProduct", "Name", cmbProductName.Text.Trim());

                string strpath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog\\" + cmbProductName.Text + ".ini";
                if (File.Exists(strpath))
                {
                    CIni IniProg = new CIni(strpath);
                    string strTemp = "";

                    strTemp = IniProg.IniReadValue("Param", "L1Set");
                    txtL1Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "L2Set");
                    txtL2Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "W1Set");
                    txtW1Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "W2Set");
                    txtW2Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "Y3Set");
                    txtY3Standard.Text = strTemp;


                    strTemp = IniProg.IniReadValue("Param", "No1");
                    txtLowerLimit.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "No2");
                    txtUpperLimit.Text = strTemp;

                    strTemp = IniProg.IniReadValue("Param", "Dir");
                    listBoxDirectory.SelectedIndex = Convert.ToInt16(strTemp); 
                    strTemp = IniProg.IniReadValue("Param", "Parity");
                    listBoxParity.SelectedIndex = Convert.ToInt16(strTemp);

                    strTemp = IniProg.IniReadValue("Param", "MasterLine");//参考线
                    cmbMasterLine.SelectedIndex = Convert.ToInt16(strTemp);

                    strTemp = IniProg.IniReadValue("Param", "MarkColor");
                    listBoxColor.SelectedIndex = Convert.ToInt16(strTemp);//Mark圆颜色

                    strTemp = IniProg.IniReadValue("Param", "AverageSort");//读取是否按平均值分堆
                    if (strTemp == "1" || strTemp == "") cB_AverageSort.Checked = true;
                    else cB_AverageSort.Checked = false;

                    strTemp = IniProg.IniReadValue("Param", "XStand"); //读取涨缩标准值
                    tB_XStand.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "YStand");
                    tB_YStand.Text = strTemp;

                    strTemp = IniProg.IniReadValue("Param", "ClassTray");
                    cmbClassTray.SelectedIndex = Convert.ToInt16(strTemp);

                    strTemp = IniProg.IniReadValue("RefAngle", "L1Set");
                    txtL1AngleSet.Text = strTemp;
                    strTemp = IniProg.IniReadValue("RefAngle", "L2Set");
                    txtL2AngleSet.Text = strTemp;
                    strTemp = IniProg.IniReadValue("RefAngle", "W1Set");
                    txtW1AngleSet.Text = strTemp;
                    strTemp = IniProg.IniReadValue("RefAngle", "W2Set");
                    txtW2AngleSet.Text = strTemp;

                    strTemp = IniProg.IniReadValue("CircleDia", "Mark");
                    txtCircleDia.Text = strTemp;
                    strTemp = IniProg.IniReadValue("CircleDia", "Ref");
                    txtRefCircleDia.Text = strTemp;
                  //选择灯光
                    strTemp = IniProg.IniReadValue("Select", "Led");
                    if (strTemp == "0" || strTemp == "")
                    {
                        radioRingLed.Checked = true;
                    }
                    if (strTemp == "1")
                    {
                        radioBackLed.Checked = true;
                    }
                    if (strTemp == "2")
                    {
                        radioPosLed.Checked = true;
                    }
                    radioRingLed_CheckedChanged(sender, e);
                  //是否使用定位相机
                    strTemp = IniProg.IniReadValue("Select", "PosUsing");
                    if (strTemp == "1") chkPosUsing.Checked = true;
                    else chkPosUsing.Checked = false;
                  //6点测量CheckBox
                    strTemp = IniProg.IniReadValue("CheckBox6Point", "CheckBox");
                    if (strTemp == "1") cB_6Measuring.Checked = true;
                    else cB_6Measuring.Checked = false;
                  //定位相机参数
                    strTemp = IniProg.IniReadValue("PosCameraSet", "X");
                    PublicVar.PosCameraSetX = Convert.ToDouble(strTemp);
                    txtPosCameraSetX.Text = strTemp;
                    strTemp = IniProg.IniReadValue("PosCameraSet", "Y");
                    PublicVar.PosCameraSetY = Convert.ToDouble(strTemp);
                    txtPosCameraSetY.Text = strTemp;
                    strTemp = IniProg.IniReadValue("PosCameraSet", "Angle");
                    PublicVar.PosCameraSetAngle = Convert.ToDouble(strTemp);
                    txtPosCameraSetAngle.Text = strTemp;
                  //重新加载测量程式
                    btn_LoadParam_Click(sender, e);
                }
                
            }

            catch 
            { 
                MessageBox.Show("加载产品参数失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnParamSave_Click(object sender, EventArgs e)
        {
            if (cmbProductName.Text.Trim() == "")
            {
                MessageBox.Show("没有产品名称!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("确定保存测量程式参数吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strPath;
                strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                strPath += "\\Prog";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                strPath += "\\" + cmbProductName.Text.Trim()+".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                strTemp = numericUpDownLightC.Value.ToString();
                IniProg.IniWriteValue("Image", "Light" + PublicVar.iLedSel.ToString(), strTemp);
                strTemp = numericUpDownThreshold.Value.ToString();
                IniProg.IniWriteValue("Image", "Threshold" + PublicVar.iLedSel.ToString(), strTemp);
                strTemp = txtL1Set.Text.Trim();
                IniProg.IniWriteValue("Param", "L1Set", strTemp);
                strTemp = txtL2Set.Text.Trim();
                IniProg.IniWriteValue("Param", "L2Set", strTemp);
                strTemp = txtW1Set.Text.Trim();
                IniProg.IniWriteValue("Param", "W1Set", strTemp);
                strTemp = txtW2Set.Text.Trim();
                IniProg.IniWriteValue("Param", "W2Set", strTemp);
                strTemp = txtY3Standard.Text.Trim();
                IniProg.IniWriteValue("Param", "Y3Set", strTemp);

                strTemp = txtL1AngleSet.Text.Trim();
                IniProg.IniWriteValue("RefAngle", "L1Set", strTemp);
                strTemp = txtL2AngleSet.Text.Trim();
                IniProg.IniWriteValue("RefAngle", "L2Set", strTemp);
                strTemp = txtW1AngleSet.Text.Trim();
                IniProg.IniWriteValue("RefAngle", "W1Set", strTemp);
                strTemp = txtW2AngleSet.Text.Trim();
                IniProg.IniWriteValue("RefAngle", "W2Set", strTemp);
                strTemp = txtCircleDia.Text.Trim();
                IniProg.IniWriteValue("CircleDia", "Mark", strTemp);
                strTemp = txtRefCircleDia.Text.Trim();
                IniProg.IniWriteValue("CircleDia", "Ref", strTemp);
                strTemp = txtLowerLimit.Text.Trim();
                IniProg.IniWriteValue("Param", "No1", strTemp);
                strTemp = txtUpperLimit.Text.Trim();
                IniProg.IniWriteValue("Param", "No2", strTemp);
                strTemp = listBoxDirectory.SelectedIndex.ToString();
                IniProg.IniWriteValue("Param", "Dir", strTemp);
                strTemp = listBoxParity.SelectedIndex.ToString();
                IniProg.IniWriteValue("Param", "Parity", strTemp);
                strTemp = cmbMasterLine.SelectedIndex.ToString();
                IniProg.IniWriteValue("Param", "MasterLine", strTemp);
                strTemp = listBoxColor.SelectedIndex.ToString();
                IniProg.IniWriteValue("Param", "MarkColor", strTemp);

                strTemp = txtPosCameraSetX.Text.Trim();
                IniProg.IniWriteValue("PosCameraSet", "X", strTemp);
                strTemp = txtPosCameraSetY.Text.Trim();
                IniProg.IniWriteValue("PosCameraSet", "Y", strTemp);
                strTemp = txtPosCameraSetAngle.Text.Trim();
                IniProg.IniWriteValue("PosCameraSet", "Angle", strTemp);
                strTemp = cmbClassTray.SelectedIndex.ToString();
                IniProg.IniWriteValue("Param", "ClassTray", strTemp);
                //平均值分堆
                if (cB_AverageSort.Checked)
                {
                    IniProg.IniWriteValue("Param", "AverageSort", "1");
                }
                else
                {
                    IniProg.IniWriteValue("Param", "AverageSort", "0");
                }
                //涨缩标准值
                strTemp = tB_XStand.Text;
                IniProg.IniWriteValue("Param", "XStand", strTemp);
                strTemp = tB_YStand.Text;
                IniProg.IniWriteValue("Param", "YStand", strTemp);


                if (radioRingLed.Checked)
                    PublicVar.iLedSel = 0;
                else if (radioBackLed.Checked)
                    PublicVar.iLedSel = 1;
                strTemp = PublicVar.iLedSel.ToString();
                IniProg.IniWriteValue("Select", "Led", strTemp);
              //读取是否使用定位相机
                if (chkPosUsing.Checked)
                    IniProg.IniWriteValue("Select", "PosUsing", "1");
                else
                    IniProg.IniWriteValue("Select", "PosUsing", "0");
                
                txtLowerLimit.Enabled = false;
                txtUpperLimit.Enabled = false;
                txtL1Set.Enabled = false;
                txtL2Set.Enabled = false;
                txtW1Set.Enabled = false;
                txtW2Set.Enabled = false;
                txtY3Standard.Enabled = false;
                listBoxDirectory.Enabled = false ;
                listBoxParity.Enabled = false;
                cmbMasterLine.Enabled = false;
                txtPosCameraSetX.Enabled = false;
                txtPosCameraSetY.Enabled = false;
                txtPosCameraSetAngle.Enabled = false;
                cmbClassTray.Enabled = false;
                cB_AverageSort.Enabled = false;
                chkPosUsing.Enabled = false;
                chkRefCircle.Enabled = false;
                txtCycleTime.Enabled = false;
                txtRefCircleDia.Enabled = false;
                tB_XStand.Enabled = false;
                tB_YStand.Enabled = false;
                cB_6Measuring.Enabled = false;
              //更新距离textBox控件
                comboBoxDist.Items.Clear();
                comboBoxDist.Items.Add("0");
                comboBoxDist.Items.Add(txtL1Set.Text.Trim());
                if (txtL2Set.Text.Trim() != txtL2Set.Text.Trim())
                    comboBoxDist.Items.Add(txtL2Set.Text.Trim());
                comboBoxDist.Items.Add(txtW1Set.Text.Trim());
                if (txtW2Set.Text.Trim() != txtW1Set.Text.Trim())
                    comboBoxDist.Items.Add(txtW2Set.Text.Trim());

            }

            if (chkPosUsing.Checked)//如果使用了定位相机
            {
                if (MessageBox.Show("现在要设置图形模板吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var task = new Task(() =>
                    {
                        this.BeginInvoke(new System.Action(() =>
                        {                           
                            ToolCameraSelect.SelectedIndex = 1;//选择定位相机
                            radioPosLed.Checked = true;//选择定位光源
                            radioPosLed_CheckedChanged(sender, e);//打开定位光源
                            if (timer1.Enabled == false) timer1.Enabled = true;//检查是否打开摄像机
                        }));
                    });
                    task.Start();//多线程打开光源

                    FormWaitting fw = new FormWaitting(2000);//等待窗口
                    fw.ShowDialog();

                    timer1.Enabled = false;
                    FrmModel frmM = new FrmModel();
                    frmM.ShowDialog();


                    if (MessageBox.Show("现在要激活定位设置吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        GrabC(1);
                        CircleParam m_CirlceParam = ImageProcessPos(ImgView.Image);
                        double x = 0, y = 0, a = 0;
                        x = m_CirlceParam.X;
                        y = m_CirlceParam.Y;
                        a = m_CirlceParam.R;
                        if (x > 0 && y > 0)
                        {
                            string s = "x=" + x.ToString("f2") + " \ny=" + y.ToString("f2") + " \nAngle=" + a.ToString("f3");
                            if (MessageBox.Show("定位信息已经找到,更新数据吗?(" + s + ")", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                                if (!Directory.Exists(strPath))
                                    Directory.CreateDirectory(strPath);
                                strPath += "\\Prog";
                                if (!Directory.Exists(strPath))
                                    Directory.CreateDirectory(strPath);
                                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                                var IniProg = new CIni(strPath);
                                txtPosCameraSetX.Text = x.ToString("f2");
                                txtPosCameraSetY.Text = y.ToString("f2");
                                txtPosCameraSetAngle.Text = a.ToString("f3");
                                string strTemp = txtPosCameraSetX.Text.Trim();
                                PublicVar.PosCameraSetX = Convert.ToDouble(strTemp);
                                IniProg.IniWriteValue("PosCameraSet", "X", strTemp);
                                strTemp = txtPosCameraSetY.Text.Trim();
                                PublicVar.PosCameraSetY = Convert.ToDouble(strTemp);
                                IniProg.IniWriteValue("PosCameraSet", "Y", strTemp);
                                strTemp = txtPosCameraSetAngle.Text.Trim();
                                PublicVar.PosCameraSetAngle = Convert.ToDouble(strTemp);
                                IniProg.IniWriteValue("PosCameraSet", "Angle", strTemp);

                            }
                        }
                        else
                            MessageBox.Show("定位设置失败,请检查相机是否正常工作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //更新摄像控件

                        if (timer1.Enabled == false)
                        {
                            ToolBar1.Items[4].Enabled = false;
                            ToolBar1.Items[5].Enabled = true;
                            timer1.Enabled = true;
                        }

                        //重新打开检测相机,并激活环形光
                        ToolCameraSelect.SelectedIndex = 0;
                        radioRingLed.Checked = true;
                    }

                }
            }
            //重新读取当前产品参数到指定位置
            cmbProductName_SelectedIndexChanged(sender, e);

        }

        private void btnParamEdit_Click(object sender, EventArgs e)
        {
            txtLowerLimit.Enabled = true;
            txtUpperLimit.Enabled = true;
            txtL1Set.Enabled = true;
            txtL2Set.Enabled = true;
            txtW1Set.Enabled = true;
            txtW2Set.Enabled = true;
            txtY3Standard.Enabled = true;
            txtPosCameraSetX.Enabled = true;
            txtPosCameraSetY.Enabled = true;
            txtPosCameraSetAngle.Enabled = true;
            txtCycleTime.Enabled = true;
            txtRefCircleDia.Enabled = true;

            listBoxDirectory.Enabled = true;
            listBoxParity.Enabled = true;

            cmbMasterLine.Enabled = true;            
            cmbClassTray.Enabled = true;

            cB_AverageSort.Enabled = true;
            chkPosUsing.Enabled = true;
            chkRefCircle.Enabled = true;
            cB_LCheck.Enabled = true;
            
            tB_XStand.Enabled = true;
            tB_YStand.Enabled = true;

            cB_6Measuring.Enabled = true;
        }

        private void CheckWarranty()
        {
            CIni ReadMac = new CIni("c:\\hhiatsn.ini");
            string sTime = ReadMac.IniReadValue("Time", "T");
            if (sTime != "-1")
            {
                DateTime datetime = DateTime.Now;
                string[] d = new string[5];
                d[0] = sTime.Substring(0, 2);//day
                d[1] = sTime.Substring(2, 2);//month
                d[2] = sTime.Substring(4, 2);//year_L
                d[3] = sTime.Substring(6, 2);//len
                d[4] = sTime.Substring(8, 2);//Year_R
                DateTime datetimeStart = new DateTime(Convert.ToInt16(d[2] + d[4]), Convert.ToInt16(d[1]), Convert.ToInt16(d[0]), 0, 0, 0);
                int iday = Convert.ToInt16(d[3]) * 30 - (datetime - datetimeStart).Days;

                if (iday < 5)
                {
                    labelDate.Text = "有效期还剩" + iday.ToString() + "天";
                    labelDate.Visible = true;
                    
                }
                else
                    labelDate.Visible = false;
            }

        }
        FormSafetyWatting FSW;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //IN1:start sign
            //IN2:PLC alarm
            //IN3:empty run
            //IN4:check ready
            //IN5:PLC ready
            //IN6:cy safety sn
            //OUT1:Lower
            //OUT2:Normal
            //OUT3:Upper
            //OUT4:test finished
            //OUT5:PC alarm
            //OUT6:pc safety 
            //OUT7:
            //OUT8:Laser ON
            //OUT9:EXCEPTION
            chkCalibration_CheckedChanged(sender, e);
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))//安全保护
            {
                timer1.Enabled = false;
                PublicVar.ForceClose = false;
                FSW = new FormSafetyWatting();//卡在等待页面
                SafeWattingForm.Enabled = true;
                FSW.ShowDialog();
                SafeWattingForm.Enabled = false;
                if (PublicVar.ForceClose)
                {
                    timer1.Enabled = false;
                    this.Close();
                    return;
                }
                else
                {
                    timer1.Enabled = true;
                    return;
                }  
            
            }
            strTemp = IniSetting.IniReadValue("IO", "IN2");
            if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))//alarm
            {
                lblAlarm.Visible = true;
            }
            else
                lblAlarm.Visible = false;
            if (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < 0.1 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && bStop==true)//当按下停止按钮后的处理程序
            {
                //Enable Buttons
                ManyPointToolStripMenuItem.Checked = false;
                btnParamEdit.Enabled = true;
                btnParamSave.Enabled = true;
                btnDelete.Enabled = true;
                ToolBar1.Items[0].Enabled = true;
                ToolBar1.Items[1].Enabled = false ;
                ToolBar1.Items[2].Enabled = true;
                ToolBar1.Items[3].Enabled = true;
                ToolBar1.Items[4].Enabled = true;
                ToolBar1.Items[5].Enabled = false;
                ToolBar1.Items[6].Enabled = true;
                ToolBar1.Items[7].Enabled = true;
                ToolBar1.Items[8].Enabled = true;
                ToolBar1.Items[9].Enabled = true;
                ToolBar1.Items[10].Enabled = true;
                ToolBar1.Items[11].Enabled = true;
                ToolBar1.Items[12].Enabled = true;
                BtnLeft.Enabled = true;
                BtnRight.Enabled = true;
                BtnRst.Enabled = true;
                BtnUp.Enabled = true;
                BtnDown.Enabled = true;
                chkThreshold.Enabled = true;
                listBoxColor.Enabled = true;
                chkCalibration.Enabled = true;
                numericUpDownLightC.Enabled = true;
                numericUpDownThreshold.Enabled = true;
                cmbProductName.Enabled = true;
                groupBox5.Enabled = true;
                groupBox3.Enabled = true;
                //Initialization dataGridView1
                dataGridView1.RowCount = 0;
                dataGridView1.Refresh();
                dataGridView1.RowCount = 10;
                dataGridView1.ColumnCount = 6;
                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 20;
                for (int i = 2; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Columns[i].Width = (dataGridView1.Width - 100) / 4;
                }
                bAuto = false;
                ToolCmbMode.SelectedIndex = 0;
                iStep = 0;
                ExcelUnInit();
                bStop = false;
            }
            ///////////////////////////////////////////////////////
            if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) == 0 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) == 0)
            {
                if (m_ClassMotion.CHXMotorALM || m_ClassMotion.CHXMotorPEL || m_ClassMotion.CHXMotorNEL || m_ClassMotion.CHYMotorALM || m_ClassMotion.CHYMotorPEL || m_ClassMotion.CHYMotorNEL)
                {
                    strTemp = IniSetting.IniReadValue("IO", "OUT5");//ALARM
                    m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
                    Thread.Sleep(1000);
                    m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
                }
                if (m_ClassMotion.CHXMotorALM)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("相机马达X驱动器报警!","警告",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Close();
                    return;
                }
                if (m_ClassMotion.CHXMotorPEL )
                {
                    timer1.Enabled = false;
                    MessageBox.Show("相机马达X正限位!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
                if (m_ClassMotion.CHXMotorNEL )
                {
                    timer1.Enabled = false;
                    MessageBox.Show("相机马达X负限位!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                if (m_ClassMotion.CHYMotorALM)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("相机马达Y驱动器报警!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
                if (m_ClassMotion.CHYMotorPEL)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("相机马达Y正限位!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
                if (m_ClassMotion.CHYMotorNEL)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("相机马达Y负限位!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }
            double Xm, Ym;
            PublicVar.CurrentCHXMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHXMotor, true) * PublicVar.CHXEncoder_Unit ;
            PublicVar.CurrentCHYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHYMotor, true) * PublicVar.CHYEncoder_Unit ;
            Xm = PublicVar.CurrentCHXMotorPos * Math.Cos(PublicVar.CHXRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHYMotorPos * Math.Sin(PublicVar.CHYRepairAngle * 3.14159 / 180);
            Ym = PublicVar.CurrentCHYMotorPos * Math.Cos(PublicVar.CHYRepairAngle * 3.14159 / 180) + PublicVar.CurrentCHXMotorPos * Math.Sin(PublicVar.CHXRepairAngle * 3.14159 / 180);
            PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false ) * PublicVar.CHCYMotor_Unit;
            PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false ) * PublicVar.CHCZMotor_Unit;
            txtCurrentXpos.Text = Xm.ToString("f4");
            txtCurrentYpos.Text = Ym.ToString("f4");
            txtCurrentCYpos.Text = PublicVar.CurrentCHCYMotorPos.ToString("f2");
            txtCurrentCZpos.Text = PublicVar.CurrentCHCZMotorPos.ToString("f2");
            if (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < 0.1)
            {
                strTemp = IniSetting.IniReadValue("IO", "OUT6");//PC SAFETY
                m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
               
            }
            else
            {
                strTemp = IniSetting.IniReadValue("IO", "OUT6");
                m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);

            }
            if (bAuto)
            {
                    if (ManyPointToolStripMenuItem.Checked == false)
                    {
                        strTemp = IniSetting.IniReadValue("IO", "IN3");//empty run
                        if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
                            AutoEmptyRunProg(Sequence, CenterX, CenterY, InnerR, OuterR, StartA, EndA, MotorX, MotorY);///////////////
                        else
                        {
                            if (PublicVar.MasterNumHZ != 0)
                            {
                                if ((dataGridView1.RowCount - 1) % PublicVar.MasterNumHZ == 0 && bMasterTest == false && iStep < 2)
                                {
                                    AutoRunMasterProg();////////////
                                }
                                else
                                    AutoRunProg(Sequence, CenterX, CenterY, InnerR, OuterR, StartA, EndA, MotorX, MotorY);////////////
                            }
                            if (bMasterTest == true || PublicVar.MasterNumHZ == 0)
                            {
                                AutoRunProg(Sequence, CenterX, CenterY, InnerR, OuterR, StartA, EndA, MotorX, MotorY);///////////
                            }
                        }
                    }
                    else
                    {
                        AutoRepairRun();/////////
                    }
            }
            else//live on
            {
                GrabC(ToolCameraSelect.SelectedIndex);//camera select
                double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
                LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
                LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
                RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
                RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
                if (PublicVar.CurrentCHYMotorPos < LeftTopY || PublicVar.CurrentCHYMotorPos > RightBottomY)
                {
                    m_ClassMotion.StopAxis(m_ClassMotion.CHYMotor );
                   
                    return;
                }
                if (PublicVar.CurrentCHXMotorPos < LeftTopX|| PublicVar.CurrentCHXMotorPos > RightBottomX)
                {
                    m_ClassMotion.StopAxis(m_ClassMotion.CHXMotor);
                    return;
                }

            }
            Thread.Sleep(100);
            ////////////////////////////////////////////////////////////////////
           
            //马达报警触发安全窗口
            if (m_ClassMotion.CHXMotorEmg || m_ClassMotion.CHYMotorEmg || m_ClassMotion.CHCYMotorEmg || m_ClassMotion.CHCZMotorEmg)
            {
                timer1.Enabled = false;
                PublicVar.ForceClose = false;
                FSW = new FormSafetyWatting();//卡在等待页面
                SafeWattingForm.Enabled = true;
                FSW.ShowDialog();
                SafeWattingForm.Enabled = false;
                if (PublicVar.ForceClose)
                {
                    timer1.Enabled = false;
                    this.Close();
                    return;
                }
                else
                {
                    timer1.Enabled = true;
                    return;
                }  
            }
            //检查控件是否可用
            if (ToolBar1.Items[4].Enabled == true || ToolBar1.Items[5].Enabled == false)
            {
                ToolBar1.Items[4].Enabled =false ;
                ToolBar1.Items[5].Enabled =true ;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            try
            {
                m_ClassMotion.E_Stop();

                int[] iBri = new int[4];
                bool[] bOpen = new bool[4];
                iBri[0] =0;
                bOpen[0] = false;
                iBri[1] = 0;
                bOpen[1] = false;
                iBri[2] = 0;
                bOpen[2] = false;
                iBri[3] = 0;
                bOpen[3] = false;
                string strBri = "";
                strBri = m_ClassCom.SendStr(iBri, bOpen);
                com.Write(strBri);
                Thread.Sleep(50);
                if (frm2 != null) frm2.Close();
                m_Camera.UnInitPortCamera();
             }
            catch { }
            com.Close();
            
            ExcelUnInit();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ImgView.Roi.Clear();
            ImgView.Image.Overlays.Default.Clear();
            if(MessageBox.Show ("清除所有数据吗？","hello",MessageBoxButtons .YesNo )==DialogResult .Yes )
               dataGridView2.RowCount = 1;

        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHXMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if (Convert.ToDouble(comboBoxDist.Text.Trim()) < LeftTopX || Convert.ToDouble(comboBoxDist.Text.Trim()) > RightBottomX)
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            //添加每次点击时自动打开摄像
            if (timer1.Enabled==false)
            {
                timer1.Enabled = true;
                ToolBar1.Items[4].Enabled = false;
                ToolBar1.Items[5].Enabled = true;
                Thread.Sleep(50);
            }
            //
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHXMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHXMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHXMotor_Unit;
            bFlag = m_ClassMotion.CHXMotorPEL;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() != "0")
                {
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        double dPos = Convert.ToDouble(comboBoxDist.Text.Trim()) / PublicVar.CHXMotor_Unit;
                         dPos *= PublicVar.CHXCaliCorr;
                        dPos *= PublicVar.CHXCaliCorr;
                        //m_ClassMotion.Absolute_Move(userAxis, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                        csDmc2410.Dmc2410.d2410_set_profile(userAxis, 0, MaxSpeed, Tacc, Tdec);   //设置速度、加速度
                        csDmc2410.Dmc2410.d2410_t_pmove(userAxis, Convert.ToInt32(dPos), 0);//作相对t型运动
                    }
                }
            }

            ToolBar1.Items[1].Enabled = true;//打开停止按钮

            
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis = m_ClassMotion.CHYMotor;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            double LeftTopX, LeftTopY, RightBottomX, RightBottomY;
            LeftTopX = Convert.ToDouble(IniSetting.IniReadValue("X", "LeftTop"));
            LeftTopY = Convert.ToDouble(IniSetting.IniReadValue("Y", "LeftTop"));
            RightBottomX = Convert.ToDouble(IniSetting.IniReadValue("X", "RightBottom"));
            RightBottomY = Convert.ToDouble(IniSetting.IniReadValue("Y", "RightBottom"));
            if (Convert.ToDouble(comboBoxDist.Text.Trim()) < LeftTopY || Convert.ToDouble(comboBoxDist.Text.Trim()) > RightBottomY)
            {
                m_ClassMotion.StopAxis(userAxis);
                return;
            }
            //添加每次点击时自动打开摄像
            if (timer1.Enabled==false)
            {
                timer1.Enabled = true;
                ToolBar1.Items[4].Enabled = false;
                ToolBar1.Items[5].Enabled = true;
                Thread.Sleep(50);
            }
            //
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHYMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHYMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHYMotor_Unit;
            bFlag = m_ClassMotion.CHYMotorPEL;
            if (MaxSpeed > 0 && bFlag == false)
            {
                if (comboBoxDist.Text.Trim() != "0")
                {
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHXMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHYMotor) != 0)
                    {
                        double dPos = Convert.ToDouble(comboBoxDist.Text.Trim()) / PublicVar.CHYMotor_Unit;
                        dPos *= PublicVar.CHXCaliCorr;
                        dPos *= PublicVar.CHYCaliCorr;
                        //m_ClassMotion.Absolute_Move(userAxis, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                        csDmc2410.Dmc2410.d2410_set_profile(userAxis, 0, MaxSpeed, Tacc, Tdec);   //设置速度、加速度
                        csDmc2410.Dmc2410.d2410_t_pmove(userAxis, Convert.ToInt32(dPos), 0);//作相对t型运动
                    }
                 }
            }
            ToolBar1.Items[1].Enabled = true;//打开停止按钮
      
        }


        private void tabControl1_Click(object sender, EventArgs e)
        {
        //    try
        //    {
        //        string strPath;
        //        strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
        //        if (!Directory.Exists(strPath))
        //            Directory.CreateDirectory(strPath);
        //        strPath += "\\Prog";
        //        if (!Directory.Exists(strPath))
        //            Directory.CreateDirectory(strPath);
        //        strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
        //        CIni IniProg = new CIni(strPath);
        //        string strTemp = "";
        //        if (cmbProductName.Text != "" && tabControl1.SelectedIndex == 1)
        //        {
        //            if (MessageBox.Show("加载当前产品数据吗？", "Hello", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //            {
        //                dataGridView2.RowCount = 1;
        //                dataGridView2.Refresh();
        //                strTemp = IniProg.IniReadValue("Total", "Count");
        //                int iTotal = Convert.ToInt16(strTemp);
        //                dataGridView2.RowCount = iTotal + 1;
        //                for (int i = 1; i <= iTotal; i++)
        //                {
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "NO.");
        //                    dataGridView2[0, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "CenterX");
        //                    dataGridView2[1, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "CenterY");
        //                    dataGridView2[2, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "InnerR");
        //                    dataGridView2[3, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "OuterR");
        //                    dataGridView2[4, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "StartA");
        //                    dataGridView2[5, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "EndA");
        //                    dataGridView2[6, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "MotorX");
        //                    dataGridView2[7, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "MotorY");
        //                    dataGridView2[8, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "R");
        //                    dataGridView2[9, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "Color");
        //                    dataGridView2[10, i].Value = strTemp;
        //                    strTemp = IniProg.IniReadValue(i.ToString(), "Type");
        //                    dataGridView2[11, i].Value = strTemp;
        //                }

        //            }
        //        }
        //        if (cmbProductName.Text != "" && tabControl1.SelectedIndex == 2)
        //        {
        //            strTemp = IniProg.IniReadValue("Param", "L1Set");
        //            txtL1Set.Text = strTemp;
        //            strTemp = IniProg.IniReadValue("Param", "L2Set");
        //            txtL2Set.Text = strTemp;
        //            strTemp = IniProg.IniReadValue("Param", "W1Set");
        //            txtW1Set.Text = strTemp;
        //            strTemp = IniProg.IniReadValue("Param", "W2Set");
        //            txtW2Set.Text = strTemp;
        //            strTemp = IniProg.IniReadValue("Param", "No1");
        //            txtLowerLimit.Text = strTemp;
        //            strTemp = IniProg.IniReadValue("Param", "No2");
        //            txtUpperLimit.Text = strTemp;
        //        }
        //    }
        //    catch { MessageBox.Show("加载参数错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            try
            {
                if(dataGridView2.RowCount<=2)
                {
                    string strPath;
                    strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    strPath += "\\Prog";
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                    CIni IniProg = new CIni(strPath);
                    string strTemp = "";
                    if (cmbProductName.Text != "" && tabControl1.SelectedIndex == 1)
                    {
                        dataGridView2.RowCount = 1;
                        dataGridView2.Refresh();
                        strTemp = IniProg.IniReadValue("Total", "Count");
                        int iTotal = Convert.ToInt16(strTemp);
                        dataGridView2.RowCount = iTotal + 1;
                        for (int i = 1; i <= iTotal; i++)
                        {
                            strTemp = IniProg.IniReadValue(i.ToString(), "NO.");
                            dataGridView2[0, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "CenterX");
                            dataGridView2[1, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "CenterY");
                            dataGridView2[2, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "InnerR");
                            dataGridView2[3, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "OuterR");
                            dataGridView2[4, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "StartA");
                            dataGridView2[5, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "EndA");
                            dataGridView2[6, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "MotorX");
                            dataGridView2[7, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "MotorY");
                            dataGridView2[8, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "R");
                            dataGridView2[9, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "Color");
                            dataGridView2[10, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "Type");
                            dataGridView2[11, i].Value = strTemp;
                        }

                        }
                    
                }


            }
            catch 
            { 
                return;
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                //添加每次点击时自动打开摄像
                if (timer1.Enabled==false)
                {
                    timer1.Enabled = true;
                    ToolBar1.Items[4].Enabled = false;
                    ToolBar1.Items[5].Enabled = true;
                    //Thread.Sleep(50);
                }
                //
                double MaxSpeed = 0.1;
                double Tacc = 0.1;
                double Tdec = 0.1;
                bool bFlag = false;
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHXMotor_Unit;
                double dMotorX, dMotorY;

                int iRow;
                if (e.RowIndex == 0)
                    return;
                else
                    iRow = e.RowIndex;
                dMotorX = Convert.ToDouble(dataGridView2[7, iRow].Value);
                dMotorY = Convert.ToDouble(dataGridView2[8, iRow].Value);
                dMotorX /= PublicVar.CHXMotor_Unit;
                dMotorY /= PublicVar.CHYMotor_Unit;
                bFlag = m_ClassMotion.CHXMotorNEL | m_ClassMotion.CHXMotorPEL | m_ClassMotion.CHXMotorALM;
                ushort uAxis = 0;
                if (bFlag == false)
                {
                    uAxis = m_ClassMotion.CHXMotor;
                    m_ClassMotion.Absolute_Move(uAxis, Convert.ToInt32(dMotorX), 0, MaxSpeed, Tacc, Tdec);
                }
                bFlag = m_ClassMotion.CHYMotorNEL | m_ClassMotion.CHYMotorPEL | m_ClassMotion.CHYMotorALM;
                if (bFlag == false)
                {
                    uAxis = m_ClassMotion.CHYMotor;
                    m_ClassMotion.Absolute_Move(uAxis, Convert.ToInt32(dMotorY), 0, MaxSpeed, Tacc, Tdec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定更新测量程式的当前位置吗？", "自动测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    int ix = dataGridView2.CurrentRow.Index;
                    if (ix > 0)
                    {
                        dataGridView2[7, ix].Value = txtCurrentXpos.Text;
                        dataGridView2[8, ix].Value = txtCurrentYpos.Text;
                        if (chkRefCircle.Checked)
                            dataGridView2[9, ix].Value = Convert.ToDouble(txtRefCircleDia.Text) / 2;
                        else
                            dataGridView2[9, ix].Value = Convert.ToDouble(txtCircleDia.Text) / 2;
                        dataGridView2[10, ix].Value = listBoxColor.SelectedIndex;
                        if(radioRingLed .Checked )
                            dataGridView2[11, ix].Value = 0;
                        else
                            dataGridView2[11, ix].Value = 1;

                        btnSave_Click(sender, e);

                        //打开摄像机
                        if (timer1.Enabled == false)
                        {
                            timer1.Enabled = true;
                        }
                    }
                }
                catch { }
            }
        }

        private void cmbMasterLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMasterLine.SelectedIndex >0)
            {
                txtL1AngleSet.Enabled = true;
                txtL2AngleSet.Enabled = true;
                txtW1AngleSet.Enabled = true;
                txtW2AngleSet.Enabled = true;
                txtRefCircleDia.Enabled = true;
                if ( cmbMasterLine .SelectedIndex ==2 &&   MessageBox.Show("重新设定角度吗？", "坐标测量", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    txtL1AngleSet.Text = "0";
                    txtL2AngleSet.Text = "0";
                    txtW1AngleSet.Text = "0";
                    txtW2AngleSet.Text = "0";
                    bFirst = true;
                }
            }
            else
            {
                txtL1AngleSet.Enabled = false;
                txtL2AngleSet.Enabled = false;
                txtW1AngleSet.Enabled = false;
                txtW2AngleSet.Enabled = false;
                txtRefCircleDia.Enabled = false;
                bFirst = false;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if ((iStep < 2||iMasterStep < 2) && bButton == false)
            {
                bButton = true;
                btnTest.BackColor = Color.Green;
            }

        }

        private void chkCalibration_CheckedChanged(object sender, EventArgs e)
        {
            string strTemp;
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            if (chkCalibration.Checked)
            {
                strTemp = IniSetting.IniReadValue("CHY", "RepairAngle");
                PublicVar.CHYRepairAngle = Convert.ToDouble(strTemp);
                strTemp = IniSetting.IniReadValue("CHX", "RepairAngle");
                PublicVar.CHXRepairAngle = Convert.ToDouble(strTemp);
            }
            else
            {
                PublicVar.CHYRepairAngle = 0;
                PublicVar.CHXRepairAngle = 0;
            }

        }

 
        private void 整体标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm2.nWidth = ImgView.Image.Width;
            frm2.nHeight = ImgView.Image.Height;
            frm2.Show();

        }

        private void ManyPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStandardPanel frmPanel = new frmStandardPanel();
            frmPanel.ShowDialog();
            if(frmPanel .bFlag ==true )
                ManyPointToolStripMenuItem.Checked = !ManyPointToolStripMenuItem.Checked;
            string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\RepairDataMachine";
            string strFile = strPath + "\\X.dat";
            if (File.Exists(strFile)) File.Delete(strFile);
            strFile = strPath + "\\Y.dat";
            if (File.Exists(strFile)) File.Delete(strFile);
            strFile = strPath + "\\_X.dat";
            if (File.Exists(strFile)) File.Delete(strFile);
            strFile = strPath + "\\_Y.dat";
            if (File.Exists(strFile)) File.Delete(strFile);

        }

        private void comboBoxDist_MouseLeave(object sender, EventArgs e)
        {
            if (comboBoxDist.Text.Trim() == "") comboBoxDist.Text = "0";
        }

 
        private void comboBoxDist_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnCYRun_Click(object sender, EventArgs e)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
            {
                MessageBox.Show("PLC not ready");
                return;
            }
            strTemp = IniSetting.IniReadValue("IO", "OUT6");
            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
            if (m_ClassMotion.CHCZMotorORG == true)
            {
                btnCYRun.Enabled = false;
                btnCZRun.Enabled = false;
                btnReset.Enabled = false;
                btnCZInitPos.Enabled = false;
                double MaxSpeed = 0.1;
                double Tacc = 0.1;
                double Tdec = 0.1;
                bool bFlag = false;
                ushort userAxis;
                userAxis = m_ClassMotion.CHCYMotor;
                if (radioHighSpeed.Checked)
                    MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCYMotor_Unit;
                else if (radioMidSpeed.Checked)
                    MaxSpeed = PublicVar.MidSpeed / PublicVar.CHCYMotor_Unit;
                else if (radioLowerSpeed.Checked)
                    MaxSpeed = PublicVar.LowSpeed / PublicVar.CHCYMotor_Unit;
                bFlag = (m_ClassMotion.CHCYMotorPEL & (Convert.ToDouble(comboBoxDist1.Text.Trim()) >= PublicVar.CurrentCHCYMotorPos));
                bFlag &= (m_ClassMotion.CHCYMotorNEL & (Convert.ToDouble(comboBoxDist1.Text.Trim()) <= PublicVar.CurrentCHCYMotorPos));

                if (MaxSpeed > 0 && bFlag == false )
                {
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && m_ClassMotion.CHCZMotorORG == true)
                    {
                        double dPos = Convert.ToDouble(comboBoxDist1.Text.Trim()) / PublicVar.CHCYMotor_Unit;
                        if (dPos < 0) dPos = 0;
                        m_ClassMotion.Absolute_Move(userAxis, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                    }
                }
                comboBoxDist1.Text = "0";
                btnCYRun.Enabled = true;
                btnCZRun.Enabled = true;
                btnReset.Enabled = true;
                btnCZInitPos.Enabled = true;
            }
            if(m_ClassMotion.CHCZMotorORG == false )
                MessageBox.Show("光源马达CZ没在原点安全位置");

        }

        private void btnCZRun_Click(object sender, EventArgs e)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
            {
                MessageBox.Show("PLC not ready");
                return;
            }
            strTemp = IniSetting.IniReadValue("IO", "IN6");//cy Motor safety
            if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp))&&(Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) < 0.05 || Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < 0.05))
            {
                btnCYRun.Enabled = false;
                btnCZRun.Enabled = false;
                btnReset.Enabled = false;
                btnCZInitPos.Enabled = false;
                double MaxSpeed = 0.1;
                double Tacc = 0.1;
                double Tdec = 0.1;
                bool bFlag = false;
                ushort userAxis;
                userAxis = m_ClassMotion.CHCZMotor;
                if (radioHighSpeed.Checked)
                    MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCZMotor_Unit;
                else if (radioMidSpeed.Checked)
                    MaxSpeed = PublicVar.MidSpeed / PublicVar.CHCZMotor_Unit;
                else if (radioLowerSpeed.Checked)
                    MaxSpeed = PublicVar.LowSpeed / PublicVar.CHCZMotor_Unit;
                bFlag = (m_ClassMotion.CHCZMotorPEL & (Convert.ToDouble(comboBoxDist1.Text.Trim()) >= PublicVar.CurrentCHCZMotorPos));
                bFlag &= (m_ClassMotion.CHCZMotorNEL & (Convert.ToDouble(comboBoxDist1.Text.Trim()) <= PublicVar.CurrentCHCZMotorPos));

                if (MaxSpeed > 0 && bFlag == false)
                {
                    if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                        (PublicVar.CurrentCHCYMotorPos <= PublicVar.CHCYMotorInitPos || PublicVar.CurrentCHCYMotorPos >= PublicVar.CHCYMotorWorkPos))
                    {
                        double dPos = Convert.ToDouble(comboBoxDist1.Text.Trim()) / PublicVar.CHCZMotor_Unit;
                        if (dPos < 0) dPos = 0;
                        m_ClassMotion.Absolute_Move(userAxis, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                    }
                }
                comboBoxDist1.Text = "0";
                btnCYRun.Enabled = true;
                btnCZRun.Enabled = true;
                btnReset.Enabled = true;
                btnCZInitPos.Enabled = true;
            }
            else
            {
                MessageBox.Show("注意光源马达的安全");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
            {
                MessageBox.Show("PLC not ready");
                return;
            }
            btnCYRun.Enabled = false;
            btnCZRun.Enabled = false;
            btnReset.Enabled = false;
            btnCZInitPos.Enabled = false;
            if (m_ClassMotion.ReplaceCHCZMotor(new double[] { PublicVar.CHCZMotorHomeInitVel / PublicVar.CHCZMotor_Unit, 
                                PublicVar.CHCZMotorHomeMaxVel/ PublicVar.CHCZMotor_Unit, PublicVar.CHCZMotorHomeACC, PublicVar.CHCZMotorHomeDEC, PublicVar.CHCZMotorHomeMaxVel/ PublicVar.CHCZMotor_Unit }, Convert.ToInt32(PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit)))
            {
                if(m_ClassMotion.CHCZMotorORG )
                    m_ClassMotion.ReplaceCHCYMotor(new double[] { PublicVar.CHCYMotorHomeInitVel / PublicVar.CHCYMotor_Unit, 
                                                    PublicVar.CHCYMotorHomeMaxVel/ PublicVar.CHCYMotor_Unit, PublicVar.CHCYMotorHomeACC, PublicVar.CHCYMotorHomeDEC, PublicVar.CHCYMotorHomeMaxVel/ PublicVar.CHCYMotor_Unit }, Convert.ToInt32(PublicVar.CHCYMotorInitPos / PublicVar.CHCYMotor_Unit));
            }
            btnCYRun.Enabled = true;
            btnCZRun.Enabled = true;
            btnReset.Enabled = true;
            btnCZInitPos.Enabled = true;
            strTemp = IniSetting.IniReadValue("IO", "OUT6");
            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
            comboBoxDist1.Text = "0";
            MessageBox.Show("OK");
        }

        private void 自动修正补偿值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rdata.AutoCalcuRepairData(System.Windows.Forms.Application.StartupPath);
            MessageBox.Show("OK");
        }

        private void btnPress_Click(object sender, EventArgs e)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
            {
                MessageBox.Show("PLC not ready");
                return;
            }
            strTemp = IniSetting.IniReadValue("IO", "OUT6");
            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 1);
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                (PublicVar.CurrentCHCZMotorPos >= PublicVar.CHCZMotorInitPos) && Math.Abs ( PublicVar .CurrentCHCYMotorPos -PublicVar .CHCYMotorInitPos)<0.1 )
            {
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCZMotor_Unit;
                double dPos = PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit;
                m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
            }
            while (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor)== 0)
            {
                PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
            }
            if (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < 0.1 && m_ClassMotion.CHCZMotorORG == true && m_ClassMotion.CHCYMotorORG == true)
            {
                if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                {
                     MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCYMotor_Unit;
                    double dPos = PublicVar.CHCYMotorWorkPos  / PublicVar.CHCYMotor_Unit;
                    m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                }
                strTemp = IniSetting.IniReadValue("IO", "IN6");//cy Motor safety
                while (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) == 0 ||1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
                {
                    PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                    PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                }
                if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < 0.05)
                {
                     MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCZMotor_Unit;
                    double dPos = (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1) / PublicVar.CHCZMotor_Unit;
                    m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                }
                while (Math.Abs(PublicVar.CurrentCHCZMotorPos - (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1)) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) == 0)
                {
                    PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                    PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                }
                if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < 0.05)
                {
                    MaxSpeed = PublicVar.CHCZMotorLowVel / PublicVar.CHCZMotor_Unit;
                    double dPos = PublicVar.CHCZMotorWorkPos / PublicVar.CHCZMotor_Unit;
                    m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                }
                while (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorWorkPos) >0.1 ||m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) == 0)
                {
                    PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                    PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                }
            }
            MessageBox.Show("OK");

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
            {
                MessageBox.Show("PLC not ready");
                return;
            }
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 && Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < 0.05)
            {
                MaxSpeed = PublicVar.CHCZMotorLowVel / PublicVar.CHCZMotor_Unit;
                double dPos = (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1) / PublicVar.CHCZMotor_Unit;
                m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
            }
            while (Math.Abs(PublicVar.CurrentCHCZMotorPos - (PublicVar.CHCZMotorWorkPos - PublicVar.CHCZMotorWorkPos1)) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) == 0)
            {
                PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
            }

            if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                  Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorWorkPos) < 0.1)
            {
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCZMotor_Unit;
                double dPos = PublicVar.CHCZMotorInitPos / PublicVar.CHCZMotor_Unit;
                m_ClassMotion.Absolute_Move(m_ClassMotion.CHCZMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
            }
            while (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) == 0)
            {
                PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
            }
            if (Math.Abs(PublicVar.CurrentCHCZMotorPos - PublicVar.CHCZMotorInitPos) < 0.1 && m_ClassMotion.CHCZMotorORG == true)
            {
                if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0)
                {
                    MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCYMotor_Unit;
                    double dPos = PublicVar.CHCYMotorInitPos  / PublicVar.CHCYMotor_Unit;
                    m_ClassMotion.Absolute_Move(m_ClassMotion.CHCYMotor, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                }
                while (Math.Abs(PublicVar.CurrentCHCYMotorPos - PublicVar.CHCYMotorInitPos) > 0.1 || m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) == 0)
                {
                    PublicVar.CurrentCHCYMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCYMotor, false) * PublicVar.CHCYMotor_Unit;
                    PublicVar.CurrentCHCZMotorPos = m_ClassMotion.Get_Axis_Position(m_ClassMotion.CHCZMotor, false) * PublicVar.CHCZMotor_Unit;
                }
            }
            strTemp = IniSetting.IniReadValue("IO", "OUT6");
            m_ClassMotion.Write_Out_Bit(0, Convert.ToUInt16(strTemp), 0);
            MessageBox.Show("OK");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (1 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))
            {
                MessageBox.Show("PLC not ready");
                return;
            }
            btnCYRun.Enabled = false;
            btnCZRun.Enabled = false;
            btnReset.Enabled = false;
            btnCZInitPos.Enabled = false;
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            bool bFlag = false;
            ushort userAxis;
            userAxis = m_ClassMotion.CHCZMotor;
            if (radioHighSpeed.Checked)
                MaxSpeed = PublicVar.HighSpeed / PublicVar.CHCZMotor_Unit;
            else if (radioMidSpeed.Checked)
                MaxSpeed = PublicVar.MidSpeed / PublicVar.CHCZMotor_Unit;
            else if (radioLowerSpeed.Checked)
                MaxSpeed = PublicVar.LowSpeed / PublicVar.CHCZMotor_Unit;
            bFlag = (m_ClassMotion.CHCZMotorPEL );
            bFlag &= (m_ClassMotion.CHCZMotorNEL);

            if (MaxSpeed > 0 && bFlag == false)
            {
                if (m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCYMotor) != 0 && m_ClassMotion.CheckAxisDone(m_ClassMotion.CHCZMotor) != 0 &&
                    (PublicVar.CurrentCHCYMotorPos <= PublicVar.CHCYMotorInitPos || PublicVar.CurrentCHCYMotorPos >= PublicVar.CHCYMotorWorkPos))
                {
                    double dPos = PublicVar .CHCZMotorInitPos  / PublicVar.CHCZMotor_Unit;
                    m_ClassMotion.Absolute_Move(userAxis, Convert.ToInt32(dPos), 0, MaxSpeed, Tacc, Tdec);
                }
            }
            comboBoxDist1.Text = "0";

            btnCYRun.Enabled = true;
            btnCZRun.Enabled = true;
            btnReset.Enabled = true;
            btnCZInitPos.Enabled = true;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void radioRingLed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProductName.Text == "") return;
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                if (radioRingLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light0");
                    if (strTemp == "") { strTemp = "20"; }
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold0");
                    if (strTemp == "") { strTemp = "150"; }
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                }
                else if (radioBackLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light1");
                    if (strTemp == "") { strTemp = "200"; }
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold1");
                    if (strTemp == "") { strTemp = "20"; }
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                }
                else if (radioPosLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light2");
                    if (strTemp == "") { strTemp = "200"; }
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold2");
                    if (strTemp == "") { strTemp = "40"; }
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                }

            }
            catch { }
        }

        private void radioPosLed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProductName.Text == "") return;
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                if (radioRingLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light0");
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold0");
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                }
                else if (radioBackLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light1");
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold1");
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                }
                else if (radioPosLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light2");
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold2");
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                }
            }
            catch { }
        }

        private void radioBackLed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProductName.Text == "") return;
                string strPath = System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog";
                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                if (radioRingLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light0");
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold0");
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                    numericUpDownLightC_ValueChanged(sender, e);
                }
                else if (radioBackLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light1");
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold1");
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                    numericUpDownLightC_ValueChanged(sender, e);
                }
                else if (radioPosLed.Checked)
                {
                    strTemp = IniProg.IniReadValue("Image", "Light2");
                    numericUpDownLightC.Value = Convert.ToInt16(strTemp);
                    strTemp = IniProg.IniReadValue("Image", "Threshold2");
                    numericUpDownThreshold.Value = Convert.ToInt16(strTemp);
                    numericUpDownLightC_ValueChanged(sender, e);
                }
            }
            catch { }
        }

        private void btnZJOGNeg_Click(object sender, EventArgs e)
        {

        }

        private void btnZJOGNeg_MouseDown(object sender, MouseEventArgs e)
        {
            if (chkZSafetysn.Checked == true)
                m_ClassMotion.EnableEmgStop(1);
            double MaxSpeed = 0.1;
            double Tacc = 0.1;
            double Tdec = 0.1;
            ushort userAxis = m_ClassMotion.CHCZMotor;
            MaxSpeed = 10/ PublicVar.CHCZMotor_Unit;
            m_ClassMotion.Jog(userAxis, 0, MaxSpeed, Tacc, Tdec, 0);
        }

        private void btnZJOGNeg_MouseUp(object sender, MouseEventArgs e)
        {
            ushort userAxis = m_ClassMotion.CHCZMotor;
            m_ClassMotion.StopAxis(userAxis);
            m_ClassMotion.EnableEmgStop(0);
            chkZSafetysn.Checked = false;
            btnZJOGNeg.Enabled = false;

        }

        private void chkZSafetysn_CheckedChanged(object sender, EventArgs e)
        {
            CIni SettingCamera = new CIni(System.Windows.Forms.Application.StartupPath + "\\Doc\\CameraSN.ini");
            sDebug = SettingCamera.IniReadValue("debug", "debug");
            if (sDebug == "1")
                btnZJOGNeg.Enabled = true;
            else
                btnZJOGNeg.Enabled = false;

        }

        private void btnModelSet_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            FrmModel frmM = new FrmModel();
            frmM.ShowDialog();
            timer1.Enabled = true;
         }

        private void timer2_checkLimit_Tick(object sender, EventArgs e)
        {
            string strPath;
            strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp = "";
            strTemp = IniSetting.IniReadValue("IO", "IN5");
            if (0 == m_ClassMotion.Read_In(0, Convert.ToUInt16(strTemp)))//safety
            {
                FSW.Invoke(new System.Action(() =>
                {
                    FSW.Dispose();
                }));
            }
            
        }

        private void btn_LoadParam_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath;
                strPath = System.Windows.Forms.Application.StartupPath + "\\DOC";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                strPath += "\\Prog";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                strPath += "\\" + cmbProductName.Text.Trim() + ".ini";
                CIni IniProg = new CIni(strPath);
                string strTemp = "";
                if (cmbProductName.Text != "" && tabControl1.SelectedIndex == 1)
                {                    
                        dataGridView2.RowCount = 1;
                        dataGridView2.Refresh();
                        strTemp = IniProg.IniReadValue("Total", "Count");
                        int iTotal = Convert.ToInt16(strTemp);
                        dataGridView2.RowCount = iTotal + 1;
                        for (int i = 1; i <= iTotal; i++)
                        {
                            strTemp = IniProg.IniReadValue(i.ToString(), "NO.");
                            dataGridView2[0, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "CenterX");
                            dataGridView2[1, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "CenterY");
                            dataGridView2[2, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "InnerR");
                            dataGridView2[3, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "OuterR");
                            dataGridView2[4, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "StartA");
                            dataGridView2[5, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "EndA");
                            dataGridView2[6, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "MotorX");
                            dataGridView2[7, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "MotorY");
                            dataGridView2[8, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "R");
                            dataGridView2[9, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "Color");
                            dataGridView2[10, i].Value = strTemp;
                            strTemp = IniProg.IniReadValue(i.ToString(), "Type");
                            dataGridView2[11, i].Value = strTemp;
                        }

                    
                }
                if (cmbProductName.Text != "" && tabControl1.SelectedIndex == 2)
                {
                    strTemp = IniProg.IniReadValue("Param", "L1Set");
                    txtL1Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "L2Set");
                    txtL2Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "W1Set");
                    txtW1Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "W2Set");
                    txtW2Set.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "No1");
                    txtLowerLimit.Text = strTemp;
                    strTemp = IniProg.IniReadValue("Param", "No2");
                    txtUpperLimit.Text = strTemp;
                }
            }
            catch { MessageBox.Show("加载参数错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btn_FacSetting_Click(object sender, EventArgs e)
        {
            fRmLogin frmlogin = new fRmLogin();
            frmlogin.ShowDialog();
            if (PublicVar.bPasswordManager)
            {
                comboBoxDist1.Enabled = true;
                chkZSafetysn.Enabled=true;
                btnZJOGNeg.Enabled=true;
                checkBoxTest.Enabled=true;
                btnCZInitPos.Enabled=true;
                btnCYRun.Enabled=true;
                btnCZRun.Enabled=true;
                btnReset.Enabled = true;
                btnTest.Enabled = true;

                label21.Visible=true;
                txtCycleTime.Visible=true;
                chkRefCircle.Visible=true;
                txtRefCircleDia.Visible = true;
                btnTest.Visible = true;
                chkCalibration.Visible = true;

            }
            else if (PublicVar.bPasswordOperator)
            {

                comboBoxDist1.Enabled = false;
                chkZSafetysn.Enabled = false;
                btnZJOGNeg.Enabled = false;
                checkBoxTest.Enabled = false;
                btnCZInitPos.Enabled = false;
                btnCYRun.Enabled = false;
                btnCZRun.Enabled = false;
                btnReset.Enabled = false;
                btnTest.Enabled = false;

                label21.Visible = false;
                txtCycleTime.Visible = false;
                chkRefCircle.Visible = false;
                txtRefCircleDia.Visible = false;
                btnTest.Visible = false;
                chkCalibration.Visible = true;
            }
        }

        private void rB_solidCircle_CheckedChanged(object sender, EventArgs e)
        {
            listBoxDirectory.SelectedIndex = 1;
            listBoxParity.SelectedIndex = 1;
        }

        private void rB_unsolidCircle_CheckedChanged(object sender, EventArgs e)
        {
            listBoxDirectory.SelectedIndex = 1;
            listBoxParity.SelectedIndex = 0;
        }

        private void cmbProductName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                //1,校验名称
                string cmbName;
                string[] checkName;
                try
                {
                    cmbName = cmbProductName.Text;
                    checkName = cmbName.Split('-');
                    if (checkName[1] == "") throw new Exception();
                    var fs = File.Create(System.Windows.Forms.Application.StartupPath + "\\DOC\\Prog\\" + cmbName + ".ini");
                    fs.Close();
                }
                catch
                {
                    MessageBox.Show("命名不规范，请重新命名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //2.询问建立几个点的圆参数
                FormAskNum formAskNum = new FormAskNum(cmbProductName.Text);
                formAskNum.ShowDialog();


                //3.刷新列表
                string strPath;
                strPath = Directory.GetCurrentDirectory();
                string str1 = strPath + "\\Doc\\Prog";
                if (Directory.Exists(str1))
                {
                    int iUpperBound = Directory.GetFiles(str1).GetLength(0);//获取目录的数量
                    string[] strFile = new string[iUpperBound];//按照目录数量建立数组
                    strFile = Directory.GetFiles(str1);
                    cmbProductName.Items.Clear();
                    for (int i = 0; i < iUpperBound; i++)
                    {
                        char[] delimiterChars = { '\\', '.', '\t' };//要删除的元素
                        string[] words = strFile[i].Split(delimiterChars);
                        int iwords = words.GetUpperBound(0);
                        cmbProductName.Items.Add(words[iwords - 1]);
                    }
                }

                //4.选中刚刚的条目
                cmbProductName.Sorted = true;
                int NumProductName = cmbProductName.Items.IndexOf(cmbName);                               //需要校验是否为选中当前产品参数
                cmbProductName.SelectedItem = NumProductName;

                //5.跳转到产品参数
                tabControl1.SelectedIndex = 2;

                //6.读取当前产品参数到指定位置
                cmbProductName_SelectedIndexChanged(sender, e);

                //7.聚焦到参数编辑控件
                btnParamEdit.Focus();

                
            }
        }

        private void txtL1Set_TextChanged(object sender, EventArgs e)
        {
            txtL2Set.Text = txtL1Set.Text;
        }

        private void txtW2Set_TextChanged(object sender, EventArgs e)
        {
            //txtW2Set.Text = txtW1Set.Text;
        }

        private void txtW1Set_TextChanged(object sender, EventArgs e)
        {
            txtW2Set.Text = txtW1Set.Text;
        }

      


 
     }
}
