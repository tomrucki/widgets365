namespace Sensors.Verification.SensorLogging;

class SensorLogFileReader
{
    private readonly TextReader _logFile;
    private readonly Queue<string?> _peekLines;

    public SensorLogFileReader(TextReader logFile)
    {
        _logFile = logFile ?? throw new ArgumentNullException(nameof(logFile));
        _peekLines = new();
    }

    private string? PeakLine()
    {
        var nextLine = _logFile.ReadLine();
        _peekLines.Enqueue(nextLine);
        return nextLine;
    }

    private string? ReadLine()
    {
        if (_peekLines.Count > 0)
        {
            return _peekLines.Dequeue();
        }

        return _logFile.ReadLine();
    }

    public IEnumerable<SensorLog> ReadToEnd()
    {
        var headerReference = ReadHeader();

        while (true)
        {
            var result = ReadNextSensorLog(headerReference);
            if (result is null)
            {
                break;
            }

            yield return result;
        }
    }

    SensorLog? ReadNextSensorLog(SensorReferenceValues headerReference)
    {
        // no more logs
        var startLine = ReadLine();
        if (string.IsNullOrWhiteSpace(startLine))
        {
            return null;
        }

        // sensor info
        var sensorInfo = ParseSensorInfo(startLine);

        // values
        var readingValues = ReadSensorValues();
        if (readingValues.Count == 0)
        {
            throw new LogFileReaderException($"sensor values - no values found");
        }

        // log entry is valid
        var sensorLog = new SensorLog(headerReference, sensorInfo) { Values = readingValues };
        return sensorLog;
    }

    List<decimal> ReadSensorValues()
    {
        var values = new List<decimal>();
        var logLine = PeakLine();
        while (TryParseSensorValue(logLine, out decimal value))
        {
            values.Add(value);

            // advance the reader after peeking
            _ = ReadLine();

            // peek next
            logLine = PeakLine();
        }

        return values;
    }

    static bool TryParseSensorValue(string? logLine, out decimal value)
    {
        value = default;

        if (logLine is null)
        {
            return false;
        }

        var values = logLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var expectedTokenCount = 2;
        if (values.Length != expectedTokenCount)
        {
            return false;
        }

        // since the datetime is not being utilized, we skip parsing the datetime and care only about the sensor value
        // (also speeds up the log file processing)
        var sensorValueStr = values[1];
        if (decimal.TryParse(sensorValueStr, out var sensorValue))
        {
            value = sensorValue;
            return true;
        }

        return false;
    }

    static SensorInfo ParseSensorInfo(string sensorInfoLine)
    {
        var values = sensorInfoLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var expectedTokenCount = 2;
        if (values.Length != expectedTokenCount)
        {
            throw new LogFileReaderTokenCountException("sensor info", expectedTokenCount, values.Length);
        }

        return new SensorInfo(sensorType: values[0], name: values[1]);
    }

    SensorReferenceValues ReadHeader()
    {
        var headerLine = _logFile.ReadLine();
        if (string.IsNullOrWhiteSpace(headerLine))
        {
            throw new LogFileReaderException("reference can not be empty");
        }

        var values = headerLine.Split(' ');
        var expectedTokenCount = 4;
        if (values.Length != expectedTokenCount)
        {
            throw new LogFileReaderTokenCountException("reference", expectedTokenCount, values.Length);
        }

        var numericValues = values
            .Skip(1)
            .Select(x =>
            {
                var isValid = decimal.TryParse(x, out var num);
                return (isValid, num);
            })
            .ToArray();
        if (numericValues.Any(x => x.isValid == false))
        {
            throw new LogFileReaderException("reference - not a valid number");
        }

        return new SensorReferenceValues(
            temperature: numericValues[0].num,
            relativeHumidity: numericValues[1].num,
            carbonMonoxidePpm: numericValues[2].num
        );
    }
}

class LogFileReaderException(string message) 
    : Exception($"Invalid log file format: {message}") { }

class LogFileReaderTokenCountException(string message, int expected, int actual) 
    : LogFileReaderException($"{message} - expected {expected} columns but found {actual}") { }
