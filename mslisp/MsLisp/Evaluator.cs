using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Environment;
using MsLisp.Datums;

namespace MsLisp
{
    public class Evaluator
    {

        public static readonly GlobalEnvironment environment = new GlobalEnvironment();

        public Evaluator()
        {
        }
        
        
        public static IDatum Eval(List<IDatum> datums)
        {
            IDatum eval = null;
            datums.ForEach((data) =>
            {
                eval = Eval(data, environment);
            });
            return Bool.True;
        }

        public static IDatum Eval(IDatum x, ScopedEnvironment env)
        {
            if (x is Atom || x is SExpression || x is Null)
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

                SExpression procedure = Evaluator.Eval(list.CAR(), env) as SExpression;
                if (procedure == null)
                    throw new TypeException("{0} is not a procedure.", list.CAR());

                return procedure.Evaluate(list.CDR(), env);
            }
        }

    }

}
