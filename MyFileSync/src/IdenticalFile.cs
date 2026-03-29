using System;
using System.Collections.Generic;
using System.Text;

namespace MyFileSync
{
    struct IdenticalFile
    {
        private FileDescription _a;
        private FileDescription _b;

        public IdenticalFile(FileDescription a, FileDescription b)
        {
            _a = a;
            _b = b;
        }

        public FileDescription A
        {
            get
            {
                return _a;
            }
        }

        public FileDescription B
        {
            get
            {
                return _b;
            }
        }
    }
}
