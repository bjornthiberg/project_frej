"""
Module for transmitting data to the backend API and Kafka.
"""

import json
import logging
import requests
from kafka import KafkaProducer


logger = logging.getLogger(__name__)


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
            timeout=10,  # Increased timeout to handle slow network conditions
        )
        response.raise_for_status()  # Raise an HTTPError for bad responses

        logger.info("Data successfully sent to the API")
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


def send_to_kafka(data, kafka_broker, kafka_topic):
    """
    Sends sensor data to a Kafka broker.

    Args:
        data (dict): A dictionary containing sensor data.
        kafka_broker (str): The address of the Kafka broker.
        kafka_topic (str): The topic to send the data to
    """

    try:
        serialized_data = json.dumps(data).encode("utf-8")

        producer = KafkaProducer(bootstrap_servers=kafka_broker)
        producer.send(kafka_topic, value=serialized_data)
        producer.flush()

        logger.info("Data successfully sent to Kafka")

        return True
    except Exception as e:
        logger.error("Error sending data to Kafka: %s", e)
        return False
