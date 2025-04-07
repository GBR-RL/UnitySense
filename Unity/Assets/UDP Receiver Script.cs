using System;                      // Provides fundamental classes and base classes for the .NET framework.
using System.Net;                  // Contains classes for basic network operations, such as IP addresses and endpoints.
using System.Net.Sockets;          // Enables low-level network communications using sockets.
using System.Text;                 // Provides classes to manipulate and encode text.
using UnityEngine;                 // Core Unity engine namespace, includes classes for game objects, components, and MonoBehaviour.

public class UDPReceiver : MonoBehaviour
{
    
    private UdpClient udpClient;
    private int listenPort = 12345;   // Matches the Arduino code port
    private string receivedData = ""; // UDP client and port for receiving data
    
    // Shared data variables
    public float temperature = 0.0f; // Temperature data from MPU6050
    public int proximity = 0;        // IR sensor proximity data
    public int lastproximity = 0;    // Tracks previous proximity state
    public int highprox = 0;            // Rising edge detector for the proximity sensor

    public float ax, ay, az; // Accelerometer readings
    public static float gx, gy, gz; //Gyroscope readings
    public static int encoderPosition = 0;     // Rotary encoder position
    public static int lastencPosition = 0;     // Tracks previous encoder position
    public static int encoderSwitch = 0;       // Rotary encoder switch state
    public static int lastSwitch = 0;          // Tracks previous encoder switch state

    // Door references (assigned via the Unity Inspector)
    public TemperatureDisplay temperaturedisplay; // Reference to temperature display script
    public Door1 door1;                           // Reference to Door1 script
    public Door2 door2;                           // Reference to Door2 script
    public Door3 door3;                           // Reference to Door3 script

    // Start listening for UDP data
    void Start()
    {
        udpClient = new UdpClient(listenPort); // Initialize UDP client
        udpClient.BeginReceive(OnUDPDataReceived, null); // Start async receive
        Debug.Log($"Listening for UDP data on port {listenPort}...");
    }

// Update each frame (pass data to other scripts here)
void Update()
{
    if (lastproximity == 0 && proximity == 1)
    {
        highprox = 1; // Trigger a high state if proximity changes from 0 to 1
    }

    // Send updated data to connected scripts
    SendDataToScripts();

    // Update state tracking variables
    lastproximity = proximity;
    lastSwitch = encoderSwitch;
    highprox = 0;
}

    // Handle incoming UDP data
    private void OnUDPDataReceived(IAsyncResult result)
    {
        try
        {
            // Receive UDP data
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
            byte[] receivedBytes = udpClient.EndReceive(result, ref remoteEndPoint);
            receivedData = Encoding.UTF8.GetString(receivedBytes); // Convert bytes to string

            // Parse the received data
            ParseReceivedData(receivedData);

            // Continue listening for more data
            udpClient.BeginReceive(OnUDPDataReceived, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error receiving UDP data: {ex.Message}"); // Handle errors
        }
    }

// Parse the received comma-separated data
private void ParseReceivedData(string data)
{
    string[] dataParts = data.Split(',');  // Split data by commas

    if (dataParts.Length == 10) // Ensure correct data format
    {
        // Parse the data into appropriate variables
        if (int.TryParse(dataParts[0], out proximity))
            Debug.Log($"Proximity: {proximity}");

        if (float.TryParse(dataParts[1], out temperature))
            Debug.Log($"Temperature: {temperature}");

        if (float.TryParse(dataParts[2], out ax))
            Debug.Log($"Filtered Accelerometer X: {ax}");

        if (float.TryParse(dataParts[3], out ay))
            Debug.Log($"Filtered Accelerometer Y: {ay}");

        if (float.TryParse(dataParts[4], out az))
            Debug.Log($"Filtered Accelerometer Z: {az}");

        if (float.TryParse(dataParts[5], out gx))
            Debug.Log($"Filtered Gyroscope X: {gx}");

        if (float.TryParse(dataParts[6], out gy))
            Debug.Log($"Filtered Gyroscope Y: {gy}");

        if (float.TryParse(dataParts[7], out gz))
            Debug.Log($"Filtered Gyroscope Z: {gz}");

        if (int.TryParse(dataParts[8], out encoderPosition))
            Debug.Log($"Encoder Position: {encoderPosition}");

        if (int.TryParse(dataParts[9], out encoderSwitch))
                Debug.Log($"Encoder Switch: {encoderSwitch}");
    }
}

    // Method to send the data to other scripts
    private void SendDataToScripts()
    {
        // Update temperature display
        if (temperaturedisplay != null)
        {
            temperaturedisplay.UpdateTemperature(temperature);
        }

        // Trigger door-related logic
        if (door1 != null)
        {
            door1.CheckHigh1(highprox);
        }

        if (door2 != null)
        {
            door2.CheckHigh2(highprox);
        }

        if (door3 != null)
        {
            door3.CheckHigh3(highprox);
        }
    }
}
