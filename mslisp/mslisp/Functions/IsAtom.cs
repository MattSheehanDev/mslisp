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
    class IsAtom : SExpression
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
        private IDatum checkIfAtom(Vector list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return new Datum(DatumType.BOOLEAN, false);
            else if (list.Count == 2)
                return this._checkIfAtom(list[1], env);
            else
                return this._checkIfAtom(list.CDR(), env);
        }

        private IDatum _checkIfAtom(IDatum token, ScopedEnvironment env)
        {
            IDatum value = Evaluator.Eval(token, env);
            if (Datum.isAtom(value))
                return env.Fetch("#t");
            return env.Fetch("nil");
        }

        private IDatum _checkIfAtom(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Count; i++)
            {
                IDatum token = Evaluator.Eval(list[i], env);

                if (!Datum.isAtom(token))
                    return env.Fetch("nil");
            }

            return env.Fetch("#t");
        }

    }
}
