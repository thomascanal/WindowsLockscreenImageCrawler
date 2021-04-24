using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsLockscreenImageCrawler
{
    public static class WindowsLockscreenImageCrawler
    {
        private static string _targetFolderPath;
        private static string _sourceFolderPath;

        private static void Main()
        {
            CreateFolder();
            var pathToPictures = GetFilenames();
            CopyFilesToTargetFolder(pathToPictures);
            Console.WriteLine("Bilder wurden kopiert");
            Console.ReadKey();
        }

        private static IEnumerable<string> GetFilenames()
        {
            var localappdataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            const string pictureFolderPath = @"Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";
            _sourceFolderPath = Path.Combine(localappdataFolderPath, pictureFolderPath);

            var pathToPictures = Directory.GetFiles(_sourceFolderPath);
            return pathToPictures;
        }

        private static void CopyFilesToTargetFolder(IEnumerable<string> pathToPictures)
        {
            foreach (var picture in pathToPictures)
            {
                var fi = new FileInfo(picture);
                var size = fi.Length;

                if (size <= 200000) continue;
                var fileName = $"{Path.GetFileName(picture)}.jpg";
                var destinationFile = Path.Combine(_targetFolderPath, fileName);
                File.Copy(picture, destinationFile, true);
            }
        }

        private static void CreateFolder()
        {
            var currentDate = DateTime.Today.ToShortDateString();
            var folderName = $"Sperrbildschirmbilder vom {currentDate}";
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            _targetFolderPath = Path.Combine(desktopPath, folderName);

            if (!Directory.Exists(_targetFolderPath))
            {
                Directory.CreateDirectory(_targetFolderPath);
            }
        }
    }
}
