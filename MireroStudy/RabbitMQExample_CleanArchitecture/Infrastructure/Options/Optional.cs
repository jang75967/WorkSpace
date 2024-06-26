﻿using Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Options;

public class Optional<T> : IOptional<T> where T : class, new()
{
    private readonly IOptionsMonitor<T> _options;
    private readonly IConfigurationRoot _configuration;
    private readonly string _section;
    private readonly string _file;

    public Optional(
        IOptionsMonitor<T> options,
        IConfigurationRoot configuration,
        string section,
        string file)
    {
        _options = options;
        _configuration = configuration;
        _section = section;
        _file = file;
    }
    public Optional<T> WithParameters(
    IOptionsMonitor<T> options,
    IConfigurationRoot configuration,
    string section,
    string file)
    {
        return new Optional<T>(options, configuration, section, file);
    }
    public T Value => _options.CurrentValue;
    public T Get(string name) => _options.Get(name);

    public void Update(Action<T> applyChanges)
    {
        var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(_file));
        var sectionObject = jObject!.TryGetValue(_section, out JToken section) ?
            JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

        applyChanges(sectionObject!);

        jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
        File.WriteAllText(_file, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        _configuration.Reload();
    }
}