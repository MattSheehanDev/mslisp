using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Environment;
using MsLisp.Datums;
using MsLisp.Lexical;

namespace MsLisp.Expressions
{
    /*
     * ADDITION
     * (+ number number)
     */
    public class Addition : SExpression
    {
        public Addition()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 3)
                throw new ArgumentException("ADD takes two arguments.");

            Number first = Evaluator.Eval(list[1], env) as Number;
            Number second = Evaluator.Eval(list[2], env) as Number;

            return first + second;
        }

    }


    /*
     * MULTIPLICATION
     * (* 10 7 ...)
     */
    public class Multiplication : SExpression
    {
        public Multiplication()
        {
        }


        // multiplication can go one of two ways
        // 1. (*) => 1
        // 2. (* number...) => number1 * ... * numberN
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                return _Multiply();
            else
                return _Multiply(list.CDR(), env);
        }

        private IDatum _Multiply()
        {
            return new Number(1);
        }

        private IDatum _Multiply(Vector list, ScopedEnvironment env)
        {
            Number times = null;

            for (var i = 0; i < list.Length; i++)
            {
                Number num = Evaluator.Eval(list[i], env) as Number;

                if (num == null)
                    throw new TypeException("* expected numbers.");

                times = times == null ? num : times * num;
            }
            return times;
        }

    }


    /*
     * SUBTRACTION
     * (- 10 7)
     */
    public class Subtraction : SExpression
    {
        private readonly Number zero;

        public Subtraction()
        {
            this.zero = new Number(0);
        }


        // subtraction can go one of three ways
        // 1. (-) => error
        // 2. (- number) => 0 - number
        // 3. (- number...) => number1 - ... - numberN
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                throw new ArgumentException("- is missing argument(s).");
            else if (list.Length == 2)
                return _Subtract(list[1], env);
            else
                return _Subtract(list.CDR(), env);
        }

        private IDatum _Subtract(IDatum token, ScopedEnvironment env)
        {
            Number num = Evaluator.Eval(token, env) as Number;
            return zero - num;
        }

        private IDatum _Subtract(Vector list, ScopedEnvironment env)
        {
            Number diff = null;

            for (var i = 0; i < list.Length; i++)
            {
                Number num = Evaluator.Eval(list[i], env) as Number;

                if (num == null)
                    throw new TypeException("- expected numbers.");

                diff = diff == null ? num : diff - num;
            }
            return diff;
        }

    }


    /*
     * DIVISION
     * (/ 21 7)
     */
    public class Division : SExpression
    {
        private readonly Number one;


        public Division()
        {
            this.one = new Number(1);
        }
        
        // division can go one of three ways
        // 1. (/) => error
        // 2. (/ number) => 1 / number
        // 3. (/ number...) => number1 / ... / numberN
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                throw new ArgumentException("/ is missing argument(s)");
            else if (list.Length == 2)
                return _Divide(list[1], env);
            else
                return _Divide(list.CDR(), env);
        }

        private IDatum _Divide(IDatum token, ScopedEnvironment env)
        {
            Number num = Evaluator.Eval(token, env) as Number;
            return one / num;
        }

        private IDatum _Divide(Vector list, ScopedEnvironment env)
        {
            Number quotient = null;

            for (var i = 0; i < list.Length; i++)
            {
                Number num = Evaluator.Eval(list[i], env) as Number;

                if (num == null)
                    throw new TypeException("/ expected numbers.");

                quotient = quotient == null ? num : quotient / num;
            }
            return quotient;
        }
    }

}
