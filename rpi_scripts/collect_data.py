from rpi_scripts.data.models import SensorReading
from rpi_scripts.environment import Environment

from peewee import SqliteDatabase
import logging

DB_NAME = "sensor_data.db"

logger = logging.getLogger(__name__)

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
            pressure=data["pressure"],
            temperature=data["temperature"],
            humidity=data["humidity"],
            lux=data["lux"],
            uvs=data["uvs"],
            gas=data["gas"],
            timestamp=data["time"],
        )
        logging.info("Data inserted successfully")
    except Exception as e:
        logging.error(f"Error inserting data: {e}")
