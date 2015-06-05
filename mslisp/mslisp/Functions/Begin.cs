using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;


namespace mslisp.Functions
{

    /*
     * BEGIN
     * (begin exp*) => last valueof(exp)
     */
    class Begin : TokenFunction
    {
        public Begin()
        {
            this.value = this.Evaluate;
        }


        private IToken Evaluate(TokenList list, ScopedEnvironment env)
        {
            if (list.Count < 2)
                throw new ArgumentException("BEGIN is missing arguments.");

            var args = list.CDR();
            IToken value = null;

            for (var i = 0; i < args.Count; i++)
            {
                value = args[i];
            }

            return value;
        }

    }
}
