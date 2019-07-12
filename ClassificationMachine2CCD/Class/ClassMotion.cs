using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using csDmc2410;
using System.Threading;
namespace Class_Motion
{
    public class ClassMotion
    {
        public ushort CHXMotor;
        public ushort CHYMotor;
        public ushort CHCYMotor;
        public ushort CHCZMotor;
        ReaderWriterLock readWrite;
        public ushort ioCard1 = 0;
        public ClassMotion(bool bInit)
        {
            CHXMotor = 0;
            CHYMotor = 1;
            CHCZMotor = 2;
            CHCYMotor = 3;

            readWrite = new ReaderWriterLock();
            if (bInit)
                InitMotorCard(bInit);
        }
        ~ClassMotion()
        {
            CloseMotorCard();
        }
        public void WinExe(string strExe)
        {
            Dmc2410.WinExec(strExe, 4);
        }
        public void CloseLock()
        {
            if(readWrite.IsReaderLockHeld )
                readWrite.ReleaseReaderLock();
            if(readWrite.IsWriterLockHeld )
                readWrite.ReleaseWriterLock();
            if(readWrite.IsWriterLockHeld||readWrite.IsReaderLockHeld )
                readWrite.ReleaseLock();
       }
        public void CloseMotorCard()
        {
            Dmc2410.d2410_board_close();
        }
        public void InitMotorCard(bool bInit)
        {
            try
            {
                Int32 nCard = 0;
                nCard = Dmc2410.d2410_board_init();
                if (bInit == false) return;
                if (nCard <= 0)
                    MessageBox.Show("初始化DMC2410卡失败！", "出错");
                else
                {
                    /////////////////////Emergency stop/////////////////////////
                    Dmc2410.d2410_config_EMG_PIN(0, 1, 0);
                    ///////////////////////Card0- Axis X  ///////////////////////////
                    Dmc2410.d2410_write_SEVON_PIN(CHXMotor, 0);
                    Dmc2410.d2410_set_pulse_outmode(CHXMotor, 0);
                    Dmc2410.d2410_config_EL_MODE(CHXMotor, 0);
                    Dmc2410.d2410_config_ALM_PIN_Extern(CHXMotor, 1, 0, 0, 0);
                    Dmc2410.d2410_counter_config(CHXMotor, 3);
                    Dmc2410 .d2410_set_encoder (CHXMotor ,0);
                    ////////////////////////Card0-Axis Y///////////////////////////
                    Dmc2410.d2410_write_SEVON_PIN(CHYMotor, 0);
                    Dmc2410.d2410_config_ALM_PIN_Extern(CHYMotor, 1, 0, 0, 0);
                    Dmc2410.d2410_config_EL_MODE(CHYMotor, 0);
                    Dmc2410.d2410_set_pulse_outmode(CHYMotor, 0);
                    Dmc2410.d2410_counter_config(CHYMotor, 3);
                    Dmc2410 .d2410_set_encoder (CHYMotor ,0);
                    ///////////////////////Card0- Axis CY  ///////////////////////////
                    Dmc2410.d2410_write_SEVON_PIN(CHCYMotor, 0);
                    Dmc2410.d2410_set_pulse_outmode(CHCYMotor, 0);
                    Dmc2410.d2410_config_EL_MODE(CHCYMotor, 0);
                    Dmc2410.d2410_config_ALM_PIN_Extern(CHCYMotor, 1, 0, 0, 0);
                    Dmc2410.d2410_counter_config(CHCYMotor, 3);
                    Dmc2410.d2410_set_encoder(CHCYMotor, 0);
                    ////////////////////////Card0-Axis CZ///////////////////////////
                    Dmc2410.d2410_write_SEVON_PIN(CHCZMotor, 0);
                    Dmc2410.d2410_config_ALM_PIN_Extern(CHCZMotor, 1, 0, 0, 0);
                    Dmc2410.d2410_config_EL_MODE(CHCZMotor, 0);
                    Dmc2410.d2410_set_pulse_outmode(CHCZMotor, 0);
                    Dmc2410.d2410_counter_config(CHCZMotor, 3);
                    Dmc2410.d2410_set_encoder(CHCZMotor, 0);
                }
            }
            catch { }
        }
        public void EnableEmgStop(ushort enable=0)
        {
            /////////////////////Emergency stop/////////////////////////
            Dmc2410.d2410_config_EMG_PIN(0, 1, enable);
        }
        private static long TimeDiff(DateTime t, DateTime t2)
        {
            long lReturn = -1;
            System.TimeSpan NowValue = new TimeSpan(t.Ticks);
            System.TimeSpan TimeValue = new TimeSpan(t2.Ticks);
            System.TimeSpan DateDiff = TimeSpan.Zero;
            try
            {
                DateDiff = TimeValue.Subtract(NowValue);
                int hours = DateDiff.Hours;
                int minutes = DateDiff.Minutes;
                int seconds = DateDiff.Seconds;
                int milliseconds = DateDiff.Milliseconds;

                if (hours < 0 && minutes < 0 && seconds < 0 && milliseconds < 0)
                    hours += 24;

                lReturn = hours * 3600 * 1000
                    + minutes * 60 * 1000
                    + seconds * 1000
                    + milliseconds;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return lReturn;
        }
        public  bool IsOutTime(DateTime t, int TimeLen)
        {
            if (TimeDiff(t, DateTime.Now) > TimeLen)
                return false;
             return true;
        }
   
#region  ORG PEL NEL

      public bool CHXMotorALM
        {
            get
            {
                return (Dmc2410.d2410_axis_io_status(CHXMotor) & 2048) == 2048;
            }
        }
      public bool CHXMotorORG
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHXMotor) & 16384) == 16384;
          }
      }
      public bool CHXMotorPEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHXMotor) & 4096) == 4096;
          }
      }
      public bool CHXMotorNEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHXMotor) & 8192) == 8192;
          }
      }
      public bool CHXMotorEmg
        {
            get
            {
                return (Dmc2410.d2410_get_rsts(CHXMotor) & 128) == 128;
            }

        }

      public bool CHYMotorALM
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHYMotor) & 2048) == 2048;
          }
      }
      public bool CHYMotorORG
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHYMotor) & 16384) == 16384;
          }
      }
      public bool CHYMotorPEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHYMotor) & 4096) == 4096;
          }
      }
      public bool CHYMotorNEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHYMotor) & 8192) == 8192;
          }
      }
      public bool CHYMotorEmg
      {
          get
          {
              return (Dmc2410.d2410_get_rsts(CHYMotor) & 128) == 128;
          }

      }

      public bool CHCYMotorALM
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCYMotor) & 2048) == 2048;
          }
      }
      public bool CHCYMotorORG
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCYMotor) & 16384) == 16384;
          }
      }
      public bool CHCYMotorPEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCYMotor) & 4096) == 4096;
          }
      }
      public bool CHCYMotorNEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCYMotor) & 8192) == 8192;
          }
      }
      public bool CHCYMotorEmg
      {
          get
          {
              return (Dmc2410.d2410_get_rsts(CHCYMotor) & 128) == 128;
          }

      }

      public bool CHCZMotorALM
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCZMotor) & 2048) == 2048;
          }
      }
      public bool CHCZMotorORG
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCZMotor) & 16384) == 16384;
          }
      }
      public bool CHCZMotorPEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCZMotor) & 4096) == 4096;
          }
      }
      public bool CHCZMotorNEL
      {
          get
          {
              return (Dmc2410.d2410_axis_io_status(CHCZMotor) & 8192) == 8192;
          }
      }
      public bool CHCZMotorEmg
      {
          get
          {
              return (Dmc2410.d2410_get_rsts(CHCZMotor) & 128) == 128;
          }

      }


