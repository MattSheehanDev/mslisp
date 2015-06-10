using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;

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


        public new void Add(string str, IToken token)
        {
            // wraps base Add, but makes sure str is upper
            base.Add(str, token);
        }

        public dynamic find(string variable)
        {
            if (base.ContainsKey(variable))
                return this;
            else if (this.outerenv != null)
                return this.outerenv.find(variable);

            throw new SyntaxException(String.Format("Symbol {0} not found.", variable));
        }
    }
}
