using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Environment;
using MsLisp.Datums;

namespace MsLisp.Macro
{

    /*
     * MACRO
     * (macro (lambda (params*) body) => s-expression
     */
    class Macro : SExpression
    {
        public Macro()
        {
        }

        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 1)
                throw new ArgumentException("MACRO is missing arguments.");

            // lambda evaluated
            SExpression exp = Evaluator.Eval(list[0], env) as SExpression;

            Func<Vector, ScopedEnvironment, IDatum> func = (largs, lenv) =>
            {
                //var arglist = largs.CDR();

                // since macro just wraps a lambda, but since lambda always
                // evaluates it's arguments before invoking it's body expression,
                // we want to wrap the parameters in [quote]'s so that we
                // end with the same original arguments
                var data = new List<IDatum>();
                //data.Add(largs[0]);
                for (var i = 0; i < largs.Length; i++)
                {
                    var quoted = new List<IDatum>();
                    quoted.Add(new Symbol("quote"));
                    quoted.Add(largs[i]);
                    data.Add(new Vector(quoted.ToArray()));
                }
                var expand = new Vector(data.ToArray());

                // evaluate lambda with [quote]d args.
                // the result should be some code.
                // 'expansion' should use the env from when
                // the macro was declared. This is because
                // expansion of macros shouldn't depend
                // on where they are expanded, such as in
                // nested lambda's.
                var result = exp.Evaluate(expand, env);

                // we then want to evaluate the returned code.
                // evaluate with the most recent environment
                return Evaluator.Eval(result, lenv);
            };

            return new SExpression(func);
        }

    }



    class QuasiQuote : SExpression
    {
        private Symbol unquote;
        private Symbol splice;


        public QuasiQuote()
        {
            this.unquote = new Symbol("unquote");
            this.splice = new Symbol("splice");
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 1)
                throw new ArgumentException("QUASIQUOTE takes 1 argument.");
            
            //var quasiquoted = list[1];
            return this.Expand(list[0], env);
        }


        private IDatum Expand(IDatum datum, ScopedEnvironment env)
        {
            // if vector, expand each child datum
            if (datum is Vector)
            {
                var first = this.CheckExpression(datum as Vector);

                if (first == this.unquote)
                {
                    return Evaluator.Eval(datum, env);
                }
                else if (first == this.splice)
                {
                    return Evaluator.Eval(datum, env);
                }
                else
                {
                    var q = datum as Vector;

                    var list = new List<IDatum>();
                    for (var i = 0; i < q.Length; i++)
                    {
                        var data = q[i];
                        var exp = this.Expand(data, env);

                        if(data is Vector && exp is Vector)
                        {
                            if(this.CheckExpression(data as Vector) == this.splice)
                            {
                                list.AddRange((IDatum[])exp.Value);
                                continue;
                            }
                        }

                        list.Add(exp);
                    }
                    return new Vector(list.ToArray());
                }
            }

            // not a vector, means there are no children to expand
            return datum;
        }


        private IDatum CheckExpression(Vector exp)
        {
            var first = exp.CAR();

            if (this.unquote.Equals(first))
                return this.unquote;
            else if (this.splice.Equals(first))
                return this.splice;

            return exp;
        }

    }



    class UnQuote : SExpression
    {
        public UnQuote()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 1)
                throw new ArgumentException("UNQUOTE takes 2 arguments.");

            // unquote doesn't evaluate but it does insert symbol bindings from
            // the unmacro'd environment.
            //var datum = env.Fetch(list[1].Value.ToString());
            var datum = Evaluator.Eval(list[0], env);
            return datum;
            //// unquote starts evaluating again from inside a quasiquote
            //return Evaluator.Eval(list[1], env);
        }

    }



    class Splice : SExpression
    {
        public Splice()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 1)
                throw new ArgumentException("SPLICE takes 2 arguments.");

            return Evaluator.Eval(list[0], env);
        }

    }

}
