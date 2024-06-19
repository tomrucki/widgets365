using Sensors.Verification.SensorLogging;

namespace Sensors.Verification.SensorPolicies;

interface ISensorPolicy
{
    public string SensorType { get; }
    public string Evaluate(SensorLog sensorLog);
}
