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
     * QUOTE
     * (quote exp) => exp
     */
    class Quote : FuncToken
    {
        public Quote()
        {
            this.value = this.AsData;
        }


        private IToken AsData(ListToken list, ScopedEnvironment env)
        {
            if (list.Count != 2)
                throw new ArgumentException("QUOTE is missing arguments.");

            return list[1];
        }

    }
}
