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



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is Symbol)) return false;
            return this.Equals((Symbol)obj);
        }

        // When we're comparing symbols,
        // symbols with the same identifier are the considered
        // the same symbol.
        // The value that the symbol is bound to might differ
        // based upon the current lexical scope,
        // but the symbols themselves equal the same.
        public bool Equals(Symbol symbol)
        {
            if (ReferenceEquals(null, symbol)) return false;
            if (ReferenceEquals(this, symbol)) return true;
            return symbol.identifier == this.identifier;
        }


        public override string ToString()
        {
            return string.Format("{0}", this.identifier);
        }

    }
}
