---
title: "Sync git modules to .gitmodules"
manufacturer: "ST"
part-number: "LSM303AGR"
---
# LSM303AGR

Ultra-compact high-performance eCompass module:
ultra-low-power 3D accelerometer and 3D magnetometer

## Features

* 3 magnetic field channels and 3 acceleration channels
* ±50 gauss magnetic dynamic range
* ±2/±4/±8/16 g selectable acceleration fullscales
* 16-bit data output
* SPI / I2C serial interfaces
* Analog supply voltage 1.71 V to 3.6 V
* Selectable power mode/resolution for accelerometer and magnetometer
* Single measurement mode for magnetometer
* Programmable interrupt generators for freefall, motion detection and magnetic field detection
* Embedded self-test
* Embedded temperature sensor
* Embedded FIFO
* ECOPACK®, RoHS and “Green” compliant

## Applications

* Tilt-compensated compasses
* Map rotation
* Position detection
* Motion-activated functions
* Free-fall detection

# Configuration

## R:STATUS_REG_AUX_A (07h)

	--	TOR	--	--	--	TDA	--	--

TOR: Temperature data overrun

​	0: no overrun has occurred
​	1: new temperature data has overwritten the previous data 

TDA: Temperature data available 

​	0: new temperature data is not yet available 
​	1: new temperature data is available)

## R:OUT_TEMP_L_A (0Ch), OUT_TEMP_H_A (0Dh)

Temperature sensor 

