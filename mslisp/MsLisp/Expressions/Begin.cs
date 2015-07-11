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
     * BEGIN
     * (begin exp*) => last valueof(exp)
     */
    public class Begin : SExpression
    {
        public Begin()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 1)
                throw new ArgumentException("BEGIN is missing arguments.");
            
            IDatum value = Null.Instance;
            for (var i = 0; i < list.Length; i++)
            {
                value = Evaluator.Eval(list[i], env);
            }
            return value;
        }

    }
}
