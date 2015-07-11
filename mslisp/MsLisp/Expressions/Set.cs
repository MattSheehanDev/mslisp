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
            if (list.Length < 2)
                throw new ArgumentException("SET is missing arguments.");
            
            var variable = list[0].Value as string;
            
            // re-bind environment symbol and datum
            ScopedEnvironment envscope = env.Find(variable);
            envscope[variable] = Evaluator.Eval(list[1], env);
            

            return Null.Instance;
        }

    }

}
