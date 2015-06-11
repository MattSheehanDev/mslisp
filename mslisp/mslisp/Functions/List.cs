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
    class CAR : FuncToken
    {
        public CAR()
        {
            this.value = this.First;
        }


        private IToken First(ListToken list, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(list[1], env);

            if (!(value is ListToken))
                throw new SyntaxException(string.Format("{0} is not a valid list.", value.Value));

            ListToken listvalue = (ListToken)value;

            if (listvalue.Count == 0)
                return env.Fetch("nil");

            return listvalue[0];
        }
    }
    

    /*
     * CDR
     * (cdr expression) => rest of list
     */
    class CDR : FuncToken
    {
        public CDR()
        {
            this.value = this.Rest;
        }


        private IToken Rest(ListToken list, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(list[1], env);

            if (!(value is ListToken))
                throw new SyntaxException(string.Format("{0} is not a valid list.", value.Value));

            ListToken listvalue = (ListToken)value;

            if (listvalue.Count <= 1)
                return env.Fetch("nil");

            var rest = new ListToken();
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
    class CONS : FuncToken
    {
        public CONS()
        {
            this.value = this.Append;
        }


        private IToken Append(ListToken list, ScopedEnvironment env)
        {
            if (list.Count != 3)
                throw new ArgumentException("CONS has wrong number of arguments.");

            ListToken exprs = list.CDR();
            IToken expr1 = exprs[0];
            IToken expr2 = exprs[1];

            IToken value1 = Evaluator.Eval(expr1, env);
            IToken value2 = Evaluator.Eval(expr2, env);

            //if (value1 is ListToken && value2 is ListToken)
            //{
            //    ListToken list1 = (ListToken)value1;
            //    ListToken list2 = (ListToken)value2;

            //    list2.InsertRange(0, list2);
            //    return list2;
            //}
            if (value2 is ListToken)
            {
                ListToken list2 = (ListToken)value2;
                list2.Insert(0, value1);
                return list2;
            }
            else if (value1 is ListToken)
            {
                ListToken list1 = (ListToken)value1;
                list1.Add(value2);
                return list1;
            }
            else
            {
                throw new ArgumentException(string.Format("CONS problem with {0} or {1}.", value1.Value, value2.Value));
            }
        }

    }

}
