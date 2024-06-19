namespace Sensors.Verification.SensorPolicies.Statistics;

class Stats(decimal mean, decimal populationStandardDeviation)
{
    public decimal Mean { get; } = mean;
    public decimal PopulationStandardDeviation { get; } = populationStandardDeviation;
}
