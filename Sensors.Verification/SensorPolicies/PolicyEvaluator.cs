using Sensors.Verification.SensorLogging;

namespace Sensors.Verification.SensorPolicies;

class PolicyEvaluator(params ISensorPolicy[] sensorPolicies)
{
    private readonly ISensorPolicy[] _sensorPolicies = sensorPolicies;

    public string Evaluate(SensorLog sensorLog)
    {
        foreach (var policy in _sensorPolicies)
        {
            if (policy.SensorType == sensorLog.Info.SensorType)
            {
                return policy.Evaluate(sensorLog);
            }
        }

        throw new PolicyEvaluatorException($"Unsupported sensor type {sensorLog.Info.SensorType}");
    }
}

class PolicyEvaluatorException(string message) : Exception(message) { }
