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
     * IF
     * (if condition exp1 exp2) => val1 || val2
     */
    public class IfThen : SExpression
    {
        public IfThen()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 3)
                throw new ArgumentException("IF has wrong number of arguments.");
            
            IDatum condition = list[0];
            IDatum expr1 = list[1];
            IDatum expr2 = list[2];

            IDatum value = Evaluator.Eval(condition, env);

            // anything thats not nil is true.
            if (!Null.Instance.Equals(value))
                return Evaluator.Eval(expr1, env);
            else
                return Evaluator.Eval(expr2, env);
        }

    }

}
