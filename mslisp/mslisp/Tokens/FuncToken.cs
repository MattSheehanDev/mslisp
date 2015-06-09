using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;

namespace mslisp.Tokens
{
    class FuncToken : IToken
    {
        protected TokenType type;
        protected Func<ListToken, ScopedEnvironment, IToken> value;

        public TokenType Type { get { return this.type; } }
        public object Value { get { return this.value; } }


        public FuncToken(Func<ListToken, ScopedEnvironment, IToken> func)
        {
            this.type = TokenType.LAMBDA;
            this.value = func;
        }
        public FuncToken()
        {
            this.type = TokenType.LAMBDA;
        }


        public IToken Invoke(ListToken list, ScopedEnvironment env)
        {
            return this.value.Invoke(list, env);
        }

    }

}
