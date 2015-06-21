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
            if (list.Length < 2)
                throw new ArgumentException("MACRO is missing arguments.");

            // lambda evaluated
            SExpression exp = Evaluator.Eval(list[1], env) as SExpression;

            Func<Vector, ScopedEnvironment, IDatum> func = (largs, lenv) =>
            {
                //var arglist = largs.CDR();

                // since macro just wraps a lambda, but since lambda always
                // evaluates it's arguments before invoking it's body expression,
                // we want to wrap the parameters in [quote]'s so that we
                // end with the same original arguments
                var data = new List<IDatum>();
                data.Add(largs[0]);
                for (var i = 1; i < largs.Length; i++)
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
        public QuasiQuote()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("QUASIQUOTE takes 1 argument.");
            
            var quasiquoted = list[1];
            return this.Expand(list, quasiquoted, env);
        }


        private IDatum Expand(Vector parent, IDatum datum, ScopedEnvironment env)
        {
            // if vector, expand each child datum
            if (datum is Vector)
            {
                var q = datum as Vector;
                var first = q.CAR();

                if (first.ToString() == "unquote")
                {
                    return Evaluator.Eval(datum, env);
                }
                else if (first.ToString() == "splice")
                {
                    //var result = Evaluator.Eval(datum, env);
                    //var list = (IDatum[])parent.Value;

                    //var list = new List<IDatum>();
                }
                else
                {
                    var list = new List<IDatum>();
                    for (var i = 0; i < q.Length; i++)
                    {
                        list.Add(this.Expand(q, q[i], env));
                    }
                    return new Vector(list.ToArray());
                }
            }

            // not a vector, means there are no children to expand
            return datum;
        }
    }



    class UnQuote : SExpression
    {
        public UnQuote()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("UNQUOTE takes 2 arguments.");

            // unquote doesn't evaluate but it does insert symbol bindings from
            // the unmacro'd environment.
            //var datum = env.Fetch(list[1].Value.ToString());
            var datum = Evaluator.Eval(list[1], env);
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
            if (list.Length != 2)
                throw new ArgumentException("SPLICE takes 2 arguments.");

            return base.Evaluate(list, env);
        }

    }

}
