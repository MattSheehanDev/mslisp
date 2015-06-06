using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using mslisp.Environment;

namespace mslisp
{
    class Program
    {
        public static GlobalEnvironment env = new GlobalEnvironment();


        static void Main(string[] args)
        {
            // add C-c keyboard event listener
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);

            // repl
            while(true)
            {
                try
                {
                    IToken prompt = env["*prompt*"];
                    Console.Write(string.Format("{0}> ", (string)prompt.Value));

                    var expr = Console.ReadLine();

                    var reader = new StringReader(expr);
                    var parser = new Parser(reader);
                    var tokens = parser.Parse();

                    //var parsed = parser.Parse(expr);
                    tokens.ForEach((list) =>
                    {
                        var eval = Evaluator.Eval(list, env);

                        var str = parser.Stringify(eval);
                        Console.WriteLine(str);
                    });
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch(SyntaxException ex)
                {
                    Console.WriteLine(ex.Message);
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

}
