"""
Module for handling the local database.
"""

import logging
from peewee import SqliteDatabase
from rpi_scripts.data.models import SensorReading, db

DB_NAME = "sensor_data.db"

logger = logging.getLogger(__name__)

database = SqliteDatabase(DB_NAME)
db.initialize(database)


def initialize_db():
    """
    Connecting to database and create tables.
    """
    try:
        with database:
            database.create_tables([SensorReading], safe=True)
        logger.info("Database initialized")
    except Exception as e:
        logger.error("Error initializing database: %s", e)
        raise


def insert_reading_into_db(data):
    """
    Insert a sensor reading into the database.

    Args:
        data (dict): A dictionary containing a SensorReading model's fields.
    """
    try:
        SensorReading.create(
            pressure=data["pressure"],
            temperature=data["temperature"],
            humidity=data["humidity"],
            timestamp=data["timestamp"],
        )
        logger.info("Data inserted successfully")
    except Exception as e:
        logger.error("Error inserting data: %s", e)
        raise
