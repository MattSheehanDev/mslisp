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
     * BEGIN
     * (begin exp*) => last valueof(exp)
     */
    class Begin : FuncToken
    {
        public Begin()
        {
            this.value = this.Evaluate;
        }


        private IToken Evaluate(ListToken list, ScopedEnvironment env)
        {
            if (list.Count < 2)
                throw new ArgumentException("BEGIN is missing arguments.");

            var args = list.CDR();
            IToken value = null;

            for (var i = 0; i < args.Count; i++)
            {
                value = Evaluator.Eval(args[i], env);
            }

            return value;
        }

    }
}
