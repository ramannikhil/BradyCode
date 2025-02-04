using BradyCode.Model;
using System.Collections.Concurrent;
using System.Configuration;
using System.Xml.Serialization;

namespace BradyCode
{
    public class XmlBuilder
    {
        private readonly Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();
        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            // To avoid handling multiple events,
            // the below appraoach ensure only one event is handled with the XML file insertion into the Input folder
            if (_timers.ContainsKey(e.FullPath))
            {
                _timers[e.FullPath].Change(500, Timeout.Infinite); // Reset timer
            }
            else
            {
                Timer timer = new Timer((state) =>
                {
                    Console.WriteLine($"Added new XML file to the Input File path: {e.FullPath}");
                    BuildXMLFile(e.FullPath);
                    _timers.Remove(e.FullPath);
                }, null, 500, Timeout.Infinite);

                _timers[e.FullPath] = timer;
            }
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
            string outputfilePath = ConfigReader.OutputFilePath;

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
