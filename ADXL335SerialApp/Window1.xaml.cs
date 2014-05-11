using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using jp.Comms;

namespace ADXL335
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// Serial Port Manager Instance
        /// Provide functionality for handling the send and receive of data 
        /// from a serial port.
        /// </summary>
        SerialPortManager serialPortManager;

        /// <summary>
        /// A Adxl335Protocol message handler
        /// Translates messages received from the Teensy Accelerometer into 
        /// x, y, z values
        /// </summary>
        Adxl335Protocol msgHandler = new Adxl335Protocol();

        const string comPort = "COM6";
        const int baudRate = 38400;
        const bool useDataQueue = false;

        public Window1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delegate to allow updating window objects from external thread
        /// </summary>
        /// <param name="message"></param>
        public delegate void UpdateTextCallback(string message);
        private void UpdateText(string message) 
        {
            //serialPortManager.DataQueue.Dequeue();
            msgHandler.GetData(message);

            Xg.X2 = Xg.X1 + (80.0 * msgHandler.yGValue);
            Yg.Y2 = Yg.Y1 + (-80.0 * msgHandler.xGValue);
            Zg.X2 = Zg.X1 + (40.0 * msgHandler.zGValue);
            Zg.Y2 = Zg.Y1 + (40.0 * msgHandler.zGValue);

           // Uncomment to Log Event Incomming messages to the Text Box 
           // and create a local log file of messages
           //// string eventMsg=String.Format("{0} - {1}", System.DateTime.Now, message);
           //// richTextBox1.AppendText(eventMsg);
           //// LogEvents(eventMsg);
        }

        /// <summary>
        /// Handles the Start/Stop button click event. 
        /// Starts the Serial port if the button said Start.
        /// Closes the Serial Port if the button said Stop.
        /// </summary>
        /// <param name="sender">The start/stop button.</param>
        /// <param name="e">The button click event args.</param>
        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (button1.Content.ToString() == "Stop")
                {
                    serialPortManager.CloseSerialPort();
                    button1.Content = "Start";
                }
                else
                {
                    serialPortManager = new SerialPortManager(comPort, baudRate);
                    serialPortManager.UseDataQueue = false;
                    serialPortManager.OnDataReceived += this.serialPort_DataReceivedHandler;
                    button1.Content = "Stop";
                }
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
                if (serialPortManager != null && serialPortManager.IsAvailable())
                {
                    serialPortManager.CloseSerialPort();
                }
             
                button1.Content = "Stop";
            }
        }

        /// <summary>
        /// Data Received Handler. 
        /// Subscribes to the Serial Port Receive event and gets called when a message is received.
        /// </summary>
        /// <param name="message">The message received.</param>
        void serialPort_DataReceivedHandler(string message)
        {
            try
            {
                richTextBox1.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { message });
                
            }
            catch (Exception ex1)
            {
                richTextBox1.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), new object[] { ex1.Message });
            }
        }

        /// <summary>
        /// Take care of some clean up on Window Closing to shut down nicely
        /// </summary>
        /// <param name="sender">The sending object from the window.</param>
        /// <param name="e">The Cancel Event Args from the window closing event.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (serialPortManager != null && serialPortManager.IsAvailable())
            {
                serialPortManager.CloseSerialPort();
            }
        }

        /// <summary>
        /// Log Events 
        /// Used to log event messages to a Sensor Events Log text file if uncommented above.
        /// </summary>
        /// <param name="EventMessageText">The event message to log.</param>
        private void LogEvents(string EventMessageText)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo("SensorEventsLog.txt");

            if (fi != null && !fi.Exists)
            {
                System.IO.FileStream fs = fi.Create();
                fs.Close();
                fs.Dispose();
                fs = null;
            }

            System.IO.StreamWriter sw = fi.AppendText();
            sw.WriteLine(EventMessageText);
            sw.Close();
            sw.Dispose();
            sw = null;
        }
    }
}
