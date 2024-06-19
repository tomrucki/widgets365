using Sensors.Verification.SensorLogging;

namespace Sensors.Verification.SensorPolicies;

class HumidityPolicy : BaseRangePolicy
{
    public const decimal AllowedRange = 1m;

    public override string SensorType => "humidity";

    public HumidityPolicy() : base(AllowedRange) { }

    protected override decimal SelectReferenceValue(SensorLog sensorLog) 
        => sensorLog.Reference.RelativeHumidity;
}
