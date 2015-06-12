using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;
using mslisp.Tokens;

namespace mslisp.Functions
{

    /*
     * LAMBDA
     * (lambda (params*) exp) => anonymous function
     */
    class Lambda : FuncToken
    {
		public Lambda()
        {
            this.value = this.CreateScope;
        }


		private IToken CreateScope(ListToken list, ScopedEnvironment env)
        {
            if (list.Count != 3)
                throw new ArgumentException("LAMBDA definition is missing arguments.");

            if (!(list[1] is ListToken))
                throw new SyntaxException("LAMBDA does not have a parameter list.");
			
            Func<ListToken, ScopedEnvironment, IToken> func = (largs, lenv) =>
            {
                if (largs.Count < 2)
                    throw new ArgumentException("{0} does not have enough arguments.", largs[0]);

                ListToken paramslist = list.CDR();
                ListToken argslist = largs.CDR();

                ListToken parameters = (ListToken)paramslist[0];
                IToken expr = paramslist[1];
				

                // create new environment
				// not sure if i'm using the right environment???
                var scopedenv = new ScopedEnvironment(env);

                // bind parameter list with argument list
                for (var i = 0; i < parameters.Count; i++)
                {
                    IToken param = parameters[i];
                    IToken arg = Evaluator.Eval(argslist[i], lenv);

                    scopedenv.Add((string)param.Value, arg);
                }

                var result = Evaluator.Eval(expr, scopedenv);
                return result;
            };

            return new FuncToken(func);
        }

    }

}
