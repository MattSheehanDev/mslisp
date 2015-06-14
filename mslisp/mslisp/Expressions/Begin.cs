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
     * BEGIN
     * (begin exp*) => last valueof(exp)
     */
    class Begin : SExpression
    {
        public Begin()
        {
            this.value = this.Evaluate;
        }


        private IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 2)
                throw new ArgumentException("BEGIN is missing arguments.");

            Vector argv = list.CDR();

            IDatum value = null;
            for (var i = 0; i < argv.Length; i++)
            {
                value = Evaluator.Eval(argv[i], env);
            }
            return value;
        }

    }
}
