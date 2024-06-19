namespace Sensors.Verification.SensorLogging;

class SensorLog(SensorReferenceValues reference, SensorInfo info)
{
    public SensorInfo Info { get; } = info;
    public SensorReferenceValues Reference { get; } = reference;
    public List<decimal> Values { get; init; } = [];
}
