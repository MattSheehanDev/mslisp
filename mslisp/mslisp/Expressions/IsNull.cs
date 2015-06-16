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
     * ISNULL
     * (null? args)
     */
    class IsNull : SExpression
    {
        // todo: don't actually check if 'null', check if empty list '()'
        public IsNull()
        {
        }


        // null? can go one of three ways.
        // 1. (null?) => T
        // 2. (null? arg) => T if null
        // 3. (null? args...) => T if all are null
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length == 1)
                return Bool.True;
            else if (list.Length == 2)
                return this._checkIfNull(list[1], env);
            else
                return this._checkIfNull(list.CDR(), env);
        }


        private IDatum _checkIfNull(IDatum token, ScopedEnvironment env)
        {
            IDatum value = Evaluator.Eval(token, env);

            // Check if empty list or nil.
            // if not either, then it's not null
            if(value is Vector)
            {
                Vector list = (Vector)value;

                // empty list is nil
                if (list.Length == 0)
                    return Bool.True;
            }
            else if (Null.Instance.Equals(value))
            {
                return Bool.True;
            }

            // return false
            return Null.Instance;
        }

        private IDatum _checkIfNull(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Length; i++)
            {
                IDatum value = this._checkIfNull(list[i], env);

                // check if true, otherwise return false
                if (!Null.Instance.Equals(value))
                    return Null.Instance;
            }

            // return true
            return Bool.True;
        }

    }
}
