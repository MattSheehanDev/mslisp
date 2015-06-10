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

        public readonly ListToken tokens;
        private readonly Scanner scanner;
        private readonly Stack<ListToken> stack;
        

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            this.tokens = new ListToken();

            // counts number of parenthesis
            this.stack = new Stack<ListToken>();
        }

        public bool Parse()
        {
            return this.readText();
        }

        private bool readText()
        {
            var curr = this.tokens;
            
            while(this.scanner.IsMore())
            {
                scanner.Next();
                var type = scanner.IsType(scanner.Current);

                if(CharType.COMMENT == type)
                {
                    while (scanner.IsMore())
                    {
                        char ret = scanner.Current;
                        char newline = scanner.Peek;

                        // comments go until the next new line
                        if(ret == '\r' && newline == '\n')
                        {
                            break;
                        }

                        scanner.Next();
                    }
                }
                else if (CharType.OPENPARENS == type)        // begin list
                {
                    this.stack.Push(curr);

                    var list = new ListToken();
                    curr.Add(list);
                    
                    curr = list;
                }
                else if (CharType.CLOSEPARENS == type)    // end list
                {
                    if (this.stack.Peek() != null)
                        curr = this.stack.Pop();
                    else
                        throw new SyntaxException("Unexpected ) delimeter.");
                }
                else if (CharType.QUOTATION == type)       // string
                {
                    var str = "";
                    while(scanner.IsMore())
                    {
                        scanner.Next();
                        
                        if(CharType.ESCAPE == scanner.IsType(scanner.Current))
                        {
                            scanner.Next();
                        }
                        else if (CharType.QUOTATION == scanner.IsType(scanner.Current))
                        {
                            break;
                        }

                        str += scanner.Current;
                    }

                    var token = new Token(TokenType.STRING, str);
                    curr.Add(token);
                }
                else if (CharType.APOSTRAPHE == type)        // '(1 2 3)
                {
                    if(CharType.OPENPARENS == scanner.IsType(scanner.Peek))
                    {
                        
                    }
                }
                else if (CharType.SYMBOL == type)        // number or symbol
                {
                    string sym = scanner.Current.ToString();
                    while(scanner.IsMore())
                    {
                        if (CharType.SYMBOL != scanner.IsType(scanner.Peek))
                            break;
                        
                        sym += scanner.Next();
                    }

                    var atom = Parser.toNumber(sym);

                    curr.Add(atom);
                }
            }

            if (this.stack.Count == 0)
                return false;

            return true;
        }

        public static Token toNumber(object num)
        {
            // try integer
            int intres;
            bool success = int.TryParse(Convert.ToString(num), out intres);
            if (success) return new Token(TokenType.INT, intres);

            // try double
            double doubleres;
            success = double.TryParse(Convert.ToString(num), out doubleres);
            if (success)
            {
                return new Token(TokenType.DOUBLE, doubleres);
            }

            // must be a symbol
            return new Token(TokenType.SYMBOL, num);
        }


        public string Stringify(IToken parsed)
        {
            if ((parsed is ListToken) == false)
            {
                return Convert.ToString(parsed.Value);
            }
            else
            {
                ListToken list = (ListToken)parsed;

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
