using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace ClassificationMachine
{
    class ClassRepairData
    {
        ///////////////////////////////////////////////
        public double[,] RepairData = null;
        private List<List<string>> ReadData(string strFile)
        {
           ArrayList sAddIndex = new ArrayList();
           List<List<string>> RepairData = new List<List<string>>();
           StreamReader sReader = new StreamReader(strFile);
            string strRead;
            while (!sReader.EndOfStream)
            {
                strRead = sReader.ReadLine();
                sAddIndex.Add(strRead);
            }
            sReader.Close();
            string sLine = sAddIndex[0].ToString();
            char[] delimiterChars = { ',', '\t' };
            string[] words = sLine.Split(delimiterChars);
            for (int i = 0; i < sAddIndex.Count; i++)
            {
                sLine = sAddIndex[i].ToString();
                sLine = sLine.Substring(0, sLine.Length - 1);
                words = sLine.Split(delimiterChars);
                List<string> item = new List<string>(words);
                RepairData.Add(item);
            }
            sAddIndex.Clear();
            return RepairData;
        }
        public double[]  RepairDataXY(string strPath1, double XPrvCurrent, double YPrvCurrent, double curX, double curY, 
                                    double InterValX, double InterValY,double XStartPos,double YStartPos)
        {
                string strPath = strPath1 + "\\DOC\\RepairDataMachine";
                string strFile = "";
                bool bRepair = false;
                List<List<string>> RepairPosX = new List<List<string>>();
                List<List<string>> RepairNegX = new List<List<string>>();
                strFile = strPath + "\\X.dat";
                if (File.Exists(strFile))
                {
                    bRepair = true;
                    RepairPosX = ReadData(strFile);
                    RepairNegX = RepairPosX;
                }
                strFile = strPath + "\\_X.dat";
                if (File.Exists(strFile))
                {
                    bRepair = true;
                    RepairNegX = ReadData(strFile);
                }
                List<List<string>> RepairPosY = new List<List<string>>();
                List<List<string>> RepairNegY = new List<List<string>>();
                strFile = strPath + "\\Y.dat";
                if (File.Exists(strFile))
                {
                    bRepair = true;
                    RepairPosY = ReadData(strFile);
                    RepairNegY = RepairPosY;
                }
                strFile = strPath + "\\_Y.dat";
                if (File.Exists(strFile))
                {
                    bRepair = true;
                    RepairNegY = ReadData(strFile);
                }
                double X = curX;// -XStartPos;
                double Y = curY;// -YStartPos;
                double x0 = 0, y0 = 0;
                double x2 = 0, y2 = 0;
                double x3 = X, y3 = Y;

                if (bRepair == true)
                {
                    try
                    {
                        int i, j;
                        i = Convert.ToInt16((curX - XStartPos) / InterValX);
                        j = Convert.ToInt16((curY - YStartPos) / InterValY);
                        if (curX > XPrvCurrent)
                            X = Convert.ToDouble(RepairPosX[j][i]);
                        else
                            X = Convert.ToDouble(RepairNegX[j][i]);
                        if (curY > YPrvCurrent)
                            Y = Convert.ToDouble(RepairPosY[j][i]);
                        else
                            Y = Convert.ToDouble(RepairNegY[j][i]);

                        x0 = InterValX * i;
                        y0 = InterValY * j;
                        try
                        {
                            if (curX - X > 0)
                                x2 = Convert.ToDouble(RepairPosX[j][i + 1]);
                            else
                                x2 = Convert.ToDouble(RepairNegX[j][i - 1]);
                            x3 = (curX - X) * InterValX / Math.Abs(x2 - X);
                        }
                        catch { x3 = curX - X; }
                        try
                        {
                            if (curY - Y > 0)
                                y2 = Convert.ToDouble(RepairPosY[j + 1][i]);
                            else
                                y2 = Convert.ToDouble(RepairNegY[j - 1][i]);
                            y3 = (curY - Y) * InterValY / Math.Abs(y2 - Y);
                        }
                        catch { y3 = curY - Y; }
                        x3 += x0 +XStartPos;
                        y3 += y0 +YStartPos;
                    }
                    catch
                    {
                        x3 = curX;
                        y3 = curY;
                    }
                }
                double[] xy = new double[2];
                xy[0] = x3;
                xy[1] = y3;
             return xy;
        }
        public void AutoCalcuRepairData(string strPath1)
        {
            string strPath = strPath1 + "\\DOC\\RepairDataMachine";
            string strFile = "";
            List<List<string>> RepairPos = new List<List<string>>();
            strFile = strPath + "\\X.dat";
            if (File.Exists(strFile))
            {
                string strFile1 = strPath + "\\X0.dat";
                if (!File.Exists(strFile1))
                    File.Copy(strFile, strFile1);
                for (int k = 0; k < 2; k++)
                {
                    RepairPos = ReadData(strFile);
                    if (RepairPos[0][0] != "0.0000")
                    {

                        for (int j = 0; j < RepairPos.Count; j++)
                        {
                            int n = 0;
                            int m = 1;
                            for (int i = 0; i < RepairPos[j].Count; i++)
                            {
                                if (i == 0 && RepairPos[j][i] == "0.0000")
                                {
                                    RepairPos[j][i] = RepairPos[j - 1][i];
                                }
                                else
                                {
                                    string strTemp = RepairPos[j][i];
                                    if (strTemp == "0.0000")
                                    {
                                        if (i == 0 || i == RepairPos[j].Count - 1)
                                        {
                                            if (RepairPos[j - 1][i] == "0.0000" && j == 0 && i > 2)
                                            {
                                                double dd = Convert.ToDouble(RepairPos[j][i - 1]) + (Convert.ToDouble(RepairPos[j][i - 2]) - Convert.ToDouble(RepairPos[j][i - 3]));
                                                RepairPos[j][i] = dd.ToString("f4");
                                            }
                                            else
                                                RepairPos[j][i] = RepairPos[j - 1][i];
                                        }
                                        else
                                        {
                                            n = i;
                                        }
                                    }
                                    else
                                    {
                                        if (n == 0)
                                            m = i;
                                        if (m < n && i > 0)
                                        {
                                            double d1 = Convert.ToDouble(RepairPos[j][i - 1 - (n - m)]);
                                            double d2 = Convert.ToDouble(RepairPos[j][i]);
                                            double d = (d2 - d1) / (n - m + 1);
                                            for (int p = m + 1; p < n + 1; p++)
                                            {
                                                RepairPos[j][p] = (d1 + d * (p - m)).ToString("f3");
                                            }
                                        }
                                        n = 0;
                                    }
                                }
                            }
                        }
                    }
                    //////////////////////save///////////////
                    StreamWriter swX = new StreamWriter(strFile);
                    for (int j = 0; j < RepairPos.Count; j++)
                    {
                        string str1 = "";
                        try
                        {
                            for (int i = 0; i < RepairPos[j].Count; i++)
                            {
                                str1 += RepairPos[j][i] + ",";
                            }
                        }
                        catch { }
                        str1.Substring(0, str1.Length - 1);
                        swX.WriteLine(str1);
                    }
                    swX.Close();
                }
            }
            //////////////////////////////////////////////////////////////////////////////
            strFile = strPath + "\\Y.dat";
            if (File.Exists(strFile))
            {
                string strFile1 = strPath + "\\Y0.dat";
                if (!File.Exists(strFile1))
                    File.Copy(strFile, strFile1);
                for (int k = 0; k < 2; k++)
                {
                    RepairPos = ReadData(strFile);
                    if (RepairPos[0][0] != "0.0000")
                    {
                        for (int i = 0; i < RepairPos[0].Count; i++)
                        {
                            int n = 0;
                            int m = 1;
                            for (int j = 0; j < RepairPos.Count; j++)
                            {
                                if (j == 0 && i > 0 && RepairPos[j][i] == "0.0000")
                                {
                                    RepairPos[j][i] = RepairPos[j][i - 1];
                                }
                                else
                                {
                                    string strTemp = RepairPos[j][i];
                                    if (strTemp == "0.0000")
                                    {
                                        if (j == 0 || j == RepairPos.Count - 1)
                                        {
                                            if (RepairPos[j][i - 1] == "0.0000" && i == 0 && j > 2)
                                            {
                                                double dd = Convert.ToDouble(RepairPos[j - 1][i]) + (Convert.ToDouble(RepairPos[j - 2][i]) - Convert.ToDouble(RepairPos[j - 3][i]));
                                                RepairPos[j][i] = dd.ToString("f4");
                                            }
                                            else
                                                RepairPos[j][i] = RepairPos[j][i - 1];
                                        }
                                        else
                                        {
                                            n = j;
                                        }
                                    }
                                    else
                                    {
                                        if (n == 0)
                                            m = j;
                                        if (m < n && j > 0)
                                        {
                                            double d1 = Convert.ToDouble(RepairPos[j - 1 - (n - m)][i]);
                                            double d2 = Convert.ToDouble(RepairPos[j][i]);
                                            double d = (d2 - d1) / (n - m + 1);
                                            for (int p = m + 1; p < n + 1; p++)
                                            {
                                                RepairPos[p][i] = (d1 + d * (p - m)).ToString("f3");
                                            }
                                        }
                                        n = 0;
                                    }
                                }
                            }
                        }
                    }
                    //////////////////////save///////////////
                    StreamWriter swY = new StreamWriter(strFile);
                    for (int j = 0; j < RepairPos.Count; j++)
                    {
                        string str1 = "";
                        try
                        {
                            for (int i = 0; i < RepairPos[j].Count; i++)
                            {
                                str1 += RepairPos[j][i] + ",";
                            }
                        }
                        catch { }
                        str1.Substring(0, str1.Length - 1);
                        swY.WriteLine(str1);
                    }
                    swY.Close();
                }
            }
        }
    }
    class RepairDataCollection
    {
        private double _Value = 0;
        private int _X = 0;
        private int _Y = 0;
        public RepairDataCollection(double dValue, int X, int Y)
        {
            this._Value = dValue;
            this._X = X;
            this._Y = Y;
        }
        public double dValue
        {
            get { return _Value; }
            set { _Value = value; }
        }
        public int X
        {
            get { return _X; }
            set { _X = value; }
        }
        public int Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
    }
}
