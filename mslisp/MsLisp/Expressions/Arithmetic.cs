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
     * (add number number)
     */
    public class Addition : SExpression
    {
        public Addition()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("ADD takes two arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            return first + second;
        }

    }


    /*
     * MULTIPLICATION
     * (multiply number number)
     */
    public class Multiplication : SExpression
    {
        public Multiplication()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("MULTIPLY takes exactly 2 arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            return first * second;
        }

    }


    /*
     * SUBTRACTION
     * (subtract number number)
     */
    public class Subtraction : SExpression
    {

        public Subtraction()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("SUBTRACT takes exactly 2 arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            var diff = first - second;
            return diff;
        }

    }


    /*
     * DIVISION
     * (divide number number)
     */
    public class Division : SExpression
    {
        
        public Division()
        {
        }
        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length != 2)
                throw new ArgumentException("DIVIDE takes exactly 2 arguments.");

            Number first = Evaluator.Eval(list[0], env) as Number;
            Number second = Evaluator.Eval(list[1], env) as Number;

            return first / second;
        }
    }

}
