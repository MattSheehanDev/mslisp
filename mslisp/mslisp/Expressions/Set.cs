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
     * SET!
     * (set! var exp) => nil
     */
    class Set : SExpression
    {
        public Set()
        {
        }

        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 3)
                throw new ArgumentException("SET is missing arguments.");

            Vector args = list.CDR();
            var variable = args[0].Value as string;
            
            // re-bind environment symbol and datum
            ScopedEnvironment envscope = env.Find(variable);
            envscope[variable] = Evaluator.Eval(args[1], env);
            

            return Null.Instance;
        }

    }

}
