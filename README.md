![Unity](https://img.shields.io/badge/Engine-Unity_2022.3-blue?logo=unity)
![Microcontroller](https://img.shields.io/badge/Hardware-ESP8266-lightgrey)
![Protocol](https://img.shields.io/badge/Communication-UDP-green)
![Platform](https://img.shields.io/badge/Platform-Real%20Sensors%20%2B%20Unity-brightgreen)
![Status](https://img.shields.io/badge/Project%20Status-Completed-success)

# 🤖 UnitySense — Real-Time Sensor-Driven Unity Control

A real-time robotic control system powered by physical sensors integrated into a 3D Unity environment. This project uses ESP8266 to send data over Wi-Fi via UDP, controlling a robot, opening doors, and displaying sensor outputs live in-game.

---

## 📌 Project Overview

This project bridges physical sensor data and Unity's virtual environment using Wi-Fi-based communication. A robot character in a Unity scene interacts with doors, temperature UI, and movement controls—all triggered by real sensor inputs.

---
## 📽️ Working Demo

Here’s a glimpse of the real-time interaction between physical sensors and the Unity environment.  
The ESP8266 sends live sensor data over Wi-Fi, which is then used to control the robot, trigger doors, and update the temperature display.

<p align="center">
  <img src="asset/demo.gif" alt="UnitySense Demo" width="300"/>
</p>

> ✅ The robot moves using gyroscope & encoder inputs  
> ✅ Doors toggle via proximity sensor  
> ✅ Temperature values are streamed in real-time and shown in Unity UI

🔗 For a complete walkthrough, see the slides in [`docs/Walkthrough.pdf`](docs/Walkthrough.pdf)

---
## 🔧 Hardware & Software Stack

### Hardware

- **ESP8266**: Microcontroller with Wi-Fi module
- **MPU6050**: Gyroscope, Accelerometer, Temperature sensor
- **Rotary Encoder**
- **Proximity (IR) Sensor**

### Software

- **Unity 2022.3 or later**
- **Arduino IDE** for ESP8266 programming
- **C# scripting** for Unity event handling (VScode)
- **UDP Protocol** for fast real-time transmission

---

## 💡 Core Features

- ✅ Live robot control in Unity using real-time gyroscope + encoder data
- ✅ Doors open based on proximity sensor triggers
- ✅ Temperature sensor displayed in real-time using Unity UI
- ✅ Movement and camera orientation mapped to sensor axes
- ✅ UDP communication pipeline from ESP8266 → Unity

---

## 🧠 Communication Protocol (UDP)

### ESP8266 Output Format

- Sends sensor values as comma-separated strings:

```
gx, gy, gz, temp, proximity, encoderPosition, encoderSwitch
```

- Example: `-12.3,45.8,-3.9,26.7,1,8,0`

### Unity UDP Receiver

- Listens on port `12345`
- Script `UDPReceiver.cs` parses incoming packets
- Updates variables in scripts for:
  - Movement
  - Camera
  - Door toggling
  - Temperature UI

---

## 🎮 Game Mechanics Mapping

| Sensor             | Unity Action                         |
| ------------------ | ------------------------------------ |
| Gyroscope (gy)     | Forward/Backward/Sprint movement     |
| Gyroscope (gx/gz)  | Yaw/Pitch camera control             |
| Encoder Position   | Left/Right strafe                    |
| Encoder Switch     | Jump                                 |
| Proximity Sensor   | Door toggle based on player position |
| Temperature Sensor | UI temperature display using TMPro   |

---

## 🛠️ Unity Scene

The Unity scene includes:

- A robot character controlled by sensor input
- A house with multiple rooms and three sensor-activated doors
- `Playground 1.unity` is the main scene file

### Key Unity Scripts:

- `UDPReceiver.cs`
- `Door1.cs`, `Door2.cs`, `Door3.cs`
- `TemperatureDisplay.cs`
- `ThirdPersonController.cs`

---

### ⚙️ Setup Instructions

#### Unity Setup

1. **Install Unity (2022.3 or later)**  
   Download from: [https://unity.com/releases](https://unity.com/releases)

2. **Open the Project via Unity Hub**  
   Select the UnitySense project folder.

3. **Load Scene**  
   Open `Playground 1.unity` inside the Unity editor.

4. **Check GameObjects**
   - **Robot**: Should have movement + camera control via sensor data.
   - **Doors**: Scripts (`Door1.cs`, etc.) should be attached and respond to proximity inputs.
   - **UDPReceiver**: Must be configured to port `12345`.

5. **Prepare the Microcontroller**
   - Upload the `esp8266_sensor_udp.ino` file to the **NodeMCU (ESP8266)** using the Arduino IDE.
   - Ensure the correct **board type** and **COM port** are selected.
   - Verify all sensors are properly connected and powered.

6. **Firewall Settings**  
   Make sure your **firewall allows UDP traffic** on port `12345`. This is necessary for Unity to receive data from the ESP8266.

7. **Run the Unity Scene**  
   Press the ▶️ **Play** button in Unity. The robot should begin responding to sensor inputs once the NodeMCU is powered and running.

---

## 📄 References

- Unity Docs: [https://docs.unity3d.com/](https://docs.unity3d.com/)
- ESP8266 Arduino Core: [https://github.com/esp8266/Arduino](https://github.com/esp8266/Arduino)
- UDP in C#: [https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.udpclient](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.udpclient)
- MPU6050 Guide: [https://randomnerdtutorials.com/mpu6050-esp8266-arduino/](https://randomnerdtutorials.com/mpu6050-esp8266-arduino/)

---

## 📩 Contact
I’m excited to connect and collaborate!  
- **Email**: [gbrohiith@gmail.com](mailto:your.email@example.com)  
- **LinkedIn**: [https://www.linkedin.com/in/rohiithgb/](https://linkedin.com/in/yourprofile)  
- **GitHub**: [https://github.com/GBR-RL/](https://github.com/yourusername)

---

## 📚 License
This project is open-source and available under the [MIT License](LICENSE).  

---

🌟 **If you like this project, please give it a star!** 🌟
