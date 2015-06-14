using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Environment;
using mslisp.Datums;

namespace mslisp
{
    class Evaluator
    {

        public Evaluator()
        {
        }

        public static IDatum Eval(IDatum x, ScopedEnvironment env)
        {
            if (x is Atom)
            {
                return x;
            }
            else if (x is Symbol)
            {
                var data = env.Fetch((string)x.Value);
                return data;
            }
            else
            {
                var list = (Vector)x;
                
                SExpression procedure = (SExpression)Evaluator.Eval(list.CAR(), env);
                return procedure.Invoke(list, env);

                //Action<string> s = Console.WriteLine;
                //s.Invoke("test");
            }
        }

    }

}
