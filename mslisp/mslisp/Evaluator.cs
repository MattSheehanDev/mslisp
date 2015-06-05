using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;


namespace mslisp
{
    class Evaluator
    {

        public Evaluator()
        {
        }

        public static IToken Eval(IToken x, ScopedEnvironment env)
        {
            if (x.isAtom())
            {
                return x;
            }
            else if (x.Type == TokenType.SYMBOL)
            {
                var sym = env.find((string)x.Value)[(string)x.Value];

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
                var list = (TokenList)x;

                IToken first = list.CAR();
                IToken procedure = Evaluator.Eval(first, env);

                var func = (Func<TokenList, ScopedEnvironment, IToken>)procedure.Value;
                //var args = list.CDR();

                IToken token = func.Invoke(list, env);
                return token;
            }
            //else
            //{
            //    var list = (TokenList)x.Value;

            //    //var token = list.Shift();
            //    var token = list.CAR();
            //    var first = (string)token.Value;


            //    if (first == "atom?")       // (atom? exp)
            //    {
            //        var val = this.Eval(list.Shift(), env);
            //        return val is List<dynamic>;
            //    }
            //    else if (first == "eq?")        // (eq? exp1 exp2)
            //    {
            //        var exp1 = list.Shift();
            //        var exp2 = list.Shift();

            //        var val1 = this.Eval(exp1, env);
            //        var val2 = this.Eval(exp1, env);

            //        //Todo: check if list?
            //        return val1 == val2;
            //    }
            //    else if (first == "null?")          // (null? exp)
            //    {
            //        var exp = list.Shift();
            //        TokenList val = this.Eval(exp, env);

            //        // The only null is lisp is the empty list.
            //        // So if it's not a list, then it's an atom which is not
            //        // null.
            //        // if it's not an atom, then it's an exception
            //        if (val is TokenList)
            //            return val.Count > 0 ? false : true;
            //        else
            //            return false;
            //    }
            //    else                                  // (proc exp*)
            //    {
            //        var proc = Evaluator.Eval(token, env);

            //        var args = list.CDR();
            //        return proc.Invoke(args);

            //        //dynamic[] exprs = new dynamic[list.Count];
            //        //for (var i = 0; i < list.Count; i++)
            //        //{
            //        //    var expr = list[i];
            //        //    var val = this.Eval(expr, env);

            //        //    if (val is Token)
            //        //        exprs[i] = val.Value;
            //        //    else
            //        //        exprs[i] = val;
            //        //}

            //        //return proc.Invoke(exprs);
            //    }
            //}

        }

    }

}
