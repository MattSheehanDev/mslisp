using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;
using mslisp.Environment;

namespace mslisp.Functions
{
    /*
     * ISEQUAL
     * (equal? args...)
     */
    class IsEqual : FuncToken
    {
        public IsEqual()
        {
            this.value = this.equals;
        }


        // equal? can go one of three ways
        // 1. (equal?) => error
        // 2. (equal? arg arg ...) => error
        // 3. (equal? arg arg) => T or ()
        public IToken equals(ListToken list, ScopedEnvironment env)
        {
            if (list.Count != 3)
                throw new ArgumentException("equal? has incorrect number of arguments. Correct number is 2.");
            else
                return this._equals(list.CDR(), env);
        }

        private IToken _equals(ListToken list, ScopedEnvironment env)
        {
            IToken first = Evaluator.Eval(list[0], env);
            IToken second = Evaluator.Eval(list[1], env);

            // todo: check if list?
            // todo: compare token values or tokens?
            if (first == second)
                return env.Fetch("#t");

            return env.Fetch("nil");
        }
    }
}
