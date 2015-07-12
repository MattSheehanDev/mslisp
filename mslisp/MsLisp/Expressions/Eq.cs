using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Datums;
using MsLisp.Environment;

namespace MsLisp.Expressions
{

    /*
     * EQ?
     * (eq? exp1 exp2) => T or ()
     */
     class Eq : SExpression
    {
        public Eq()
        {
        }


        // eq? only compares atoms,
        // but does not compare numbers (which are compared with '=')
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            var first = Evaluator.Eval(list[0], env);
            var second = Evaluator.Eval(list[1], env);

            var type = first.GetType();
            var typetwo = second.GetType();
            
            if (first is Number || second is Number)
                return Null.Instance;
            else if (first.Equals(second))
                return Bool.True;
            else
                return Null.Instance;
        }
    }

}
