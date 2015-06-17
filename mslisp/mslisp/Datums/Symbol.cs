using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsLisp.Datums
{
    public class Symbol : IDatum
    {
        private readonly string identifier;
        

        public object Value
        {
            get { return this.identifier; }
        }


        public Symbol(string identifier)
        {
            this.identifier = identifier;
        }
    }
}
