using Sensors.Verification.SensorLogging;
using Sensors.Verification.SensorPolicies;

namespace Sensors.Verification.Tests;

public class PolicyTests
{
    [Theory]
    [MemberData(nameof(CarbonMonoxidePolicyData))]
    public void CarbonMonoxidePolicy_Test(string expected, List<decimal> values, decimal ppmReference)
    {
        // Given
        var reference = new SensorReferenceValues(
            carbonMonoxidePpm: ppmReference,
            temperature: decimal.MaxValue, relativeHumidity: decimal.MaxValue);
        var sensorInfo = new SensorInfo("", "");
        var sensorLog = new SensorLog(reference, sensorInfo)
        {
            Values = values,
        };
        var policy = new CarbonMonoxidePolicy();

        // When
        var result = policy.Evaluate(sensorLog);

        // Then
        Assert.Equal(expected, result);
    }

    // reference=0, range=CarbonMonoxidePolicy.AllowedRange
    public static IEnumerable<object[]> CarbonMonoxidePolicyData =>
    [
        ["keep",    new List<decimal> { -2, -1, 0, 1, 3 }, 0],
        ["discard", new List<decimal> { -2, -1, 0, 1, 2, 4 }, 0],
    ];

    [Theory]
    [MemberData(nameof(HumidityPolicyData))]
    public void HumidityPolicy_Test(string expected, List<decimal> values, decimal relativeHumidity)
    {
        // Given
        var reference = new SensorReferenceValues(
            relativeHumidity: relativeHumidity,
            temperature: decimal.MaxValue, carbonMonoxidePpm: decimal.MaxValue);
        var sensorInfo = new SensorInfo("", "");
        var sensorLog = new SensorLog(reference, sensorInfo)
        {
            Values = values,
        };
        var policy = new HumidityPolicy();

        // When
        var result = policy.Evaluate(sensorLog);

        // Then
        Assert.Equal(expected, result);
    }

    // reference=0, range=HumidityPolicy.AllowedRange
    public static IEnumerable<object[]> HumidityPolicyData =>
    [
        ["keep",    new List<decimal> { -1, -0.5m, 0, 0.5m, 1 }, 0],
        ["discard", new List<decimal> { -1, -0.5m, 0, 0.5m, 2 }, 0],
    ];

    [Theory]
    [MemberData(nameof(ThermometerPolicyData))]
    public void ThermometerPolicy_Test(string expected, List<decimal> values, decimal temperatureReference)
    {
        // Given
        var reference = new SensorReferenceValues(
            temperature: temperatureReference,
            carbonMonoxidePpm: decimal.MaxValue, relativeHumidity: decimal.MaxValue);
        var sensorInfo = new SensorInfo("", "");
        var sensorLog = new SensorLog(reference, sensorInfo)
        {
            Values = values,
        };
        var policy = new ThermometerPolicy();

        // When
        var result = policy.Evaluate(sensorLog);

        // Then
        Assert.Equal(expected, result);
    }

    // reference=0
    public static IEnumerable<object[]> ThermometerPolicyData =>
    [
        [
            "ultra precise",
            Enumerable.Repeat(0m, 9).Append(5).ToList(), 
            0
        ],
        [
            "very precise",
            Enumerable.Repeat(0m, 99).Append(50).ToList(), 
            0
        ],
        [
            "precise",
            Enumerable.Repeat(0m, 999).Append(500).ToList(), 
            0
        ],
    ];
}
