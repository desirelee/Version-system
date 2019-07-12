using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Drawing;
using System.IO.Ports;

namespace ImageProcessHHiat
{
    using NationalInstruments.Vision;
    using NationalInstruments.Vision.Analysis;
    using NationalInstruments.Vision.WindowsForms;

  public  class MachineTool
    {
      private delegate void OutputInfoDelegate(TextBox txtInfo, string info, Color backcolor, Color forecolor);
      public static void OutputInfo(TextBox txtInfo, string info, Color backcolor, Color forecolor)
      {
          if (string.IsNullOrEmpty(info) || txtInfo.IsDisposed) return;

          if (txtInfo.InvokeRequired)
          {
              OutputInfoDelegate d = OutputInfo;
              txtInfo.BeginInvoke(d, txtInfo, info, backcolor, forecolor);
          }
          else
          {
              if (txtInfo.MaxLength < 32767 && !txtInfo.Visible) return;

              if (!string.IsNullOrEmpty(txtInfo.Text))
              {
                  txtInfo.BackColor = backcolor;
                  txtInfo.ForeColor = forecolor;
                  txtInfo.Clear();
                  if (txtInfo.Text.Length >= txtInfo.MaxLength - info.Length)
                  {
                      txtInfo.Text = info;
                  }
                  else
                  {
                      //txtInfo.Clear();
                      txtInfo.SelectionStart = txtInfo.Text.Length;
                      txtInfo.SelectedText = "\r\n" + info;
                  }
              }
              else
              {
                  //txtInfo.Clear();
                  txtInfo.Text = info;
              }
          }
      }

      public static double FindCircleHistogramMean(VisionImage image, OvalContour vaRect2)
      {
          Roi roi = new Roi();
          // Creates a new RectangleContour using the given values.
        
          roi.Add(vaRect2);
          // Histogram Grayscale
          double HistogramMean = -100;
          using (VisionImage imageMask = new VisionImage(ImageType.U8, 7))
          {
              PixelValue fillValue = new PixelValue(255);
              Range intervalRange = new Range(0, 0);

              Algorithms.RoiToMask(imageMask, roi, fillValue, image);

              // Calculates and returns statistical parameters on the image.
               HistogramMean = Algorithms.Histogram(image, 256, intervalRange, imageMask).Mean;
            
          }
          roi.Dispose();
          return HistogramMean;
         
      }

      public static double Get2LineAngle(PointContour p1, PointContour p2, PointContour p3, PointContour p4)
      {
          double baseLine = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
          double newLine = Math.Atan2(p4.Y - p3.Y, p4.X - p3.X);
          double deltaAngle = newLine - baseLine;
          return deltaAngle * 180 / Math.PI;
      }
      public static void LearnPattern(ImageViewer SourceImage,float fUpper=0,float fLower=0)
      {
          using (VisionImage plane = new VisionImage(ImageType.U8, 7))
          {
              // Extract the green color plane and copy it to the main image.
              if (SourceImage.Image.Type == ImageType.Rgb32)
              {
                  Algorithms.ExtractColorPlanes(SourceImage.Image, NationalInstruments.Vision.ColorMode.Rgb, null, plane, null);
                  Algorithms.Copy(plane, SourceImage.Image);
              }
          }
//          Algorithms.LearnPattern2(SourceImage.Image);
          OvalContour vaRect2 = new OvalContour(0, 0, 0, 0);
          Roi roi = new Roi();
          roi.Add(vaRect2);
          // Histogram Grayscale
          using (VisionImage imageMask = new VisionImage(ImageType.U8, 7))
          {
              RotationAngleRange ra = new RotationAngleRange(fLower ,fUpper );
              PixelValue fillValue = new PixelValue(255);
              Algorithms.RoiToMask(imageMask, roi, fillValue, SourceImage.Image);
              Algorithms.LearnPattern2(SourceImage.Image, imageMask, MatchingAlgorithm.MatchGrayValuePyramid, ra);
          }
          roi.Dispose();
         
      }
      public static PointContour MatchPattern(VisionImage  SourceImage, RectangleContour vaRect, string TemplateFile, int vaNumMatchesRequested, float vaMinMatchScore, float fUpper = 0, float fLower = 0)
      {
          PointContour point = new PointContour();
          point.X = -10000;
          point.Y = -10000;
          // Creates a new, empty region of interest.
          Roi roi = new Roi();
          // Creates a new RotatedRectangleContour using the given values.
          PointContour vaCenter = new PointContour(vaRect.Left + vaRect.Width / 2, vaRect.Top + vaRect.Height / 2);
          RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, vaRect.Width - 50, vaRect.Height - 50, 0);

