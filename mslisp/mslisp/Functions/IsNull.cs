using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;
using mslisp.Environment;

namespace mslisp.Functions
{
    /*
     * ISNULL
     * (null? args)
     */
    class IsNull : FuncToken
    {
        // todo: don't actually check if 'null', check if empty list '()'
        public IsNull()
        {
            this.value = this.checkIfNull;
        }


        // null? can go one of three ways.
        // 1. (null?) => T
        // 2. (null? arg) => T if null
        // 3. (null? args...) => T if all are null
        private IToken checkIfNull(ListToken list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return new Token(TokenType.BOOLEAN, true);
            else if (list.Count == 2)
                return this._checkIfNull(list[1], env);
            else
                return this._checkIfNull(list.CDR(), env);
        }


        private IToken _checkIfNull(IToken token, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(token, env);
            return new Token(TokenType.BOOLEAN, (value.Value == null));
        }

        private IToken _checkIfNull(ListToken list, ScopedEnvironment env)
        {
            bool nil = true;

            for (var i = 0; i < list.Count; i++)
            {
                IToken curr = Evaluator.Eval(list[i], env);

                if(curr.Value != null)
                {
                    nil = false;
                    break;
                }
            }

            return new Token(TokenType.BOOLEAN, nil);
        }

    }
}
