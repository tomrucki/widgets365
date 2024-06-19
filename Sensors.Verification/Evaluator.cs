using System.Text.Json;
using Sensors.Verification.SensorLogging;
using Sensors.Verification.SensorPolicies;

namespace Sensors.Verification;

public static class Evaluator
{
    static JsonSerializerOptions _serializerOptions;

    static Evaluator()
    {
        _serializerOptions = new JsonSerializerOptions { WriteIndented = true };
    }

    public static string EvaluateLogFile(string logContentsStr)
    {
        if (string.IsNullOrWhiteSpace(logContentsStr))
        {
            throw new ArgumentException("argument can't be empty", nameof(logContentsStr));
        }

        // should be swapped for StreamReader if public interface changes from log content to just file name
        var logFile = new StringReader(logContentsStr);
        var logReader = new SensorLogFileReader(logFile);
        
        var policyEvaluator = new PolicyEvaluator(
            new CarbonMonoxidePolicy(),
            new HumidityPolicy(),
            new ThermometerPolicy());
        
        var results = GetResults(logReader, policyEvaluator);
        return SerializeResults(results);
    }

    static string SerializeResults(List<EvaluatorResult> results)
    {
        var dict = results.ToDictionary(x => x.SensorName, x => x.Result);
        return JsonSerializer.Serialize(dict, _serializerOptions);
    }

    static List<EvaluatorResult> GetResults(SensorLogFileReader logReader, PolicyEvaluator policyEvaluator)
    {
        var results = new List<EvaluatorResult>();
        foreach (var item in logReader.ReadToEnd())
        {
            var result = new EvaluatorResult(item.Info.Name, policyEvaluator.Evaluate(item));
            results.Add(result);
        }

        return results;
    }
}
