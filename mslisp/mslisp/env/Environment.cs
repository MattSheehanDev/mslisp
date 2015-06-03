using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.env;

namespace mslisp
{
    class Environment : Dictionary<string, dynamic>
    {
        private Environment outerenv;
        
        public Environment(Environment env = null)
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
            else if(this.outerenv != null)
                return this.outerenv.find(variable);

            throw new SyntaxException(String.Format("Symbol {} not found.", variable));
        }
    }


    public delegate dynamic lambdatype(params dynamic[] args);


    class EntryEnvironment : Environment
    {

        public EntryEnvironment()
        {
            // global variables
            this.Add("*prompt*", "mslisp");

            // global operators
            this.Add("+", new lambdatype(this.addition));
            this.Add("-", new lambdatype(this.subtraction));
            this.Add("/", new lambdatype(this.division));
            this.Add("*", new lambdatype(this.multiplication));

            // global procedures
            this.Add("quote", new Func<Token, Token>(Globals.Quote));
        }
        

        public dynamic addition(params dynamic[] nums)
        {
            dynamic sum = 0;
            for (var i = 0; i < nums.Length; i++)
            {
                sum += nums[i];
            }
            return sum;
        }

        public dynamic subtraction(params dynamic[] nums)
        {
            dynamic diff = null;
            for (var i = 0; i < nums.Length; i++)
            {
                if (diff != null)
                    diff -= nums[i];
                else
                    diff = nums[i];
            }
            return diff;
        }

        public dynamic division(params dynamic[] nums)
        {
            dynamic quotient = null;
            for (var i = 0; i < nums.Length; i++)
            {
                if (quotient != null)
                    quotient /= nums[i];
                else
                    quotient = nums[i];
            }
            return quotient;
        }

        public dynamic multiplication(params dynamic[] nums)
        {
            dynamic times = null;
            for (var i = 0; i < nums.Length; i++)
            {
                if (times != null)
                    times *= nums[i];
                else
                    times = nums[i];
            }
            return times;
        }

    }

}
