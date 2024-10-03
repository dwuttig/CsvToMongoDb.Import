using Microsoft.Extensions.Configuration;

namespace CsvToMongoDb.QueryClient.Wpf.Configuration;

public sealed class DefaultParameterReader : IDefaultParameterReader
{
    private readonly DefaultParameters _defaultParameters;

    public DefaultParameterReader(IConfiguration configuration)
    {
        _defaultParameters = ReadDefaultParameters(configuration);
    }

    private static DefaultParameters ReadDefaultParameters(IConfiguration configuration)
    {
        var defaultParameters = new DefaultParameters();

        var defaultParameterSection = configuration.GetSection("DefaultParameters");

        foreach (var parameterGroup in defaultParameterSection.GetChildren())
        {
            foreach (var parameter in parameterGroup.GetChildren())
            {
                defaultParameters.Add(parameterGroup.Key, parameter.Key, parameter.Value);
            }
        }

        return defaultParameters;
    }
    
    public DefaultParameters GetDefaultParameters()
    {
        return _defaultParameters;
    }
}