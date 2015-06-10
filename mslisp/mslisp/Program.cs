using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using mslisp.Environment;
using mslisp.Lexical;
using mslisp.Tokens;

namespace mslisp
{
    // todo: convert all input to uppercase
    // todo: global T and ()
    // todo: load multiline files
    // todo: consolidate load and repl
    // todo: run sqrt.lisp
    class Program
    {
        static void Main(string[] args)
        {
            GlobalEnvironment env = new GlobalEnvironment();

            // add C-c keyboard event listener
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);
            
            // repl
            while(true)
            {
                try
                {
                    IToken prompt = env["*prompt*"];
                    Console.Write(string.Format("{0}> ", (string)prompt.Value));

                    string expr = Console.ReadLine();

                    var reader = new StringReader(expr);
                    var scanner = new Scanner(reader);
                    var parser = new Parser(scanner);

                    bool needMore = parser.Parse();
                    if (needMore)
                        continue;

                    var tokens = parser.tokens;
                    
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
