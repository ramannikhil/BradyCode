using System;

namespace BradyCode
{
    public class ConstantDict
    {
        public static readonly Dictionary<string, Dictionary<string, string>> GeneratorTypeMap = new()
        {
            { "Wind[Offshore]", new() { { "ValueFactor", "Low" }, { "EmissionFactor", "NA" } } },
            { "Wind[Onshore]", new() { { "ValueFactor", "High" }, { "EmissionFactor", "NA" } } },
            { "Gas[1]", new() { { "ValueFactor", "Medium" }, { "EmissionFactor", "Medium" } } },
            { "Coal[1]", new() { { "ValueFactor", "Medium" }, { "EmissionFactor", "High" } } }
        };
    }
}
