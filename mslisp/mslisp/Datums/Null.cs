using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Datums
{
    class Null : Atom
    {
        public Null() : base(DatumType.NULL, null)
        {
        }
        


        public override string ToString()
        {
            return "NIL";
        }
    }
}
