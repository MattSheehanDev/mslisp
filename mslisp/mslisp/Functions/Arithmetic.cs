﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;
using mslisp.Tokens;
using mslisp.Lexical;

namespace mslisp.Functions
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

        // addition can go one of three ways.
        // 1. (+) => 0
        // 2. (+ number) => number
        // 3. (+ number...) => number1 + ... + numberN
        private IDatum Add(Vector list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return this._Add();
            else if (list.Count == 2)
                return _Add(list[1], env);
            else
                return _Add(list.CDR(), env);
        }

        private IDatum _Add()
        {
            return new Datum(DatumType.INT, 0);
        }

        private IDatum _Add(IDatum token, ScopedEnvironment env)
        {
            return Evaluator.Eval(token, env);
        }

        private IDatum _Add(Vector list, ScopedEnvironment env)
        {
            double sum = 0;
            bool init = true;
            
            for (var i = 0; i < list.Count; i++)
            {
                IDatum num = Evaluator.Eval(list[i], env);

                if (init)
                {
                    sum = Convert.ToDouble(num.Value);
                    init = false;
                }
                else
                {
                    sum += Convert.ToDouble(num.Value);
                }
            }

            return Parser.toNumber(sum);
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


        // multiplication can go one of three ways
        // 1. (*) => 1
        // 2. (* number) => number
        // 3. (* number...) => number1 * ... * numberN
        private IDatum Multiply(Vector list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return _Multiply();
            else if (list.Count == 1)
                return _Multiply(list[1], env);
            else
                return _Multiply(list.CDR(), env);
        }

        private IDatum _Multiply()
        {
            return new Datum(DatumType.INT, 1);
        }

        private IDatum _Multiply(IDatum token, ScopedEnvironment env)
        {
            return Evaluator.Eval(token, env);
        }

        private IDatum _Multiply(Vector list, ScopedEnvironment env)
        {
            double times = 0;
            bool init = true;

            for (var i = 0; i < list.Count; i++)
            {
                IDatum num = Evaluator.Eval(list[i], env);

                if(init)
                {
                    times = Convert.ToDouble(num.Value);
                    init = false;
                }
                else
                {
                    times *= Convert.ToDouble(num.Value);
                }
            }

            return Parser.toNumber(times);
        }

    }


    /*
     * SUBTRACTION
     * (- 10 7)
     */
    class Subtraction : SExpression
    {

        public Subtraction()
        {
            this.value = this.Subtract;
        }


        // subtraction can go one of three ways
        // 1. (-) => error
        // 2. (- number) => 0 - number
        // 3. (- number...) => number1 - ... - numberN
        private IDatum Subtract(Vector list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("- is missing argument(s).");
            else if (list.Count == 2)
                return _Subtract(list[1], env);
            else
                return _Subtract(list.CDR(), env);
        }

        private IDatum _Subtract(IDatum token, ScopedEnvironment env)
        {
            IDatum num = Evaluator.Eval(token, env);
            return Parser.toNumber(0 - Convert.ToDouble(num.Value));
        }

        private IDatum _Subtract(Vector list, ScopedEnvironment env)
        {
            double diff = 0;
            bool init = true;

            for (var i = 0; i < list.Count; i++)
            {
                IDatum num = Evaluator.Eval(list[i], env);

                if (init)
                {
                    diff = Convert.ToDouble(num.Value);
                    init = false;
                }
                else
                {
                    diff -= Convert.ToDouble(num.Value);
                }
            }

            return Parser.toNumber(diff);
        }

    }


    /*
     * DIVISION
     * (/ 21 7)
     */
    class Division : SExpression
    {
        public Division()
        {
            this.value = this.Divide;
        }
        
        // division can go one of three ways
        // 1. (/) => error
        // 2. (/ number) => 1 / number
        // 3. (/ number...) => number1 / ... / numberN
        private IDatum Divide(Vector list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("/ is missing argument(s)");
            else if (list.Count == 2)
                return _Divide(list[1], env);
            else
                return _Divide(list.CDR(), env);
        }

        private IDatum _Divide(IDatum token, ScopedEnvironment env)
        {
            IDatum num = Evaluator.Eval(token, env);
            return Parser.toNumber(1 / Convert.ToDouble(num.Value));
        }

        private IDatum _Divide(Vector list, ScopedEnvironment env)
        {
            double quotient = 0;
            bool init = true;

            for (var i = 0; i < list.Count; i++)
            {
                IDatum num = Evaluator.Eval(list[i], env);

                if (num.Type == DatumType.DOUBLE)
                    type = DatumType.DOUBLE;
                
                if (init)
                {
                    quotient = Convert.ToDouble(num.Value);
                    init = false;
                }
                else
                {
                    quotient /= Convert.ToDouble(num.Value);
                }
            }

            return Parser.toNumber(quotient);
        }
    }

}
