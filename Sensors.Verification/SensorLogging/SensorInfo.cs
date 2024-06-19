namespace Sensors.Verification.SensorLogging;

class SensorInfo(string sensorType, string name)
{
    public string SensorType { get; } = sensorType;
    public string Name { get; set; } = name;
}
