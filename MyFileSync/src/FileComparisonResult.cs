using System;
using System.Collections.Generic;
using System.Text;

namespace MyFileSync
{
    class FileComparisonResult
    {

        private List<IdenticalFile> _identicals;

        private List<FileDescription> _aOnly;
        private List<FileDescription> _bOnly;

        public FileComparisonResult(List<IdenticalFile> identicals, List<FileDescription> aOnly, List<FileDescription> bOnly)
        {
            _identicals = identicals;

            _aOnly = aOnly;
            _bOnly = bOnly;
        }

        public List<FileDescription> OnlyA
        {
            get
            {
                return _aOnly;
            }
        }

        public List<FileDescription> OnlyB
        {
            get
            {
                return _bOnly;
            }
        }

        public List<IdenticalFile> Identicals
        {
            get
            {
                return _identicals;
            }
        }

        public List<FileComparisonResultRecord> GetRecords()
        {
            List<FileComparisonResultRecord> list = new List<FileComparisonResultRecord>();

            foreach (IdenticalFile identical in _identicals)
            {
                FileComparisonResultRecord record = new FileComparisonResultRecord()
                {
                    Result        = "一致",
                    FullPath1     = identical.A.FullPath, 
                    FullPath2     = identical.B.FullPath, 
                    FileSize      = identical.A.FileSize, 
                    MessageDigest = identical.A.MessageDigest,
                };

                list.Add(record);
            }

            foreach (FileDescription f in _aOnly)
            {
                FileComparisonResultRecord record = new FileComparisonResultRecord()
                {
                    Result        = "左のみ",
                    FullPath1     = f.FullPath,
                    FileSize      = f.FileSize,
                    MessageDigest = f.MessageDigest, 
                };

                list.Add(record);
            }

            foreach (FileDescription f in _bOnly)
            {
                FileComparisonResultRecord record = new FileComparisonResultRecord()
                {
                    Result        = "右のみ",
                    FullPath2     = f.FullPath,
                    FileSize      = f.FileSize,
                    MessageDigest = f.MessageDigest, 
                };

                list.Add(record);
            }

            return list;
        }

    }
}
