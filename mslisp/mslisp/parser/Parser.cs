using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace mslisp
{
    enum CharType
    {
        OPENPARENS,
        CLOSEPARENS,
        QUOTE,
        STRING,
        SPACE,
        SYMBOL,
    }
    
    
    class Parser
    {

        public readonly TokenList tokens;
        private readonly TextReader reader;
        

        public Parser(TextReader read)
        {
            this.reader = read;
            this.tokens = new TokenList();
        }

        public TokenList Parse()
        {
            this.readText();
            return this.tokens;
        }

        private void readText()
        {
            var curr = this.tokens;
            
            var stack = new Stack<TokenList>();

            char peek;
            while(reader.Peek() != -1)
            {
                peek = (char)reader.Read();
                var type = this.readChar(peek);

                if (CharType.OPENPARENS == type)     // begin list
                {
                    stack.Push(curr);

                    var list = new TokenList();
                    curr.Add(list);
                    
                    curr = list;
                }
                else if (CharType.CLOSEPARENS == type) // end list
                {
                    if (stack.Peek() != null)
                        curr = stack.Pop();
                    else
                        throw new SyntaxException("Unexpected ) delimeter.");
                }
                else if (CharType.STRING == type)       // string
                {
                    var str = "";
                    while(reader.Peek() != -1)
                    {
                        peek = (char)reader.Read();

                        if (CharType.STRING == this.readChar(peek))
                            break;

                        str += peek;
                    }

                    var token = new Token(TokenType.STRING, str);
                    curr.Add(token);
                }
                else if (CharType.QUOTE == type)        // '(1 2 3)
                {
                    peek = (char)reader.Peek();
                    type = this.readChar(peek);
                    if(CharType.OPENPARENS == type)
                    {

                    }
                }
                else if (CharType.SYMBOL == type)        // number or symbol
                {
                    string sym = (peek).ToString();
                    while(reader.Peek() != -1)
                    {
                        peek = (char)reader.Peek();
                        if (CharType.SYMBOL != this.readChar(peek))
                            break;

                        peek = (char)reader.Read();
                        sym += peek;
                    }

                    var atom = this.isNumber(sym);

                    curr.Add(atom);
                }
            }
        }

        private Token isNumber(dynamic token)
        {
            // try integer
            int intres;
            bool success = int.TryParse(token, out intres);
            if (success) return new Token(TokenType.INT, intres);

            // try double
            double doubleres;
            success = double.TryParse(token, out doubleres);
            if (success) return new Token(TokenType.DOUBLE, doubleres);

            // must be a symbol
            return new Token(TokenType.SYMBOL, token);
        }

        public CharType readChar(char c)
        {
            if (char.IsWhiteSpace(c))
                return CharType.SPACE;

            switch(c)
            {
                case '(':
                    return CharType.OPENPARENS;
                case ')':
                    return CharType.CLOSEPARENS;
                case '\"':
                    return CharType.STRING;
                case '\'':
                    return CharType.QUOTE;
                default:
                    return CharType.SYMBOL;
            }
        }

        public string Stringify(IToken parsed)
        {
            if ((parsed is TokenList) == false)
            {
                return Convert.ToString(parsed.Value);
            }
            else
            {
                TokenList list = (TokenList)parsed;

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



        //private List<string> _Tokenize(string expr)
        //{
        //    expr = expr.Replace("(", " ( ");
        //    expr = expr.Replace(")", " ) ");
        //    string[] arr = expr.Split(' ');
        //    var list = arr.Where((str) => { return str != ""; }).ToList<string>();
        //    return list;
        //}

        //private dynamic _ReadTokens(List<string> tokens)
        //{
        //    if (tokens.Count <= 0)
        //        throw new SyntaxException("Invalid input.");

        //    var token = tokens[0];
        //    tokens.Remove(token);

        //    if("(" == token)
        //    {
        //        var list = new TokenList();
        //        while (tokens[0] != ")")
        //        {
        //            list.Add(_ReadTokens(tokens));
        //        }
        //        tokens.Remove(tokens[0]);
        //        return list;
        //    }
        //    else if (")" == token)
        //    {
        //        throw new SyntaxException("Unexpected close delimeter )");
        //    }
        //    else
        //    {
        //        return isNumber(token);
        //    }
        //}

    }

}
