using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;

namespace mslisp.Functions
{

    /*
     * CAR
     * (car expression) => first
     */
    class CAR : TokenFunction
    {
        public CAR()
        {
            this.value = this.First;
        }


        private IToken First(TokenList list, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(list[1], env);

            if (!(value is TokenList))
                throw new SyntaxException(string.Format("{0} is not a valid list.", value.Value));

            TokenList listvalue = (TokenList)value;

            if (listvalue.Count == 0)
                return null;

            return listvalue[0];
        }
    }
    

    /*
     * CDR
     * (cdr expression) => rest of list
     */
    class CDR : TokenFunction
    {
        public CDR()
        {
            this.value = this.Rest;
        }


        private TokenList Rest(TokenList list, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(list[1], env);

            if (!(value is TokenList))
                throw new SyntaxException(string.Format("{0} is not a valid list.", value.Value));

            TokenList listvalue = (TokenList)value;

            if (listvalue.Count <= 1)
                return null;

            var rest = new TokenList();
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
    class CONS : TokenFunction
    {
        public CONS()
        {
            this.value = this.Append;
        }


        private IToken Append(TokenList list, ScopedEnvironment env)
        {
            if (list.Count != 3)
                throw new ArgumentException("CONS has wrong number of arguments.");

            TokenList exprs = list.CDR();
            IToken expr1 = exprs[0];
            IToken expr2 = exprs[1];

            IToken value1 = Evaluator.Eval(expr1, env);
            IToken value2 = Evaluator.Eval(expr2, env);

            if (value1 is TokenList && value2 is TokenList)
            {
                TokenList list1 = (TokenList)value1;
                TokenList list2 = (TokenList)value2;

                list2.InsertRange(0, list2);
                return list2;
            }
            else if (value2 is TokenList)
            {
                TokenList list2 = (TokenList)value2;
                list2.Insert(0, value1);
                return list2;
            }
            else if (value1 is TokenList)
            {
                TokenList list1 = (TokenList)value1;
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
