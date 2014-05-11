namespace jp.Comms
{
    public class Adxl335Protocol
    {
        public double xGValue = 0.0;
        public double yGValue = 0.0;
        public double zGValue = 0.0;

        public void GetData(string message)
        {
            string[] msgSplit = message.Split(' ');
            xGValue = System.Convert.ToDouble(msgSplit[0]);
            yGValue = System.Convert.ToDouble(msgSplit[1]);
            zGValue = System.Convert.ToDouble(msgSplit[2]);
        }
    }
}

