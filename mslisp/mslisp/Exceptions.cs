using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp
{
    class SyntaxException : Exception
    {
        public SyntaxException(string msg) : base(msg)
        {
        }
    }

    class ArgumentException : Exception
    {
        public ArgumentException(string msg) : base(msg)
        {
        }
    }


    class TypeException : Exception
    {
        public TypeException(string msg) : base(msg)
        {
        }
    }
}
