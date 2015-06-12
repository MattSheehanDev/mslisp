using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mslisp.Lexical
{
    class Scanner
    {
        private readonly TextReader reader;

        private char current;
        private char prev;
        private char peek;

        private bool inited;
        private bool updatePeek;


        public char Current { get { return this.current; } }
        public char Prev { get { return this.prev; } }
        
        public char Peek
        {
            get
            {
                if (this.updatePeek)
                {
                    this.peek = (char)this.reader.Peek();
                    this.updatePeek = false;
                }

                return this.peek;
            }
        }


        public Scanner(TextReader reader)
        {
            this.reader = reader;

            this.inited = false;
            this.updatePeek = true;
        }


        public char Next()
        {
            if (this.inited)
                this.prev = this.current;
            
            this.inited = true;
            this.updatePeek = true;

            this.current = (char)this.reader.Read();
            return this.current;
        }
        
        public LexType IsType(char value)
        {
            if (char.IsWhiteSpace(value))
                return LexType.WHITESPACE;

            switch (value)
            {
                case ';':
                    return LexType.COMMENT;
                case '(':
                    return LexType.LISTOPEN;
                case ')':
                    return LexType.LISTCLOSE;
                case '\"':
                    return LexType.QUOTATION;
                case '\'':
                    return LexType.TICK;
                case '\\':
                    return LexType.ESCAPE;
                default:
                    return LexType.SYMBOL;
            }
        }

        public bool IsMore()
        {
            return this.reader.Peek() != -1;
        }
    }
}
