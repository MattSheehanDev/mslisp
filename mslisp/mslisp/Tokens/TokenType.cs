using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Tokens
{
    enum TokenType
    {
        SYMBOL,
        STRING,
        INT,
        DOUBLE,
        BOOLEAN,
        LIST,
        LAMBDA,
        QUOTE
    }
}
