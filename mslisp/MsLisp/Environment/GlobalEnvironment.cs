using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Expressions;
using MsLisp.Datums;
using MsLisp.DotNet;
using MsLisp.Macro;

namespace MsLisp.Environment
{
    
    public class GlobalEnvironment : ScopedEnvironment
    {

        public GlobalEnvironment()
        {
            // global variables

            this.Add("new", new New());
            this.Add("get-type", new GetType());
            this.Add("invoke-static", new InvokeStatic());
            this.Add("invoke-method", new InvokeMethod());

            this.Add("#t", Bool.True);

            // global procedures
            this.Add("add", new Addition());
            this.Add("multiply", new Multiplication());
            this.Add("subtract", new Subtraction());
            this.Add("divide", new Division());
            this.Add("greater", new GreaterThan());
            this.Add("lesser", new LessThan());
            this.Add("not-lesser", new NotLessThan());
            this.Add("not-greater", new NotGreaterThan());
            this.Add("equal", new Equal());

            this.Add("car", new CAR());
            this.Add("cdr", new CDR());
            this.Add("cons", new CONS());
            this.Add("define", new Define());
            this.Add("set!", new Set());
            this.Add("if", new IfThen());
            this.Add("begin", new Begin());
            this.Add("lambda", new Lambda());

            this.Add("macro", new Macro.Macro());
            this.Add("quote", new Quote());
            this.Add("quasiquote", new QuasiQuote());
            this.Add("unquote", new UnQuote());
            this.Add("splice", new Splice());

            this.Add("typeof", new TypeOf());
            this.Add("eq?", new Eq());
        }

    }

}
