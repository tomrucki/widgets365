using Sensors.Verification.SensorLogging;
using Sensors.Verification.SensorPolicies.Statistics;

namespace Sensors.Verification.SensorPolicies;

class ThermometerPolicy : ISensorPolicy
{
    static class Results
    {
        public const string UltraPrecise = "ultra precise";
        public const string VeryPrecise = "very precise";
        public const string Precise = "precise";
    }

    public string SensorType => "thermometer";

    public string Evaluate(SensorLog sensorLog)
    {
        var stats = MathHelper.CalculateStats(sensorLog.Values);

        if (IsUltraPrecise(stats, sensorLog.Reference.Temperature))
        {
            return Results.UltraPrecise;
        }

        if (IsVeryPrecise(stats, sensorLog.Reference.Temperature))
        {
            return Results.VeryPrecise;
        }

        return Results.Precise;
    }

    static bool IsUltraPrecise(Stats s, decimal temperatureReference) 
        => temperatureReference - 0.5m <= s.Mean && s.Mean <= temperatureReference + 0.5m
        && s.PopulationStandardDeviation < 3;

    static bool IsVeryPrecise(Stats s, decimal temperatureReference) 
        => temperatureReference - 0.5m <= s.Mean && s.Mean <= temperatureReference + 0.5m
        && s.PopulationStandardDeviation < 5;
}
