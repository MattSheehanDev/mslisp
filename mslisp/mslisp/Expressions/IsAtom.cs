using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Datums;
using mslisp.Environment;

namespace mslisp.Expressions
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
            if (list.Length == 1)
                return new Atom(DatumType.BOOLEAN, false);
            else if (list.Length == 2)
                return this._checkIfAtom(list[1], env);
            else
                return this._checkIfAtom(list.CDR(), env);
        }

        private IDatum _checkIfAtom(IDatum token, ScopedEnvironment env)
        {
            IDatum value = Evaluator.Eval(token, env);
            if (this.isAtom(value))
                return env.Fetch("#t");
            return env.Fetch("nil");
        }

        private IDatum _checkIfAtom(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length; i++)
            {
                IDatum token = Evaluator.Eval(list[i], env);

                if (!this.isAtom(token))
                    return env.Fetch("nil");
            }

            return env.Fetch("#t");
        }


        private bool isAtom(IDatum data)
        {
            return data is Atom;
        }

    }
}
