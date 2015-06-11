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
     * SET!
     * (set! var exp) => nil
     */
    class Set : FuncToken
    {
        public Set()
        {
            this.value = this.Setter;
        }


        public IToken Setter(ListToken list, ScopedEnvironment env)
        {
            if (list.Count < 3)
                throw new ArgumentException("SET is missing arguments.");

            ListToken args = list.CDR();
            IToken variable = args[0];
            IToken expr = args[1];

            IToken value = Evaluator.Eval(expr, env);

            ScopedEnvironment envscope = env.Find((string)variable.Value);
            envscope[(string)variable.Value] = value;

            // empty list is nil
            return new ListToken();
        }

    }

}
