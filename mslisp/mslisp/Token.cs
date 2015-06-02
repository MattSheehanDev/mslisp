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
        EXPRESSION
    }


    interface IToken
    {
        TokenType type { get; }
        object value { get; }
        bool isAtom();
    }

    class Token : IToken
    {
        private readonly TokenType type;
        private readonly object value;


        TokenType IToken.type { get { return this.type; } }
        object IToken.value { get { return this.value; } }


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


        TokenType IToken.type { get { return this.type; } }
        object IToken.value { get { return this.value; } }


        public TokenList()
        {
            this.type = TokenType.EXPRESSION;
            this.value = null;
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
