using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Expressions;
using mslisp.Datums;

namespace mslisp.Environment
{

    class New : SExpression
    {
        public New()
        {
            this.value = this.createNew;
        }

        private IDatum createNew(Vector list, ScopedEnvironment env)
        {
            if (list.Length <= 1)
                throw new ArgumentException("NEW is missing arguments.");
            
            var type = System.Type.GetType(list[1].Value.ToString());
            var args = new List<object>();
            for(var i = 2; i < list.Length; i++)
            {
                args.Add(list[i].Value);
            }
            var instance = Activator.CreateInstance(type, args.ToArray());

            // arbitrary type for now.
            return new Atom(DatumType.SYMBOL, instance);
        }
    }


    class GlobalEnvironment : ScopedEnvironment
    {

        public GlobalEnvironment()
        {
            // global variables
            this.Add("*prompt*", new Atom(DatumType.STRING, "mslisp"));

            this.Add("new", new New());

            this.Add("#t", new Bool(true));
            this.Add("nil", new Null());

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

            this.Add("load", new Load());

            // these can be implemented in lisp.
            // also to be implemented in lisp
            // => cadr
            // => append
            // => not
            this.Add("atom?", new IsAtom());
            this.Add("null?", new IsNull());
        }

    }

}
