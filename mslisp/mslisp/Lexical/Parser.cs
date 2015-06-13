using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mslisp.Tokens;

namespace mslisp.Lexical
{   
    class Parser
    {

        //public readonly List<IDatum> tokens;
        //private readonly Scanner scanner;
        //private readonly Stack<ListToken> stack;
        

        public Parser()
        {
            //this.scanner = scanner;
            //this.tokens = new List<IDatum>();

            //// counts number of parenthesis
            //this.stack = new Stack<ListToken>();
        }


        public List<IDatum> Parse(List<Token> list)
        {
            var datums = new List<IDatum>();

            IEnumerator<Token> tokens = list.Where((value) =>
            {
                return TokenType.COMMENT != value.Type && TokenType.WHITESPACE != value.Type;
            }).GetEnumerator();
            
            while (tokens.MoveNext() && tokens.Current != null)
            {
                var value = this.Parse(tokens);
                datums.Add(value);
            }

            return datums;
        }

        private IDatum Parse(IEnumerator<Token> tokens)
        {
            if (TokenType.LISTOPEN == tokens.Current.Type)
            {
                var listvalue = new Vector();

                while (tokens.MoveNext() && TokenType.LISTCLOSE != tokens.Current.Type)
                {
                    var value = this.Parse(tokens);
                    listvalue.Add(value);
                }

                return listvalue;
            }
            else if (TokenType.LISTCLOSE == tokens.Current.Type)
            {
                // we shouldn't encounter a closing parens without
                // an opening parens which handles it's own closing parens.
                throw new SyntaxException("Unexpected ) token.");
            }
            else if (TokenType.STRING == tokens.Current.Type)
            {
                return new Datum(DatumType.STRING, tokens.Current.Value);
            }
            else if (TokenType.INT == tokens.Current.Type)
            {
                return new Datum(DatumType.INT, int.Parse(tokens.Current.Value));
            }
            else if (TokenType.DOUBLE == tokens.Current.Type)
            {
                return new Datum(DatumType.DOUBLE, double.Parse(tokens.Current.Value));
            }
            else if (TokenType.SYMBOL == tokens.Current.Type)
            {
                return new Datum(DatumType.SYMBOL, tokens.Current.Value);
            }

            throw new SyntaxException("Unrecognized token type {0}", tokens.Current.Value);
        }

        //public bool Parse()
        //{
        //    var curr = this.tokens;
            
        //    while(this.scanner.IsMore())
        //    {
        //        scanner.Next();
        //        var type = scanner.IsType(scanner.Current);

        //        if(TokenType.COMMENT == type)
        //        {
        //            while (scanner.IsMore())
        //            {
        //                char ret = scanner.Current;
        //                char newline = scanner.Peek;

        //                // comments go until the next new line
        //                if(ret == '\r' && newline == '\n')
        //                {
        //                    break;
        //                }

        //                scanner.Next();
        //            }
        //        }
        //        else if (TokenType.LISTOPEN == type)    // begin list
        //        {
        //            this.stack.Push(curr);

        //            var list = new ListToken();
        //            curr.Add(list);
                    
        //            curr = list;
        //        }
        //        else if (TokenType.LISTCLOSE == type)    // end list
        //        {
        //            if (this.stack.Peek() != null)
        //                curr = this.stack.Pop();
        //            else
        //                throw new SyntaxException("Unexpected ) delimeter.");
        //        }
        //        else if (TokenType.QUOTATION == type)       // string
        //        {
        //            var str = "";
        //            while(scanner.IsMore())
        //            {
        //                scanner.Next();
                        
        //                if(TokenType.ESCAPE == scanner.IsType(scanner.Current))
        //                {
        //                    scanner.Next();
        //                }
        //                else if (TokenType.QUOTATION == scanner.IsType(scanner.Current))
        //                {
        //                    break;
        //                }

        //                str += scanner.Current;
        //            }

        //            var token = new Token(DatumType.STRING, str);
        //            curr.Add(token);
        //        }
        //        else if (TokenType.TICK == type)        // '(1 2 3)
        //        {
        //            if(TokenType.LISTOPEN == scanner.IsType(scanner.Peek))
        //            {
                        
        //            }
        //        }
        //        else if (TokenType.SYMBOL == type)        // number or symbol
        //        {
        //            string sym = scanner.Current.ToString();
        //            while(scanner.IsMore())
        //            {
        //                if (TokenType.SYMBOL != scanner.IsType(scanner.Peek))
        //                    break;
                        
        //                sym += scanner.Next();
        //            }

        //            var atom = Parser.toNumber(sym);

        //            curr.Add(atom);
        //        }
        //    }

        //    if (this.stack.Count == 0)
        //        return false;

        //    return true;
        //}

        public static Datum toNumber(object num)
        {
            // try integer
            int intres;
            bool success = int.TryParse(Convert.ToString(num), out intres);
            if (success) return new Datum(DatumType.INT, intres);

            // try double
            double doubleres;
            success = double.TryParse(Convert.ToString(num), out doubleres);
            if (success)
            {
                return new Datum(DatumType.DOUBLE, doubleres);
            }

            // must be a symbol
            return new Datum(DatumType.SYMBOL, num);
        }


        public string Stringify(IDatum parsed)
        {
            if ((parsed is Vector) == false)
            {
                if (parsed.Value == null)
                    return "NIL";
                else if (parsed.Type == DatumType.BOOLEAN && (bool)parsed.Value == true)
                    return "T";
                return Convert.ToString(parsed.Value);
            }
            else
            {
                Vector list = (Vector)parsed;

                var str = "(";
                var strlist = list.Select((atom) =>
                {
                    return this.Stringify(atom);
                });
                str += string.Join(" ", strlist);
                str += ")";

                return str;
            }
        }

    }

}
