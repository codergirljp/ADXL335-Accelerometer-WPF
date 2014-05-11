namespace jp.Comms
{
    /// <summary>
    /// A class to translate the Adxl335 accelerometer messages received from the TeensyAdxl335.ino accelerometer code 
    /// into x, y, z values in G's. 
    /// </summary>
    public class Adxl335Protocol
    {
        // The x-axis G value 
        public double xGValue = 0.0;

        // The y axis G value
        public double yGValue = 0.0;

        // The z axis G value
        public double zGValue = 0.0;

        /// <summary>
        /// Populates the x, y, z data values from the message pass in.
        /// </summary>
        /// <param name="message">The raw serial port message.</param>
        public void GetData(string message)
        {
            string[] msgSplit = message.Split(' ');
            xGValue = System.Convert.ToDouble(msgSplit[0]);
            yGValue = System.Convert.ToDouble(msgSplit[1]);
            zGValue = System.Convert.ToDouble(msgSplit[2]);
        }
    }
}

