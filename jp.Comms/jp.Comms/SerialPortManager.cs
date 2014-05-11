namespace jp.Comms
{
    using System.Collections.Generic;
    using System.IO.Ports;

   // a call back to allow delegates to be called
   public delegate void NotifyDataReceived(string message);

    public class SerialPortManager
    {
        const string DEFAULT_PORT = "COM6";
        const int DEFAULT_BAUDRATE = 38400;

        public SerialPort serialPort;

        /// <summary>
        /// Data Queue used to queue serial port data packets if UseDataQueue is true
        /// </summary>
        public Queue<string> DataQueue = new Queue<string>();

        /// <summary>
        /// Delegate called when data is received by serial port only if UseDataQueue is false
        /// </summary>
        public event NotifyDataReceived OnDataReceived;

        public bool UseDataQueue = true;

        public SerialPortManager()
        {
            // OpenSerialPort (DEFAULT_PORT, DEFAULT_BAUDRATE);
        }

        public SerialPortManager(string port, int baudRate)
        {
            OpenSerialPort(port, baudRate);
        }

        public bool IsAvailable()
        {
            if (serialPort != null)
            {
                return serialPort.IsOpen;
            }
            return false;
        }

        public void OpenSerialPort(string port, int baudRate)
        {
            serialPort = new SerialPort("COM6", baudRate);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort.Open();
        }

        /// <summary>
        /// This normally gets called if the serial port receives data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // read the data from the serial port
            string data = (sender as SerialPort).ReadLine();

            if (UseDataQueue)
            {
                // queue the data
                DataQueue.Enqueue(data);
            }
            else
            {
                // call any subscribers
                this.OnDataReceived(data);
            }
        }

        public void CloseSerialPort()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort = null;
            }
        }

    }
}