#endregion


      public bool ReplaceCHXMotor(double[] datas, int XStrPos)//double XRStral, double XRMaxVal, double XRTacc, double XRTdcc,double XMaxval)
      {
          try
          {
              Dmc2410.d2410_set_HOME_pin_logic(CHXMotor, 0, 1);
              Dmc2410.d2410_config_home_mode(CHXMotor, 3, 0);
              Dmc2410.d2410_set_st_profile(CHXMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
              Dmc2410.d2410_home_move(CHXMotor, 2, 1);
              Thread.Sleep(100);
              DateTime CurrentTime = DateTime.Now;
              while (Dmc2410.d2410_check_done(CHXMotor) == 0)
              {
                  if (!IsOutTime(CurrentTime, 800000))
                  {
                     // Dmc2410.d2410_emg_stop();
                      Dmc2410.d2410_imd_stop(CHXMotor);
                      MessageBox.Show("马达X系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      return false;
                  }
              }
              Thread.Sleep(100);
              if ( CHXMotorNEL)
              {
                  DateTime CurrentTime1 = DateTime.Now;
                  Dmc2410.d2410_set_st_profile(CHXMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                  Dmc2410.d2410_s_vmove(CHXMotor, 1);
                  Thread.Sleep(200);
                  while (true)
                  {
                      if (CHXMotorORG)
                      {
                          Thread.Sleep(100);
                          Dmc2410.d2410_imd_stop(CHXMotor);
                          Thread.Sleep(20);
                          break;
                      }
                  }
                  while (Dmc2410.d2410_check_done(CHXMotor) == 0)
                  {
                      if (!IsOutTime(CurrentTime1, 80000))
                      {
                          // Dmc2410.d2410_emg_stop();
                          Dmc2410.d2410_imd_stop(CHXMotor);
                          MessageBox.Show("马达X异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          return false;
                      }
                  }
              }
              Thread.Sleep(20);
              if (CHXMotorORG)
              {
                  DateTime CurrentTime1 = DateTime.Now;
                  Dmc2410.d2410_set_st_profile(CHXMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                  Dmc2410.d2410_s_vmove(CHXMotor, 1);
                  Thread.Sleep(200);
                  while (true)
                  {
                      if (!CHXMotorORG)
                      {
                          Thread.Sleep(100);
                          Dmc2410.d2410_imd_stop(CHXMotor);
                          Thread.Sleep(20);
                          break;
                      }
                  }
                  while (Dmc2410.d2410_check_done(CHXMotor) == 0)
                  {
                      if (!IsOutTime(CurrentTime1, 80000))
                      {
                          // Dmc2410.d2410_emg_stop();
                          Dmc2410.d2410_imd_stop(CHXMotor);
                          MessageBox.Show("马达X异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          return false;
                      }
                  }
              }
              Thread.Sleep(100);
              Dmc2410.d2410_set_HOME_pin_logic(CHXMotor, 0, 1);
              Dmc2410.d2410_config_home_mode(CHXMotor, 3, 0);
              Dmc2410.d2410_set_st_profile(CHXMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
              Dmc2410.d2410_home_move(CHXMotor, 2, 1);
              Thread.Sleep(100);
              CurrentTime = DateTime.Now;
              while (Dmc2410.d2410_check_done(CHXMotor) == 0 && ((Dmc2410.d2410_axis_io_status(CHXMotor) & 8192) != 8192))
              {
                  if (!IsOutTime(CurrentTime, 800000))
                  {
                      // Dmc2410.d2410_emg_stop();
                      Dmc2410.d2410_imd_stop(CHXMotor);
                      MessageBox.Show("马达X系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      return false;
                  }
              }
              Dmc2410.d2410_set_st_profile(CHXMotor, 10, datas[1], 0.1, 0.1, 0.01, 0.01);
              Dmc2410.d2410_set_position(CHXMotor, 0);
              Dmc2410.d2410_set_encoder(CHXMotor, 0);
              Dmc2410.d2410_s_pmove(CHXMotor, XStrPos, 1);
              Thread.Sleep(100);
              while (Dmc2410.d2410_check_done(CHXMotor) == 0)
              {

                  Thread.Sleep(20);
               }
          }
          catch { return false; }
          return true;
      }
      public bool ReplaceCHYMotor(double[] datas, int YStrPos)//double YRStral, double YRMaxVal, double YRTacc, double YRTdcc)
      {
          try
          {
              //为了避免撞击，让Y轴先走60mm
              double m_nStart = 500;//起始速度
              double m_nSpeed = 1600;//运行速度
              double fAcc = 0.1;//加速时间
              int dist = 60 * 80;
              Dmc2410.d2410_set_profile(CHYMotor, m_nStart, m_nSpeed, fAcc, fAcc);   //设置速度、加速度

              Dmc2410.d2410_t_pmove(CHYMotor, dist, 0);//作相对t型运动

              while (true)
              {
                  Thread.Sleep(1000);
                  //走60mm分两种情况，如果不碰到正极限，就执行陈工的程序，如果碰到正极限，就直接回原点
                  //1
                  if (CHYMotorPEL)
                  {
                      Dmc2410.d2410_set_HOME_pin_logic(CHYMotor, 0, 1);
                      Dmc2410.d2410_config_home_mode(CHYMotor, 3, 0);
                      Dmc2410.d2410_set_st_profile(CHYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                      Dmc2410.d2410_home_move(CHYMotor, 2, 1);
                      Thread.Sleep(100);
                      while (true)
                      {
                          Thread.Sleep(1000);
                          if (Dmc2410.d2410_check_done(CHYMotor) == 1)
                          {
                              Dmc2410.d2410_set_st_profile(CHYMotor, 10, datas[1], 0.1, 0.1, 0.01, 0.01);
                              Dmc2410.d2410_set_position(CHYMotor, 0);
                              Dmc2410.d2410_set_encoder(CHYMotor, 0);
                              Dmc2410.d2410_s_pmove(CHYMotor, YStrPos, 1);
                              Thread.Sleep(10);
                              while (Dmc2410.d2410_check_done(CHYMotor) == 0)
                              {

                                  Thread.Sleep(20);
                              }
                              return true;
                          }
                              
                      }
                  }
                  //2
                  if (Dmc2410.d2410_check_done(CHYMotor) == 1)
                  {
                      Dmc2410.d2410_set_HOME_pin_logic(CHYMotor, 0, 1);
                      Dmc2410.d2410_config_home_mode(CHYMotor, 3, 0);
                      Dmc2410.d2410_set_st_profile(CHYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                      Dmc2410.d2410_home_move(CHYMotor, 2, 1);
                      Thread.Sleep(100);
                      DateTime CurrentTime = DateTime.Now;
                      while (Dmc2410.d2410_check_done(CHYMotor) == 0)
                      {
                          if (!IsOutTime(CurrentTime, 800000))
                          {
                              // Dmc2410.d2410_emg_stop();
                              Dmc2410.d2410_imd_stop(CHYMotor);
                              MessageBox.Show("马达Y系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                              return false;
                          }
                      }
                      Thread.Sleep(100);
                      if (CHYMotorNEL)
                      {
                          DateTime CurrentTime1 = DateTime.Now;
                          Dmc2410.d2410_set_st_profile(CHYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                          Dmc2410.d2410_s_vmove(CHYMotor, 1);
                          Thread.Sleep(200);
                          while (true)
                          {
                              if (CHYMotorORG)
                              {
                                  Thread.Sleep(100);
                                  Dmc2410.d2410_imd_stop(CHYMotor);
                                  Thread.Sleep(20);
                                  break;
                              }
                          }
                          while (Dmc2410.d2410_check_done(CHYMotor) == 0)
                          {
                              if (!IsOutTime(CurrentTime1, 8000000))
                              {
                                  // Dmc2410.d2410_emg_stop();
                                  Dmc2410.d2410_imd_stop(CHYMotor);
                                  MessageBox.Show("马达Y异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                  return false;
                              }
                          }
                      }
                      Thread.Sleep(100);
                      if (CHYMotorORG)
                      {
                          DateTime CurrentTime1 = DateTime.Now;
                          Dmc2410.d2410_set_st_profile(CHYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                          Dmc2410.d2410_s_vmove(CHYMotor, 1);
                          Thread.Sleep(200);
                          while (true)
                          {
                              if (!CHYMotorORG)
                              {
                                  Thread.Sleep(100);
                                  Dmc2410.d2410_imd_stop(CHYMotor);
                                  Thread.Sleep(20);
                                  break;
                              }
                          }
                          while (Dmc2410.d2410_check_done(CHYMotor) == 0)
                          {
                              if (!IsOutTime(CurrentTime1, 8000000))
                              {
                                  // Dmc2410.d2410_emg_stop();
                                  Dmc2410.d2410_imd_stop(CHYMotor);
                                  MessageBox.Show("马达Y异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                  return false;
                              }
                          }
                      }
                      Thread.Sleep(20);
                      Dmc2410.d2410_set_HOME_pin_logic(CHYMotor, 0, 1);
                      Dmc2410.d2410_config_home_mode(CHYMotor, 3, 0);
                      Dmc2410.d2410_set_st_profile(CHYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                      Dmc2410.d2410_home_move(CHYMotor, 2, 1);
                      Thread.Sleep(10);
                      CurrentTime = DateTime.Now;
                      while (Dmc2410.d2410_check_done(CHYMotor) == 0 && ((Dmc2410.d2410_axis_io_status(CHYMotor) & 8192) != 8192))
                      {
                          if (!IsOutTime(CurrentTime, 800000))
                          {
                              Dmc2410.d2410_emg_stop();
                              MessageBox.Show("马达Y系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                              return false;
                          }
                      }
                      Dmc2410.d2410_set_st_profile(CHYMotor, 10, datas[1], 0.1, 0.1, 0.01, 0.01);
                      Dmc2410.d2410_set_position(CHYMotor, 0);
                      Dmc2410.d2410_set_encoder(CHYMotor, 0);
                      Dmc2410.d2410_s_pmove(CHYMotor, YStrPos, 1);
                      Thread.Sleep(10);
                      while (Dmc2410.d2410_check_done(CHYMotor) == 0)
                      {

                          Thread.Sleep(20);
                      }
                      return true;
                  }
              }
          }

          catch { return false; }
                      //break;
                  

              
              //
              
          
      }
      public bool ReplaceCHCYMotor(double[] datas, int CYStrPos)//double CYRStral, double CYRMaxVal, double CYRTacc, double CYRTdcc,double CYMaxval)
      {
          try
          {
              
             
              Dmc2410.d2410_set_HOME_pin_logic(CHCYMotor, 0, 1);
              Dmc2410.d2410_config_home_mode(CHCYMotor, 3, 0);
              Dmc2410.d2410_set_st_profile(CHCYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
              Dmc2410.d2410_home_move(CHCYMotor, 2, 1);
              Thread.Sleep(100);
              DateTime CurrentTime = DateTime.Now;
              while (Dmc2410.d2410_check_done(CHCYMotor) == 0)
              {
                  if (!IsOutTime(CurrentTime, 800000))
                  {
                      // Dmc2410.d2410_emg_stop();
                      Dmc2410.d2410_imd_stop(CHCYMotor);
                      MessageBox.Show("马达X系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      return false;
                  }
              }
              Thread.Sleep(100);
              if (CHCYMotorNEL)
              {
                  DateTime CurrentTime1 = DateTime.Now;
                  Dmc2410.d2410_set_st_profile(CHCYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                  Dmc2410.d2410_s_vmove(CHCYMotor, 1);
                  Thread.Sleep(200);
                  while (true)
                  {
                      if (CHCYMotorORG)
                      {
                          Thread.Sleep(100);
                          Dmc2410.d2410_imd_stop(CHCYMotor);
                          Thread.Sleep(20);
                          break;
                      }
                  }
                  while (Dmc2410.d2410_check_done(CHCYMotor) == 0)
                  {
                      if (!IsOutTime(CurrentTime1, 80000))
                      {
                          // Dmc2410.d2410_emg_stop();
                          Dmc2410.d2410_imd_stop(CHCYMotor);
                          MessageBox.Show("马达CY异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          return false;
                      }
                  }
              }
              Thread.Sleep(20);
              if (CHCYMotorORG)
              {
                  DateTime CurrentTime1 = DateTime.Now;
                  Dmc2410.d2410_set_st_profile(CHCYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                  Dmc2410.d2410_s_vmove(CHCYMotor, 1);
                  Thread.Sleep(200);
                  while (true)
                  {
                      if (!CHCYMotorORG)
                      {
                          Thread.Sleep(100);
                          Dmc2410.d2410_imd_stop(CHCYMotor);
                          Thread.Sleep(20);
                          break;
                      }
                  }
                  while (Dmc2410.d2410_check_done(CHCYMotor) == 0)
                  {
                      if (!IsOutTime(CurrentTime1, 80000))
                      {
                          // Dmc2410.d2410_emg_stop();
                          Dmc2410.d2410_imd_stop(CHCYMotor);
                          MessageBox.Show("马达CY异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          return false;
                      }
                  }
              }
              Thread.Sleep(100);
              Dmc2410.d2410_set_HOME_pin_logic(CHCYMotor, 0, 1);
              Dmc2410.d2410_config_home_mode(CHCYMotor, 3, 0);
              Dmc2410.d2410_set_st_profile(CHCYMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
              Dmc2410.d2410_home_move(CHCYMotor, 2, 1);
              Thread.Sleep(100);
              CurrentTime = DateTime.Now;
              while (Dmc2410.d2410_check_done(CHCYMotor) == 0 && ((Dmc2410.d2410_axis_io_status(CHCYMotor) & 8192) != 8192))
              {
                  if (!IsOutTime(CurrentTime, 800000))
                  {
                      // Dmc2410.d2410_emg_stop();
                      Dmc2410.d2410_imd_stop(CHCYMotor);
                      MessageBox.Show("马达CY系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      return false;
                  }
              }
              Dmc2410.d2410_set_st_profile(CHCYMotor, 10, datas[1], 0.1, 0.1, 0.01, 0.01);
              Dmc2410.d2410_set_position(CHCYMotor, 0);
              Dmc2410.d2410_set_encoder(CHCYMotor, 0);
              Dmc2410.d2410_s_pmove(CHCYMotor, CYStrPos, 1);
              Thread.Sleep(100);
              while (Dmc2410.d2410_check_done(CHCYMotor) == 0)
              {

                  Thread.Sleep(20);
              }
          }
          catch { return false; }
          return true;
      }
      public bool ReplaceCHCZMotor(double[] datas, int ZStrPos)//double CZRStral, double CZRMaxVal, double CZRTacc, double CZRTdcc)
      {
          try
          {
              Dmc2410.d2410_set_HOME_pin_logic(CHCZMotor, 0, 1);
              Dmc2410.d2410_config_home_mode(CHCZMotor, 3, 0);
              Dmc2410.d2410_set_st_profile(CHCZMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
              Dmc2410.d2410_home_move(CHCZMotor, 2, 1);
              Thread.Sleep(100);
              DateTime CurrentTime = DateTime.Now;
              while (Dmc2410.d2410_check_done(CHCZMotor) == 0)
              {
                  if (!IsOutTime(CurrentTime, 800000))
                  {
                      // Dmc2410.d2410_emg_stop();
                      Dmc2410.d2410_imd_stop(CHCZMotor);
                      MessageBox.Show("马达CZ系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      return false;
                  }
              }
              Thread.Sleep(100);
              if (CHCZMotorNEL)
              {
                  DateTime CurrentTime1 = DateTime.Now;
                  Dmc2410.d2410_set_st_profile(CHCZMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                  Dmc2410.d2410_s_vmove(CHCZMotor, 1);
                  Thread.Sleep(200);
                  while (true)
                  {
                      if (CHCZMotorORG)
                      {
                          Thread.Sleep(100);
                          Dmc2410.d2410_imd_stop(CHCZMotor);
                          Thread.Sleep(20);
                          break;
                      }
                  }
                  while (Dmc2410.d2410_check_done(CHCZMotor) == 0)
                  {
                      if (!IsOutTime(CurrentTime1, 8000000))
                      {
                          // Dmc2410.d2410_emg_stop();
                          Dmc2410.d2410_imd_stop(CHCZMotor);
                          MessageBox.Show("马达CZ异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          return false;
                      }
                  }
              }
              Thread.Sleep(100);
              if (CHCZMotorORG)
              {
                  DateTime CurrentTime1 = DateTime.Now;
                  Dmc2410.d2410_set_st_profile(CHCZMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
                  Dmc2410.d2410_s_vmove(CHCZMotor, 1);
                  Thread.Sleep(200);
                  while (true)
                  {
                      if (!CHCZMotorORG)
                      {
                          Thread.Sleep(100);
                          Dmc2410.d2410_imd_stop(CHCZMotor);
                          Thread.Sleep(20);
                          break;
                      }
                  }
                  while (Dmc2410.d2410_check_done(CHCZMotor) == 0)
                  {
                      if (!IsOutTime(CurrentTime1, 8000000))
                      {
                          // Dmc2410.d2410_emg_stop();
                          Dmc2410.d2410_imd_stop(CHCZMotor);
                          MessageBox.Show("马达CZ异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          return false;
                      }
                  }
              }
              Thread.Sleep(20);
              Dmc2410.d2410_set_HOME_pin_logic(CHCZMotor, 0, 1);
              Dmc2410.d2410_config_home_mode(CHCZMotor, 3, 0);
              Dmc2410.d2410_set_st_profile(CHCZMotor, datas[0], datas[1], datas[2], datas[3], 0.01, 0.01);
              Dmc2410.d2410_home_move(CHCZMotor, 2, 1);
              Thread.Sleep(10);
              CurrentTime = DateTime.Now;
              while (Dmc2410.d2410_check_done(CHCZMotor) == 0 && ((Dmc2410.d2410_axis_io_status(CHCZMotor) & 8192) != 8192))
              {
                  if (!IsOutTime(CurrentTime, 800000))
                  {
                      Dmc2410.d2410_emg_stop();
                      MessageBox.Show("马达CZ系统复位超时，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      return false;
                  }
              }
              Dmc2410.d2410_set_st_profile(CHCZMotor, 10, datas[1], 0.1, 0.1, 0.01, 0.01);
              Dmc2410.d2410_set_position(CHCZMotor, 0);
              Dmc2410.d2410_set_encoder(CHCZMotor, 0);
              Dmc2410.d2410_s_pmove(CHCZMotor, ZStrPos, 1);
              Thread.Sleep(10);
              while (Dmc2410.d2410_check_done(CHCZMotor) == 0)
              {

                  Thread.Sleep(20);
              }
          }
          catch { return false; }
          return true;
      }
 
       public void Write_Out_Bit(ushort cardno, ushort bitno, ushort on_off)
      {
          try
          {
              readWrite.AcquireWriterLock (50);
              Dmc2410.d2410_write_outbit(cardno, bitno, on_off);
              readWrite.ReleaseWriterLock ();
          }
          catch { }
      }
      public int Read_Out_Bit(ushort cardno, ushort bitno)
      {
          int iValue=0;
          try
          {
              readWrite.AcquireReaderLock(50);
              iValue = Dmc2410.d2410_read_outbit(cardno, bitno);
               readWrite.ReleaseReaderLock();
          }
          catch { }
          return iValue;
      }
      public int Read_In(ushort cardno, ushort bitno)
      {
          int iValue=0;
          try
          {
              readWrite.AcquireReaderLock(50);
              if (bitno > 99)
                  iValue = Dmc2410.d2410_read_inport(cardno);
              else
                  iValue = Dmc2410.d2410_read_inbit(cardno, bitno);
              readWrite.ReleaseReaderLock();
          }
          catch { }
          return iValue;
      }
      public uint Absolute_Move(ushort Axis,int iPos,double Stral,double MaxVal,double  Tacc,double  Tdcc)
      {
          uint iValue=0;
          try
          {
              readWrite.AcquireWriterLock (50);
              Dmc2410.d2410_set_st_profile(Axis, Stral, MaxVal, Tacc, Tdcc, 0.01, 0.01);
              iValue = Dmc2410.d2410_s_pmove(Axis, iPos, 1);
              readWrite.ReleaseWriterLock();
          }
          catch { }
          return iValue;
      }
      public uint Absolute_LineMove(int Dist1,int Dist2,double dSpeed,double Tacc,double Tdec)
      {
          ushort[] uAxis = new ushort[2];
          uAxis[0] = CHXMotor;
          uAxis[1] = CHYMotor;
          uint iValue=0;
          try
          {
              readWrite.AcquireWriterLock(50);
              Dmc2410.d2410_set_st_profile(uAxis[0], 0, dSpeed , Tacc, Tdec, 0.01, 0.01);
              Dmc2410.d2410_s_pmove(uAxis[0], Dist1 , 1);
              Dmc2410.d2410_set_st_profile(uAxis[1], 0, dSpeed , Tacc, Tdec, 0.01, 0.01);
              Dmc2410.d2410_s_pmove(uAxis[1], Dist2, 1);
              readWrite.ReleaseWriterLock();
          }
          catch { }
          return iValue;
      }
      public uint  Relative_Move(ushort uAxis,int idist)
      {
          uint iValue = 0;
          try
          {
              readWrite.AcquireWriterLock(50);
              iValue= Dmc2410.d2410_s_pmove(uAxis, idist, 0);
              readWrite.ReleaseWriterLock();
          }
          catch { }
          return iValue;
      }
      public int Get_Rsts(ushort uAxis)
      {
          int iValue=0;
          try
          {
              readWrite.AcquireReaderLock(50);
              iValue = (int)Dmc2410.d2410_get_rsts(uAxis);
              readWrite.ReleaseReaderLock();
          }
          catch { }
          return iValue;
      }
      public uint E_Stop()
      {
          uint iValue=0;
          try
          {
              //readWrite.AcquireReaderLock(50);
              iValue = Dmc2410.d2410_emg_stop();
              //readWrite.ReleaseReaderLock();
          }
          catch { }
          return iValue;
      }
      public ushort  Axis_io_status(ushort axis)
      {
          ushort iValue=0;
          try
          {
              readWrite.AcquireReaderLock(50);
              iValue = Dmc2410.d2410_axis_io_status(axis);
              readWrite.ReleaseReaderLock();
          }
          catch { }
          return iValue;
      }
      public ushort  CheckAxisDone(ushort uAxis) 
      {
          ushort iValue=0;
          try
          {
              readWrite.AcquireReaderLock(50);
              iValue = Dmc2410.d2410_check_done(uAxis);
              readWrite.ReleaseReaderLock();
          }
          catch { }
          return iValue;
      }
      public long Get_Axis_Position(ushort uAxis,bool bEncoder=false )
      {
          long iValue=0;
          readWrite.AcquireReaderLock(50);
          try
          {
              if(bEncoder ==false )
                 iValue = Dmc2410.d2410_get_position(uAxis);
              else
                 iValue =Convert.ToInt64  ( Dmc2410.d2410_get_encoder(uAxis));
              if (iValue > Math.Pow(2, 31))
                  iValue -=Convert .ToInt64 ( Math.Pow(2, 32));
          }
          catch { }
          readWrite.ReleaseReaderLock();
          return iValue;
      }
      public void Jog(ushort  userAxis,double  strspeed,double  maxspeed,double tacc,double  tdcc,ushort  uDir)
      {
          try
          {
              readWrite.AcquireWriterLock (50);
              Dmc2410.d2410_set_st_profile(userAxis, strspeed, maxspeed, tacc, tdcc, 0.01, 0.01);
              Dmc2410.d2410_s_vmove(userAxis, uDir);
              readWrite.ReleaseWriterLock();
          }
          catch { }
      }
      public void StopAxis(ushort uAxis)
      {
          try
          {
              //readWrite.AcquireWriterLock (50);
              Dmc2410.d2410_imd_stop(uAxis);
              //readWrite.ReleaseWriterLock ();
          }
          catch { }
      }
      public int GetSevoOn(ushort axis)
      {
          int iValue=0;
          try
          {
              readWrite.AcquireReaderLock(50);
              iValue = Dmc2410.d2410_read_SEVON_PIN(axis);
              readWrite.ReleaseReaderLock();
          }
          catch { }
          return iValue;
      }
      public void SevoOn(ushort uAxis,ushort on_off)
      {
          try
          {
              readWrite.AcquireWriterLock (50);
              Dmc2410.d2410_write_SEVON_PIN(uAxis, on_off);
              readWrite.ReleaseWriterLock ();
          }
          catch { }
      }
    }

    
}
