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

            // Check if empty list or nil.
            // if not either, then it's not null
            if(value.Type == TokenType.LIST)
            {
                ListToken list = (ListToken)value;

                // empty list is nil
                if (list.Count == 0)
                    return env.Fetch("#t");
            }
            else if (value == env.Fetch("nil"))
            {
                return env.Fetch("#t");
            }

            // return false
            return env.Fetch("nil");
        }

        private IToken _checkIfNull(ListToken list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Count; i++)
            {
                IToken value = this._checkIfNull(list[i], env);

                // check if true, otherwise return false
                if (value != env.Fetch("#t"))
                    return env.Fetch("nil");
            }

            // return true
            return env.Fetch("#t");
        }

    }
}
