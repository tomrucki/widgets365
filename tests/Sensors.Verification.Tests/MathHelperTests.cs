using Sensors.Verification.SensorPolicies.Statistics;

namespace Sensors.Verification.Tests;

public class MathHelperTests
{
    [Fact]
    public void CalculateStats_ShouldThrow_WhenEmptyArgument()
    {
        // Given
        decimal[] values = [];

        // When
        var act = () => MathHelper.CalculateStats(values);

        // Then
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void CalculateStats_ShouldCalculate_Mean()
    {
        // Given
        var values = new[] { 1m, 2, 3, 6 };
        var expectedMean = 3;

        // When
        var stats = MathHelper.CalculateStats(values);

        // Then
        Assert.Equal(expectedMean, stats.Mean);
    }

    [Fact]
    public void CalculateStats_ShouldCalculate_StdDev()
    {
        // https://en.wikipedia.org/wiki/Standard_deviation#Basic_examples

        // Given
        var values = new[] { 2m, 4, 4, 4, 5, 5, 7, 9 };
        var expectedStdDev = 2;

        // When
        var stats = MathHelper.CalculateStats(values);

        // Then
        Assert.Equal(expectedStdDev, stats.PopulationStandardDeviation);
    }
}
