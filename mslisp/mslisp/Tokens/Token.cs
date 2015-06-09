using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;

namespace mslisp.Tokens
{
    interface IToken
    {
        TokenType Type { get; }
        object Value { get; }
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


        public static bool isAtom(IToken token)
        {
            if (token.Type == TokenType.INT ||
                token.Type == TokenType.DOUBLE ||
                token.Type == TokenType.STRING ||
                token.Type == TokenType.BOOLEAN)
                return true;

            return false;
        }

        public static bool isNumber(IToken token)
        {
            if (TokenType.INT == token.Type || TokenType.DOUBLE == token.Type)
                return true;
            return false;
        }
    }
    
}
