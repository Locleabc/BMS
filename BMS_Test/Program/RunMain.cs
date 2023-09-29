using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;

namespace BMS_Test.Program
{
    public class RunMain
    {
        List<byte> WriteBufferBattery = new List<byte>() { 0x7E, 0x01, 0x01, 0x00, 0xFE, 0x0D};
        int[] ReceiveBufferBattery = new int[255];
        byte[] ReadBufferBattery;
        int[] WriteBuffer = new int[255];   //WriteBuffer
        
        byte[] data = new byte[1];
        SerialPort PortPin;
        public RunMain()
        {
            PortPin = new SerialPort();
            PortPin.PortName = "COM1";
            PortPin.BaudRate = 9600;
            PortPin.Parity = Parity.None;
            PortPin.StopBits = StopBits.One;
            Task Run = new Task(ReadInfomationBattery);
            PortPin.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(PortPin_DataReceived);
        }
        private void PortPin_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Receive_Data();
        }
        private async void Receive_Data()
        {
            int ByteRead = PortPin.BytesToRead;
            if (ByteRead > 0)
            {
                ReadBufferBattery = new byte[ByteRead];
                PortPin.Read(ReadBufferBattery, 0, ByteRead);
            }
            await Task.Delay(10);
        }
        public void ReadInfomationBattery()
        {
            try
            {
                PortPin.Open();
            }
            catch
            {
                PortPin.Close();
                MessageBox.Show(PortPin.PortName + "Open Error！");
            }
            for (int n = 0; n < 255; n++)
            {
                ReceiveBufferBattery[n] = 0;
            }
            for (int i = 0; i < WriteBufferBattery.Count; i++)
            {
                data[0] = Convert.ToByte(WriteBufferBattery[i]);
                    if (PortPin.IsOpen)
                    {
                        PortPin.Write(data, 0, 1);
                    }
                    else
                    {
                        MessageBox.Show("Port Close!");
                    }
            }
            

        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Đọc dữ liệu từ cổng serial
            // Không cần thực hiện gì đặc biệt ở đây, dữ liệu sẽ được xử lý trong Timer_Tick
        }
    }
}
