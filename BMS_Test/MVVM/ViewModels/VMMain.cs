using BMS_Test.Define;
using BMS_Test.MVVM.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace BMS_Test.MVVM.ViewModels
{
    public class VMMain : PropertyChangedNotifier
    {
        SerialPort PortPin;
        byte[] DataReceive;
        byte[] Function01_DataBattery_Receive;
        byte[] Function01_DataBattery;
        byte[] Function67_Protection_Receive;
        byte[] Function67_Protection;
        byte[] Function51_BMSVersion_Receive;
        byte[] Function51_BMSVersion;
        byte[] Function220_SerialNumber_Receive;
        byte[] Function220_SerialNumber;
        short[] DEC_Function01_DataBattery_Receive;
        
        //public BMS_Board _BMS_Board = new BMS_Board();
        public VMMain()
        {
            Function01_DataBattery = new byte[] {0x7E, 0x01, 0x01, 0x00, 0xFE, 0x0D};
            Function67_Protection = new byte[] {0x7E, 0x01, 0x43, 0x00, 0xFE, 0x0D};
            Function51_BMSVersion = new byte[] {0x7E, 0x01, 0x33, 0x00, 0xFE, 0x0D};
            Function220_SerialNumber = new byte[] {0x7E, 0x01, 0xDC, 0x03, 0x06, 0x00, 0x00, 0xC2, 0x0D};
            Function01_DataBattery_Receive = new byte[255];
            Function67_Protection_Receive = new byte[255];
            Function51_BMSVersion_Receive = new byte[255];
            Function220_SerialNumber_Receive = new byte[255];
            //DataReceive = new byte[255];
            DEC_Function01_DataBattery_Receive = new short[255];

            //InitializeSerialPort();
            InitializeTimer();
            Thread thread = new Thread(Process);
            thread.Start();
        }
        private void InitializeSerialPort()
        {
            PortPin = new SerialPort();
            PortPin.PortName = "COM11";
            PortPin.BaudRate = 9600;
            PortPin.Parity = Parity.None;
            PortPin.StopBits = StopBits.One;
            PortPin.DataReceived += SerialPort_DataReceived;
            try
            {
                PortPin.Open();
            }
            catch (Exception ex)
            {
                //    // Xử lý lỗi nếu có
                MessageBox.Show("Lỗi khi mở cổng serial: " + ex.Message);
            }
        }
        private void InitializeTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (s, e) =>
            {
                OnPropertyChanged("Capacity");
                OnPropertyChanged("Total Volt");
                OnPropertyChanged("Current");
                OnPropertyChanged("Max Temp");
                OnPropertyChanged("Min Temp");
                OnPropertyChanged("Voltage_1");
                OnPropertyChanged("Voltage_2");
                OnPropertyChanged("Voltage_3");
                OnPropertyChanged("Voltage_4");
                OnPropertyChanged("Voltage_5");
                OnPropertyChanged("Voltage_6");
                OnPropertyChanged("Voltage_7");
                OnPropertyChanged("Voltage_8");
                OnPropertyChanged("Voltage_9");
                OnPropertyChanged("Voltage_10");
                OnPropertyChanged("Voltage_11");
                OnPropertyChanged("Voltage_12");
                OnPropertyChanged("Voltage_13");
                OnPropertyChanged("Voltage_14");
                OnPropertyChanged("Voltage_15");
                OnPropertyChanged("Voltage_16");

            };
            timer.Start();
        }
        private void Process()
        {
            InitializeSerialPort();
            //SendData(Function51_BMSVersion);
            //Thread.Sleep(1000);
            //SendData(Function220_SerialNumber);
            //Thread.Sleep(1000);

            while (true)
            {
                SendData(Function01_DataBattery);
                Thread.Sleep(1000);
                //SendData(Function67_Protection);
                //Thread.Sleep(1000);
            }
        }
        private void SendData(byte[] Data_Send)
        {
            
                if (PortPin.IsOpen)
                {
                    PortPin.Write(Data_Send, 0, Data_Send.Length);
                }
        }
        private void ExcuteData()
        {
            short a = 0;
            if (DataReceive[0] == 0x7E)
            {
                if (DataReceive[1] == 0x01)
                {
                    if (DataReceive[2] == 0x01)
                    {
                        Function01_DataBattery_Receive = DataReceive;
                        for (int i = 0; i < Function01_DataBattery_Receive.Length - 1; i++)
                        {
                            byte temp = Function01_DataBattery_Receive[i];
                            Function01_DataBattery_Receive[i] = Function01_DataBattery_Receive[i + 1];
                            Function01_DataBattery_Receive[i + 1] = temp;
                            i++;
                        }
                        for (int n = 0; n < 124; n++)
                        {
                            a = BitConverter.ToInt16(Function01_DataBattery_Receive, 2 * n + 6);
                            if (a > 0)
                                DEC_Function01_DataBattery_Receive[n] = a;

                        }

                    }
                }    
                    
            }    
                
            if (DataReceive[2] == 0x43)
            {
                Function67_Protection_Receive = DataReceive;
            }
            if (DataReceive[2] == 0x33)
            {
                Function51_BMSVersion_Receive = DataReceive;    
            }
            if (DataReceive[2] == 0x42)
            {

            }
            if (DataReceive[2] == 0xDC)
            {
                Function220_SerialNumber_Receive = DataReceive;
            }
        }
        public void WindowClosed()
        {
            // Đóng cổng serial khi cửa sổ đóng lại
            if (PortPin != null && PortPin.IsOpen)
            {
                PortPin.Close();
                PortPin.Dispose();
                Application.Current.Shutdown();
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);//休眠50ms
            try
            {
                int len = PortPin.BytesToRead;
                DataReceive = new byte[len];
                PortPin.Read(DataReceive, 0, len);
                PortPin.DiscardInBuffer();
                ExcuteData();
                //Receive_Data();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Lỗi khi đọc dữ liệu từ cổng serial: " + ex.Message);
            }
        }
        public string Voltage_1
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[0] - 16384)/1000.0).ToString("0.000"); }
        }
        public string Voltage_2
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[1] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_3
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[2] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_4
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[3] - 16384) / 1000.0).ToString("0.000"); }

        }
        public string Voltage_5
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[4] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_6
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[5] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_7
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[6] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_8
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[7] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_9
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[8] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_10
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[9] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_11
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[10] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_12
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[11] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_13
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[12] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_14
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[13] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_15
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[14] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Voltage_16
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[15] - 16384) / 1000.0).ToString("0.000"); }
        }
        public string Current
        {
            get { return ((double)(DEC_Function01_DataBattery_Receive[17]-30000) / 100000.0).ToString("0.000"); }
        }
        public string RemainingCapacity
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[19] / 100.0).ToString("0.00"); }
        }
        public string Temperatures1
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[23] - 70.0).ToString("0.0"); }
        }
        public string Temperatures2
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[24] - 70.0).ToString("0.0"); }
        }
        public string Temperatures3
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[25] - 70.0).ToString("0.0"); }
        }
        public string Temperatures4
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[26] - 70.0).ToString("0.0"); }
        }
        public string Temperatures5
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[27] / - 70.0).ToString("0.0"); }
        }
        public string Temperatures6
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[15] / 1000.0).ToString("0.00"); }
        }

        public string Alarm_bits
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[15] / 1000.0).ToString("0.000"); }
        }
        public string Cycles
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[15] / 1000.0).ToString("0.000"); }
        }
        public string Voltage
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[15] / 1000.0).ToString("0.000"); }
        }
        public string State_of_Health
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[15] / 1000.0).ToString("0.000"); }
        }
        public string ALM_bytes
        {
            get { return ((double)DEC_Function01_DataBattery_Receive[15] / 1000.0).ToString("0.000"); }
        }

    }
}
