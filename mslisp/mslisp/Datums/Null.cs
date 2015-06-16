using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Datums
{
    class Null : Atom
    {
        public static readonly Null Instance = new Null();


        private Null() : base(null)
        {
        }
        


        public override string ToString()
        {
            return "NIL";
        }
    }
}
