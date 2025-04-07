// Including necessary libraries for the project
#include <ESP8266WiFi.h>   // Library to handle WiFi connections on ESP8266
#include <WiFiUdp.h>       // Library for sending and receiving data over UDP
#include <Wire.h>          // I2C library for communication with MPU6050
#include <MPU6050.h>       // Library to interact with the MPU6050 sensor

// WiFi Configuration
const char* ssid = "Hello";             // Our WiFi SSID
const char* password = "987654321";         // Our WiFi Password
const char* remoteIP = "192.168.01.23";   // Our laptop's IP address
const unsigned int remotePort = 12345;      // Port on which Unity listens

WiFiUDP udp; // UDP object for data transmission
MPU6050 mpu; // MPU6050 object for sensor data

// IR Sensor Pin
const int irSensorPin = D5;      // Digital pin connected to IR sensor

// Encoder Switch Pin
const int encoderSwitchPin = D3; // Pin connected to encoder switch

// Constants for scaling
const float accelScaleFactor = 16384.0; // Scaling factor for accelerometer (±2g)
const float gyroScaleFactor = 131.0;    // Scaling factor for gyroscope (±250°/s)

// Rotary Encoder Pins
const int encoderCLK = D6; // Clock pin for rotary encoder
const int encoderDT = D7;  // Data pin for rotary encoder

// Rotary Encoder Variables
volatile int encoderPosition = 0; // Tracks the encoder's position
int lastCLKState;                 // Stores the last state of the CLK pin

// Filter parameters
const float alpha = 0.91; // Low-pass filter smoothing factor

// Filtered sensor values
float filteredAccelX = 0, filteredAccelY = 0, filteredAccelZ = 0;
float filteredGyroX = 0, filteredGyroY = 0, filteredGyroZ = 0;


// Interrupt Service Routine for handling rotary encoder position
void IRAM_ATTR handleEncoder() {
  int currentCLKState = digitalRead(encoderCLK);
  if (currentCLKState != lastCLKState && currentCLKState == LOW) {
    // Determine direction of rotation
    if (digitalRead(encoderDT) != currentCLKState) {
      encoderPosition++; // Clockwise
    } else {
      encoderPosition--; // Counterclockwise
    }
  }
  lastCLKState = currentCLKState; // Update the last state
}

void setup() {
  Serial.begin(115200);  // Initialize serial communication

  // Setup IR sensor pin as INPUT
  pinMode(irSensorPin, INPUT);

  // Setup Encoder Switch Pin as INPUT
  pinMode(encoderSwitchPin, INPUT);

  // Setup Rotary Encoder pins
  pinMode(encoderCLK, INPUT);
  pinMode(encoderDT, INPUT);
  lastCLKState = digitalRead(encoderCLK);   // Read initial state
  attachInterrupt(digitalPinToInterrupt(encoderCLK), handleEncoder, CHANGE); // Attach interrupt to CLK pin

  // Connect to WiFi
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {   // Wait for connection
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nConnected to WiFi!");

  // Initialize MPU6050
  Wire.begin();  // Start I2C communication
  mpu.initialize();  // Initialize the MPU6050 sensor
  if (mpu.testConnection()) {
    Serial.println("MPU6050 connection successful");
  } else {
    Serial.println("MPU6050 connection failed");
  }
}

void loop() {
  // Read IR sensor state (active low)
  bool irSensorState = digitalRead(irSensorPin) == LOW ? 1 : 0;

  // Read encoder switch state (active low)
  bool encoderSwitchState = digitalRead(encoderSwitchPin) == LOW ? 1 : 0;

  // Read temperature from MPU6050 and convert to Celsius
  int16_t tempRaw = mpu.getTemperature();
  float temperature = (tempRaw / 340.0) + 36.53;

  // Read raw accelerometer data
  int16_t ax, ay, az;
  mpu.getAcceleration(&ax, &ay, &az);
  

  // Read raw gyroscope data
  int16_t gx, gy, gz;
  mpu.getRotation(&gx, &gy, &gz);

// Convert raw accelerometer data to m/s²
float accelX = (ax / accelScaleFactor);
float accelY = (ay / accelScaleFactor);
float accelZ = (az / accelScaleFactor);

// Convert gyroscope data to °/s and apply offsets
float gyroX = gx / gyroScaleFactor + 0.7;
float gyroY = gy / gyroScaleFactor + 2.4;
float gyroZ = gz / gyroScaleFactor + 0.9;

  // Apply low-pass filter to accelerometer data
  filteredAccelX = alpha * accelX + (1 - alpha) * filteredAccelX;
  filteredAccelY = alpha * accelY + (1 - alpha) * filteredAccelY;
  filteredAccelZ = alpha * accelZ + (1 - alpha) * filteredAccelZ;

  // Apply low-pass filter to gyroscope data
  filteredGyroX = alpha * gyroX + (1 - alpha) * filteredGyroX;
  filteredGyroY = alpha * gyroY + (1 - alpha) * filteredGyroY;
  filteredGyroZ = alpha * gyroZ + (1 - alpha) * filteredGyroZ;

  // Read the current encoder position
  int currentEncoderPosition = encoderPosition;

  // Format data into a comma-separated string
  String data = String(irSensorState) + "," +
                String(temperature, 1) + "," +
                String(filteredAccelX, 2) + "," +
                String(filteredAccelY, 2) + "," +
                String(filteredAccelZ, 2) + "," +
                String(filteredGyroX, 2) + "," +
                String(filteredGyroY, 2) + "," +
                String(filteredGyroZ, 2) + "," +
                String(currentEncoderPosition) + "," + 
                String(encoderSwitchState); 

  // Send data via UDP
  udp.beginPacket(remoteIP, remotePort);
  udp.write(data.c_str());
  udp.endPacket();

  // Debugging: Print data to Serial Monitor
  Serial.print("Data sent: ");
  Serial.println(data);
  


  delay(10);  // Delay for data rate control
}
