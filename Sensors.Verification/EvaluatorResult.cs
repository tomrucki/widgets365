namespace Sensors.Verification;

class EvaluatorResult(string sensorName, string result)
{
    public string SensorName { get; } = sensorName;
    public string Result { get; } = result;
}
