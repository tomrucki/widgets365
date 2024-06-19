using Sensors.Verification.SensorLogging;

namespace Sensors.Verification.SensorPolicies;

class CarbonMonoxidePolicy : BaseRangePolicy
{
    public const decimal AllowedRange = 3m;

    public override string SensorType => "monoxide";

    public CarbonMonoxidePolicy() : base(AllowedRange) { }

    protected override decimal SelectReferenceValue(SensorLog sensorLog) 
        => sensorLog.Reference.CarbonMonoxidePpm;
}
