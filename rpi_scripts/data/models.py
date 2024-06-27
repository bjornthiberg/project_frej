from peewee import Model, FloatField, DateTimeField, AutoField

class SensorReading(Model):
    id = AutoField()
    pressure = FloatField(null=True)
    temperature = FloatField(null=True)
    humidity = FloatField(null=True)
    lux = FloatField(null=True)
    uvs = FloatField(null=True)
    gas = FloatField(null=True)
    timestamp = DateTimeField()

    class Meta:
        indexes = (
            (('timestamp',), False),
        )
