using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;

namespace mslisp.Datums
{
    /*
     * Lambda's defined during runtime, will evaluate to s-expressions
     * with the value set.
     * Expressions that are predefined should just override the Evaluate method.
     */
    class SExpression : IDatum
    {
        protected Func<Vector, ScopedEnvironment, IDatum> value;

        public object Value { get { return this.value; } }


        public SExpression(Func<Vector, ScopedEnvironment, IDatum> func)
        {
            this.value = func;
        }
        public SExpression()
        {
        }


        public virtual IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            return this.value.Invoke(list, env);
        }

    }

}
