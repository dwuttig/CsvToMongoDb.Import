using System.Xml.Serialization;

namespace CsvToMongoDb.QueryClient.Wpf.Models;

[XmlRoot("UserSettings")]
public class UserSettings
{
    private DefaultParametersSelectionWrapper _defaultParametersSelectionWrapper = new DefaultParametersSelectionWrapper();

    [XmlElement("DefaultParametersSelection")]
    public DefaultParametersSelectionWrapper DefaultParametersSelectionWrapper
    {
        get => _defaultParametersSelectionWrapper;
        set => _defaultParametersSelectionWrapper = value;
    }
    
    [XmlElement("SelectedTheme")]
    public string SelectedTheme { get; set; }

    public Dictionary<string, bool> GetDefaultParametersSelection()
    {
        return _defaultParametersSelectionWrapper.Parameters.ToDictionary(p => p.Key, p => p.IsSelected);
    }

    public void AddOrUpdateDefaultParametersSelection(string key, bool isSelected)
    {
        var parameter = _defaultParametersSelectionWrapper.Parameters.FirstOrDefault(p => p.Key == key);

        if (parameter == null)
        {
            _defaultParametersSelectionWrapper.Parameters.Add(
                new Parameter
                {
                    Key = key,
                    IsSelected = isSelected
                });
        }
        else
        {
            parameter.IsSelected = isSelected;
        }
    }
}

[XmlRoot("DefaultParametersSelectionWrapper")]
public class DefaultParametersSelectionWrapper
{
    public List<Parameter> Parameters { get; set; }
    
    public DefaultParametersSelectionWrapper()
    {
        Parameters = new List<Parameter>();
    }
}

[XmlRoot("Parameter")]
public class Parameter
{
    [XmlAttribute("Key")]
    public string Key { get; set; }

    [XmlAttribute("IsSelected")]
    public bool IsSelected { get; set; }
}