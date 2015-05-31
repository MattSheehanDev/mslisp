using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp
{
    class Evaluator
    {

        public Evaluator()
        {
        }

        public dynamic Eval(dynamic x, Environment env)
        {
            if(!(x is ListStack))
            {
                if (x is string)                // variable reference
                    return env.find(x)[x];
                else                            // constant literal
                    return x;
            }
            else
            {
                var list = (ListStack)x;
                var first = list.Shift();

                if (first == "quote")            // (quote exp)
                {
                    return list.Shift();
                }
                else if (first == "atom?")       // (atom? exp)
                {
                    var val = this.Eval(list.Shift(), env);
                    return val is List<dynamic>;
                }
                else if (first == "eq?")        // (eq? exp1 exp2)
                {
                    var exp1 = list.Shift();
                    var exp2 = list.Shift();

                    var val1 = this.Eval(exp1, env);
                    var val2 = this.Eval(exp1, env);

                    //Todo: check if list?
                    return val1 == val2;
                }
                else if (first == "car")        // (car exp)
                {
                    var exp = list.Shift();
                    var val = this.Eval(exp, env);

                    if (val is ListStack)
                        return ((ListStack)val)[0];

                    //Todo: stringify val1
                    throw new SyntaxException(string.Format("Expression {0} is not a valid list.", exp));
                }
                else if (first == "cdr")        // (cdr exp)
                {
                    var exp = list.Shift();
                    var val = this.Eval(exp, env);

                    if (val is ListStack)
                        return ((ListStack)val).Skip(1);

                    throw new SyntaxException(string.Format("Expression {0} is not a valid list", exp));
                }
                else if (first == "cons")           // (cons exp1 exp2)
                {
                    var exp1 = list.Shift();
                    var exp2 = list.Shift();

                    var val1 = this.Eval(exp1, env);
                    var val2 = this.Eval(exp2, env);

                    if (val1 is ListStack && val2 is ListStack)
                        ((ListStack)val2).InsertRange(0, val2);
                    else if (val2 is ListStack)
                        return ((ListStack)val2).Insert(0, val1);
                    else if (val2 is ListStack)
                        return ((ListStack)val1).Add(val2);

                    throw new SyntaxException(string.Format("Expression {0} or {1} must contain a list.", exp1, exp2));
                }
                else if (first == "cond")           // (cond (c1 e1) ... (cn en))
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        ListStack cond = list[i];

                        if (!(cond is ListStack))
                            throw new SyntaxException(string.Format("Conditional {0} is not a conditional clause.", cond));

                        if (this.Eval(cond.Shift(), env))
                            return this.Eval(cond.Shift(), env);
                    }
                }
                else if (first == "null?")          // (null? exp)
                {
                    var exp = list.Shift();
                    ListStack val = this.Eval(exp, env);

                    // The only null is lisp is the empty list.
                    // So if it's not a list, then it's an atom which is not
                    // null.
                    // if it's not an atom, then it's an exception
                    if (val is ListStack)
                        return val.Count > 0 ? false : true;
                    else
                        return false;
                }
                else if (first == "if")             // (if cond exp1 exp2)
                {
                    var cond = list.Shift();
                    var val = this.Eval(cond, env);

                    var exp1 = list.Shift();
                    var exp2 = list.Shift();

                    return val ? this.Eval(exp1, env) : this.Eval(exp2, env);
                }

            }

            return "";
        }

    }

}
