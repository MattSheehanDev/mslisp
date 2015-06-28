using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Datums;

namespace MsLisp.Environment
{
    public class ScopedEnvironment : Dictionary<string, IDatum>
    {
        private ScopedEnvironment outerenv;

        public ScopedEnvironment(ScopedEnvironment env = null)
        {
            this.outerenv = env;
        }


        public new void Add(string str, IDatum token)
        {
            // wraps base Add, but makes sure str is upper
            if (!base.ContainsKey(str)) {
                base.Add(str, token);
            }
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

        public bool HasOuterEnvironment(ScopedEnvironment env)
        {
            if(this.outerenv != null)
            {
                if (this.outerenv == env)
                    return true;
                else
                    return this.outerenv.HasOuterEnvironment(env);
            }
            return false;
        }

        public IDatum Fetch(string variable)
        {
            if (this.ContainsKey(variable))
                return this[variable];
            else if (this.outerenv != null)
                return this.outerenv.Fetch(variable);

            throw new SyntaxException("Symbol {0} not found.", variable);
        }
    }
}
