using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace MbcsToUnicode
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Usage();
                return 1;
            }

            Console.WriteLine(string.Format("Processing dir: {0} with pattern: {1}", args[0], args[1]));

            try
            {
                FileRepository fileRepo = new FileRepository();
                fileRepo.DoProcessDirectories(args[0], args[1]);

                _DoProcessFiles(fileRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error cough while reading input information from given dir");
                Console.WriteLine(ex.StackTrace);
                return 1;
            }

            /*
            Console.WriteLine(string.Format("Reading input information from dir: {0}", args[0]));
            List<SuggestionMetricsInputDTO> input = null;
            try
            {
                input = new UserMetricsInputCSVReader().ReadDirectory(args[0]);
                if (input.Count == 0)
                {
                    Console.WriteLine("Could not read any input information from given dir");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error cough while reading input information from given dir");
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
            */

            return 0;
        }

        private static void _DoProcessFiles(FileRepository fileRepo)
        {
            RegexRepository regexRepo = new RegexRepository();

            //Do process files
            foreach (string file in fileRepo.FileList)
            {
                Console.WriteLine(string.Format("Processing {0}", file.Substring(file.LastIndexOf(@"\") + string.Format(@"\").Length, file.Length - file.LastIndexOf(@"\") - string.Format(@"\").Length)
                    ));

                string[] linesFile = File.ReadAllLines(file);
                List<string> processed = regexRepo.ProcessFileByLine(linesFile);

                foreach (string processed_line in processed)
                {
                    Console.WriteLine(processed_line);
                }

                if (processed.Count > 0)
                {
                    Console.WriteLine("Press any key for next file...");
                    ConsoleKeyInfo cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.R)
                    {

                    }
                }
            }
        }

        private static void Usage()
        {
            Console.WriteLine("MBCS to Unicode for VC++ applications transformer. Two arguments expected");
            Console.WriteLine("First argument indicating the folder to process");
            Console.WriteLine("Second argument indicating the file patter (*.cpp, *.h, etc)");
        }
    }
}
