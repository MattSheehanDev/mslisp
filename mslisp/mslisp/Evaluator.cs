using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp
{
    class Evaluator
    {

        public Evaluator()
        {
        }

        public dynamic Eval(dynamic x, Environment env)
        {
            if(!(x is List<dynamic>))
            {
                if (x is string)                // variable reference
                    return env.find(x)[x];
                else                            // constant literal
                    return x;
            }
            else
            {
                var list = (List<dynamic>)x;
                var first = list.First();

                if(first == "quote")            // (quote exp)
                {

                }
            }
        }

    }
}
