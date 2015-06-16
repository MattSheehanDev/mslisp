using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using mslisp.Datums;
using mslisp.Environment;

namespace mslisp.DotNet
{
    class New : SExpression
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

            // arbitrary type for now.
            return new Atom(instance);
        }
    }


    class GetType : SExpression
    {
        public GetType()
        {
        }

        
        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            if (list.Length <= 1)
                throw new ArgumentException("GET-TYPE is missing arguments.");

            var args = list.CDR();
            var arr = (IDatum[])args.Value;

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


    class InvokeStatic : SExpression
    {
        public InvokeStatic()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            // (invoke-static type method args)
            if (list.Length < 3)
                throw new ArgumentException("INVOKE-STATIC is missing arguments.");

            var cdr = list.CDR();
            var arr = (IDatum[])cdr.Value;

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


    class InvokeMethod : SExpression
    {
        public InvokeMethod()
        {
        }


        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            // (invoke-method instance method args)
            if (list.Length < 3)
                throw new ArgumentException("INVOKE-METHOD is missing arguments.");

            var cdr = list.CDR();
            var arr = (IDatum[])cdr.Value;

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
