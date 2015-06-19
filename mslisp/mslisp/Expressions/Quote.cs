using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Environment;
using MsLisp.Datums;

namespace MsLisp.Expressions
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


        public override string ToString()
        {
            return "quote";
        }

    }
}
