using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace mslisp
{
    class Program
    {
        public static EntryEnvironment env = new EntryEnvironment();
        //public static Parser parser = new Parser();
        public static Evaluator evaluator = new Evaluator();

        static void Main(string[] args)
        {
            // add C-c keyboard event listener
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);

            // repl
            while(true)
            {
                try
                {
                    Console.Write(string.Format("{0}> ", env["*prompt*"]));

                    var expr = Console.ReadLine();

                    var reader = new StringReader(expr);
                    var parser = new Parser(reader);
                    var tokens = parser.Parse();

                    //var parsed = parser.Parse(expr);
                    tokens.ForEach((list) =>
                    {
                        var eval = evaluator.Eval(list, env);

                        var str = parser.Stringify(eval);
                        Console.WriteLine(str);
                    });
                }
                catch (Exception ex)
                {
                    ReplError(ex);
                }
            }
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
