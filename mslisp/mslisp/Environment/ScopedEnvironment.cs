using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Environment
{
    class ScopedEnvironment : Dictionary<string, IToken>
    {
        private ScopedEnvironment outerenv;

        public ScopedEnvironment(ScopedEnvironment env = null)
        {
            this.outerenv = env;
        }


        public void define(string key, Array code)
        {

        }

        public dynamic find(string variable)
        {
            if (base.ContainsKey(variable))
                return this;
            else if (this.outerenv != null)
                return this.outerenv.find(variable);

            throw new SyntaxException(String.Format("Symbol {} not found.", variable));
        }
    }
}
