using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp
{
    enum TokenType
    {
        SYMBOL,
        STRING,
        INT,
        DOUBLE,
        BOOLEAN,
        LIST,
        LAMBDA,
        QUOTE
    }


    interface IToken
    {
        TokenType Type { get; }
        object Value { get; }
        bool isAtom();
    }

    class Token : IToken
    {
        private readonly TokenType type;
        private readonly object value;


        public TokenType Type { get { return this.type; } }
        public object Value { get { return this.value; } }


        public Token(TokenType type, object value)
        {
            this.type = type;
            this.value = value;
        }


        public bool isAtom()
        {
            if (this.type == TokenType.INT ||
                this.type == TokenType.DOUBLE ||
                this.type == TokenType.STRING ||
                this.type == TokenType.BOOLEAN)
                return true;

            return false;
        }
    }


    class TokenList : List<IToken>, IToken
    {
        private readonly TokenType type;
        private readonly object value;

        public TokenType Type { get { return this.type; } }
        public object Value { get { return this.value; } }


        public TokenList()
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

        public TokenList CDR()
        {
            if (this.Count <= 1)
                return null;

            var rest = new TokenList();
            for (var i = 1; i < this.Count; i++)
            {
                rest.Add(this[i]);
            }
            return rest;
        }

        public bool isAtom()
        {
            return false;
        }
    }
}
