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
    public class IfElse : SExpression
    {
        public IfElse()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 4)
                throw new ArgumentException("IF has wrong number of arguments.");

            Vector args = list.CDR();
            IDatum condition = args[0];
            IDatum expr1 = args[1];
            IDatum expr2 = args[2];

            IDatum value = Evaluator.Eval(condition, env);

            // anything thats not nil is true.
            if (!Null.Instance.Equals(value))
                return Evaluator.Eval(expr1, env);
            else
                return Evaluator.Eval(expr2, env);
        }

    }

}
