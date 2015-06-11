using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;
using mslisp.Tokens;

namespace mslisp
{
    class Evaluator
    {

        public Evaluator()
        {
        }

        public static IToken Eval(IToken x, ScopedEnvironment env)
        {
            if (Token.isAtom(x))
            {
                return x;
            }
            else if (x.Type == TokenType.SYMBOL)
            {
                var sym = env.Fetch((string)x.Value);

                if (sym is IToken)
                {
                    return (IToken)sym;
                }
                else
                {
                    IToken token = new Token(TokenType.LAMBDA, sym);
                    return token;
                }
            }
            else
            {
                var list = (ListToken)x;

                IToken first = list.CAR();
                IToken procedure = Evaluator.Eval(first, env);

                var func = (Func<ListToken, ScopedEnvironment, IToken>)procedure.Value;

                IToken token = func.Invoke(list, env);
                return token;
            }
        }

    }

}
