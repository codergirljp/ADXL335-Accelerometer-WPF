ADXL335-Accelerometer-WPF
=========================
This is a simple Windows WPF application written in C# for use with the Teensy 3.1 Accelerometer Project.
The Teensy 3.1 Accelerometer project can be found at: 
http://codergirljp.blogspot.com/2014/05/adxl335-accelerometer-on-teensy-31.html



Description -
This application demonstrates how to receive serial data from the accelerometer and use it to change objects displayed in a Windows WPF application.
The data received from the Accelerometer is used to change the size of lines mapped to the X, Y, and Z axis values of the accelerometer.
The Z axis is mapped a  -45 degree angle from the X,Y axis. 

ADXL335SerialApp/ADXL335SerialApp.sln - 
Solution file to build all projects needed for the WPF Application.

ADXL335SerialApp/ADXL335SerialApp.csproj - 
The project file for the WPF interface 
A simple WPF application to use with the Teensy 3.1 Accelerometer 
to display the x,y,z axis data as lines on the screen

jp.Comms/jp.Comms/Comms.csproj - 
Provides serial communications and message protocol 
to receive data from an ADXL335 Teensy 3.1 project running TeensyAdxl335.ino 
 