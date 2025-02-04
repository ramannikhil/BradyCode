using System.Configuration;

public static class ConfigReader
{
    public static readonly string InputFilePathDir = ConfigurationManager.AppSettings["inputFilePathDir"];
    public static readonly string OutputFilePath = ConfigurationManager.AppSettings["outputFilePath"];
    public static readonly string ReferenceXmlData = ConfigurationManager.AppSettings["referenceXmlData"];
}