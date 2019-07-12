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
    public partial class FrmMachineParam : Form
    {
         public FrmMachineParam()
        {
            InitializeComponent();
        }
        private void EnableControl()
        {
            txtCheckCameraXUnit.Enabled = PublicVar .bPasswordManager;
            txtCheckCameraYUnit.Enabled = PublicVar.bPasswordManager;
            txtMotorXUnit.Enabled = PublicVar.bPasswordManager;
            txtMotorYUnit.Enabled = PublicVar.bPasswordManager;
            txtLeftTopX.Enabled = PublicVar.bPasswordManager;
            txtLeftTopY.Enabled = PublicVar.bPasswordManager;
            txtRightTopX.Enabled = PublicVar.bPasswordManager;
            txtRightTopY.Enabled = PublicVar.bPasswordManager;
            txtRightBottomX.Enabled = PublicVar.bPasswordManager;
            txtRightBottomY.Enabled = PublicVar.bPasswordManager;

            txtRun1.Enabled = PublicVar.bPasswordManager;
            txtRun2.Enabled = PublicVar.bPasswordManager;
            txtRun3.Enabled = PublicVar.bPasswordManager;
            txtRun7.Enabled =( PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun8.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun5.Enabled = PublicVar.bPasswordManager;
            txtRun6.Enabled = PublicVar.bPasswordManager;
            txtRun4.Enabled = PublicVar.bPasswordManager;

            txtHome1.Enabled = PublicVar.bPasswordManager;
            txtHome2.Enabled = PublicVar.bPasswordManager;
            txtHome3.Enabled = PublicVar.bPasswordManager;
            txtHome4.Enabled = PublicVar.bPasswordManager;
            txtHome5.Enabled = PublicVar.bPasswordManager;
            txtHome6.Enabled = PublicVar.bPasswordManager;
            txtHome7.Enabled = PublicVar.bPasswordManager;
            txtHome8.Enabled = PublicVar.bPasswordManager;
            txtCHXInitPos.Enabled = !PublicVar.bPasswordOperator;
            txtCHYInitPos.Enabled = !PublicVar.bPasswordOperator;
            txtXCaliCorr.Enabled = (PublicVar.bPasswordManager|PublicVar.bPasswordEngineer);
            txtYCaliCorr.Enabled = (PublicVar.bPasswordManager|PublicVar.bPasswordEngineer);
            txtRepairXAngle.Enabled = PublicVar.bPasswordManager;
            txtRepairYAngle.Enabled = PublicVar.bPasswordManager;
            txtRepairIntervalX.Enabled = PublicVar.bPasswordManager;
            txtRepairIntervalY.Enabled = PublicVar.bPasswordManager;
            txtRepairStartX.Enabled = PublicVar.bPasswordManager;
            txtRepairStartY.Enabled = PublicVar.bPasswordManager;
            txtMasterPos1X.Enabled = PublicVar.bPasswordManager;
            txtMasterPos1Y.Enabled = PublicVar.bPasswordManager;
            txtMasterPos2X.Enabled = PublicVar.bPasswordManager;
            txtMasterPos2Y.Enabled = PublicVar.bPasswordManager;
            txtMasterPos3X.Enabled = PublicVar.bPasswordManager;
            txtMasterPos3Y.Enabled = PublicVar.bPasswordManager;
            txtMasterPos4X.Enabled = PublicVar.bPasswordManager;
            txtMasterPos4Y.Enabled = PublicVar.bPasswordManager;
            txtMasterNumHZ.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtMasterL1.Enabled = PublicVar.bPasswordManager;
            txtMasterL2.Enabled = PublicVar.bPasswordManager;
            txtMasterW1.Enabled = PublicVar.bPasswordManager;
            txtMasterW2.Enabled = PublicVar.bPasswordManager;
            txtMasterThreshold.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtMasterLight.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtPosCameraUnit.Enabled = (PublicVar.bPasswordManager);
            txtHome9.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtHome10.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtHome11.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtHome12.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtHome13.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtHome14.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtHome15.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtHome16.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtMotorCYUnit.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtMotorCZUnit.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun9.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun10.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun11.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun12.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun13.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun14.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun15.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtRun16.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCHCYInitPos.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCHCZInitPos.Enabled =(PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCHCYWorkPos.Enabled =PublicVar.bPasswordManager;
            txtCHCZWorkPos.Enabled =PublicVar.bPasswordManager;
            txtCHCZWorkPos1.Enabled =(PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCHCZLowVel.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrX1K.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrX1Offset.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrX2K.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrX2Offset.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrY1K.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrY1Offset.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrY2K.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCorrY2Offset.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCameraXEncoder.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);
            txtCameraYEncoder.Enabled = (PublicVar.bPasswordEngineer | PublicVar.bPasswordManager);

        }
        private void ReadParam()
        {

            string strPath;
            strPath = Application.StartupPath + "\\DOC";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "CameraX");
            txtCheckCameraXUnit.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "CameraY");
            txtCheckCameraYUnit.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "CameraPos");
            txtPosCameraUnit.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "X");
            txtMotorXUnit.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "Y");
            txtMotorYUnit.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "CY");
            txtMotorCYUnit.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "CZ");
            txtMotorCZUnit.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "EncoderX");
            txtCameraXEncoder.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Unit", "EncoderY");
            txtCameraYEncoder.Text = strTemp;
            strTemp = IniSetting.IniReadValue("X", "LeftTop");
            txtLeftTopX.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Y", "LeftTop");
            txtLeftTopY.Text = strTemp;
            strTemp = IniSetting.IniReadValue("X", "RightTop");
            txtRightTopX.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Y", "RightTop");
            txtRightTopY.Text = strTemp;
            strTemp = IniSetting.IniReadValue("X", "RightBottom");
            txtRightBottomX.Text = strTemp;
            strTemp = IniSetting.IniReadValue("Y", "RightBottom");
            txtRightBottomY.Text = strTemp;

          //定位相机是否二值化
            strTemp = IniSetting.IniReadValue("Select", "CameraThre");
            if (strTemp == "1") chk_CameraThreshold.Checked = true;
            else chk_CameraThreshold.Checked = false;

            txtRun1.Text = IniSetting.IniReadValue("CHX", "InitVel");
            txtRun2.Text = IniSetting.IniReadValue("CHX", "ACC");
            txtRun3.Text = IniSetting.IniReadValue("CHX", "DEC");
            txtRun7.Text = IniSetting.IniReadValue("CHX", "Speed");
            txtRun4.Text = IniSetting.IniReadValue("CHY", "InitVel");
            txtRun5.Text = IniSetting.IniReadValue("CHY", "ACC");
            txtRun6.Text = IniSetting.IniReadValue("CHY", "DEC");
            txtRun8.Text = IniSetting.IniReadValue("CHY", "Speed");
            txtRun9.Text = IniSetting.IniReadValue("CHCY", "InitVel");
            txtRun10.Text = IniSetting.IniReadValue("CHCY", "ACC");
            txtRun11.Text = IniSetting.IniReadValue("CHCY", "DEC");
            txtRun12.Text = IniSetting.IniReadValue("CHCY", "Speed");
            txtRun13.Text = IniSetting.IniReadValue("CHCZ", "InitVel");
            txtRun14.Text = IniSetting.IniReadValue("CHCZ", "ACC");
            txtRun15.Text = IniSetting.IniReadValue("CHCZ", "DEC");
            txtRun16.Text = IniSetting.IniReadValue("CHCZ", "Speed");
            txtCHCYInitPos.Text = IniSetting.IniReadValue("CHCY", "InitPos");
            txtCHCZInitPos.Text = IniSetting.IniReadValue("CHCZ", "InitPos");
            txtCHCYWorkPos.Text=IniSetting.IniReadValue("CHCY", "WorkPos");
            txtCHCZWorkPos.Text = IniSetting.IniReadValue("CHCZ", "WorkPos");
            txtCHCZWorkPos1.Text = IniSetting.IniReadValue("CHCZ", "WorkPos1");
            txtCHCZLowVel.Text = IniSetting.IniReadValue("CHCZ", "LowVel");
            txtHome1.Text = IniSetting.IniReadValue("CHX", "HomeInitVel");
            txtHome2.Text = IniSetting.IniReadValue("CHX", "HomeMaxVel");
            txtHome3.Text = IniSetting.IniReadValue("CHX", "HomeACC");
            txtHome4.Text = IniSetting.IniReadValue("CHX", "HomeDEC");
            txtHome5.Text = IniSetting.IniReadValue("CHY", "HomeInitVel");
            txtHome6.Text = IniSetting.IniReadValue("CHY", "HomeMaxVel");
            txtHome7.Text = IniSetting.IniReadValue("CHY", "HomeACC");
            txtHome8.Text = IniSetting.IniReadValue("CHY", "HomeDEC");
            txtHome9.Text = IniSetting.IniReadValue("CHCY", "HomeInitVel");
            txtHome10.Text = IniSetting.IniReadValue("CHCY", "HomeMaxVel");
            txtHome11.Text = IniSetting.IniReadValue("CHCY", "HomeACC");
            txtHome12.Text = IniSetting.IniReadValue("CHCY", "HomeDEC");
            txtHome13.Text = IniSetting.IniReadValue("CHCZ", "HomeInitVel");
            txtHome14.Text = IniSetting.IniReadValue("CHCZ", "HomeMaxVel");
            txtHome15.Text = IniSetting.IniReadValue("CHCZ", "HomeACC");
            txtHome16.Text = IniSetting.IniReadValue("CHCZ", "HomeDEC");

            txtCHXInitPos.Text  = IniSetting.IniReadValue("CHX", "InitPos");
            txtCHYInitPos.Text = IniSetting.IniReadValue("CHY", "InitPos");

            txtHighSpeed.Text=IniSetting.IniReadValue("JogSpeed", "High");
            txtMidSpeed.Text=IniSetting.IniReadValue("JogSpeed", "Mid" );
            txtLowSpeed.Text=IniSetting.IniReadValue("JogSpeed", "Low");
            txtXCaliCorr.Text=IniSetting.IniReadValue("CHX", "CaliCorr");
            txtRepairXAngle.Text = IniSetting.IniReadValue("CHX", "RepairAngle");
            txtYCaliCorr.Text = IniSetting.IniReadValue("CHY", "CaliCorr");
            txtRepairYAngle.Text = IniSetting.IniReadValue("CHY", "RepairAngle");
            try
            {
                txtRepairIntervalX.Text = IniSetting.IniReadValue("RepairInterval", "X");
                txtRepairIntervalY.Text = IniSetting.IniReadValue("RepairInterval", "Y");
                txtRepairStartX.Text = IniSetting.IniReadValue("RepairStart", "X");
                txtRepairStartY.Text = IniSetting.IniReadValue("RepairStart", "Y");
            }
            catch
            {
                txtRepairIntervalX.Text = "2";
                txtRepairIntervalY.Text = "2";
                txtRepairStartX.Text = "10";
                txtRepairStartY.Text = "10";
            }
            try
            {
                txtMasterPos1X.Text=IniSetting.IniReadValue("Master", "PosX1");
                txtMasterPos1Y.Text=IniSetting.IniReadValue("Master", "PosY1");
                txtMasterPos2X.Text=IniSetting.IniReadValue("Master", "PosX2");
                txtMasterPos2Y.Text=IniSetting.IniReadValue("Master", "PosY2");
                txtMasterPos3X.Text=IniSetting.IniReadValue("Master", "PosX3");
                txtMasterPos3Y.Text=IniSetting.IniReadValue("Master", "PosY3");
                txtMasterPos4X.Text=IniSetting.IniReadValue("Master", "PosX4");
                txtMasterPos4Y.Text=IniSetting.IniReadValue("Master", "PosY4");
                txtMasterNumHZ.Text=IniSetting.IniReadValue("Master", "NumHZ");
                txtMasterL1.Text=IniSetting.IniReadValue("Master", "L1");
                txtMasterL2.Text=IniSetting.IniReadValue("Master", "L2");
                txtMasterW1.Text=IniSetting.IniReadValue("Master", "W1");
                txtMasterW2.Text=IniSetting.IniReadValue("Master", "W2");
                txtMasterLight.Text=IniSetting.IniReadValue("Master", "Light");
                txtMasterThreshold.Text=IniSetting.IniReadValue("Master", "Threshold");

                txtCorrX1K.Text =IniSetting.IniReadValue("Corr", "X1K");
                txtCorrX1Offset.Text = IniSetting.IniReadValue("Corr", "X1Offset");
                txtCorrX2K.Text = IniSetting.IniReadValue("Corr", "X2K");
                txtCorrX2Offset.Text = IniSetting.IniReadValue("Corr", "X2Offset");
                txtCorrY1K.Text = IniSetting.IniReadValue("Corr", "Y1K");
                txtCorrY1Offset.Text = IniSetting.IniReadValue("Corr", "Y1Offset");
                txtCorrY2K.Text = IniSetting.IniReadValue("Corr", "Y2K");
                txtCorrY2Offset.Text = IniSetting.IniReadValue("Corr", "Y2Offset");
            }
            catch { }

        }
        private void SaveParam()
        {
            string strPath;
            strPath = Application.StartupPath + "\\DOC";
            if(!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath );
            strPath += "\\Setting.ini";
            CIni IniSetting = new CIni(strPath);
            string strTemp;
            strTemp = txtCheckCameraXUnit.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "CameraX", strTemp);
            strTemp = txtCheckCameraYUnit.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "CameraY", strTemp);
            strTemp = txtPosCameraUnit.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "CameraPos", strTemp);

            strTemp = txtMotorXUnit.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "X", strTemp);
            strTemp = txtMotorYUnit.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "Y", strTemp);
            strTemp = txtMotorCYUnit.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "CY", strTemp);
            strTemp = txtMotorCZUnit.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "CZ", strTemp);
            strTemp = txtCameraXEncoder.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "EncoderX", strTemp);
            strTemp = txtCameraYEncoder.Text.Trim();
            if (strTemp == "")
                strTemp = "0.001";
            IniSetting.IniWriteValue("Unit", "EncoderY", strTemp);
            strTemp = txtLeftTopX.Text.Trim();
            if (strTemp == "")
                strTemp = "0";
            IniSetting.IniWriteValue("X", "LeftTop", strTemp);
            strTemp = txtLeftTopY.Text.Trim();
            if (strTemp == "")
                strTemp = "0";
            IniSetting.IniWriteValue("Y", "LeftTop", strTemp);

            strTemp = txtRightTopX.Text.Trim();
            if (strTemp == "")
                strTemp = "0";
            IniSetting.IniWriteValue("X", "RightTop", strTemp);
            strTemp = txtRightTopY.Text.Trim();
            if (strTemp == "")
                strTemp = "0";
            IniSetting.IniWriteValue("Y", "RightTop", strTemp);

            strTemp = txtRightBottomX.Text.Trim();
            if (strTemp == "")
                strTemp = "0";
            IniSetting.IniWriteValue("X", "RightBottom", strTemp);
            strTemp = txtRightBottomY.Text.Trim();
            if (strTemp == "")
                strTemp = "0";
            IniSetting.IniWriteValue("Y", "RightBottom", strTemp);

            IniSetting.IniWriteValue("CHX", "HomeInitVel", txtHome1.Text.Trim());
            IniSetting.IniWriteValue("CHX", "HomeMaxVel", txtHome2.Text.Trim());
            IniSetting.IniWriteValue("CHX", "HomeACC", txtHome3.Text.Trim());
            IniSetting.IniWriteValue("CHX", "HomeDEC", txtHome4.Text.Trim());
            IniSetting.IniWriteValue("CHY", "HomeInitVel", txtHome5.Text.Trim());
            IniSetting.IniWriteValue("CHY", "HomeMaxVel", txtHome6.Text.Trim());
            IniSetting.IniWriteValue("CHY", "HomeACC", txtHome7.Text.Trim());
            IniSetting.IniWriteValue("CHY", "HomeDEC", txtHome8.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "HomeInitVel", txtHome9.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "HomeMaxVel", txtHome10.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "HomeACC", txtHome11.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "HomeDEC", txtHome12.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "HomeInitVel", txtHome13.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "HomeMaxVel", txtHome14.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "HomeACC", txtHome15.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "HomeDEC", txtHome16.Text.Trim());
            IniSetting.IniWriteValue("CHX", "InitVel", txtRun1.Text.Trim());
            IniSetting.IniWriteValue("CHX", "ACC", txtRun2.Text.Trim());
            IniSetting.IniWriteValue("CHX", "DEC", txtRun3.Text.Trim());
            IniSetting.IniWriteValue("CHX", "Speed", txtRun7.Text.Trim());
            IniSetting.IniWriteValue("CHY", "InitVel", txtRun4.Text.Trim());
            IniSetting.IniWriteValue("CHY", "ACC", txtRun5.Text.Trim());
            IniSetting.IniWriteValue("CHY", "DEC", txtRun6.Text.Trim());
            IniSetting.IniWriteValue("CHY", "Speed", txtRun8.Text.Trim());
            IniSetting.IniWriteValue("CHX", "InitPos", txtCHXInitPos.Text.Trim());
            IniSetting.IniWriteValue("CHY", "InitPos", txtCHYInitPos.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "InitVel", txtRun9.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "ACC", txtRun10.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "DEC", txtRun11.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "Speed", txtRun12.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "InitVel", txtRun13.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "ACC", txtRun14.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "DEC", txtRun15.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "Speed", txtRun16.Text.Trim());
            IniSetting.IniWriteValue("CHCY", "InitPos", txtCHCYInitPos.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "InitPos", txtCHCZInitPos.Text.Trim());

            IniSetting.IniWriteValue("CHCY", "WorkPos", txtCHCYWorkPos.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "WorkPos", txtCHCZWorkPos.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "WorkPos1", txtCHCZWorkPos1.Text.Trim());
            IniSetting.IniWriteValue("CHCZ", "LowVel", txtCHCZLowVel.Text.Trim());

            IniSetting.IniWriteValue("JogSpeed", "High", txtHighSpeed .Text .Trim ());
            IniSetting.IniWriteValue("JogSpeed", "Mid", txtMidSpeed .Text .Trim ());
            IniSetting.IniWriteValue("JogSpeed", "Low", txtLowSpeed .Text .Trim ());
            IniSetting.IniWriteValue("CHX", "CaliCorr", txtXCaliCorr.Text .Trim ());
            IniSetting.IniWriteValue("CHX", "RepairAngle", txtRepairXAngle.Text.Trim());
            IniSetting.IniWriteValue("CHY", "CaliCorr", txtYCaliCorr.Text.Trim());
            IniSetting.IniWriteValue("CHY", "RepairAngle", txtRepairYAngle.Text.Trim());
            IniSetting.IniWriteValue("RepairInterval", "X", txtRepairIntervalX.Text.Trim());
            IniSetting.IniWriteValue("RepairInterval", "Y", txtRepairIntervalY.Text.Trim());
            IniSetting.IniWriteValue("RepairStart", "X", txtRepairStartX.Text.Trim());
            IniSetting.IniWriteValue("RepairStart", "Y", txtRepairStartY.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosX1", txtMasterPos1X.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosY1", txtMasterPos1Y.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosX2", txtMasterPos2X.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosY2", txtMasterPos2Y.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosX3", txtMasterPos3X.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosY3", txtMasterPos3Y.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosX4", txtMasterPos4X.Text.Trim());
            IniSetting.IniWriteValue("Master", "PosY4", txtMasterPos4Y.Text.Trim());
            IniSetting.IniWriteValue("Master", "NumHZ", txtMasterNumHZ.Text.Trim());
            IniSetting.IniWriteValue("Master", "L1", txtMasterL1.Text.Trim());
            IniSetting.IniWriteValue("Master", "L2", txtMasterL2.Text.Trim());
            IniSetting.IniWriteValue("Master", "W1", txtMasterW1.Text.Trim());
            IniSetting.IniWriteValue("Master", "W2", txtMasterW2.Text.Trim());
            IniSetting.IniWriteValue("Master", "Light", txtMasterLight.Text.Trim());
            IniSetting.IniWriteValue("Master", "Threshold", txtMasterThreshold.Text.Trim());

            IniSetting.IniWriteValue("Corr", "X1K", txtCorrX1K .Text .Trim ());
            IniSetting.IniWriteValue("Corr", "X1Offset", txtCorrX1Offset.Text.Trim());
            IniSetting.IniWriteValue("Corr", "X2K", txtCorrX2K.Text.Trim());
            IniSetting.IniWriteValue("Corr", "X2Offset", txtCorrX2Offset.Text.Trim());
            IniSetting.IniWriteValue("Corr", "Y1K", txtCorrY1K.Text.Trim());
            IniSetting.IniWriteValue("Corr", "Y1Offset", txtCorrY1Offset.Text.Trim());
            IniSetting.IniWriteValue("Corr", "Y2K", txtCorrY2K.Text.Trim());
            IniSetting.IniWriteValue("Corr", "Y2Offset", txtCorrY2Offset.Text.Trim());

            //定位相机是否二值化
            if (chk_CameraThreshold.Checked)
            {
                IniSetting.IniWriteValue("Select", "CameraThre", "1");
            }
            else
            {
                IniSetting.IniWriteValue("Select", "CameraThre", "0");
            }
        }

        private void FrmMachineParam_Load(object sender, EventArgs e)
        {
            EnableControl();
            try
            {
                ReadParam();
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定保存吗？", "保存", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveParam();
                PublicVar.bPasswordOperator = true;
                PublicVar.bPasswordManager = false;
                PublicVar.bPasswordEngineer = false;
            }
        }


    }
}
