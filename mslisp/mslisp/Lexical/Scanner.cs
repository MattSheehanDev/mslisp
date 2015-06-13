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


        public Scanner(string expr)
        {
            this.reader = new StringReader(expr);

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
        
        //public TokenType IsType(char value)
        //{
        //    if (char.IsWhiteSpace(value))
        //        return TokenType.WHITESPACE;

        //    switch (value)
        //    {
        //        case ';':
        //            return TokenType.COMMENT;
        //        case '(':
        //            return TokenType.LISTOPEN;
        //        case ')':
        //            return TokenType.LISTCLOSE;
        //        case '\"':
        //            return TokenType.QUOTATION;
        //        case '\'':
        //            return TokenType.TICK;
        //        case '\\':
        //            return TokenType.ESCAPE;
        //        default:
        //            return TokenType.SYMBOL;
        //    }
        //}

        public bool IsMore()
        {
            return this.reader.Peek() != -1;
        }
    }
}
