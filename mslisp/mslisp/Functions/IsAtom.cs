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
     * ISATOM
     * (atom? args)
     */
    class IsAtom : FuncToken
    {
        // todo: check atoms by !list
        public IsAtom()
        {
            this.value = this.checkIfAtom;
        }


        // isatom? can go one of three ways.
        // 1. (atom?) => false
        // 2. (atom? arg) => T if atom
        // 3. (atom? args...) => T if all atoms
        private IToken checkIfAtom(ListToken list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return new Token(TokenType.BOOLEAN, false);
            else if (list.Count == 2)
                return this._checkIfAtom(list[1], env);
            else
                return this._checkIfAtom(list.CDR(), env);
        }

        private IToken _checkIfAtom(IToken token, ScopedEnvironment env)
        {
            IToken value = Evaluator.Eval(token, env);
            if (Token.isAtom(value))
                return env.Fetch("#t");
            return env.Fetch("nil");
        }

        private IToken _checkIfAtom(ListToken list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Count; i++)
            {
                IToken token = Evaluator.Eval(list[i], env);

                if (!Token.isAtom(token))
                    return env.Fetch("nil");
            }

            return env.Fetch("#t");
        }

    }
}
