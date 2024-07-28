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


def insert_reading_into_db(data: dict):
    """
    Insert a sensor reading into the database.

    Args:
        data (dict): A dictionary containing a SensorReading model's fields.
    """
    try:
        reading = SensorReading.create(
            pressure=data["pressure"],
            temperature=data["temperature"],
            humidity=data["humidity"],
            timestamp=data["timestamp"],
            sent=False,
        )
        logger.info("Data inserted successfully")
        return reading.id

    except Exception as e:
        logger.error("Error inserting data: %s", e)
        raise


def get_unsent_readings():
    """
    Get all unsent readings from the database.
    """
    try:
        return SensorReading.select().where(SensorReading.sent == False)
    except Exception as e:
        logger.error("Error getting unsent readings: %s", e)
        raise


def set_reading_sent_status(id: int, sent_status: bool):
    """
    Update the sent status of a reading in the database.

    Args:
        id (int): The ID of the reading.
        sent_status (bool): The new sent status.
    """
    try:
        query = SensorReading.update(sent=sent_status).where(SensorReading.id == id)
        query.execute()
    except Exception as e:
        logger.error("Error updating sent status: %s", e)
        raise
