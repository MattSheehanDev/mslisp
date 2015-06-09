﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Functions;
using mslisp.Tokens;


namespace mslisp.Environment
{
    class GlobalEnvironment : ScopedEnvironment
    {

        public GlobalEnvironment()
        {
            // global variables
            this.Add("*prompt*", new Token(TokenType.STRING, "mslisp"));

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
