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
     * DEFINE
     * (define var exp) => nil
     */
    class Define : FuncToken
    {
        public Define()
        {
            this.value = this.Def;
        }

        private IToken Def(ListToken list, ScopedEnvironment env)
        {
            if (list.Count != 3)
                throw new ArgumentException("DEFINE has wrong number of arguments.");

            ListToken args = list.CDR();
            IToken variable = args[0];
            IToken expression = args[1];

            env[(string)variable.Value] = Evaluator.Eval(expression, env);

            // the empty list is nil
            return new ListToken();
        }

    }
    
}
