using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing ;
using System.Windows .Forms ;
using System.Management;
using System.IO.Ports;
using System.IO;

namespace ClassificationMachine
{
   class PublicVar
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
        public static string GetSerial(int iNo = 0)
        {
            string madAddr = null;
            if (iNo == 1000/*read*/ || iNo == 100/*save*/)
            {//硬盘
                ManagementClass mcHardDisk = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection mocHardDisk = mcHardDisk.GetInstances();
                foreach (ManagementObject mo in mocHardDisk)
                {
                    madAddr = mo.Properties["Model"].Value.ToString();
                    break;
                }
                if (iNo == 100)//save
                {
                    if (!File.Exists("c:\\hhiatsn.ini") && madAddr != null)
                    {
                        CIni WriteMac = new CIni("c:\\hhiatsn.ini");
                        WriteMac.IniWriteValue("SN", "ID", madAddr);
                        DateTime datetime = DateTime.Now;
                        string[] d = new string[6];
                        d[0] = "00" + datetime.Day.ToString();//day
                        d[1] = "00" + datetime.Month.ToString();//month
                        d[2] = datetime.Year.ToString();
                        d[3] = "03";//len
                        d[4] = d[2].Substring(0, 2);//year-L
                        d[5] = d[2].Substring(2, 2);//year_R
                        string s = "";
                        s = d[0].Substring(d[0].Length - 2, 2) + d[1].Substring(d[1].Length - 2, 2) + d[4] + d[3] + d[5];
                        WriteMac.IniWriteValue("Time", "T", s);
                    }
                }
            }
            return madAddr;
        }
        public static bool License(int iPwd)
        {
            string strSerial = "";
            strSerial = GetSerial(iPwd);
            CIni ReadMac = new CIni("c:\\hhiatsn.ini");
            string strID = ReadMac.IniReadValue("SN", "ID");
            if (strSerial != strID && strID != "hhiat123")
            {
                MessageBox.Show("System Error");
                
                return false;
            }
            string sTime = ReadMac.IniReadValue("Time", "T");
            if (sTime != "-1")
            {
                DateTime datetime = DateTime.Now;
                string[] d = new string[6];
                d[0] = sTime.Substring(0, 2);//day
                d[1] = sTime.Substring(2, 2);//month
                d[2] = sTime.Substring(4, 2);//year_L
                d[3] = sTime.Substring(6, 2);//len
                d[4] = sTime.Substring(8, 2);//Year_R
                DateTime datetimeStart = new DateTime(Convert.ToInt16(d[2] + d[4]), Convert.ToInt16(d[1]), Convert.ToInt16(d[0]), 0, 0, 0);
                if ((datetime - datetimeStart).Days > Convert.ToInt16(d[3]) * 30 || (datetime - datetimeStart).Days < 0)
                {
                    d[0] = "00" + datetime.Day.ToString();//day
                    d[1] = "00" + datetime.Month.ToString();//month
                    d[2] = datetime.Year.ToString();
                    d[3] = "00";//len
                    d[4] = d[2].Substring(0, 2);//year-L
                    d[5] = "00";//year_R
                    string s = "";
                    s = d[0].Substring(d[0].Length - 2, 2) + d[1].Substring(d[1].Length - 2, 2) + d[4] + d[3] + d[5];
                    ReadMac.IniWriteValue("Time", "T", s);
                   // MessageBox.Show("System Time Error");
                    return false;
                }
            }
            return true;
        }
        public static bool bPasswordOperator=false, bPasswordEngineer=false, bPasswordManager=false;
        public static double CHXMotorInitVel, CHXMotorACC, CHXMotorDEC, CurrentCHXMotorPos;
        public static double CHYMotorInitVel, CHYMotorACC, CHYMotorDEC, CurrentCHYMotorPos;
        public static double CHCYMotorInitVel, CHCYMotorACC, CHCYMotorDEC, CurrentCHCYMotorPos;
        public static double CHCZMotorInitVel, CHCZMotorACC, CHCZMotorDEC, CurrentCHCZMotorPos;
        public static double CHYMotorHomeInitVel, CHYMotorHomeMaxVel, CHYMotorHomeACC, CHYMotorHomeDEC, CHYMotorSpeed, CHYMotorInitPos, CHYMotor_Unit, CHYEncoder_Unit;
        public static double CHXMotorHomeInitVel, CHXMotorHomeMaxVel, CHXMotorHomeACC, CHXMotorHomeDEC, CHXMotorSpeed, CHXMotorInitPos, CHXMotor_Unit, CHXEncoder_Unit;
        public static double CHCYMotorHomeInitVel, CHCYMotorHomeMaxVel, CHCYMotorHomeACC, CHCYMotorHomeDEC, CHCYMotorSpeed, CHCYMotorInitPos, CHCYMotor_Unit,CHCYMotorWorkPos;
        public static double CHCZMotorHomeInitVel, CHCZMotorHomeMaxVel, CHCZMotorHomeACC, CHCZMotorHomeDEC, CHCZMotorSpeed, CHCZMotorInitPos, CHCZMotor_Unit, CHCZMotorWorkPos, CHCZMotorWorkPos1, CHCZMotorLowVel;
        public static double CameraX_Unit,CameraY_Unit,CameraPos_Unit, CHXCaliCorr, CHYCaliCorr, CHXRepairAngle, CHYRepairAngle;
        public static double HighSpeed, MidSpeed, LowSpeed;
        public static double RepairStartX, RepairStartY, RepairIntervalX, RepairIntervalY,StandardPanelLength,StandardPanelWidth;
        public static double MasterPos1X, MasterPos1Y, MasterPos2X, MasterPos2Y, MasterPos3X, MasterPos3Y, MasterPos4X, MasterPos4Y;
        public static int MasterNumHZ;
        public static double MasterL1, MasterL2, MasterW1, MasterW2;
        public static double PosCameraSetX, PosCameraSetY, PosCameraSetAngle;
        public static double CorrX1K, CorrX1Offset, CorrX2K, CorrX2Offset, CorrY1K, CorrY1Offset, CorrY2K, CorrY2Offset;
        public static int iLedSel;
       //Excel 储存对角线数值（测量值除以标准值并保留5位小数）
        public static double bL13, bL24;
       //储存两对边测量值的平均值
        public static double W_Average, L_Average;
       //强制关闭程序
        public static bool ForceClose = false;
   }
 }
