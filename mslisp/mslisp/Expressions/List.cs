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
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 2)
                throw new ArgumentException("CAR is missing argument.");

            var args = list.CDR();
            var value = Evaluator.Eval(args[0], env) as Vector;
            
            if(value == null)
                throw new SyntaxException("{0} is not a valid list.", value.Value);
            else if (value.Length == 0)
                return Null.Instance;

            return value[0];
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
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            var value = Evaluator.Eval(list[1], env) as Vector;

            if (value == null)
                throw new SyntaxException("{0} is not a valid list.", value.Value);
            else if (value.Length <= 1)
                return Null.Instance;

            var rest = new List<IDatum>();
            for (var i = 1; i < value.Length; i++)
            {
                rest.Add(value[i]);
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
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 3)
                throw new ArgumentException("CONS has wrong number of arguments.");

            Vector exprs = list.CDR();

            IDatum value1 = Evaluator.Eval(exprs[0], env);
            IDatum value2 = Evaluator.Eval(exprs[1], env);


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
