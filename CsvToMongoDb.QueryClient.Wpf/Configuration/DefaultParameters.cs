namespace CsvToMongoDb.QueryClient.Wpf.Configuration;

public sealed class DefaultParameters
{
    private readonly Dictionary<string, IDictionary<string, string>> _defaultParameters = new();

    public void Add(string groupName, string parameterKey, string parameterName)
    {
        if (!_defaultParameters.ContainsKey(groupName))
        {
            _defaultParameters.Add(groupName, new Dictionary<string, string>());
        }
        
        _defaultParameters[groupName].Add(parameterKey, parameterName);
    }
    
    
    public IEnumerable<string> GetAllDefaultParameterGroups()
    {
        return _defaultParameters.Keys;
    }
    
    public IEnumerable<string> GetAllDefaultParameters(string group)
    {
        return _defaultParameters[group].Keys;
    }
    
    public string GetDefaultParameterName(string group, string parameterKey)
    {
        return _defaultParameters[group][parameterKey];
    }
}