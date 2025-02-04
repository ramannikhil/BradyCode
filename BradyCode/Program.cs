using BradyCode;

namespace MyApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Creating an XML file...");
            FileUpdateNotifier fileNotifierObj = new FileUpdateNotifier();
            fileNotifierObj.NotifyFIleChange();

            Console.ReadKey();
        }
    }
}
