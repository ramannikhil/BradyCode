using System.Xml.Serialization;
using System;

namespace BradyCode.Model
{
    [XmlRoot("GenerationOutput")]
    public class GenerationOutput
    {
        [XmlElement("Totals")]
        public Totals Totals { get; set; }

        [XmlElement("MaxEmissionGenerators")]
        public MaxEmissionGenerators MaxEmissionGenerators { get; set; }

        [XmlElement("ActualHeatRates")]
        public ActualHeatRates ActualHeatRates { get; set; }
    }

    public class Totals
    {
        [XmlElement("Generator")]
        public List<Generator> Generators { get; set; } = new List<Generator>();
    }

    public class Generator
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Total")]
        public double ? Total { get; set; }
    }

    public class MaxEmissionGenerators
    {
        [XmlElement("Day")]
        public List<EmissionDay> Days { get; set; } = new List<EmissionDay>();
    }

    public class EmissionDay
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Date")]
        public DateTimeOffset Date { get; set; }

        [XmlElement("Emission")]
        public double  Emission { get; set; }
    }

    public class ActualHeatRates
    {
        [XmlElement("ActualHeatRate")]
        public List<ActualHeatRate> HeatRates { get; set; } = new List<ActualHeatRate>();
    }

    public class ActualHeatRate
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("HeatRate")]
        public double  HeatRate { get; set; }
    }
}
