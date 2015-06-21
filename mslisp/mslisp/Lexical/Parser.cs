using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsLisp.Datums;
using MsLisp.Expressions;

namespace MsLisp.Lexical
{
    public class Parser
    {

        // quote symbol is used everywhere a 'tick' is found,
        // so it's useful to keep a copy around.
        // same with the rest of these.
        private readonly Symbol quote;
        private readonly Symbol quasiquote;
        private readonly Symbol unquote;

        
        public Parser()
        {
            this.quote = new Symbol("quote");
            this.quasiquote = new Symbol("quasiquote");
            this.unquote = new Symbol("unquote");
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

                // empty list is NULL instance.
                // nil == ()
                if (list.Count == 0)
                    return Null.Instance;

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
            else if (TokenType.QUOTE == tokens.Current.Type)
            {
                // a tick is an abbreviated quote,
                // so replace tick with (quote exp);
                var quoted = new List<IDatum>();        // create new vector
                quoted.Add(this.quote);                 // add quote expression
                tokens.MoveNext();
                quoted.Add(this.Parse(tokens));         // add whatever the next datum is

                return new Vector(quoted.ToArray());    // return vector
            }
            else if (TokenType.QUASIQUOTE == tokens.Current.Type)
            {
                var quasi = new List<IDatum>();
                quasi.Add(this.quasiquote);
                tokens.MoveNext();
                quasi.Add(this.Parse(tokens));

                return new Vector(quasi.ToArray());
            }
            else if (TokenType.UNQUOTE == tokens.Current.Type)
            {
                // throw error if not in quasiquote
                var unquoted = new List<IDatum>();
                unquoted.Add(this.unquote);
                tokens.MoveNext();
                unquoted.Add(this.Parse(tokens));

                return new Vector(unquoted.ToArray());
            }
            else if (TokenType.SPLICE == tokens.Current.Type)
            {
                // throw error if not in quasiquote
            }

            throw new SyntaxException("Unrecognized token type {0}", tokens.Current.Value);
        }
        
    }

}
