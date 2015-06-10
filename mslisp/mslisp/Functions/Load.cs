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
    class Load : FuncToken
    {
        public Load()
        {
            this.value = this.LoadFile;
        }


        private IToken LoadFile(ListToken list, ScopedEnvironment env)
        {
            if (list.Count != 2)
                throw new ArgumentException("LOAD has wrong number of arguments.");

            IToken filename = list[1];

            if (filename.Type != TokenType.STRING)
                throw new TypeException("LOAD can only load string paths.");

            string file = File.ReadAllText((string)filename.Value);

            var reader = new StringReader(file);
            var scanner = new Scanner(reader);
            var parser = new Parser(scanner);
            parser.Parse();
            

            parser.tokens.ForEach((token) =>
            {
                var eval = Evaluator.Eval(token, env);
            });

            return new Token(TokenType.BOOLEAN, true);
        }
        
    }
}
