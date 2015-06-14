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
     * CAR
     * (car expression) => first
     */
    class CAR : SExpression
    {
        public CAR()
        {
            this.value = this.First;
        }


        private IDatum First(Vector list, ScopedEnvironment env)
        {
            IDatum value = Evaluator.Eval(list[1], env);

            if (!(value is Vector))
                throw new SyntaxException("{0} is not a valid list.", value.Value);

            Vector listvalue = (Vector)value;

            if (listvalue.Length == 0)
                return env.Fetch("nil");

            return listvalue[0];
        }
    }
    

    /*
     * CDR
     * (cdr expression) => rest of list
     */
    class CDR : SExpression
    {
        public CDR()
        {
            this.value = this.Rest;
        }


        private IDatum Rest(Vector list, ScopedEnvironment env)
        {
            IDatum value = Evaluator.Eval(list[1], env);

            if (!(value is Vector))
                throw new SyntaxException("{0} is not a valid list.", value.Value);

            Vector listvalue = (Vector)value;

            if (listvalue.Length <= 1)
                return env.Fetch("nil");

            var rest = new List<IDatum>();
            for (var i = 1; i < listvalue.Length; i++)
            {
                rest.Add(listvalue[i]);
            }
            return new Vector(rest.ToArray());
        }
    }

    /*
     * CONS
     * (cons exp1 exp2) => list
     */
    class CONS : SExpression
    {
        public CONS()
        {
            this.value = this.Append;
        }


        private IDatum Append(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 3)
                throw new ArgumentException("CONS has wrong number of arguments.");

            Vector exprs = list.CDR();
            IDatum expr1 = exprs[0];
            IDatum expr2 = exprs[1];

            IDatum value1 = Evaluator.Eval(expr1, env);
            IDatum value2 = Evaluator.Eval(expr2, env);


            if (value2 is Vector)
            {
                var cons = new List<IDatum>();
                
                cons.Add(value1);

                Vector list2 = (Vector)value2;
                for (var i = 0; i < list2.Length; i++)
                {
                    cons.Add(list2[i]);
                }

                return new Vector(cons.ToArray());
            }
            else if (value1 is Vector)
            {
                var cons = new List<IDatum>();

                cons.Add(value2);

                Vector list1 = (Vector)value1;
                for (var i = 0; i < list1.Length; i++)
                {
                    cons.Add(list1[i]);
                }

                return new Vector(cons.ToArray());
            }
            else
            {
                throw new ArgumentException("CONS problem with {0} or {1}.", value1.Value, value2.Value);
            }
        }

    }

}
