import sys
import logging
from os import getenv
from dotenv import load_dotenv
from time import sleep

from rpi_scripts.collect_data import initialize_db, collect_reading, insert_reading
from rpi_scripts.environment import Environment
from rpi_scripts.transmit_data import post_sensor_reading

DEFAULT_SLEEP_INTERVAL = 5

logging.basicConfig(
    level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s"
)
logger = logging.getLogger(__name__)


def main(sleep_interval):
    """Collects and inserts reading every sleep_interval seconds.
    Then sends to the API."""

    initialize_db()
    env = Environment()

    load_dotenv()

    API_URL = getenv("API_URL")
    API_PORT = getenv("API_PORT")
    API_KEY = getenv("API_KEY")

    if not all([API_URL, API_PORT, API_KEY]):
        logger.error("API_URL, API_PORT, and API_KEY must be set in the environment.")
        sys.exit(1)

    while True:
        try:
            data = collect_reading(env)
            if data:
                insert_reading(data)

            response = post_sensor_reading(data, API_URL, API_PORT, API_KEY)

            if response:
                logger.info(f"API response: {response}")
            else:
                logger.error("No or empty response from API")

            sleep(sleep_interval)

        except KeyboardInterrupt:
            logging.info("Terminated by user")
            break
        except Exception as e:
            logging.error(f"Error in main loop: {e}")
            sleep(sleep_interval)


if __name__ == "__main__":
    if len(sys.argv) < 2:
        sleep_interval = DEFAULT_SLEEP_INTERVAL
    else:
        try:
            sleep_interval = int(sys.argv[1])
        except ValueError:
            logging.error("Invalid sleep interval provided. Using default interval.")
            sleep_interval = DEFAULT_SLEEP_INTERVAL

    main(sleep_interval)
