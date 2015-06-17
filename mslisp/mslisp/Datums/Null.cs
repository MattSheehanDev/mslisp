using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsLisp.Datums
{

    public class Null : Vector
    {
        public static readonly Null Instance = new Null();

        
        private Null() : base(new IDatum[0])
        {
        }
        

        public override string ToString()
        {
            return "NIL";
        }
    }
}
