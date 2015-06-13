using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Tokens
{
    enum DatumType
    {
        SYMBOL,
        STRING,
        INT,
        DOUBLE,
        BOOLEAN,
        NULL,
        LIST,
        LAMBDA
    }
}
