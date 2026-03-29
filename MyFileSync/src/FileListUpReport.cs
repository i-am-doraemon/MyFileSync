using System;
using System.Collections.Generic;
using System.Text;

namespace MyFileSync
{
    public struct FileListUpReport
    {
        public FileListUpReport(int total, int count)
        {
            Total = total;
            Count = count;
        }

        public int Total
        {
            get;
        }

        public int Count
        {
            get;
        }

        public int Percent
        {
            get
            {
                return (int) (100.0 * (double)Count / (double)Total);
            }
        }

        public bool IsCompleted()
        {
            if (Count < Total)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public bool IsCancelled()
        {
            if (Count < 0 && Total < 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
