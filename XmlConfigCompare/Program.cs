namespace XmlConfigCompare
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: XmlConfigCompare path-to-config-1 path-to-config-2 where-to-save-diff");
                return;
            }

            RunConsole(args[0], args[1], args[2]);
        }

        private static void RunConsole(string config1Path, string config2Path, string diffPath)
        {
            if (!File.Exists(config1Path))
            {
                Console.WriteLine("File not exists: " + config1Path);
                return;
            }
            if (!File.Exists(config2Path))
            {
                Console.WriteLine("File not exists: " + config2Path);
                return;
            }
            
            Console.WriteLine("Comparing...");

            var comparator = new Comparator();

            var keysConfig1 = comparator.XMLToDictionary(config1Path);
            var keysConfig2 = comparator.XMLToDictionary(config2Path);

            var result = comparator.Compare(keysConfig1, keysConfig2);

            var report = comparator.GenerateReport(keysConfig1, keysConfig2, result);

            Console.WriteLine("Saving report...");

            if (File.Exists(diffPath))
            {
                Console.WriteLine("File exists, removing: " + diffPath);
                File.Delete(diffPath);
            }

            try
            {
                var file = File.CreateText(diffPath);
                file.WriteLine("Comparison result between {0} (1) and {1} (2)", config1Path, config2Path);
                file.Write(report);
                file.Flush();
                file.Close();

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving the report: " + ex.Message);
            }
        }
    }
}