          roi.Add(vaRotatedRect);
          // MatchPattern Grayscale
          // string TemplateFile = "D:\\t1.png";
          MatchingAlgorithm matchAlgorithm = MatchingAlgorithm.MatchGrayValuePyramid;
          float[] minAngleVals = { fLower, 0 };
          float[] maxAngleVals = { fUpper, 0 };
          int[] advancedOptionsItems = { 102, 106, 107, 108, 109, 111, 112, 113, 103, 104, 105, 100 };
          double[] advancedOptionsValues = { 10, 300, 0, 6, 1, 20, 10, 20, 1, 20, 0, 5 };
          int numberAdvOptions = 12;
          //int vaNumMatchesRequested = 1;
          //float vaMinMatchScore = 800;

          using (VisionImage imageTemplate = new VisionImage(ImageType.U8, 7))
          {
              Collection<PatternMatchReport> patternMatchingResults = new Collection<PatternMatchReport>();

              // Read the image template.
              imageTemplate.ReadVisionFile(TemplateFile);
              // Set the angle range.
              Collection<RotationAngleRange> angleRange = new Collection<RotationAngleRange>();
              for (int i = 0; i < 2; ++i)
              {
                  angleRange.Add(new RotationAngleRange(minAngleVals[i], maxAngleVals[i]));
              }

              // Set the advanced options.
              Collection<PMMatchAdvancedSetupDataOption> advancedMatchOptions = new Collection<PMMatchAdvancedSetupDataOption>();
              for (int i = 0; i < numberAdvOptions; ++i)
              {
                  advancedMatchOptions.Add(new PMMatchAdvancedSetupDataOption((MatchSetupOption)advancedOptionsItems[i], advancedOptionsValues[i]));
              }

              // Searches for areas in the image that match a given pattern.
              patternMatchingResults = Algorithms.MatchPattern3(SourceImage, imageTemplate, matchAlgorithm, vaNumMatchesRequested, vaMinMatchScore, angleRange, roi, advancedMatchOptions);
              string sPatterScore = "";
              if (patternMatchingResults.Count > 0)
              {
                  for (int i = 0; i < 1/* patternMatchingResults.Count*/; ++i)
                  {
                      point.X = patternMatchingResults[i].Position.X;
                      point.Y = patternMatchingResults[i].Position.Y;
                      sPatterScore += " " + patternMatchingResults[i].Score.ToString();
                      //SourceImage.Overlays.Default.AddRectangle(new RectangleContour(point.X - imageTemplate.Width / 2 - 1, point.Y - imageTemplate.Height / 2 - 1, imageTemplate.Width, imageTemplate.Height), Rgb32Value.GreenColor);
                      LineContour l1 = new LineContour();
                      l1.Start.X = point.X - (imageTemplate.Width / 2 - 3);
                      l1.Start.Y = point.Y;
                      l1.End.X = point.X + (imageTemplate.Width / 2 - 3);
                      l1.End.Y = point.Y;
                      SourceImage.Overlays.Default.AddLine(l1, Rgb32Value.RedColor );
                      LineContour l2 = new LineContour();
                      l2.Start.X = point.X;
                      l2.Start.Y = point.Y - (imageTemplate.Height / 2 - 3);
                      l2.End.X = point.X;
                      l2.End.Y = point.Y + (imageTemplate.Height / 2 - 3);
                      SourceImage.Overlays.Default.AddLine(l2, Rgb32Value.RedColor);
                  }
              }

          }
          roi.Dispose();
          return point;
      }

    }
}
