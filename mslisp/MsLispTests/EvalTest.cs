using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MsLisp;
using MsLisp.Lexical;
using MsLisp.Environment;
using MsLisp.Datums;

namespace MsLispTests
{
    [TestClass]
    public class EvalTest
    {

        [TestMethod]
        public void Eval()
        {
            try {
                // load std.lisp with eval function defined
                var parser = new Parser();
                var lexer = new Lexer(File.ReadAllText("../../../MsRepl/std.lisp"));
                Evaluator.Eval(parser.Parse(lexer.Tokenize()));

                // load evaltest.lisp
                lexer = new Lexer(File.ReadAllText("../../evaltest.lisp"));
                var tokens = parser.Parse(lexer.Tokenize());

                tokens.ForEach((datum) =>
                {   
                    IDatum data = Evaluator.Eval(datum, Evaluator.environment);
                    var arr = datum as Vector;

                    var first = arr.CAR().ToString();
                    if (first == "assert-equal" || first == "assert-not-equal")
                    {
                        Assert.AreSame(Bool.True, data);
                    }
                });
                
                // no exceptions is a success
                Assert.IsTrue(true);
            }
            catch(Exception ex)
            {
                // fail test
                Assert.IsFalse(true);
            }
        }
  
    }

}
