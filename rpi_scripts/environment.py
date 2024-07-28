"""
Module for interfacing with the Sense HAT sensor.
"""

import time
import datetime
import logging
from sense_hat import SenseHat

logger = logging.getLogger(__name__)


class Environment:
    """
    Class to interact with the Sense HAT sensor.
    """

    def __init__(self):
        self.sense = SenseHat()

    def get_sensor_data(self):
        """Returns sensor data as a tuple"""
        timestamp = datetime.datetime.now().isoformat()

        try:
            temperature = self.sense.get_temperature()
        except Exception as temp_error:
            logging.error("Error reading temperature: %s", temp_error)
            temperature = None

        try:
            pressure = self.sense.get_pressure()
        except Exception as pressure_error:
            logging.error("Error reading pressure: %s", pressure_error)
            pressure = None

        try:
            humidity = self.sense.get_humidity()
        except Exception as humidity_error:
            logging.error("Error reading humidity: %s", humidity_error)
            humidity = None

        return pressure, temperature, humidity, timestamp

    def get_sensor_data_dict(self):
        """Returns sensor data as a dictionary object"""
        pressure, temperature, humidity, timestamp = self.get_sensor_data()
        return {
            "pressure": pressure,
            "temperature": temperature,
            "humidity": humidity,
            "timestamp": timestamp,
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
            data = env.get_sensor_data_dict()
            print(data)
            time.sleep(1)
    except KeyboardInterrupt:
        logging.info("Terminated by user")
    except Exception as main_error:
        logging.error("An error occurred: %s", main_error)
