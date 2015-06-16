using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Datums;

namespace mslisp.Lexical
{   
    class Parser
    {
        
        public Parser()
        {
        }


        public List<IDatum> Parse(List<Token> list)
        {
            var Atoms = new List<IDatum>();

            IEnumerator<Token> tokens = list.Where((value) =>
            {
                return TokenType.COMMENT != value.Type && TokenType.WHITESPACE != value.Type;
            }).GetEnumerator();
            
            while (tokens.MoveNext() && tokens.Current != null)
            {
                var value = this.Parse(tokens);
                Atoms.Add(value);
            }

            return Atoms;
        }

        private IDatum Parse(IEnumerator<Token> tokens)
        {
            if (TokenType.LISTOPEN == tokens.Current.Type)
            {
                var list = new List<IDatum>();
                while(tokens.MoveNext() && TokenType.LISTCLOSE != tokens.Current.Type)
                {
                    list.Add(this.Parse(tokens));
                }
                return new Vector(list.ToArray());
            }
            else if (TokenType.LISTCLOSE == tokens.Current.Type)
            {
                // we shouldn't encounter a closing parens without
                // an opening parens which handles it's own closing parens.
                throw new SyntaxException("Unexpected ) token.");
            }
            else if (TokenType.STRING == tokens.Current.Type)
            {
                return new Atom(tokens.Current.Value);
            }
            else if (TokenType.INT == tokens.Current.Type)
            {
                return new Number(int.Parse(tokens.Current.Value));
            }
            else if (TokenType.DOUBLE == tokens.Current.Type)
            {
                return new Number(double.Parse(tokens.Current.Value));
            }
            else if (TokenType.SYMBOL == tokens.Current.Type)
            {
                return new Symbol(tokens.Current.Value);
            }

            throw new SyntaxException("Unrecognized token type {0}", tokens.Current.Value);
        }
        
    }

}
