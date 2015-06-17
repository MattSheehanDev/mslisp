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
     * DEFINE
     * (define var exp) => nil
     */
    class Define : SExpression
    {
        public Define()
        {
        }

        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 3)
                throw new ArgumentException("DEFINE has wrong number of arguments.");

            Vector args = list.CDR();
            IDatum variable = args[0];
            IDatum expression = args[1];

            env.Add((string)variable.Value, Evaluator.Eval(expression, env));

            // the empty list is nil
            return Null.Instance;
        }

    }
    
}
