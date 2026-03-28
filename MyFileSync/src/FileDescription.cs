using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyFileSync
{
    class FileDescription : IEquatable<FileDescription>
    {
        private const int MaxReadSize = 1024 * 1024;

        private string _messageDigest;

        private string _fullPath;

        private string _filePath;
        private string _fileName;
        
        private long   _fileSize;

        public FileDescription(FileInfo fileInfo)
        {
            _fullPath = fileInfo.FullName;

            _filePath = Path.GetDirectoryName(_fullPath)!;
            _fileName = Path.GetFileName(_fullPath);

            _fileSize = fileInfo.Length;

            _messageDigest = GetMessageDigest(_fullPath);
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public string FullPath
        {
            get
            {
                return _fullPath;
            }
        }

        public string MessageDigest
        {
            get
            {
                return _messageDigest;
            }
        }

        public long FileSize
        {
            get
            {
                return _fileSize;
            }
        }

        private string GetMessageDigest(string fileName)
        {
            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[MaxReadSize];
                    int totalBytesRead = fileStream.Read(buffer, 0, buffer.Length);

                    ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(
                                                        buffer, 0, totalBytesRead);
                    byte[] rawDigest = SHA256.HashData(span);

                    string digest = Convert.ToHexString(rawDigest);
                    return digest.ToLower();
                }
            } catch
            {
                return string.Empty;
            }
        }

        public bool Equals(FileDescription? other)
        {
            if (other != null)
            {
                return this.MessageDigest == other.MessageDigest && 
                    this.FileSize == other.FileSize;
            } else
            {
                return false;
            }
        }
    }
}
