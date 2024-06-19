using Sensors.Verification.SensorLogging;
using Sensors.Verification.SensorPolicies;

namespace Sensors.Verification.Tests;

public class PolicyEvaluatorTests
{
    [Fact]
    public void PolicyEvaluator_ShouldReturnResult_WhenValidSensorType()
    {
        // Given
        var evaluator = new PolicyEvaluator(new HumidityPolicy());
        var validLog = new SensorLog(new SensorReferenceValues(0, 0, 0), new SensorInfo("humidity", "humid-1"));
    
        // When
        var result = evaluator.Evaluate(validLog);
    
        // Then
        Assert.NotEmpty(result);
    }

    [Fact]
    public void PolicyEvaluator_ShouldThrow_WhenInvalidSensorType()
    {
        // Given
        var evaluator = new PolicyEvaluator(new HumidityPolicy());
        var invalidLog = new SensorLog(new SensorReferenceValues(0, 0, 0), new SensorInfo("invalid", "humid-1"));
    
        // When
        var act = () => evaluator.Evaluate(invalidLog);
    
        // Then
        Assert.Throws<PolicyEvaluatorException>(act);
    }
}
