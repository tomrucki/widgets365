namespace Sensors.Verification.SensorPolicies.Statistics;

static class MathHelper
{
    public static Stats CalculateStats(IEnumerable<decimal> values) 
    {
        if (values.Any() == false)
        {
            throw new ArgumentException("argument can't be empty", nameof(values));
        }

        var mean = values.Average();
        var variance = values
            .Select(x => x - mean)
            .Select(x => x * x)
            .Sum() / values.Count();
        var popStdDev = (decimal)Math.Sqrt((double)variance);
        return new Stats(mean, popStdDev);
    }
}
