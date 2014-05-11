namespace jp.Comms
{
    using System.Collections.Generic;
    using System.IO.Ports;

    /// <summary>
    /// Delegate for the OnDataRecieved event
    /// </summary>
    /// <param name="message">The serial port message being received.</param>
    public delegate void NotifyDataReceived(string message);

    /// <summary>
    /// Serial Port Manager Class
    /// Provides functionally to manager serail port communications
    /// with a device. 
    /// </summary>
    public class SerialPortManager
    {
        /// <summary>
        /// The Serial Port instance.
        /// </summary>
        public SerialPort serialPort;

        /// <summary>
        /// Data Queue used to queue serial port data packets if UseDataQueue is true
        /// </summary>
        public Queue<string> DataQueue = new Queue<string>();

        /// <summary>
        /// Delegate called when data is received by serial port only if UseDataQueue is false
        /// </summary>
        public event NotifyDataReceived OnDataReceived;

        /// <summary>
        /// UseDataQueue flag.
        /// Set to true if the data queue should be used.
        /// By default the flag is set to false.
        /// </summary>
        public bool UseDataQueue = false;


        /// <summary>
        /// The default constructor for the Serial Port Manager class.
        /// Creates a new instance of the Serial Port Manager class.
        /// If this is used, OpenSerialPort must be called later to open the serial port.
        /// </summary>
        public SerialPortManager()
        {
        }


        /// <summary>
        /// A constructor for the Serial Port Manager class.
        /// Creates a new instance of the Serial Port Manager class and opens serial port 
        /// using the port and baud rate defined by the user.
        /// </summary>
        /// <param name="port">User defined COM Port to use.</param>
        /// <param name="baudRate">User defined Baud Rate to use.</param>
        public SerialPortManager(string port, int baudRate)
        {
            OpenSerialPort(port, baudRate);
        }

        /// <summary>
        /// Checks to see if the Serial Port is Open.
        /// </summary>
        /// <returns>Returns true if the port is open, false otherwise.</returns>
        public bool IsAvailable()
        {
            if (serialPort != null)
            {
                return serialPort.IsOpen;
            }
            return false;
        }

        /// <summary>
        /// Opens a serial port on the specified comm port using the specifed baud rate.
        /// This gets called by the constructor if the serial port, buad rate are passed in. 
        /// </summary>
        /// <param name="port">The COM port to use for the serial connection.</param>
        /// <param name="baudRate">The baud rate to use.</param>
        public void OpenSerialPort(string port, int baudRate)
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort(port, baudRate);
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                serialPort.Open();
            }
        }

        /// <summary>
        /// This normally gets called if the serial port receives data 
        /// Reads a line of data from the serial port as a string.
        /// </summary>
        /// <param name="sender">Serial Port object.</param>
        /// <param name="e">Serial Data Received Event Args.</param>
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

        /// <summary>
        /// Closes the Serial Port and disposes of it.
        /// </summary>
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

