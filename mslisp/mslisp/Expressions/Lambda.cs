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
            if (list.Length != 3)
                throw new ArgumentException("LAMBDA definition is missing arguments.");

            if (!(list[1] is Vector))
                throw new SyntaxException("LAMBDA does not have a parameter list.");
			
            Func<Vector, ScopedEnvironment, IDatum> func = (largs, lenv) =>
            {
                // first argument is s-expression symbol
                // second+ argument(s) are arguments
                if (largs.Length < 2)
                    throw new ArgumentException("{0} does not have enough arguments.", largs[0]);

                Vector paramslist = list.CDR();
                Vector argslist = largs.CDR();

                Vector parameters = (Vector)paramslist[0];
                IDatum expr = paramslist[1];


                // create new environment
                // not sure if i'm using the right environment???
                var scopedenv = new ScopedEnvironment(env);

                //// bind parameter list with argument list
                //for (var i = 0; i < parameters.Length; i++)
                //{
                //    IDatum param = parameters[i];
                //    IDatum arg = Evaluator.Eval(argslist[i], lenv);

                //    if (param is Vector)
                //    {
                //        var plist = param as Vector;
                //        var pargs = arg as Vector;

                //        if (pargs == null)
                //            throw new ArgumentException("{0} expected a list.", list.CAR());

                //        for (var j = 0; j < plist.Length; j++)
                //        {
                //            //IDatum argi = Evaluator.Eval(pargs[i], lenv);
                //            scopedenv.Add((string)plist[j].Value, pargs[j]);
                //        }
                //    }
                //    else
                //    {
                //        scopedenv.Add((string)param.Value, arg);
                //    }
                //}

                this.BindArguments(scopedenv, lenv, parameters, argslist);

                var result = Evaluator.Eval(expr, scopedenv);
                return result;
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
                            vlist.Add(Evaluator.Eval(arguments[j], env));
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

        private void RestParam(ScopedEnvironment env, IDatum param, IEnumerable<IDatum> args)
        {
            env.Add((string)param.Value, new Vector(args.ToArray()));
        }

    }

}
