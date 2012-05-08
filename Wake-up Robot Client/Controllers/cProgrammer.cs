using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows;
/***************************************************************************************
 * Legend for the putting the Datetime in the µC 
 *  v = reply version
 *  g = global
 *      t = return current time
 *      a = return next alarm
 *      p = program time
 *          1 = year +10
 *          2 = year +1
 *          3 = month +10
 *          4 = month +1
 *          5 = day +10
 *          6 = day +1
 *          7 = hour +10
 *          8 = hour +1
 *          9 = minute +10
 *          b = minute +1
 *          c = second +10
 *          d = second +1
 */ 
namespace Wake_up_Robot_Client.Controllers
{
    class cProgrammer
    {
        private SerialPort port;
        private byte[] buf = new byte[9];
        char[] buf2 = new char[4096];

        #region public
        
        public cProgrammer(string portname)
        {
            
            try
            {
                port = new SerialPort(portname, 9600);
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: Exception handling
            }
        }

        public void ProgramAlarms(List<Alarm> alarms)
        {
            try
            {
                if (!port.IsOpen)
                    port.Open(); //Open the connection if it's not already open

                port.Write(CurrentTimeBuffer, 0, CurrentTimeBuffer.Length);
                foreach (Alarm alarm in alarms)
                {
                    var alarmbuffer = AlarmBuffer(alarm);
                    port.Write(alarmbuffer, 0, alarmbuffer.Length);
                    alarm.Enable();
                }
            }
            catch (Exception ex)
            {
                throw ex; //TODO: Exception handling
            }
        }

        
        public void ProgramAlarm(Alarm alarm)
        {
            List<Alarm> alarms = new List<Alarm>();
            alarms.Add(alarm);
            ProgramAlarms(alarms);
        }

        public void Read()
        {
            if (!port.IsOpen)
                port.Open();
            buf[0] = (byte)'g';
            buf[1] = (byte)'a';            
            buf[2] = (byte)'\r';         
            port.Write(buf, 0, 3);
           
                           
        }

        static public string[] GetSerialPortNames()
        {
            return SerialPort.GetPortNames();            
        }

        
        #endregion //public

        #region private
        
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!port.IsOpen)
                port.Open();
            string s2 = port.ReadExisting();
            MessageBox.Show(s2);

            port.Close();
        }

        /// <summary>
        /// Creates a byte array that can be send to the PIC
        /// </summary>
        /// <returns>Byte array from current time</returns>
        private byte[] CurrentTimeBuffer
        {
            get
            {
                byte[] buf = new byte[9];
                buf[0] = (byte)'g';
                buf[1] = (byte)'c';
                buf[2] = IntToHex(DateTime.Now.Year);
                buf[3] = IntToHex(DateTime.Now.Month);
                buf[4] = IntToHex(DateTime.Now.Day);
                buf[5] = IntToHex(DateTime.Now.Hour);
                buf[6] = IntToHex(DateTime.Now.Minute);
                buf[7] = IntToHex(DateTime.Now.Second);
                buf[8] = (byte)'\r';
                return buf;
            }
        }

        /// <summary>
        /// Creates a byte array from an alarm so it can be programmed in the PIC
        /// </summary>
        /// <param name="alarm">alarm to be turned in a byte array</param>
        /// <returns>Byte array from alarm datetime</returns>
        private byte[] AlarmBuffer(Alarm alarm)
        {
            byte[] buf = new byte[9];
            buf[0] = (byte)'g';
            buf[1] = (byte)'p';
            buf[2] = IntToHex(alarm.DateTime.Year);
            buf[3] = IntToHex(alarm.DateTime.Month);
            buf[4] = IntToHex(alarm.DateTime.Day);
            buf[5] = IntToHex(alarm.DateTime.Hour);
            buf[6] = IntToHex(alarm.DateTime.Minute);
            buf[7] = IntToHex(alarm.DateTime.Second);
            buf[8] = (byte)'\r';
            return buf;

        }

        private byte IntToHex(int number)
        {
            byte bbuf;
            byte bbuf2;
            byte bOutput;
            if (number > 9)
            {
                if (number > 100)
                    bbuf = (byte)((number - 2000) / 10);
                else
                    bbuf = (byte)((number) / 10);
                bbuf = (byte)(bbuf << 4);
                do
                {
                    number -= 10;
                }
                while (number >= 10);

                bbuf2 = (byte)(number & 0x0f);
                bOutput = (byte)(bbuf2 | bbuf);
            }
            else
            {
                bOutput = (byte)number;
            }

            return bOutput;
        }

        #endregion //private
    }
}
