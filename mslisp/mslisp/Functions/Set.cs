using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;

namespace mslisp.Functions
{

    /*
     * SET!
     * (set! var exp) => nil
     */
    class Set : TokenFunction
    {
        public Set()
        {
            this.value = this.Setter;
        }


        public IToken Setter(TokenList list, ScopedEnvironment env)
        {
            if (list.Count < 3)
                throw new ArgumentException("SET is missing arguments.");

            TokenList args = list.CDR();
            IToken variable = args[0];
            IToken expr = args[1];

            var envscope = env.find((string)variable.Value);
            IToken value = Evaluator.Eval(expr, env);

            envscope[(string)variable.Value] = value;

            // empty list is nil
            return new TokenList();
        }

    }

}
