using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Expressions;
using MsLisp.Datums;
using MsLisp.DotNet;

namespace MsLisp.Environment
{
    
    public class GlobalEnvironment : ScopedEnvironment
    {

        public GlobalEnvironment()
        {
            // global variables
            this.Add("*prompt*", new Atom("mslisp"));

            this.Add("new", new New());
            this.Add("get-type", new GetType());
            this.Add("invoke-static", new InvokeStatic());
            this.Add("invoke-method", new InvokeMethod());

            this.Add("#t", Bool.True);
            this.Add("nil", Null.Instance);

            // global procedures
            this.Add("+", new Addition());
            this.Add("*", new Multiplication());
            this.Add("-", new Subtraction());
            this.Add("/", new Division());
            this.Add(">", new GreaterThan());
            this.Add("<", new LessThan());
            this.Add(">=", new NotLessThan());
            this.Add("<=", new NotGreaterThan());
            this.Add("=", new Equals());

            this.Add("car", new CAR());
            this.Add("cdr", new CDR());
            this.Add("cons", new CONS());
            this.Add("define", new Define());
            this.Add("set!", new Set());
            this.Add("equals?", new IsEqual());
            this.Add("if", new IfElse());
            this.Add("cond", new Conditions());
            this.Add("begin", new Begin());
            
            this.Add("quote", new Quote());
            this.Add("lambda", new Lambda());

            this.Add("atom?", new IsAtom());
        }

    }

}
