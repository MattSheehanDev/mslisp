using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Datums;
using MsLisp.Environment;

namespace MsLisp.Expressions
{
    /*
     * ISATOM
     * (atom? args)
     */
    class IsAtom : SExpression
    {
        // todo: check atoms by !list
        public IsAtom()
        {
        }


        // isatom? can go one of three ways.
        // 1. (atom?) => false
        // 2. (atom? arg) => T if atom
        // 3. (atom? args...) => T if all atoms
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list == null)
                return Bool.True;
            else if (list.Length == 1)
                return this._checkIfAtom(list.CAR(), env);
            else
                return this._checkIfAtom(list, env);
        }

        private IDatum _checkIfAtom(IDatum token, ScopedEnvironment env)
        {
            IDatum value = Evaluator.Eval(token, env);
            if (this.isAtom(value))
                return Bool.True;
            return Null.Instance;
        }

        private IDatum _checkIfAtom(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length; i++)
            {
                IDatum token = Evaluator.Eval(list[i], env);

                if (!this.isAtom(token))
                    return Null.Instance;
            }

            return Bool.True;
        }


        private bool isAtom(IDatum data)
        {
            // everything that's not a list is an atom
            if (data is Vector && !(data is Null))
                return false;
            return true;
        }

    }
}
