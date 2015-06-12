using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Lexical
{
    enum LexType
    {
        LISTOPEN,
        LISTCLOSE,
        TICK,
        QUOTATION,
        ESCAPE,
        COMMENT,
        WHITESPACE,
        SYMBOL
    }

    class LexValue
    {
        private LexType type;
        private string value;

        public LexType Type { get { return this.type; } }
        public string Value { get { return this.value; } }


        public LexValue(string value, LexType type)
        {
            this.value = value;
            this.type = type;
        }
    }

    class Lexer
    {
        private readonly Scanner scanner;
        private readonly List<LexValue> tokens;

        public Lexer(Scanner scanner)
        {
            this.scanner = scanner;
            this.tokens = new List<LexValue>();
        }


        public void Scan()
        {
            while(this.scanner.IsMore())
            {
                var curr = this.scanner.Next();
                var type = this.scanner.IsType(curr);

                var value = ListOpen(curr);
                if (value != null)
                {
                    this.tokens.Add(value);
                    continue;
                }
            }
        }


        private LexValue ListOpen(char value)
        {
            if (value == '(')
                return null;
            return new LexValue(value.ToString(), LexType.LISTOPEN);
        }

        private LexValue Comment()
        {
            if (this.scanner.Current != ';')
                return null;

            var comment = this.scanner.Current.ToString();
            while (this.scanner.IsMore())
            {
                if (this.scanner.Current == '\r' && this.scanner.Peek == '\n')
                    break;

                comment += scanner.Next();
            }

            return new LexValue(comment, LexType.COMMENT);
        }

    }
}
