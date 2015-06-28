using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsLisp.Datums
{
    public class Bool : Atom
    {
        public static readonly Bool True = new Bool(true);


        private Bool(bool value) : base(value)
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
