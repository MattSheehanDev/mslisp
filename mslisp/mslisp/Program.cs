using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace mslisp
{
    class Program
    {
        public static EntryEnvironment env = new EntryEnvironment();
        public static Parser parser = new Parser();

        static void Main(string[] args)
        {
            // add C-c keyboard event listener
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);

            // repl
            while(true)
            {
                try
                {
                    var expr = Console.ReadLine();
                    var parsed = parser.Parse(expr);
                    var str = parser.Stringify(parsed);
                    Console.WriteLine(str);
                    // parse, eval, print value
                    // if value console writeline
                }
                catch (Exception ex)
                {
                    ReplError(ex);
                }
            }
        }


        static dynamic Eval(dynamic x, Environment e = null)
        {
            if (e == null) e = env;

            if(typeof(string) == x)                 // variable reference
            {
                return e.find(x)[x];
            }
            else if (x[0] == "quote")               // (quote exp)
            {
                
            }

            return null;
        }
        static dynamic Eval(int x, Environment e = null)
        {
            return null;
        }

        static void CancelKeyPress(Object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("{0}-{1} pressed.", args.SpecialKey, args.Cancel);
            Console.WriteLine("Exiting mslisp.");
        }
        
        static void ReplError(Exception ex)
        {
            var trace = new StackTrace(ex);
            for (int i = 0; i < trace.FrameCount; i++)
            {
                var frame = trace.GetFrame(i);
                Console.WriteLine("Line {0}: {1}", frame.GetFileLineNumber(), frame.GetMethod());
            }
        }
    }


    class SyntaxException : Exception
    {
        public SyntaxException(String msg) : base(msg)
        {
        }
    }

}
