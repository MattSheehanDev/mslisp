using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsLisp.Lexical
{

    public enum TokenType
    {
        LISTOPEN,           // (
        LISTCLOSE,          // )
        STRING,             // "   "
        INT,                // 2
        DOUBLE,             // 2.0
        SYMBOL,             // #t and nil are symbols, so we don't need a boolean token (we still have boolean data values though)
        COMMENT,            // ;
        WHITESPACE,         //
        QUOTE,              // '
        QUASIQUOTE,         // `
        UNQUOTE,            // ,
        SPLICE              // ,@
    }

    public class Token
    {
        private TokenType type;
        private string value;

        public TokenType Type { get { return this.type; } }
        public string Value { get { return this.value; } }


        public Token(TokenType type, string value)
        {
            this.value = value;
            this.type = type;
        }
    }

}
