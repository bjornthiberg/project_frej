"""
Main script to handle data collection, insertion, and transmission to Kafka.
"""

import sys
import logging
from os import getenv
from time import sleep
from dotenv import load_dotenv

from rpi_scripts.data.database_utils import initialize_db, insert_reading_into_db
from rpi_scripts.environment import Environment
from rpi_scripts.transmit_data import send_to_kafka

DEFAULT_GATHER_INTERVAL = 5

logging.basicConfig(
    level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s"
)
logger = logging.getLogger(__name__)


def main(interval):
    """
    Collects and inserts reading every sleep_interval seconds.
    Then sends to Kafka Producer.
    """

    initialize_db()
    environment = Environment()

    load_dotenv()

    kafka_broker = getenv("KAFKA_BROKER")
    kafka_topic = getenv("KAFKA_TOPIC")

    if not all([kafka_broker, kafka_topic]):
        logger.error("KAFKA_BROKER and KAFKA_TOPIC must be set in the environment.")
        sys.exit(1)

    while True:
        try:
            sensor_reading = environment.get_sensor_data_dict()
            if sensor_reading:
                insert_reading_into_db(sensor_reading)
                logger.info("Data inserted into the database")
            else:
                logger.error("Failed to read sensor data")

            success = send_to_kafka(sensor_reading, kafka_broker, kafka_topic)

            if success:
                logger.info("Data sent to Kafka successfully")
            else:
                logger.error("Failed to send data to Kafka")

            sleep(interval)

        except KeyboardInterrupt:
            logging.info("Terminated by user")
            break
        except Exception as e:
            logging.error("Error in main loop: %s", e)
            sleep(interval)


if __name__ == "__main__":
    if len(sys.argv) < 2:
        GATHER_INTERVAL = DEFAULT_GATHER_INTERVAL
    else:
        try:
            GATHER_INTERVAL = int(sys.argv[1])
        except ValueError:
            logging.error("Invalid sleep interval provided. Using default interval.")
            GATHER_INTERVAL = DEFAULT_GATHER_INTERVAL

    main(GATHER_INTERVAL)
