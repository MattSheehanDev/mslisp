using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp
{
    class Environment : Dictionary<string, dynamic>
    {
        private Environment outerenv;
        
        public Environment(Environment env = null)
        {
            this.outerenv = env;
        }

        public dynamic find(string variable)
        {
            if (base.ContainsKey(variable))
                return this;
            else if(this.outerenv != null)
                return this.outerenv.find(variable);

            throw new SyntaxException(String.Format("Symbol {} not found.", variable));
        }
    }


    class EntryEnvironment : Environment
    {
        public EntryEnvironment()
        {
            var addition = new AddDelegate(this.addition);
            this.Add("+", addition);
        }

        private int addition(params int[] values)
        {
            var sum = 0;
            for (var i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            return sum;
        }
    }

    public delegate int AddDelegate(params int[] values);
}
