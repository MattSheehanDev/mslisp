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
     * QUOTE
     * (quote exp) => exp
     */
    class Quote : SExpression
    {
        public Quote()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("QUOTE is missing arguments.");

            return list[1];
        }

    }
}
