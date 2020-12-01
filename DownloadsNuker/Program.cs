using System;
using System.IO;
using SearchOption = System.IO.SearchOption;

namespace DownloadsNuker
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Evaluating Downloads Folder...");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";
            Console.WriteLine(path);
            string[] fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            long size=0;
            foreach (var file in fileList)
            {
                FileInfo fileInfo = new  FileInfo(file);
                size += fileInfo.Length;
            }
            size = size / 1024 / 1024 / 1024;
            Console.WriteLine("Downloads Folder is " + size + "GB");
            if (size > 200)
            {
                Console.WriteLine("Would you like to delete your Downloads Directory?");
                Console.WriteLine("[y/N]");
                var userKey = Console.ReadKey();
                if (userKey.Key == ConsoleKey.Y)
                {
                    foreach (var file in fileList)
                    {
                        Console.WriteLine("Deleting " + file);
                        File.Delete(file);
                    }
                    Console.WriteLine("Process Complete!");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Will prompt at next launch.");
                    Console.Read();
                }
            }
            else
            {
                Console.WriteLine("Your downloads folder is less than 200GB! Congrats!");
                System.Threading.Thread.Sleep(3000);
            }
            Environment.Exit(0);
        }
    }
}