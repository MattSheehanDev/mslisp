using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;
using mslisp.Environment;
using mslisp.Lexical;

namespace mslisp.Functions
{
    /*
     * GREATER THAN
     * (> 1 2 ...)
     */
    class GreaterThan : FuncToken
    {
        public GreaterThan()
        {
            this.value = this.Greater;
        }

        // greater than an go one of three ways
        // 1. (>) => error
        // 2. (> number) => T
        // 3. (> number...) => T if all pair comparisons, from left to right, are T
        private IToken Greater(ListToken list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("> is missing some arguments.");
            else if (list.Count == 2)
                return this._Greater(list[1], env);
            else
                return this._Greater(list.CDR(), env);
        }


        private IToken _Greater(IToken token, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(token, env);

            // single argument always returns true
            if (Token.isNumber(token))
                return new Token(TokenType.BOOLEAN, true);

            throw new TypeException("> cannot compare non-numerical types.");
        }

        private IToken _Greater(ListToken list, ScopedEnvironment env)
        {
            bool greater = true;

            for (var i = 0; i < list.Count - 1; i++)
            {
                IToken curr = Evaluator.Eval(list[i], env);
                IToken next = Evaluator.Eval(list[i + 1], env);

                if (!Token.isNumber(curr) || !Token.isNumber(next))
                    throw new TypeException("> cannot compare non-numerical types.");
                
                var currValue = Convert.ToDouble(curr.Value);
                var nextValue = Convert.ToDouble(next.Value);

                if (currValue <= nextValue)
                {
                    greater = false;
                    break;
                }
            }

            return new Token(TokenType.BOOLEAN, greater);
        }

    }


    /*
     * LESS THAN
     * (< 1 2 ...)
     */
    class LessThan : FuncToken
    {
        public LessThan()
        {
            this.value = this.Less;
        }


        // less than can go one of three ways.
        // 1. (<) => error
        // 2. (< number) => T
        // 3. (< numbers...) => T if all pair comparisons, from left to right, are T
        private IToken Less(ListToken list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("< is missing some arguments.");
            else if (list.Count == 2)
                return this._Less(list[1], env);
            else
                return this._Less(list.CDR(), env);
        }

        private IToken _Less(IToken token, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(token, env);

            if (Token.isNumber(token))
                return new Token(TokenType.BOOLEAN, true);

            throw new TypeException("< cannot compare non-numerical types.");
        }

        private IToken _Less(ListToken list, ScopedEnvironment env)
        {
            bool less = true;

            for (var i = 0; i < list.Count - 1; i++)
            {
                IToken curr = Evaluator.Eval(list[i], env);
                IToken next = Evaluator.Eval(list[i + 1], env);

                if (!Token.isNumber(curr) || !Token.isNumber(next))
                    throw new TypeException("< cannot compare non-numerical types.");

                var currValue = Convert.ToDouble(curr.Value);
                var nextValue = Convert.ToDouble(next.Value);

                if (currValue >= nextValue)
                {
                    less = false;
                    break;
                }
            }

            return new Token(TokenType.BOOLEAN, less);
        }

    }


    /*
     * GREATER THAN OR EQUAL TO
     * (>= 1 2 ...)
     */
    class NotLessThan : FuncToken
    {
        // class is called NotLessThan because it's shorter than GreaterThanOrEqualTo
        public NotLessThan()
        {
            this.value = this.Greater;
        }


        // >= can go one of three ways.
        // 1. (>=) => error
        // 2. (>= number) => T
        private IToken Greater(ListToken list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException(">= is missing some arguments.");
            else if (list.Count == 2)
                return this._Greater(list[1], env);
            else
                return this._Greater(list.CDR(), env);
        }


        private IToken _Greater(IToken token, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(token, env);

            if (Token.isNumber(token))
                return new Token(TokenType.BOOLEAN, true);

            throw new TypeException(">= cannot compare non-numerical types.");
        }

        private IToken _Greater(ListToken list, ScopedEnvironment env)
        {
            bool less = true;

            for (var i = 0; i < list.Count - 1; i++)
            {
                IToken curr = Evaluator.Eval(list[i], env);
                IToken next = Evaluator.Eval(list[i + 1], env);

                if (!Token.isNumber(curr) || !Token.isNumber(next))
                    throw new TypeException(">= cannot compare non-numerical types.");

                var currValue = Convert.ToDouble(curr.Value);
                var nextValue = Convert.ToDouble(next.Value);

                if (currValue < nextValue)
                {
                    less = false;
                    break;
                }
            }

            return new Token(TokenType.BOOLEAN, less);
        }

    }


    /*
     * LESS THAN OR EQUAL TO
     * (<= 1 2 ...)
     */
     class NotGreaterThan : FuncToken
    {
        public NotGreaterThan()
        {
            this.value = this.Less;
        }


        // <= can go one of three ways
        // 1. (<=) => error
        // 2. (<= number) => T
        // 3. (<= numbers...) => T if all pairs, from left to right, are T
        private IToken Less(ListToken list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("<= is missing some arguments.");
            else if (list.Count == 2)
                return this._Less(list[1], env);
            else
                return this._Less(list.CDR(), env);
        }


        private IToken _Less(IToken token, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(token, env);

            if (Token.isNumber(token))
                return new Token(TokenType.BOOLEAN, true);

            throw new TypeException("<= cannot compare non-numerical types.");
        }

        private IToken _Less(ListToken list, ScopedEnvironment env)
        {
            bool greater = true;

            for (var i = 0; i < list.Count - 1; i++)
            {
                IToken curr = Evaluator.Eval(list[i], env);
                IToken next = Evaluator.Eval(list[i + 1], env);

                if (!Token.isNumber(curr) || !Token.isNumber(next))
                    throw new TypeException("<= cannot compare non-numerical types.");

                var currValue = Convert.ToDouble(curr.Value);
                var nextValue = Convert.ToDouble(next.Value);

                if(currValue > nextValue)
                {
                    greater = false;
                    break;
                }
            }

            return new Token(TokenType.BOOLEAN, greater);
        }

    }


    /*
     * EQUALS
     * (= 2 2 ...)
     */
    class Equals : FuncToken
    {
        public Equals()
        {
            this.value = this.IsEquals;
        }


        // = can go one of three ways
        // 1. (=) => error
        // 2. (= number) => T
        // 3. (= number...) => T if all pairs, from left to right, are T
        public IToken IsEquals(ListToken list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("= is missing some arguments.");
            else if (list.Count == 2)
                return this._IsEquals(list[1], env);
            else
                return this._IsEquals(list.CDR(), env);
        }


        private IToken _IsEquals(IToken token, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(token, env);

            if (Token.isNumber(token))
                return new Token(TokenType.BOOLEAN, true);

            throw new TypeException("= cannot compare non-numerical types.");
        }

        private IToken _IsEquals(ListToken list, ScopedEnvironment env)
        {
            var equals = true;

            for (var i = 0; i < list.Count - 1; i++)
            {
                IToken curr = Evaluator.Eval(list[i], env);
                IToken next = Evaluator.Eval(list[i + 1], env);

                if (!Token.isNumber(curr) || !Token.isNumber(next))
                    throw new TypeException("= cannot compare non-numerical types.");

                var currValue = Convert.ToDouble(curr.Value);
                var nextValue = Convert.ToDouble(curr.Value);

                if(currValue != nextValue)
                {
                    equals = false;
                    break;
                }
            }

            return new Token(TokenType.BOOLEAN, equals);
        }

    }

}