using BradyCode.Model;
using System.Configuration;
using System.Xml.Linq;

namespace BradyCode
{
    public class XmlFileReader
    {
        public ActualHeatRates GetActualHeatRates(string inputFilePath)
        {
            var xml = XDocument.Load(inputFilePath);

            var netHeatRate=
            xml.Descendants("ActualNetGeneration")
             .Select(x => new ActualHeatRate
             {
                 Name = GetSpecificElementFromParent(x, "Name"),
                 HeatRate = CalculateHeatRate(x)
               
             }).ToList();

            return new ActualHeatRates{ HeatRates = netHeatRate } ;
        }

        public static double CalculateHeatRate(XElement x)
        {
            double actualNetGeneration = ParseDouble(x.Value.ToString());
            var totalHeatInput = ParseDouble(GetSpecificElementFromParent(x, "TotalHeatInput"));
            return totalHeatInput / actualNetGeneration;
        }

        public Totals GetTotalsFromGenerator(string inputFilePath)
        {
            var xml = XDocument.Load(inputFilePath);

            var generators =
               xml.Descendants("Generation")
                .Select(x => new Generator
                {
                    Name = GetSpecificElementFromParent(x, "Name"),
                    Total = x?.Descendants("Day")
                        .Sum(
                            y => ParseDouble(GetSpecificElement(y, "Energy"))
                                    * ParseDouble(GetSpecificElement(y, "Price"))
                                    * ReferenceDataXmlReader(GetSpecificElementFromParent(x,"Name"), "ValueFactor")
                        )
                }).ToList();

            return new Totals {  Generators = generators };
        }


        public static string GetSpecificElementFromParent(XElement xElement, string elementName)
        {
            return xElement?.Parent?.Element(elementName)?.Value.ToString();
        }

        public  static string GetSpecificElement(XElement xElement, string elementName)
        {
            return xElement?.Element(elementName)?.Value.ToString();
        }

        public MaxEmissionGenerators GetMaxEmissionGenerator(string inputFilePath)
        {
            var xml = XDocument.Load(inputFilePath);
            
            var dayelement =
                 xml.Descendants("Day")
                 .GroupBy(day => day.Element("Date")?.Value)
                 .Select(x =>
                       x.Select(y => new
                       {
                           Date = DateTimeOffset.Parse(GetSpecificElement(y, "Date")),
                           Name = y.Parent?.Parent?.Element("Name")?.Value.ToString(),
                           Emission =
                                       GetEmission(
                                            GetSpecificElement(y, "Energy"),
                                             y.Parent?.Parent?.Element("EmissionsRating")?.Value.ToString(),
                                             y.Parent?.Parent?.Element("Name")?.Value.ToString()
                                       )
                       }
                        )
                 )
                .ToList();

            var finaldata = dayelement.Select(x => new EmissionDay
                {
                    Date = x.First().Date,
                    Emission = x.Max(y => y.Emission),
                    Name = x.MaxBy(y => y.Emission).Name
                }
             );

           return new MaxEmissionGenerators { Days = finaldata.ToList() };
        }

        public double GetEmission(string energy, string emissionRating, string ef)
        {
            return ParseDouble(energy) * ParseDouble(emissionRating) * ReferenceDataXmlReader(ef, "EmissionsFactor");
        }

        private static double ParseDouble(string value)
        {
            return double.TryParse(value, out double result) ? result : 0f;
        }

        // todo move to different class and split the functionality
        public double ReferenceDataXmlReader(string generatorName, string factor)
        {
            string referenceXmlDataPath = ConfigurationManager.AppSettings["referenceXmlData"];
            XDocument xml = XDocument.Load(referenceXmlDataPath);
            var factorElement = xml?.Root?.Element("Factors");


            // declare the ef vf dictinoary here
            //ConstantDict f1 = new ConstantDict();

            //var referenceDict = ConstantDict.GeneratorTypeMap[generatorName];
            Dictionary<string, string> referenceDict;
            ConstantDict.GeneratorTypeMap.TryGetValue(generatorName, out referenceDict);

            if (factor == "EmissionsFactor")
            {
                var EmissionFactorElement = factorElement.Element("EmissionsFactor");
                double emmissionFactor = referenceDict["EmissionFactor"] != "NA"
                    ? ParseDouble(EmissionFactorElement.Element(referenceDict["EmissionFactor"]).Value.ToString()) : 0;

                return emmissionFactor;
            }

            var ValueFactorElement = factorElement.Element("ValueFactor");
            double valueFactor = referenceDict["ValueFactor"] != "NA"
                ? ParseDouble(ValueFactorElement.Element(referenceDict["ValueFactor"]).Value.ToString()) : 0;

            return valueFactor;
        }
    }
}
