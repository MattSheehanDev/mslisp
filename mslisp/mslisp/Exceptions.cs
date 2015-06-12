using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp
{
    class SyntaxException : Exception
    {
        public SyntaxException(string msg, params object[] obj) : base(String.Format(msg, obj))
        {
        }
    }

    class ArgumentException : Exception
    {
        public ArgumentException(string msg, params object[] obj) : base(String.Format(msg, obj))
        {
        }
    }


    class TypeException : Exception
    {
        public TypeException(string msg, params object[] obj) : base(String.Format(msg, obj))
        {
        }
    }
}
