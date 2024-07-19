import time
import datetime
import logging
from sense_hat import SenseHat

logger = logging.getLogger(__name__)


class Environment:
    def __init__(self):
        self.sense = SenseHat()

    def get_sensor_data(self):
        reading_time = datetime.datetime.now().isoformat()

        try:
            temperature = self.sense.get_temperature()
        except Exception as e:
            logging.error(f"Error reading temperature: {e}")
            temperature = None

        try:
            pressure = self.sense.get_pressure()
        except Exception as e:
            logging.error(f"Error reading pressure: {e}")
            pressure = None

        try:
            humidity = self.sense.get_humidity()
        except Exception as e:
            logging.error(f"Error reading humidity: {e}")
            humidity = None

        return pressure, temperature, humidity, reading_time

    def get_sensor_data_json(self):
        pressure, temperature, humidity, reading_time = self.get_sensor_data()
        return {
            "pressure": pressure,
            "temperature": temperature,
            "humidity": humidity,
            "timestamp": reading_time,
            "errors": {
                "pressure": pressure is None,
                "temperature": temperature is None,
                "humidity": humidity is None,
            },
        }


if __name__ == "__main__":
    env = Environment()

    try:
        while True:
            data = env.get_sensor_data_json()
            print(data)
            time.sleep(1)
    except KeyboardInterrupt:
        print("Terminated by user")
    except Exception as e:
        logging.error(f"An error occurred: {e}")
