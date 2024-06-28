import requests
import logging

logger = logging.getLogger(__name__)


def post_sensor_reading(data, API_URL, API_PORT, API_KEY):
    headers = {"Authorization": f"Bearer {API_KEY}", "Content-Type": "application/json"}

    try:
        response = requests.post(
            f"{API_URL}:{API_PORT}/api/sensorData",
            json=data,
            headers=headers,
            timeout=10,  # Increased timeout to handle slow network conditions
        )
        response.raise_for_status()  # Raise an HTTPError for bad responses

        logger.info("Data successfully sent to the API")
        return response.json()

    except requests.exceptions.HTTPError as http_err:
        logger.error(f"HTTP error: {http_err}")
    except requests.exceptions.ConnectionError as conn_err:
        logger.error(f"Connection error: {conn_err}")
    except requests.exceptions.Timeout as timeout_err:
        logger.error(f"Timeout error: {timeout_err}")
    except requests.exceptions.RequestException as req_err:
        logger.error(f"An error occurred: {req_err}")
    except Exception as e:
        logger.error(f"Unexpected error: {e}")

    return None
