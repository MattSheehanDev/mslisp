using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using mslisp.Tokens;
using mslisp.Environment;
using mslisp.Lexical;

namespace mslisp.Functions
{
    class Load : SExpression
    {
        private readonly Parser parser;

        public Load()
        {
            this.parser = new Parser();
            this.value = this.LoadFile;
        }


        private IDatum LoadFile(Vector list, ScopedEnvironment env)
        {
            if (list.Count != 2)
                throw new ArgumentException("LOAD has wrong number of arguments.");

            IDatum filename = list[1];

            if (filename.Type != DatumType.STRING)
                throw new TypeException("LOAD can only load string paths.");

            string file = File.ReadAllText((string)filename.Value);

            
            var lexer = new Lexer(file);
            var tokens = parser.Parse(lexer.Tokenize());
            

            tokens.ForEach((token) =>
            {
                var eval = Evaluator.Eval(token, env);
            });

            return new Datum(DatumType.BOOLEAN, true);
        }
        
    }
}
