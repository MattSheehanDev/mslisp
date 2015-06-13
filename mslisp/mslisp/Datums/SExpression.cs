using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;

namespace mslisp.Tokens
{
    class SExpression : IDatum
    {
        protected DatumType type;
        protected Func<Vector, ScopedEnvironment, IDatum> value;

        public DatumType Type { get { return this.type; } }
        public object Value { get { return this.value; } }


        public SExpression(Func<Vector, ScopedEnvironment, IDatum> func)
        {
            this.type = DatumType.LAMBDA;
            this.value = func;
        }
        public SExpression()
        {
            this.type = DatumType.LAMBDA;
        }


        public IDatum Invoke(Vector list, ScopedEnvironment env)
        {
            return this.value.Invoke(list, env);
        }

    }

}
