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

        public static IDatum Eval(IDatum x, ScopedEnvironment env)
        {
            if (Datum.isAtom(x))
            {
                return x;
            }
            else if (x.Type == DatumType.SYMBOL)
            {
                var sym = env.Fetch((string)x.Value);
                
                if (sym is IDatum)
                {
                    return (IDatum)sym;
                }
                else
                {
                    IDatum token = new Datum(DatumType.LAMBDA, sym);
                    return token;
                }
            }
            else
            {
                var list = (Vector)x;

                IDatum first = list.CAR();
                IDatum procedure = Evaluator.Eval(first, env);

                var func = (SExpression)procedure;

                IDatum token = func.Invoke(list, env);
                return token;
            }
        }

    }

}
