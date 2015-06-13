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
     * IF
     * (if condition exp1 exp2) => val1 || val2
     */
    class IfElse : SExpression
    {
        public IfElse()
        {
            this.value = this.CheckIfElse;
        }


        private IDatum CheckIfElse(Vector list, ScopedEnvironment env)
        {
            if (list.Count != 4)
                throw new ArgumentException("IF has wrong number of arguments.");

            Vector args = list.CDR();
            IDatum condition = args[0];
            IDatum expr1 = args[1];
            IDatum expr2 = args[2];

            IDatum value = Evaluator.Eval(condition, env);

            // anything thats not nil is true.
            if (value != env.Fetch("nil"))
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
            this.value = this.CheckConditions;
        }


        public IDatum CheckConditions(Vector list, ScopedEnvironment env)
        {
            if (list.Count < 2)
                throw new ArgumentException("COND is missing arguments.");


            var conditions = list.CDR();

            for (var i = 0; i < conditions.Count; i++)
            {
                IDatum item = conditions[i];

                if (!(item is Vector))
                    throw new SyntaxException("Conditional pair {0} is missing an expresion.", item.Value);

                Vector pair = (Vector)item;

                if (pair.Count != 2)
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
            return env.Fetch("nil");
        }

    }

}
