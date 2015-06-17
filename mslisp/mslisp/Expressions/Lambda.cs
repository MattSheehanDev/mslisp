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
                if (largs.Length < 2)
                    throw new ArgumentException("{0} does not have enough arguments.", largs[0]);

                Vector paramslist = list.CDR();
                Vector argslist = largs.CDR();

                Vector parameters = (Vector)paramslist[0];
                IDatum expr = paramslist[1];
				

                // create new environment
				// not sure if i'm using the right environment???
                var scopedenv = new ScopedEnvironment(env);

                // bind parameter list with argument list
                for (var i = 0; i < parameters.Length; i++)
                {
                    IDatum param = parameters[i];
                    IDatum arg = Evaluator.Eval(argslist[i], lenv);

                    scopedenv.Add((string)param.Value, arg);
                }

                var result = Evaluator.Eval(expr, scopedenv);
                return result;
            };

            return new SExpression(func);
        }

    }

}
