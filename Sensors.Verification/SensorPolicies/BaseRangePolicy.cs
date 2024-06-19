using Sensors.Verification.SensorLogging;

namespace Sensors.Verification.SensorPolicies;

abstract class BaseRangePolicy : ISensorPolicy
{
    static class Results 
    {
        public const string Keep = "keep";
        public const string Discard = "discard";
    }

    private readonly decimal _range;

    abstract protected decimal SelectReferenceValue(SensorLog sensorLog);

    abstract public string SensorType { get; }

    public string Evaluate(SensorLog sensorLog)
    {
        var valueReference = SelectReferenceValue(sensorLog);
        var valueMin = valueReference - _range;
        var valueMax = valueReference + _range;

        foreach (var item in sensorLog.Values)
        {
            if (item < valueMin || item > valueMax) 
            {
                return Results.Discard;
            }
        }

        return Results.Keep;
    }

    

    public BaseRangePolicy(decimal range)
    {
        if (range <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(range), "argument must be greater than 0");
        }

        _range = range;
    }
}
