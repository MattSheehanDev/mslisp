using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;

namespace mslisp
{
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
    
}
