using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MsLisp.Datums;
using MsLisp.Environment;


namespace MsLisp.DotNet
{
    public class New : SExpression
    {
        public New()
        {
        }

        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length <= 1)
                throw new ArgumentException("NEW is missing arguments.");

            var type = System.Type.GetType(Evaluator.Eval(list[1], env).Value.ToString());
            var args = new List<object>();
            for (var i = 2; i < list.Length; i++)
            {
                args.Add(Evaluator.Eval(list[i], env).Value);
            }
            var instance = Activator.CreateInstance(type, args.ToArray());
            
            return new Atom(instance);
        }
    }

}
