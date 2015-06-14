using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Datums
{
    class Bool : Atom
    {
        public Bool(bool value) : base(DatumType.BOOLEAN, value)
        {
        }


        public override string ToString()
        {
            if (true.Equals(this.Value))
                return "T";
            else
                return "NIL";
        }
    }
}
