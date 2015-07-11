using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Environment;
using MsLisp.Datums;

namespace MsLisp.Expressions
{

    /*
     * LAMBDA
     * (lambda (params*) exp) => anonymous function
     */
    class Lambda : SExpression
    {
		public Lambda()
        {
        }


		public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("LAMBDA definition is missing arguments.");

            if (!(list[0] is Vector))
                throw new SyntaxException("LAMBDA does not have a parameter list.");

            // parameters and body expression
            Vector parameters = list[0] as Vector;
            //Vector parameters = (Vector)paramslist[0];
            IDatum expr = list[1];


            // the big quesion.
            // should the environment that the expression is initialized
            // in be used to evaluate it or the environment that it's
            // invoked in be used to initialize it.
            // i'm leaning toward the former because it seems to
            // work better, but the latter makes more sense to me.
            Func<Vector, ScopedEnvironment, IDatum> func = (largs, lenv) =>
            {
                //// first argument is s-expression symbol
                //// second+ argument(s) are arguments
                //if (largs.Length < 2)
                //    throw new ArgumentException("{0} does not have enough arguments.", largs[0]);

                //Vector argslist = largs.CDR();

                // create new environment
                // not sure if i'm using the right environment???
                var scopedenv = new ScopedEnvironment(env);
                this.BindArguments(scopedenv, lenv, parameters, largs);
                return Evaluator.Eval(expr, scopedenv);
            };

            return new SExpression(func);
        }


        // Binds arguments to lambda parameters when invoked
        private void BindArguments(ScopedEnvironment env, ScopedEnvironment context, Vector parameters, Vector arguments)
        {
            // bind parameter list with argument list
            for (var i = 0; i < parameters.Length; i++)
            {
                IDatum param = parameters[i];
                IDatum arg = Evaluator.Eval(arguments[i], context);     // evaluate in correct context
                
                if (param is Vector)
                {
                    var vparams = param as Vector;
                    var vargs = arg as Vector;
                    IDatum[] datums = (IDatum[])vargs.Value;

                    if (vargs == null)
                        throw new ArgumentException("LAMBDA expected a list.");

                    for (var j = 0; j < vparams.Length; j++)
                    {
                        var paramj = vparams[j];
                        var argj = vargs[j];

                        // check for &rest params
                        if(this.CheckKeyword(paramj))
                        {
                            var vec = new Vector(datums.Skip(j).ToArray());
                            env.Add((string)vparams[j+1].Value, vec);
                            break;
                        }

                        env.Add((string)vparams[j].Value, vargs[j]);
                    }
                }
                else
                {
                    // check for &rest params
                    if (this.CheckKeyword(param))
                    {
                        var vlist = new List<IDatum>();
                        vlist.Add(arg);
                        for (var j = i + 1; j < arguments.Length; j++)
                        {
                            vlist.Add(Evaluator.Eval(arguments[j], context));
                        }

                        env.Add((string)parameters[i + 1].Value, new Vector(vlist.ToArray()));
                        break;
                    }

                    env.Add((string)param.Value, arg);
                }
            }
        }

        private bool CheckKeyword(IDatum param)
        {
            if ((string)param.Value == "&rest")
                return true;
            return false;
        }

    }

}
