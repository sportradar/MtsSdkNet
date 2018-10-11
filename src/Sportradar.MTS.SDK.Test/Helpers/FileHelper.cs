/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Sportradar.MTS.SDK.Test.Helpers
{
    public static class FileHelper
    {
        public static Stream OpenFile(string dirPath, string fileName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(dirPath));
            Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
            Contract.Ensures(Contract.Result<Stream>() != null);

            var filePath = dirPath.TrimEnd('/') + "/" + fileName.TrimStart('/');
            return OpenFile(filePath);
        }

        public static Stream OpenFile(string filePath)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(filePath));
            filePath = FindFileInDir(filePath);
            return File.OpenRead(filePath);
        }

        public static Task<Stream> OpenFileAsync(string filePath)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(filePath));
            filePath = FindFileInDir(filePath);
            return Task.Factory.StartNew(() => OpenFile(filePath));
        }

        public static string ReadFile(string dirPath, string fileName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(dirPath));
            Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
            Contract.Ensures(Contract.Result<string>() != null);

            var stream = OpenFile(dirPath, fileName);
            var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            return result;
        }

        public static string FindFileInDir(string fileName, string startDir = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(startDir))
            {
                startDir = Directory.GetCurrentDirectory();
            }
            if (File.Exists(fileName))
            {
                return fileName;
            }
            foreach (var dir in Directory.GetDirectories(startDir))
            {
                foreach (var fileInfo in new DirectoryInfo(dir).GetFiles())
                {
                    if (fileInfo.Name == fileName)
                    {
                        return fileInfo.FullName;
                    }
                }
            }

            return fileName;
        }
    }
}