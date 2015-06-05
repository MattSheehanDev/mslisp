using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;

namespace mslisp.Functions
{

    class TokenFunction : IToken
    {
        protected TokenType type;
        protected Func<TokenList, ScopedEnvironment, IToken> value;

        public TokenType Type { get { return this.type; } }
        public object Value { get { return this.value; } }

        
        public TokenFunction(Func<TokenList, ScopedEnvironment, IToken> func)
        {
            this.type = TokenType.LAMBDA;
            this.value = func;
        }
        public TokenFunction()
        {
            this.type = TokenType.LAMBDA;
        }


        public IToken Invoke(TokenList list, ScopedEnvironment env)
        {
            return this.value.Invoke(list, env);
        }

        public bool isAtom()
        {
            return false;
        }
    }


    /*
     * ADDITION
     * (+ 1 2 ...)
     */
    class Addition : TokenFunction
    {
        public Addition()
        {
            this.value = this.Add;
        }

        // addition can go one of three ways.
        // 1. (+) => 0
        // 2. (+ number) => number
        // 3. (+ number...) => number1 + ... + numberN
        private IToken Add(TokenList list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return this._Add();
            else if (list.Count == 2)
                return _Add(list[1], env);
            else
                return _Add(list.CDR(), env);
        }

        private IToken _Add()
        {
            return new Token(TokenType.INT, 0);
        }

        private IToken _Add(IToken token, ScopedEnvironment env)
        {
            return Evaluator.Eval(token, env);
        }

        private IToken _Add(TokenList list, ScopedEnvironment env)
        {
            dynamic sum = null;
            TokenType type = TokenType.INT;

            for (var i = 0; i < list.Count; i++)
            {
                IToken num = Evaluator.Eval(list[i], env);

                if (sum != null)
                    sum += (dynamic)num.Value;
                else
                    sum = (dynamic)num.Value;
            }

            if (sum is float || sum is double)
                type = TokenType.DOUBLE;

            return new Token(type, sum);
        }
    }


    /*
     * MULTIPLICATION
     * (* 10 7 ...)
     */
    class Multiplication : TokenFunction
    {
        public Multiplication()
        {
            this.value = this.Multiply;
        }


        // multiplication can go one of three ways
        // 1. (*) => 1
        // 2. (* number) => number
        // 3. (* number...) => number1 * ... * numberN
        private IToken Multiply(TokenList list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return _Multiply();
            else if (list.Count == 1)
                return _Multiply(list[1], env);
            else
                return _Multiply(list.CDR(), env);
        }

        private IToken _Multiply()
        {
            return new Token(TokenType.INT, 1);
        }

        private IToken _Multiply(IToken token, ScopedEnvironment env)
        {
            return Evaluator.Eval(token, env);
        }

        private IToken _Multiply(TokenList list, ScopedEnvironment env)
        {
            dynamic times = null;
            TokenType type = TokenType.INT;

            for (var i = 0; i < list.Count; i++)
            {
                IToken num = Evaluator.Eval(list[i], env);

                if (times != null)
                    times *= (dynamic)num.Value;
                else
                    times = (dynamic)num.Value;
            }

            if (times is float || times is double)
                type = TokenType.DOUBLE;

            return new Token(type, times);
        }

    }


    /*
     * SUBTRACTION
     * (- 10 7)
     */
    class Subtraction : TokenFunction
    {

        public Subtraction()
        {
            this.value = this.Subtract;
        }


        // subtraction can go one of three ways
        // 1. (-) => error
        // 2. (- number) => 0 - number
        // 3. (- number...) => number1 - ... - numberN
        private IToken Subtract(TokenList list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("- is missing argument(s).");
            else if (list.Count == 2)
                return _Subtract(list[1], env);
            else
                return _Subtract(list.CDR(), env);
        }

        private IToken _Subtract(IToken token, ScopedEnvironment env)
        {
            IToken num = Evaluator.Eval(token, env);
            if (num.Type == TokenType.INT)
                return new Token(TokenType.INT, 0 - (int)num.Value);
            return new Token(TokenType.DOUBLE, 0 - (float)num.Value);
        }

        private IToken _Subtract(TokenList list, ScopedEnvironment env)
        {
            dynamic diff = null;
            TokenType type = TokenType.INT;

            for (var i = 0; i < list.Count; i++)
            {
                IToken num = Evaluator.Eval(list[i], env);

                if (diff != null)
                    diff -= (dynamic)num.Value;
                else
                    diff = (dynamic)num.Value;
            }

            if (diff is float || diff is double)
                type = TokenType.DOUBLE;

            return new Token(type, diff);
        }

    }


    /*
     * DIVISION
     * (/ 21 7)
     */
    class Division : TokenFunction
    {
        public Division()
        {
            this.value = this.Divide;
        }
        
        // division can go one of three ways
        // 1. (/) => error
        // 2. (/ number) => 1 / number
        // 3. (/ number...) => number1 / ... / numberN
        private IToken Divide(TokenList list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                throw new ArgumentException("/ is missing argument(s)");
            else if (list.Count == 2)
                return _Divide(list[1], env);
            else
                return _Divide(list.CDR(), env);
        }

        private IToken _Divide(IToken token, ScopedEnvironment env)
        {
            IToken num = Evaluator.Eval(token, env);
            return new Token(TokenType.DOUBLE, 1 / (float)num.Value);
        }

        private IToken _Divide(TokenList list, ScopedEnvironment env)
        {
            dynamic quotient = null;
            TokenType type = TokenType.INT;

            for (var i = 0; i < list.Count; i++)
            {
                IToken num = Evaluator.Eval(list[i], env);

                if (quotient != null)
                    quotient /= (dynamic)num.Value;
                else
                    quotient = (dynamic)num.Value;
            }

            if (quotient is float || quotient is double)
                type = TokenType.DOUBLE;

            return new Token(type, quotient);
        }
    }

}
