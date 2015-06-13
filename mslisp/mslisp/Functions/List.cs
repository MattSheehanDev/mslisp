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

            if (listvalue.Count == 0)
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

            if (listvalue.Count <= 1)
                return env.Fetch("nil");

            var rest = new Vector();
            for (var i = 1; i < listvalue.Count; i++)
            {
                rest.Add(listvalue[i]);
            }
            return rest;
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
            if (list.Count != 3)
                throw new ArgumentException("CONS has wrong number of arguments.");

            Vector exprs = list.CDR();
            IDatum expr1 = exprs[0];
            IDatum expr2 = exprs[1];

            IDatum value1 = Evaluator.Eval(expr1, env);
            IDatum value2 = Evaluator.Eval(expr2, env);

            //if (value1 is ListToken && value2 is ListToken)
            //{
            //    ListToken list1 = (ListToken)value1;
            //    ListToken list2 = (ListToken)value2;

            //    list2.InsertRange(0, list2);
            //    return list2;
            //}
            if (value2 is Vector)
            {
                var cons = new Vector();
                
                cons.Add(value1);

                Vector list2 = (Vector)value2;
                list2.ForEach((v) =>
                {
                    cons.Add(v);
                });

                return cons;
            }
            else if (value1 is Vector)
            {
                var cons = new Vector();

                cons.Add(value2);

                Vector list1 = (Vector)value1;
                list1.ForEach((v) =>
                {
                    cons.Add(v);
                });

                return cons;
            }
            else
            {
                throw new ArgumentException("CONS problem with {0} or {1}.", value1.Value, value2.Value);
            }
        }

    }

}
