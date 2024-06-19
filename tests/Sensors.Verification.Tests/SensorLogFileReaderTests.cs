using Sensors.Verification.SensorLogging;

namespace Sensors.Verification.Tests;

public class SensorLogFileReaderTests
{
    [Fact]
    public void Reader_Throws_WhenMissingHeader()
    {
        // Given
        var logFile = new StringReader("");
        var sut = new SensorLogFileReader(logFile);

        // When
        var act = () => sut.ReadToEnd().FirstOrDefault();

        // Then
        Assert.Throws<LogFileReaderException>(act);
    }

    [Fact]
    public void Reader_Throws_WhenNotEnoughValuesInHeader()
    {
        // Given
        var logFile = new StringReader("ref 1 2");
        var sut = new SensorLogFileReader(logFile);

        // When
        var act = () => sut.ReadToEnd().FirstOrDefault();

        // Then
        Assert.Throws<LogFileReaderTokenCountException>(act);
    }

    [Fact]
    public void Reader_Throws_WhenInvalidValuesInHeader()
    {
        // Given
        var logFile = new StringReader("ref 1 a 2");
        var sut = new SensorLogFileReader(logFile);

        // When
        var act = () => sut.ReadToEnd().FirstOrDefault();

        // Then
        Assert.Throws<LogFileReaderException>(act);
    }

    [Fact]
    public void Reader_ReadsReferenceValues_WhenValidHeader()
    {
        // Given
        var logFile = new StringReader(
            """
            ref 1.2 3.4 2
            humidity hum-1
            2020-01-01T14:15 45.3
            """);
        var sut = new SensorLogFileReader(logFile);
        var expectedTemperature = 1.2m;
        var expectedRelativeHumidity = 3.4m;
        var expectedCarbonMonoxidePpm = 2m;

        // When
        var result = sut.ReadToEnd().FirstOrDefault()!;

        // Then
        Assert.Equal(expectedTemperature, result.Reference.Temperature);
        Assert.Equal(expectedRelativeHumidity, result.Reference.RelativeHumidity);
        Assert.Equal(expectedCarbonMonoxidePpm, result.Reference.CarbonMonoxidePpm);
    }

    [Fact]
    public void Reader_Throws_WhenInvalidSensorInfo()
    {
        // Given
        var logFile = new StringReader(
            """
            ref 1.2 3.4 2
            humidity
            2020-01-01T14:15 45.3
            """);
        var sut = new SensorLogFileReader(logFile);

        // When
        var act = () => sut.ReadToEnd().FirstOrDefault();
    
        // Then
        Assert.Throws<LogFileReaderTokenCountException>(act);
    }

    [Fact]
    public void Reader_ReadsSensorInfo_WhenValid()
    {
        // Given
        var logFile = new StringReader(
            """
            ref 1.2 3.4 2
            humidity hum-1
            2020-01-01T14:15 45.3
            """);
        var sut = new SensorLogFileReader(logFile);
        var expectedSensorType = "humidity";
        var expectedSensorName = "hum-1";

        // When
        var result = sut.ReadToEnd().FirstOrDefault()!;

        // Then
        Assert.Equal(expectedSensorType, result.Info.SensorType);
        Assert.Equal(expectedSensorName, result.Info.Name);
    }

    [Fact]
    public void Reader_Throws_WhenNoSensorReadingValues()
    {
        // Given
        var logFile = new StringReader(
            """
            ref 1.2 3.4 2
            humidity hum-1
            """);
        var sut = new SensorLogFileReader(logFile);

        // When
        var act = () => sut.ReadToEnd().FirstOrDefault();
    
        // Then
        Assert.Throws<LogFileReaderException>(act);
    }

    [Fact]
    public void Reader_ReadsSensorReadingValues_WhenValid()
    {
        // Given
        var logFile = new StringReader(
            """
            ref 1.2 3.4 2
            humidity hum-1
            2020-01-01T14:15 15.3
            2020-01-01T14:16 25.3
            """);
        var sut = new SensorLogFileReader(logFile);
        var expectedValues = new List<decimal> { 15.3m, 25.3m };

        // When
        var result = sut.ReadToEnd().FirstOrDefault()!;

        // Then
        Assert.Equal(expectedValues, result.Values);
    }

    [Fact]
    public void Reader_ReadsMultipleSensorLogs_WhenValid()
    {
        // Given
        var logFile = new StringReader(
            """
            ref 1.2 3.4 2
            humidity hum-1
            2020-01-01T14:15 15.3
            2020-01-01T14:16 25.3
            humidity hum-3
            2020-01-01T14:16 5.3
            2020-01-01T14:17 65.3
            2020-01-01T14:18 95.3
            """);
        var sut = new SensorLogFileReader(logFile);
        var expectedLogCount = 2;
        var expectedValues1 = new List<decimal> { 15.3m, 25.3m };
        var expectedValues2 = new List<decimal> { 5.3m, 65.3m, 95.3m };

        // When
        var result = sut.ReadToEnd().ToList();

        // Then
        Assert.Equal(expectedLogCount, result.Count);
        Assert.Equal(expectedValues1, result[0].Values);
        Assert.Equal(expectedValues2, result[1].Values);
    }
}
