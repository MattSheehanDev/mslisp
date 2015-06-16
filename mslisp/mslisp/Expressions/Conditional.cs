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
     * IF
     * (if condition exp1 exp2) => val1 || val2
     */
    class IfElse : SExpression
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


    /*
     * COND
     * (cond (c1 e1) ... (cn en)) => value(en) || nil
     */
     class Conditions : SExpression
    {
        public Conditions()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 2)
                throw new ArgumentException("COND is missing arguments.");


            var conditions = list.CDR();

            for (var i = 0; i < conditions.Length; i++)
            {
                IDatum item = conditions[i];

                if (!(item is Vector))
                    throw new SyntaxException("Conditional pair {0} is missing an expresion.", item.Value);

                Vector pair = (Vector)item;

                if (pair.Length != 2)
                    throw new ArgumentException("Conditional has wrong number of arguments");

                IDatum condition = pair[0];
                IDatum result = Evaluator.Eval(condition, env);

                if ((bool)result.Value)
                {
                    IDatum expression = pair[1];
                    IDatum value = Evaluator.Eval(expression, env);
                    return value;
                }
            }

            // no conditional executed, return false.
            return Null.Instance;
        }

    }

}
