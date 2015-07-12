using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Datums;
using MsLisp.Environment;

namespace MsLisp.Expressions
{

    /*
     * TYPEOF
     * (typeof exp) => string
     */
    class TypeOf : SExpression
    {
        public TypeOf()
        {
        }

        public override IDatum Evaluate(Vector list, ScopedEnvironment env)
        {
            var value = Evaluator.Eval(list.CAR(), env);

            if (value is Bool)
                return new Atom("boolean");
            else if (value is Number)
                return new Atom("number");
            else if (value is SExpression)
                return new Atom("atom");
            else if (value is Atom)
                return new Atom("atom");
            else if (value is Symbol)
                return new Atom("symbol");
            else
                return new Atom("list");
        }
    }
    
}
