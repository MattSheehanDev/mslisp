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
    public class InvokeStatic : SExpression
    {
        public InvokeStatic()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            // (invoke-static type method args)
            if (list.Length < 2)
                throw new ArgumentException("INVOKE-STATIC is missing arguments.");
            
            var arr = (IDatum[])list.Value;

            var type = (System.Type)Evaluator.Eval(arr[0], env).Value;
            var method = (string)Evaluator.Eval(arr[1], env).Value;
            var args = arr.Skip(2).Select((datum) =>
            {
                return Evaluator.Eval(datum, env).Value;
            }).ToArray();

            //var m = type.GetMethod(method, new Type[] { arr[2].Value.GetType() });


            var result = type.InvokeMember(method,
                BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static,
                null, null, args);

            return new Atom(result);
        }
    }
}
