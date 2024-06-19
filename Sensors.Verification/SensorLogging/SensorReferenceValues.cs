namespace Sensors.Verification.SensorLogging;

class SensorReferenceValues(decimal temperature, decimal relativeHumidity, decimal carbonMonoxidePpm)
{
    public decimal Temperature { get; } = temperature;
    public decimal RelativeHumidity { get; } = relativeHumidity;
    public decimal CarbonMonoxidePpm { get; } = carbonMonoxidePpm;
}
