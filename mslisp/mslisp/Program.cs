﻿using System;
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
    // todo: convert all datums to uppercase
    // todo: load multiline files
    // todo: consolidate load and repl
    class Program
    {
        static void Main(string[] args)
        {
            // add C-c keyboard event listener
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);

            var env = new GlobalEnvironment();
            var parser = new Parser();
            string expr = "";

            // repl
            while(true)
            {
                try
                {
                    IDatum prompt = env["*prompt*"];
                    Console.Write(string.Format("{0}> ", (string)prompt.Value));

                    expr += Console.ReadLine();

                    // todo: need a better way to count parenthesis
                    var open = expr.Count(c => c == '(');
                    var close = expr.Count(c => c == ')');

                    if (open != close)
                        continue;

                    
                    // have lexer count parens?
                    // lexer should count line numbers.
                    var lexer = new Lexer(expr);
                    var tokens = parser.Parse(lexer.Tokenize());

                    
                    tokens.ForEach((list) =>
                    {
                        var eval = Evaluator.Eval(list, env);

                        var str = parser.Stringify(eval);
                        Console.WriteLine(str);
                    });


                    // cleanup
                    expr = "";
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
            Console.WriteLine("Ctrl-C pressed.");
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
