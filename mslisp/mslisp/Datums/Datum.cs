using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;

namespace mslisp.Tokens
{
    interface IDatum
    {
        DatumType Type { get; }
        object Value { get; }
    }

    class Datum : IDatum
    {
        private readonly DatumType type;
        private readonly object value;


        public DatumType Type { get { return this.type; } }
        public object Value { get { return this.value; } }


        public Datum(DatumType type, object value)
        {
            this.type = type;
            this.value = value;
        }


        public static bool isAtom(IDatum token)
        {
            if (token.Type == DatumType.INT ||
                token.Type == DatumType.DOUBLE ||
                token.Type == DatumType.STRING ||
                token.Type == DatumType.BOOLEAN)
                return true;

            return false;
        }

        public static bool isNumber(IDatum token)
        {
            if (DatumType.INT == token.Type || DatumType.DOUBLE == token.Type)
                return true;
            return false;
        }
    }
    
}
