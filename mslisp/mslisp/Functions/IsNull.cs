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
    class IsNull : SExpression
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
        private IDatum checkIfNull(Vector list, ScopedEnvironment env)
        {
            if (list.Count == 1)
                return new Datum(DatumType.BOOLEAN, true);
            else if (list.Count == 2)
                return this._checkIfNull(list[1], env);
            else
                return this._checkIfNull(list.CDR(), env);
        }


        private IDatum _checkIfNull(IDatum token, ScopedEnvironment env)
        {
            IDatum value = Evaluator.Eval(token, env);

            // Check if empty list or nil.
            // if not either, then it's not null
            if(value.Type == DatumType.LIST)
            {
                Vector list = (Vector)value;

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

        private IDatum _checkIfNull(Vector list, ScopedEnvironment env)
        {
            for (var i = 0; i < list.Count; i++)
            {
                IDatum value = this._checkIfNull(list[i], env);

                // check if true, otherwise return false
                if (value != env.Fetch("#t"))
                    return env.Fetch("nil");
            }

            // return true
            return env.Fetch("#t");
        }

    }
}
