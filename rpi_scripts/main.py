"""
Main script to handle data collection, insertion, and transmission to the backend.
"""

import sys
import logging
import requests
from os import getenv
from time import sleep
from threading import Thread, Lock
from dotenv import load_dotenv

from rpi_scripts.data.database_utils import (
    initialize_db,
    insert_reading_into_db,
    get_unsent_readings,
    set_reading_sent_status,
)
from rpi_scripts.environment import Environment

DEFAULT_GATHER_INTERVAL = 5
MAX_BACKOFF_TIME = 3600

logging.basicConfig(
    level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s"
)
logger = logging.getLogger(__name__)

db_lock = Lock()


def collect_data(interval):
    """
    Collects and inserts reading every interval seconds.
    """
    environment = Environment()

    while True:
        try:
            sensor_reading = environment.get_sensor_data_dict()
            if sensor_reading:
                with db_lock:
                    insert_reading_into_db(sensor_reading)
                logger.info("Data inserted into the database")

            sleep(interval)

        except KeyboardInterrupt:
            logging.info("Terminated by user")
            break
        except Exception as e:
            logger.error("Error in data collection loop: %s", e)
            sleep(interval)


def transmit_unsent_data(api_url, api_port, api_key):
    """
    Continuously attempts to resend all unsent sensor data to the backend API with exponential backoff.
    """
    backoff_time = 1  # Initial backoff time in seconds

    while True:
        try:
            with db_lock:
                unsent_readings = get_unsent_readings()

            for reading in unsent_readings:
                data = {
                    "pressure": reading.pressure,
                    "temperature": reading.temperature,
                    "humidity": reading.humidity,
                    "timestamp": reading.timestamp,
                }
                post_success = post_sensor_reading(data, api_url, api_port, api_key)
                if post_success:
                    logger.info("Data sent to API successfully")
                    with db_lock:
                        set_reading_sent_status(reading.id, True)
                    backoff_time = 1  # Reset backoff time after a successful send
                else:
                    logger.error(
                        "Failed to resend data to API. Retrying in %s seconds...",
                        backoff_time,
                    )
                    sleep(backoff_time)
                    backoff_time = min(
                        backoff_time * 2, MAX_BACKOFF_TIME
                    )  # Exponential backoff with cap

            sleep(1)

        except Exception as e:
            logger.error("Error in data transmission loop: %s", e)
            sleep(backoff_time)
            backoff_time = min(
                backoff_time * 2, MAX_BACKOFF_TIME
            )  # Exponential backoff with cap


def post_sensor_reading(data, api_url, api_port, api_key):
    """
    Sends sensor data to the backend API.

    Args:
        data (dict): A dictionary containing sensor data.
        api_url (str): The address of the API.
        api_port (str): The port of the API.
        api_key (str): The API key for authentication.
    """
    headers = {"Authorization": f"{api_key}", "Content-Type": "application/json"}

    try:
        response = requests.post(
            f"{api_url}:{api_port}/api/sensorData",
            json=data,
            headers=headers,
            timeout=10,
        )
        response.raise_for_status()

        return response.json()

    except requests.exceptions.HTTPError as http_err:
        logger.error("HTTP error: %s", http_err)
    except requests.exceptions.ConnectionError as conn_err:
        logger.error("Connection error: %s", conn_err)
    except requests.exceptions.Timeout as timeout_err:
        logger.error("Timeout error: %s", timeout_err)
    except requests.exceptions.RequestException as req_err:
        logger.error("An error occurred: %s", req_err)
    except Exception as e:
        logger.error("Unexpected error: %s", e)

    return None


def main(interval):
    """
    Initializes the database and starts data collection and transmission loops.
    """

    initialize_db()
    load_dotenv()

    api_url = getenv("API_URL")
    api_port = getenv("API_PORT")
    api_key = getenv("API_KEY")

    if not all([api_url, api_port, api_key]):
        logger.error("API_URL, API_PORT, or API_KEY not found in environment variables")
        sys.exit(1)

    data_collection_thread = Thread(target=collect_data, args=(interval,))
    data_collection_thread.start()

    data_transmission_thread = Thread(
        target=transmit_unsent_data, args=(api_url, api_port, api_key)
    )
    data_transmission_thread.start()

    data_collection_thread.join()
    data_transmission_thread.join()


if __name__ == "__main__":
    try:
        GATHER_INTERVAL = (
            int(sys.argv[1]) if len(sys.argv) > 1 else DEFAULT_GATHER_INTERVAL
        )
    except ValueError:
        logging.error("Invalid sleep interval provided. Using default interval.")
        GATHER_INTERVAL = DEFAULT_GATHER_INTERVAL

    main(GATHER_INTERVAL)
