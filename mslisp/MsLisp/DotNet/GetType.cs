using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Datums;
using MsLisp.Environment;

namespace MsLisp.DotNet
{
    public class GetType : SExpression
    {
        public GetType()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length < 1)
                throw new ArgumentException("GET-TYPE is missing arguments.");
            
            var arr = (IDatum[])list.Value;

            var names = arr.Select((datum) =>
            {
                // return an array of strings
                return Evaluator.Eval(datum, env).Value.ToString();
            });

            var name = string.Join(".", names);
            var type = System.Type.GetType(name);

            return new Atom(type);
        }
    }
}
