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
                    Console.WriteLine(
                        "Rename your Bitwarden passwords .csv file to \"passwords.csv\", place it in your C: drive and press ENTER");
                    Console.WriteLine("Alternatively your can manually enter the path to your .csv file below");
                    Console.WriteLine("Example: \"C:\\Users\\user\\Desktop\\bitwarden_export.csv\"");
                    string filePath = Console.ReadLine();

                    if (filePath == "")
                    {
                        filePath = "C:\\passwords.csv";
                    }

                    string fileLocationDir = filePath.Substring(0, filePath.LastIndexOf('\\'));
                    string fileNameWithExtension = filePath.Split('\\').Last();
                    string fileName = fileNameWithExtension.Substring(0, fileNameWithExtension.LastIndexOf('.'));
                    string fileExtension = fileNameWithExtension.Split('.').Last();


                    if (fileExtension != "csv")
                    {
                        throw new Exception("ERROR: Invalid file extension. Must be .csv");
                    }

                    Console.WriteLine("Processing passwords...");
                    using (var reader = new StreamReader(filePath))
                    {
                        StringBuilder outputCsv = new StringBuilder();
                        outputCsv.AppendLine(@"name,url,username,password");

                        List<string?> headers = new List<string?>();
                        int indexOfName = 0;
                        int indexOfUrl = 0;
                        int indexOfUsername = 0;
                        int indexOfPassword = 0;

                        int lineNumber = 0;
                        while (!reader.EndOfStream)
                        {
                            // Read properties from line 0
                            string? line = reader.ReadLine();
                            string?[] values = line.Split(',');

                            if (lineNumber == 0)
                            {
                                headers = values.ToList();
                                indexOfName = headers.IndexOf("name");
                                indexOfUrl = headers.IndexOf("login_uri");
                                indexOfUsername = headers.IndexOf("login_username");
                                indexOfPassword = headers.IndexOf("login_password");

                                lineNumber++;
                                continue;
                            }

                            // Skip android logins
                            if (line.Contains("android://"))
                            {
                                continue;
                            }

                            if ((indexOfName + indexOfUrl + indexOfUsername + indexOfPassword) == 0)
                            {
                                throw new Exception("Invalid password file format. Please contact the developer!");
                            }

                            var name = values[indexOfName];
                            var uri = values[indexOfUrl];
                            var username = values[indexOfUsername];
                            var password = values[indexOfPassword];

                            string chromeCsvLine = $"{name},{uri},{username},{password}";

                            outputCsv.AppendLine(chromeCsvLine);
                            lineNumber++;
                            Console.WriteLine(name);
                        }

                        Console.WriteLine("Processing finished!");
                        Console.WriteLine("");

                        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                        string outputCsvString = outputCsv.ToString();

                        string outputFileName = $"BitwardenToChromePasswords_{timeStamp}.csv";
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
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine("ERROR: Cannot write output file. Please run the program as an administrator!");
                    Console.WriteLine("Press any key to exit!");

                    if (Console.ReadKey().Key != null)
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    if (!string.IsNullOrEmpty(e.Message))
                    {
                        Console.WriteLine(e.Message);
                    }
                    else
                    {
                        Console.WriteLine("ERROR: File not found, please enter a valid path!");
                        Console.WriteLine("Press any key to try again!");
                    }

                    if (Console.ReadKey().Key != null)
                    {
                        Console.Clear();
                    }
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