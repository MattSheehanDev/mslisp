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

        public dynamic Eval(IToken x, Environment env)
        {
            if (x.isAtom())
            {
                return x.Value;
            }
            else if (x.Type == TokenType.SYMBOL)
            {
                return env.find((string)x.Value)[(string)x.Value];
            }
            else
            {
                var list = (TokenList)x.Value;

                var token = list.Shift();
                var first = (string)token.Value;

                //if (first == "quote")            // (quote exp)
                //{
                //    return list.Shift();
                //}
                if (first == "atom?")       // (atom? exp)
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

                    if (val is TokenList)
                        return ((TokenList)val)[0];

                    //Todo: stringify val1
                    throw new SyntaxException(string.Format("Expression {0} is not a valid list.", exp));
                }
                else if (first == "cdr")        // (cdr exp)
                {
                    var exp = list.Shift();
                    var val = this.Eval(exp, env);

                    if (val is TokenList)
                        return ((TokenList)val).Skip(1);

                    throw new SyntaxException(string.Format("Expression {0} is not a valid list", exp));
                }
                else if (first == "cons")           // (cons exp1 exp2)
                {
                    var exp1 = list.Shift();
                    var exp2 = list.Shift();

                    var val1 = this.Eval(exp1, env);
                    var val2 = this.Eval(exp2, env);

                    if (val1 is TokenList && val2 is TokenList)
                        ((TokenList)val2).InsertRange(0, val2);
                    else if (val2 is TokenList)
                        return ((TokenList)val2).Insert(0, val1);
                    else if (val2 is TokenList)
                        return ((TokenList)val1).Add(val2);

                    throw new SyntaxException(string.Format("Expression {0} or {1} must contain a list.", exp1, exp2));
                }
                else if (first == "cond")           // (cond (c1 e1) ... (cn en))
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        TokenList cond = (TokenList)list[i];

                        if (!(cond is TokenList))
                            throw new SyntaxException(string.Format("Conditional {0} is not a conditional clause.", cond));

                        if (this.Eval(cond.Shift(), env))
                            return this.Eval(cond.Shift(), env);
                        else
                            // return new nil
                            return new TokenList();
                    }
                }
                else if (first == "null?")          // (null? exp)
                {
                    var exp = list.Shift();
                    TokenList val = this.Eval(exp, env);

                    // The only null is lisp is the empty list.
                    // So if it's not a list, then it's an atom which is not
                    // null.
                    // if it's not an atom, then it's an exception
                    if (val is TokenList)
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
                else if (first == "set!")                // (set! var exp)
                {
                    var variable = list.Shift();
                    var exp = list.Shift();

                    var venv = env.find((string)variable.Value);
                    venv[variable.Value] = this.Eval(exp, env);

                    // set! returns nil
                    return new TokenList();
                }
                else if(first == "define")           // (define var exp)
                {
                    var variable = list.Shift();
                    var exp = list.Shift();

                    env.Add((string)variable.Value, this.Eval(exp, env));

                    // define procedure returns nil
                    return new TokenList();
                }
                else if(first == "lambda")           // (lambda (params*) exp)
                {
                    lambdatype l = (values) => {
                        TokenList paramlist = (TokenList)list.Shift();
                        var exp = list.Shift();

                        // Create a new environment.
                        var lenv = new Environment(env);

                        // Add each parameter to the new environment
                        // and bind each argument
                        for (var i = 0; i < paramlist.Count; i++)
                        {
                            lenv[(string)paramlist[i].Value] = values[i];
                        }

                        // Evaluate the expression.
                        return this.Eval(exp, lenv);
                    };
                    
                    return l;
                }
                else if (first == "begin")           // (begin exp*)
                {
                    dynamic val = null;
                    list.ForEach((exp) =>
                    {
                        val = this.Eval(exp, env);
                    });
                    return val;
                }
                else                                  // (proc exp*)
                {
                    var proc = this.Eval(token, env);

                    dynamic[] exprs = new dynamic[list.Count];
                    for (var i = 0; i < list.Count; i++)
                    {
                        var expr = list[i];
                        var val = this.Eval(expr, env);

                        if (val is Token)
                            exprs[i] = val.Value;
                        else
                            exprs[i] = val;
                    }

                    return proc.Invoke(exprs);
                }
            }

            // If nothing else is returned,
            // return nil.
            return new TokenList();
        }

    }

}
