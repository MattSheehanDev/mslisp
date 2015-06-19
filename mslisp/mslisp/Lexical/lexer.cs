using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsLisp.Lexical
{
    enum CharType
    {
        OPENPARENS,
        CLOSEPARENS,
        QUOTATION,
        ESCAPE,
        SEMICOLON,
        WHITESPACE,
        TICK,
        OTHER
    }


    public class Lexer
    {
        private readonly Scanner scanner;
        private readonly List<Token> tokens;

        public Lexer(Scanner scanner)
        {
            this.scanner = scanner;
            this.tokens = new List<Token>();
        }

        public Lexer(string expr)
        {
            this.scanner = new Scanner(expr);
            this.tokens = new List<Token>();
        }
        

        public List<Token> Tokenize()
        {
            while(this.scanner.IsMore())
            {
                var curr = this.scanner.Next();

                switch(this.Character(curr))
                {
                    case CharType.WHITESPACE:
                        this.tokens.Add(this.WhiteSpace());
                        break;
                    case CharType.SEMICOLON:
                        this.tokens.Add(this.Comment());
                        break;
                    case CharType.OPENPARENS:
                        this.tokens.Add(this.ListOpen());
                        break;
                    case CharType.CLOSEPARENS:
                        this.tokens.Add(this.ListClose());
                        break;
                    case CharType.QUOTATION:
                        this.tokens.Add(this.String());
                        break;
                    case CharType.TICK:
                        this.tokens.Add(this.Tick());
                        break;
                    case CharType.OTHER:
                        this.tokens.Add(this.NumberOrSymbol());
                        break;
                }
            }

            return this.tokens;
        }

        private CharType Character(char value)
        {
            if (char.IsWhiteSpace(value))
                return CharType.WHITESPACE;
            if (value == ';')
                return CharType.SEMICOLON;
            else if (value == '(')
                return CharType.OPENPARENS;
            else if (value == ')')
                return CharType.CLOSEPARENS;
            else if (value == '"')
                return CharType.QUOTATION;
            else if (value == '\\')
                return CharType.ESCAPE;
            else if (value == '\'')
                return CharType.TICK;
            else
                return CharType.OTHER;
        }


        private Token WhiteSpace()
        {
            // get all the contiguous whitespace.
            // whitespace can be clumped together since it's not syntactically important
            var whitespace = this.scanner.Current.ToString();
            while (this.scanner.IsMore())
            {
                if (!char.IsWhiteSpace(this.scanner.Peek))
                    break;

                whitespace += this.scanner.Next();
            }
            return new Token(TokenType.WHITESPACE, whitespace);
        }

        private Token ListOpen()
        {
            this.Expect(this.scanner.Current, '(');
            return new Token(TokenType.LISTOPEN, this.scanner.Current.ToString());
        }

        private Token ListClose()
        {
            this.Expect(this.scanner.Current, ')');
            return new Token(TokenType.LISTCLOSE, this.scanner.Current.ToString());
        }

        private Token String()
        {
            this.Expect(this.scanner.Current, '"');

            var str = "";
            while (this.scanner.IsMore())
            {
                this.scanner.Next();

                if(CharType.QUOTATION == this.Character(this.scanner.Current) &&
                   CharType.ESCAPE != this.Character(this.scanner.Prev))
                {
                    break;
                }

                if (CharType.ESCAPE == this.Character(this.scanner.Current))
                    continue;

                str += this.scanner.Current;
            }

            return new Token(TokenType.STRING, str);
        }

        private Token NumberOrSymbol()
        {
            var num = scanner.Current.ToString();
            while (this.scanner.IsMore())
            {
                if (CharType.OTHER != this.Character(this.scanner.Peek))
                    break;

                num += this.scanner.Next();
            }

            // check if value is an integer
            int intnum;
            bool success = int.TryParse(Convert.ToString(num), out intnum);
            if (success)
                return new Token(TokenType.INT, num);

            // check if value is a double
            double doublenum;
            success = double.TryParse(Convert.ToString(num), out doublenum);
            if (success)
                return new Token(TokenType.DOUBLE, num);

            // value must be some symbol
            return new Token(TokenType.SYMBOL, num);
        }

        private Token Comment()
        {
            this.Expect(this.scanner.Current, ';');

            var comment = this.scanner.Current.ToString();
            while (this.scanner.IsMore())
            {
                // comments go until the end of the line
                if (this.scanner.Current == '\r' && this.scanner.Peek == '\n')
                {
                    this.scanner.Next();
                    break;
                }

                comment += this.scanner.Next();
            }

            return new Token(TokenType.COMMENT, comment);
        }


        private Token Tick()
        {
            this.Expect(this.scanner.Current, '\'');
            return new Token(TokenType.TICK, this.scanner.Current.ToString());
        }


        private void Expect(char value, char expected)
        {
            if (value != expected)
                throw new SyntaxException("Expected {0} and instead found {1}.", expected, value);
        }

    }
}
