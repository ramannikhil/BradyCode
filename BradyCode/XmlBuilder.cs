using BradyCode.Model;
using System.Configuration;
using System.Xml.Serialization;

namespace BradyCode
{
    public class XmlBuilder
    {
        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            if (!String.IsNullOrEmpty(e.FullPath))
            {
                BuildXMLFile(e.FullPath);
            }

            Console.WriteLine($"Added new XML file to the Input File path: {e.FullPath}");
        }

        public void BuildXMLFile(string inputFilePath)
        { 
            XmlFileReader referenceDataXmlReader = new XmlFileReader();

            var output = new GenerationOutput() { 
                ActualHeatRates = referenceDataXmlReader.GetActualHeatRates(inputFilePath),
                MaxEmissionGenerators = referenceDataXmlReader.GetMaxEmissionGenerator(inputFilePath), 
                Totals = referenceDataXmlReader.GetTotalsFromGenerator(inputFilePath)
            };

            // get the outputfilePath from App.config, write the data to this xml file
            string outputfilePath = ConfigurationManager.AppSettings["outputfilePath"];

            SaveToXml(output, outputfilePath);
         }
        public static void SaveToXml(GenerationOutput generationOutput, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GenerationOutput));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, generationOutput);
            }
            Console.WriteLine($"Successfully create the output XML file... Path: {filePath} ");
        }
    }
}
