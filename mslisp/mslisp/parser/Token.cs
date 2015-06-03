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
        EXPRESSION,
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
            this.type = TokenType.EXPRESSION;
            this.value = this;
        }

        public IToken Shift()
        {
            var item = this.First();
            this.RemoveAt(0);
            return item;
        }

        public bool isAtom()
        {
            return false;
        }
    }
}
