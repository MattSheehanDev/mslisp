using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Tokens
{
    class ListToken : List<IToken>, IToken
    {
        private readonly TokenType type;
        private readonly object value;

        public TokenType Type { get { return this.type; } }
        public object Value { get { return this.value; } }


        public ListToken()
        {
            this.type = TokenType.LIST;
            this.value = this;
        }

        public IToken Shift()
        {
            var item = this.First();
            this.RemoveAt(0);
            return item;
        }

        public IToken CAR()
        {
            if (this.Count == 0)
                return null;

            return this[0];
        }

        public ListToken CDR()
        {
            if (this.Count <= 1)
                return null;

            var rest = new ListToken();
            for (var i = 1; i < this.Count; i++)
            {
                rest.Add(this[i]);
            }
            return rest;
        }

    }

}
