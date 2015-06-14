﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;
using mslisp.Datums;
using mslisp.Lexical;

namespace mslisp.Expressions
{
    /*
     * ADDITION
     * (+ 1 2 ...)
     */
    class Addition : SExpression
    {
        public Addition()
        {
            this.value = this.Add;
        }

        // addition can go one of two ways.
        // 1. (+) => 0
        // 2. (+ number...) => number1 + ... + numberN
        private IDatum Add(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                return this._Add();
            else
                return _Add(list.CDR(), env);
        }

        private IDatum _Add()
        {
            return new Number(0);
        }

        private IDatum _Add(Vector list, ScopedEnvironment env)
        {
            Number sum = null;

            for (var i = 0; i < list.Length; i++)
            {
                Number num = Evaluator.Eval(list[i], env) as Number;

                if (num == null)
                    throw new TypeException("+ expected numbers.");
                
                sum = sum == null ? num : sum + num;
            }
            return sum;
        }
    }

    
    /*
     * MULTIPLICATION
     * (* 10 7 ...)
     */
    class Multiplication : SExpression
    {
        public Multiplication()
        {
            this.value = this.Multiply;
        }


        // multiplication can go one of two ways
        // 1. (*) => 1
        // 2. (* number...) => number1 * ... * numberN
        private IDatum Multiply(Vector list, ScopedEnvironment env)
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
    class Subtraction : SExpression
    {
        private readonly Number zero;

        public Subtraction()
        {
            this.value = this.Subtract;
            this.zero = new Number(0);
        }


        // subtraction can go one of three ways
        // 1. (-) => error
        // 2. (- number) => 0 - number
        // 3. (- number...) => number1 - ... - numberN
        private IDatum Subtract(Vector list, ScopedEnvironment env)
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
    class Division : SExpression
    {
        private readonly Number one;


        public Division()
        {
            this.value = this.Divide;
            this.one = new Number(1);
        }
        
        // division can go one of three ways
        // 1. (/) => error
        // 2. (/ number) => 1 / number
        // 3. (/ number...) => number1 / ... / numberN
        private IDatum Divide(Vector list, ScopedEnvironment env)
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
