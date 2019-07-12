using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassificationMachine
{
    public  enum CalcuTool { None = 0, PointToPoint, PointToLine, LineToLine, CircleToCirlce, Line, Point, Circle, ManyPointtoCircle }
    public enum ModeTool { Manual, Auto, Navigate, Program }
    public class ClassPublicTool
    {
        public static CalcuTool m_CalcuTool;
        public static ModeTool m_ModeTool;
        public static int ManyPointNum;
        /// <summary>
        /// ////////////////////////////////////////////////////////////////
        /// </summary>
        public double[] TranslateXY(double UpX, double UpY, double DnX, double DnY, double x, double y)
        { //使用直线方程算距离的公式
            double[] xy = new double[2];
            //try
            //{
            //    xy[0] = ((DnY - UpY) * x + (UpX - DnX) * y + (UpY - DnY) * UpX + (DnX - UpX) * UpY) / Math.Sqrt((DnY - UpY) * (DnY - UpY) + (UpX - DnX) * (UpX - DnX));
            //    xy[1] = ((UpY - DnY) * y + (DnX - UpX) * x + (DnY - UpY) * UpY + (UpX - DnX) * UpX) / Math.Sqrt((DnY - UpY) * (DnY - UpY) + (UpX - DnX) * (UpX - DnX));
            //}
            //catch 
            {
                xy[0] = x;
                xy[1] = y;
            }
            return xy;
        }
        public double[] TranslateXY(double UpX, double UpY, double DnX, double DnY, double DistY, double x, double y)
        {//图像旋转
            double[] xy = new double[2];
            try
            {
                double dist = 0;
                double dAngle = 0;
                dist = Math.Sqrt((UpX - DnX) * (UpX - DnX) + (UpY - DnY) * (UpY - DnY));
                double dCosa = DistY / dist;
                dAngle = Math.Atan(Math.Sqrt(dCosa * dCosa - 1));
                if (dCosa > 0)
                    dAngle = Math.Abs(dAngle);
                if (dCosa <= 0)
                    dAngle = 0 - Math.Abs(dAngle);
                xy[0] = x * Math.Cos(dAngle) - y * Math.Sin(dAngle);
                xy[1] = y * Math.Cos(dAngle) + x * Math.Sin(dAngle);
            }
            catch
            {
                xy[0] = x;
                xy[1] = y;
            }
            return xy;
        }
        /////////////////////////y=a0+a1*x 返回值则为a0 a1,y=a0+a1*x+a2*x*x 返回值则为a0 a1 a2///////////////////////////////////////
        ///<summary>
        ///用最小二乘法拟合二元多次曲线
        ///</summary>
        ///<param name="arrX">已知点的x坐标集合</param>
        ///<param name="arrY">已知点的y坐标集合</param>
        ///<param name="length">已知点的个数</param>
        ///<param name="dimension">方程的最高次数</param>

        public double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)//二元多次线性方程拟合曲线
        {
            int n = dimension + 1;                  //dimension次方程需要求 dimension+1个 系数
            double[,] Guass = new double[n, n + 1];      //高斯矩阵 例如：y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }
            double dMin = 100000000;
            double dMax = -100000;
            for (int j = 0; j < arrX.GetLength(0); j++)
            {
                if (arrX[j] > dMax) dMax = arrX[j];
                if (arrX[j] < dMin) dMin = arrX[j];
            }
            if (dMax - dMin > 1)
                return ComputGauss(Guass, n);
            else
            {
                double[] d = new double[2];
                d[0] = arrX[0];
                d[1] = 0;
                return d;
            }

        }
        public double SumArr(double[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }
        public double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }
        public double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];
            for (i = 0; i < n; i++) x[i] = 0.0;//初始化

            for (j = 0; j < n; j++)
            {
                max = 0;
                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }


                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;
                    }
                }
                if (0 == max)
                {
                    // "此线性方程为奇异线性方程" 
                    return x;
                }

                for (i = j + 1; i < n; i++)
                {
                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);
                    }
                }

            }//结束for (j=0;j<n;j++)

            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }
                x[i] = (Guass[i, n] - s) / Guass[i, i];
            }
            return x;
        }//返回值是函数的系数,例如：y=a0+a1*x 返回值则为a0 a1,例如：y=a0+a1*x+a2*x*x 返回值则为a0 a1 a2

        public static  double[]LeastSquaresFitting(int m_nNum,double[] PointX,double[] PointY)//拟合圆
        {
            double[] fCircleXYR = new double[3];
            for (int j = 0; j < 3; j++)
            {
                fCircleXYR[j] = 0.0;
            }
            if (m_nNum < 3)
            {
                return fCircleXYR;
            }

            int i = 0;

            double X1 = 0;
            double Y1 = 0;
            double X2 = 0;
            double Y2 = 0;
            double X3 = 0;
            double Y3 = 0;
            double X1Y1 = 0;
            double X1Y2 = 0;
            double X2Y1 = 0;

            for (i = 0; i < m_nNum; i++)
            {
                X1 = X1 + PointX[i];
                Y1 = Y1 + PointY[i];
                X2 = X2 + PointX[i] * PointX[i];
                Y2 = Y2 + PointY[i] * PointY[i];
                X3 = X3 + PointX[i] * PointX[i] * PointX[i];
                Y3 = Y3 + PointY[i] * PointY[i] * PointY[i];
                X1Y1 = X1Y1 + PointX[i] * PointY[i];
                X1Y2 = X1Y2 + PointX[i] * PointY[i] * PointY[i];
                X2Y1 = X2Y1 + PointX[i] * PointX[i] * PointY[i];
            }

            double C, D, E, G, H, N;
            double a, b, c;
            N = m_nNum;
            C = N * X2 - X1 * X1;
            D = N * X1Y1 - X1 * Y1;
            E = N * X3 + N * X1Y2 - (X2 + Y2) * X1;
            G = N * Y2 - Y1 * Y1;
            H = N * X2Y1 + N * Y3 - (X2 + Y2) * Y1;
            a = (H * D - E * G) / (C * G - D * D);
            b = (H * C - E * D) / (D * D - G * C);
            c = -(a * X1 + b * Y1 + X2 + Y2) / N;

            double A, B, R;
            A = a / (-2);
            B = b / (-2);
            R =Math.Sqrt(a * a + b * b - 4 * c) / 2;

            fCircleXYR[0] = A;//x
            fCircleXYR[1] = B;//y
            fCircleXYR[2] = R;//r
            return fCircleXYR;
        }

    
    }
}
