from rpi_scripts.data.models import SensorReading
from rpi_scripts.environment import Environment

from peewee import SqliteDatabase
import sys
import logging
from time import sleep

DEFAULT_SLEEP_INTERVAL = 5
DB_NAME = 'sensor_data.db'

logging.basicConfig(level=logging.INFO, format = '%(asctime)s - %(levelname)s - %(message)s')

db = SqliteDatabase(DB_NAME)

SensorReading._meta.database = db

def initialize_db():
    db.connect()
    db.create_tables([SensorReading], safe=True)
    logging.info("Database initialized")

def collect_reading(env):
    """Collect one reading from the environment."""
    try:
        data = env.get_sensor_data_json()
        logging.info(f"Data collected: {data}")
        return data
    except Exception as e:
        logging.error(f"Error collecting data: {e}")
        return None
    
def insert_reading(data):
    """Insert a sensor reading into the database."""
    try:    
        SensorReading.create(
            pressure=data['pressure'],
            temperature=data['temperature'],
            humidity=data['humidity'],
            lux=data['lux'],
            uvs=data['uvs'],
            gas=data['gas'],
            timestamp=data['time']
        )
        logging.info("Data inserted successfully")
    except Exception as e:
        logging.error(f"Error inserting data: {e}")

def main(sleep_interval):
    """Collects and inserts reading every sleep_interval seconds."""
    initialize_db()
    env = Environment()
    
    while True:
        try:
            data = collect_reading(env)
            if data:
                insert_reading(data)
            sleep(sleep_interval)
        except KeyboardInterrupt:
            logging.info("Terminated by user")
            break
        except Exception as e:
            logging.error(f"Error in main loop: {e}")
            sleep(sleep_interval)

if __name__ == '__main__':
    if len(sys.argv) < 2:
        sleep_interval = DEFAULT_SLEEP_INTERVAL
    else:
        try:
            sleep_interval = int(sys.argv[1])
        except ValueError:
            logging.error("Invalid sleep interval provided. Using default interval.")
            sleep_interval = DEFAULT_SLEEP_INTERVAL

    main(sleep_interval)
