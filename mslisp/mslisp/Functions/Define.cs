using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;

namespace mslisp.Functions
{

    /*
     * DEFINE
     * (define var exp) => nil
     */
    class Define : TokenFunction
    {
        public Define()
        {
            this.value = this.Def;
        }

        private IToken Def(TokenList list, ScopedEnvironment env)
        {
            if (list.Count != 3)
                throw new ArgumentException("DEFINE has wrong number of arguments.");

            TokenList args = list.CDR();
            IToken variable = args[0];
            IToken expression = args[1];

            env[(string)variable.Value] = Evaluator.Eval(expression, env);

            // the empty list is nil
            return new TokenList();
        }

    }
    
}
