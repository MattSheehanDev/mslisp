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
    class IfElse : FuncToken
    {
        public IfElse()
        {
            this.value = this.CheckIfElse;
        }


        private IToken CheckIfElse(ListToken list, ScopedEnvironment env)
        {
            if (list.Count != 4)
                throw new ArgumentException("IF has wrong number of arguments.");

            ListToken args = list.CDR();
            IToken condition = args[0];
            IToken expr1 = args[1];
            IToken expr2 = args[2];

            IToken value = Evaluator.Eval(condition, env);
            if ((bool)value.Value)
                return Evaluator.Eval(expr1, env);
            else
                return Evaluator.Eval(expr2, env);
        }

    }


    /*
     * COND
     * (cond (c1 e1) ... (cn en)) => value(en) || nil
     */
     class Conditions : FuncToken
    {
        public Conditions()
        {
            this.value = this.CheckConditions;
        }


        public IToken CheckConditions(ListToken list, ScopedEnvironment env)
        {
            if (list.Count < 2)
                throw new ArgumentException("COND is missing arguments.");


            var conditions = list.CDR();

            for (var i = 0; i < conditions.Count; i++)
            {
                IToken item = conditions[i];

                if (!(item is ListToken))
                    throw new SyntaxException(string.Format("Conditional pair {0} is missing an expresion.", item.Value));

                ListToken pair = (ListToken)item;

                if (pair.Count != 2)
                    throw new ArgumentException("Conditional has wrong number of arguments");

                IToken condition = pair[0];
                IToken result = Evaluator.Eval(condition, env);

                if ((bool)result.Value)
                {
                    IToken expression = pair[1];
                    IToken value = Evaluator.Eval(expression, env);
                    return value;
                }
            }

            return new ListToken();
        }

    }

}
