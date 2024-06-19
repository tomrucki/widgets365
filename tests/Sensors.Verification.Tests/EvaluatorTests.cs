namespace Sensors.Verification.Tests;

public class EvaluatorTests
{
    [Fact]
    public void Evaluate_Throws_WhenLogEmpty()
    {
        // Given
        var logFileContent = "";

        // When
        var act = () => Evaluator.EvaluateLogFile(logFileContent);
    
        // Then
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Evaluate_ReturnsOutput_WhenValidLog()
    {
        // Given
        var logFileContent = 
            """
            reference 10 50 20
            humidity hum-1
            2020-01-01T14:15 49.3
            2020-01-01T14:16 49.6
            2020-01-01T14:17 50.3
            2020-01-01T14:18 50.1
            2020-01-01T14:19 50.4
            humidity hum-3
            2020-01-01T14:15 49.3
            2020-01-01T14:16 49.6
            2020-01-01T14:17 50.3
            2020-01-01T14:18 50.1
            2020-01-01T14:19 52.4
            """;
        var expectedOutput =
            """
            {
              "hum-1": "keep",
              "hum-3": "discard"
            }
            """;

        // When
        var result = Evaluator.EvaluateLogFile(logFileContent);

        // Then
        Assert.Equal(expectedOutput, result);
    }
}
