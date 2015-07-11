using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Datums;
using MsLisp.Environment;
using MsLisp.Lexical;

namespace MsLisp.Expressions
{
    /*
     * GREATER THAN
     * (greater number number)
     */
    public class GreaterThan : SExpression
    {
        public GreaterThan()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("GREATER takes exactly 2 arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            if (first > second)
                return Bool.True;
            else
                return Null.Instance;
        }

    }


    /*
     * LESS THAN
     * (lesser number number)
     */
    public class LessThan : SExpression
    {
        public LessThan()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("LESSER takes exactly two arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            if (first < second)
                return Bool.True;
            else
                return Null.Instance;
        }

    }


    /*
     * GREATER THAN OR EQUAL TO
     * (not-lesser number number)
     */
    public class NotLessThan : SExpression
    {
        // class is called NotLessThan because it's shorter than GreaterThanOrEqualTo
        public NotLessThan()
        {
        }

        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("NOT-LESSER takes exactly 2 arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            if (first >= second)
                return Bool.True;
            else
                return Null.Instance;
        }

    }


    /*
     * LESS THAN OR EQUAL TO
     * (not-greater number number)
     */
    public class NotGreaterThan : SExpression
    {
        public NotGreaterThan()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("NOT-GREATER takes exactly 2 arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            if (first <= second)
                return Bool.True;
            else
                return Null.Instance;
        }

    }


    /*
     * EQUALS
     * (equal number number)
     */
    public class Equal : SExpression
    {
        public Equal()
        {
        }

        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("EQUAl takes exactly two arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            if (first.Equals(second))
                return Bool.True;
            else
                return Null.Instance;
        }

    }

}