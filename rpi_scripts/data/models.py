"""
Module for the sensor reading data model.
"""

from peewee import Model, FloatField, DateTimeField, AutoField, DatabaseProxy

db = DatabaseProxy()


class SensorReading(Model):
    """
    A sensor reading with pressure, temperature, humidity, and timestamp.
    """

    id = AutoField()
    pressure = FloatField(null=True)
    temperature = FloatField(null=True)
    humidity = FloatField(null=True)
    timestamp = DateTimeField()

    class Meta:
        """
        Metadata for the model.
        """

        database = db
        indexes = ((("timestamp",), False),)
