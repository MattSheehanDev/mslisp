using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsLisp.Datums
{

    public enum DatumType
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

    public interface IDatum
    {
        //DatumType Type { get; }
        object Value { get; }
    }
    
}
