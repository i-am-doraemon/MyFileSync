using System;
using System.Collections.Generic;
using System.Text;

namespace MyFileSync
{
    class FileComparisonResultRecord
    {
        public string Result
        {
            get;
            set;
        } = string.Empty;

        public string FullPath1
        {
            get;
            set;
        } = string.Empty;

        public string FullPath2
        {
            get;
            set;
        } = string.Empty;

        public long FileSize
        {
            get;
            set;
        }

        public string MessageDigest
        {
            get;
            set;
        } = string.Empty;
    }
}
