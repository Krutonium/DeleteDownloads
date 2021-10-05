using System;
using System.Collections.Generic;
using System.IO;
using SearchOption = System.IO.SearchOption;

namespace DownloadsNuker
{
    static class Program
    {
        static void Main()
        {
            int DeleteSize = 200;
            var size = GetDownloadsFolderSize();
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DeleteSize.txt")){
                string tmp = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DeleteSize.txt");
                DeleteSize = Int32.Parse(tmp);
                Console.WriteLine("Set limit to {0} GB", DeleteSize);
            }
            while (size > DeleteSize)
            {
                Console.WriteLine("Beginning rolling delete....");
                GetAndDeleteOldestFile();
                size = GetDownloadsFolderSize();
            }
            Console.WriteLine("Done!");
            Environment.Exit(0);
        }

        private static long GetDownloadsFolderSize()
        {
            Console.WriteLine("Evaluating Downloads Folder...");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";
            Console.WriteLine(path);
            string[] fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            long size = 0;
            foreach (var file in fileList)
            {
                if(!file.Contains("Telegram Desktop"))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    size += fileInfo.Length;
                }
            }
            size = size / 1024 / 1024 / 1024;
            return size;
        }
        private static void GetAndDeleteOldestFile()
        {
            Console.WriteLine("Deleting Oldest");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";
            Console.WriteLine(path);
            string[] fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            var OldestFile = new KeyValuePair<string, DateTime>("", DateTime.Now);
            foreach (var file in fileList)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < OldestFile.Value)
                {
                    if (!file.Contains("Telegram Desktop"))
                    {
                        OldestFile = new KeyValuePair<string, DateTime>(file, fileInfo.CreationTime);
                    }
                }
            }
            Console.WriteLine("Deleting {0}", OldestFile.Key);
            File.Delete(OldestFile.Key);
        }
    }
}