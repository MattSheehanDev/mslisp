using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;
using mslisp.Datums;

namespace mslisp.Expressions
{

    /*
     * SET!
     * (set! var exp) => nil
     */
    class Set : SExpression
    {
        public Set()
        {
            this.value = this.Setter;
        }


        public IDatum Setter(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 3)
                throw new ArgumentException("SET is missing arguments.");

            Vector args = list.CDR();
            IDatum variable = args[0];
            IDatum expr = args[1];

            IDatum value = Evaluator.Eval(expr, env);

            ScopedEnvironment envscope = env.Find((string)variable.Value);
            envscope[(string)variable.Value] = value;

            // empty list is nil
            return env.Fetch("nil");
        }

    }

}
