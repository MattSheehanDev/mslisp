using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Datums
{
    class Symbol : IDatum
    {
        private readonly string identifier;
        private readonly DatumType type;


        public DatumType Type
        {
            get { return this.type; }
        }

        public object Value
        {
            get { return this.identifier; }
        }


        public Symbol(string identifier)
        {
            this.identifier = identifier;
            this.type = DatumType.SYMBOL;
        }
    }
}
