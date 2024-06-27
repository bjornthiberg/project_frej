import time
from sensors import ICM20948 #Gyroscope/Acceleration/Magnetometer
from sensors import BME280   #Atmospheric Pressure/Temperature and humidity
from sensors import LTR390   #UV
from sensors import TSL2591  #LIGHT
from sensors import SGP40
import datetime
from PIL import Image,ImageDraw,ImageFont
import math

class Environment:
    def __init__(self):
        self.atmosphere = BME280.BME280()
        self.atmosphere.get_calib_param()
        self.light = TSL2591.TSL2591()
        self.uv = LTR390.LTR390()
        self.air_quality = SGP40.SGP40()

    def get_data(self):        
        reading_time = datetime.datetime.now().isoformat()
        try:
            bme_data = self.atmosphere.readData()
            pressure = bme_data[0]
            temperature = bme_data[1]
            humidity = bme_data[2]
        except Exception as e:
            print(f"Error reading BME280 data: {e}")
            pressure = None
            temperature = None
            humidity = None

        try:
            lux = self.light.Lux()
        except Exception as e:
            print(f"Error reading TSL2591 data: {e}")
            lux = None

        try:
            uvs = self.uv.UVS()
        except Exception as e:
            print(f"Error reading LTR390 data: {e}")
            uvs = None

        try:
            gas = self.air_quality.measureRaw(temperature, humidity)
        except Exception as e:
            print(f"Error reading SGP40 data: {e}")
            gas = None

        return pressure, temperature, humidity, lux, uvs, gas, reading_time

    def get_data_json(self):
        pressure, temperature, humidity, lux, uvs, gas, reading_time = self.get_data()
        return {
            "pressure": pressure,
            "temperature": temperature,
            "humidity": humidity,
            "lux": lux,
            "uvs": uvs,
            "gas": gas,
            "time": reading_time
        }

if __name__ == '__main__':
    env = Environment()
    while True:
        print(env.get_data_json())
        time.sleep(1)
