using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BitwardenToChromePasswordConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Please enter the path to your passwords .csv file");
                    Console.WriteLine("Example: \"C:\\Users\\user\\Downloads\\passwords.csv\"");
                    Console.ResetColor();
                    string filePath = Console.ReadLine();

                    string fileLocationDir = filePath.Substring(0, filePath.LastIndexOf('\\'));
                    string fileNameWithExtension = filePath.Split('\\').Last();
                    string fileName = fileNameWithExtension.Substring(0, fileNameWithExtension.LastIndexOf('.'));
                    string fileExtension = fileNameWithExtension.Split('.').Last();


                    if (fileExtension != "csv")
                    {
                        ThrowError(new Exception(), "Invalid file extension. Must be .csv");
                        continue;
                    }

                    using (var reader = new StreamReader(filePath))
                    {
                        StringBuilder outputCsv = new StringBuilder();
                        outputCsv.AppendLine(@"name,url,username,password");


                        int lineNumber = 0;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            lineNumber++;

                            //Skip first line
                            if (lineNumber == 1) continue;

                            /*
                             BitWarden csv format
                            0 - folder,
                            1 - favorite,
                            2 - type,
                            3 - name,
                            4 - notes,
                            5 - fields,
                            6 - login_uri,
                            7 - login_username,
                            8 - login_password,
                            9 - login_totp
                            */
                            var values = line.Split(',');

                            var folder = values[0];
                            var favourite = values[1];
                            var type = values[2];
                            var name = values[3];
                            var notes = values[4];
                            var fields = values[5];
                            var uri = values[6];
                            var username = values[7];
                            var password = values[8];
                            var otp = values[9];

                            string chromeCsvLine = $"{name},{uri},{username},{password}";
                            /*
                             Chrome csv format
                             0 - name,
                             1 - url,
                             2 - username,
                             3 - password
                             */
                            outputCsv.AppendLine(chromeCsvLine);
                        }


                        string outputCsvString = outputCsv.ToString();

                        string outputFileName = $"chrome_{fileName}.csv";
                        string outputFilePath = $"{fileLocationDir}\\{outputFileName}";
                        using (StreamWriter writetext = new StreamWriter(outputFilePath))
                        {
                            writetext.Write(outputCsvString);
                        }

                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Operation successful!");
                        Console.ResetColor();

                        Console.WriteLine($"Output file: {outputFileName}");
                        Console.WriteLine($"Output location: {outputFilePath}");
                        Console.WriteLine("Press Enter to quit");
                        Console.ReadLine();
                        return;
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    ThrowError(e,
                        "Make sure that the input file location is in the correct format as shown in the example!");
                    continue;
                }
                catch (Exception e)
                {
                    ThrowError(e, "");
                    continue;
                }
            }
        }

        static void ThrowError(Exception e, string errorMessage)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("ERROR");
            Console.WriteLine(errorMessage);
            Console.ResetColor();
            Console.WriteLine(e);
        }
    }
}