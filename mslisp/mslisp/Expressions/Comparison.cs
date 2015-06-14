using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Datums;
using mslisp.Environment;
using mslisp.Lexical;

namespace mslisp.Expressions
{
    /*
     * GREATER THAN
     * (> 1 2 ...)
     */
    class GreaterThan : SExpression
    {
        public GreaterThan()
        {
            this.value = this.Greater;
        }

        // greater than an go one of three ways
        // 1. (>) => error
        // 2. (> number) => T
        // 3. (> number...) => T if all pair comparisons, from left to right, are T
        private IDatum Greater(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                throw new ArgumentException("> is missing some arguments.");
            else if (list.Length == 2)
                return this._Greater(list[1], env);
            else
                return this._Greater(list.CDR(), env);
        }


        private IDatum _Greater(IDatum token, ScopedEnvironment env)
        {
            Number value = Evaluator.Eval(token, env) as Number;

            // single argument always returns true
            if (value != null)
                return env.Fetch("#t");

            throw new TypeException("> cannot compare non-numerical types.");
        }

        private IDatum _Greater(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length - 1; i++)
            {
                // use 'as' instead of a cast.
                // if 'as' fails, no exception is thrown, null is returned intead.
                Number curr = Evaluator.Eval(list[i], env) as Number;
                Number next = Evaluator.Eval(list[i + 1], env) as Number;

                if (curr == null || next == null)
                    throw new TypeException("> expected numbers.");

                if (curr <= next)
                    return env.Fetch("nil");
            }

            return env.Fetch("#t");
        }

    }


    /*
     * LESS THAN
     * (< 1 2 ...)
     */
    class LessThan : SExpression
    {
        public LessThan()
        {
            this.value = this.Less;
        }


        // less than can go one of three ways.
        // 1. (<) => error
        // 2. (< number) => T
        // 3. (< numbers...) => T if all pair comparisons, from left to right, are T
        private IDatum Less(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                throw new ArgumentException("< is missing some arguments.");
            else if (list.Length == 2)
                return this._Less(list[1], env);
            else
                return this._Less(list.CDR(), env);
        }

        private IDatum _Less(IDatum token, ScopedEnvironment env)
        {
            Number value = Evaluator.Eval(token, env) as Number;

            if (value != null)
                return env.Fetch("#t");

            throw new TypeException("< cannot compare non-numerical types.");
        }

        private IDatum _Less(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length - 1; i++)
            {
                Number curr = Evaluator.Eval(list[i], env) as Number;
                Number next = Evaluator.Eval(list[i + 1], env) as Number;

                if (curr == null || next == null)
                    throw new TypeException("< expected numbers.");

                if (curr >= next)
                    return env.Fetch("nil");
            }

            // return true;
            return env.Fetch("#t");
        }

    }


    /*
     * GREATER THAN OR EQUAL TO
     * (>= 1 2 ...)
     */
    class NotLessThan : SExpression
    {
        // class is called NotLessThan because it's shorter than GreaterThanOrEqualTo
        public NotLessThan()
        {
            this.value = this.Greater;
        }


        // >= can go one of three ways.
        // 1. (>=) => error
        // 2. (>= number) => T
        private IDatum Greater(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                throw new ArgumentException(">= is missing some arguments.");
            else if (list.Length == 2)
                return this._Greater(list[1], env);
            else
                return this._Greater(list.CDR(), env);
        }


        private IDatum _Greater(IDatum token, ScopedEnvironment env)
        {
            Number value = Evaluator.Eval(token, env) as Number;

            if (value != null)
                return env.Fetch("#t");

            throw new TypeException(">= cannot compare non-numerical types.");
        }

        private IDatum _Greater(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length - 1; i++)
            {
                Number curr = Evaluator.Eval(list[i], env) as Number;
                Number next = Evaluator.Eval(list[i + 1], env) as Number;

                if (curr == null || next == null)
                    throw new TypeException(">= expected numbers.");

                if (curr < next)
                    return env.Fetch("nil");
            }

            return env.Fetch("#t");
        }

    }


    /*
     * LESS THAN OR EQUAL TO
     * (<= 1 2 ...)
     */
     class NotGreaterThan : SExpression
    {
        public NotGreaterThan()
        {
            this.value = this.Less;
        }


        // <= can go one of three ways
        // 1. (<=) => error
        // 2. (<= number) => T
        // 3. (<= numbers...) => T if all pairs, from left to right, are T
        private IDatum Less(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                throw new ArgumentException("<= is missing some arguments.");
            else if (list.Length == 2)
                return this._Less(list[1], env);
            else
                return this._Less(list.CDR(), env);
        }


        private IDatum _Less(IDatum token, ScopedEnvironment env)
        {
            Number value = Evaluator.Eval(token, env) as Number;

            // a comparison of one number always returns true.
            if (value != null)
                return env.Fetch("#t");

            throw new TypeException("<= cannot compare non-numerical types.");
        }

        private IDatum _Less(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length - 1; i++)
            {
                Number curr = Evaluator.Eval(list[i], env) as Number;
                Number next = Evaluator.Eval(list[i + 1], env) as Number;

                if (curr == null || next == null)
                    throw new TypeException("<= expected numbers.");

                if (curr > next)
                    return env.Fetch("nil");
            }

            return env.Fetch("#t");
        }

    }


    /*
     * EQUALS
     * (= 2 2 ...)
     */
    class Equals : SExpression
    {
        public Equals()
        {
            this.value = this.IsEquals;
        }


        // = can go one of three ways
        // 1. (=) => error
        // 2. (= number) => T
        // 3. (= number...) => T if all pairs, from left to right, are T
        public IDatum IsEquals(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                throw new ArgumentException("= is missing some arguments.");
            else if (list.Length == 2)
                return this._IsEquals(list[1], env);
            else
                return this._IsEquals(list.CDR(), env);
        }


        private IDatum _IsEquals(IDatum token, ScopedEnvironment env)
        {
            Number value = Evaluator.Eval(token, env) as Number;

            if (value != null)
                return env.Fetch("#t");

            throw new TypeException("= cannot compare non-numerical types.");
        }

        private IDatum _IsEquals(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length - 1; i++)
            {
                IDatum curr = Evaluator.Eval(list[i], env);
                IDatum next = Evaluator.Eval(list[i + 1], env);

                if (curr.GetType() != typeof(Atom) || next.GetType() != typeof(Atom))
                    return env.Fetch("nil");

                var cAtom = (Atom)curr;
                var nAtom = (Atom)next;

                if (cAtom != nAtom)
                    return env.Fetch("nil");

                //if (!Atom.isNumber(curr) || !Atom.isNumber(next))
                //    throw new TypeException("= cannot compare non-numerical types.");

                //var currValue = Convert.ToDouble(curr.Value);
                //var nextValue = Convert.ToDouble(next.Value);

                //if (currValue != nextValue)
                //    return env.Fetch("nil");
            }

            return env.Fetch("#t");
        }

    }

}