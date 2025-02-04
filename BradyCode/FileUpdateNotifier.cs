using System.Configuration;

namespace BradyCode
{
    public class FileUpdateNotifier
    {
        public void NotifyFIleChange()
        {
            string inputFilePathDir = ConfigurationManager.AppSettings["inputFilePathDir"];
            
            var isValidPathDir = Path.IsPathFullyQualified(inputFilePathDir);

            try
            {
                if (!Directory.Exists(inputFilePathDir))
                {
                    throw new DirectoryNotFoundException($"Directory not found: {inputFilePathDir}");
                }
                FileSystemWatcher watcher = new FileSystemWatcher(inputFilePathDir);
                watcher.NotifyFilter = NotifyFilters.LastWrite;

                // Notify the changes to the XMlBuilder, once the file is added to the Input folder
                // will invoke other functions and generates the data
                XmlBuilder xmlBuilder = new XmlBuilder();
                watcher.Changed += xmlBuilder.OnChanged;
                watcher.Filter = "*.xml";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Error: Directory not found - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
