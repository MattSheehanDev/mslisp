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

    public class InvokeMethod : SExpression
    {
        public InvokeMethod()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            // (invoke-method instance method args)
            if (list.Length < 2)
                throw new ArgumentException("INVOKE-METHOD is missing arguments.");
            
            var arr = (IDatum[])list.Value;

            var type = Evaluator.Eval(arr[0], env).Value;
            var method = (string)Evaluator.Eval(arr[1], env).Value;
            var args = arr.Skip(2).Select((datum) =>
            {
                return Evaluator.Eval(datum, env).Value;
            }).ToArray();

            var result = type.GetType().InvokeMember(method,
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null, type, args);

            return new Atom(result);
        }
    }
}
