using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;

namespace mslisp.Environment
{
    class ScopedEnvironment : Dictionary<string, IDatum>
    {
        private ScopedEnvironment outerenv;

        public ScopedEnvironment(ScopedEnvironment env = null)
        {
            this.outerenv = env;
        }


        public void define(string key, Array code)
        {

        }


        public new void Add(string str, IDatum token)
        {
            // wraps base Add, but makes sure str is upper
            base.Add(str, token);
        }

        // returns environment that variable can be found.
        public ScopedEnvironment Find(string variable)
        {
            if (base.ContainsKey(variable))
                return this;
            else if (this.outerenv != null)
                return this.outerenv.Find(variable);

            throw new SyntaxException("Symbol {0} not found.", variable);
        }

        public IDatum Fetch(string variable)
        {
            if (base.ContainsKey(variable))
                return this[variable];
            else if (this.outerenv != null)
                return this.outerenv.Fetch(variable);

            throw new SyntaxException("Symbol {0} not found.", variable);
        }
    }
}
