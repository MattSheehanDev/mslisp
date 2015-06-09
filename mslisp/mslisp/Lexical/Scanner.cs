using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mslisp.Lexical
{
    enum CharType
    {
        OPENPARENS,
        CLOSEPARENS,
        APOSTRAPHE,
        QUOTATION,
        ESCAPE,
        SPACE,
        SYMBOL
    }

    class CharValue
    {
        private char value;
        private CharType type;

        public char Value { get { return this.value; } }
        public CharType Type { get { return this.type; } }


        public CharValue(char value, CharType type)
        {
            this.value = value;
            this.type = type;
        }
    }

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
        
        public CharType IsType(char value)
        {
            if (char.IsWhiteSpace(value))
                return CharType.SPACE;

            switch (value)
            {
                case '(':
                    return CharType.OPENPARENS;
                case ')':
                    return CharType.CLOSEPARENS;
                case '\"':
                    return CharType.QUOTATION;
                case '\'':
                    return CharType.APOSTRAPHE;
                case '\\':
                    return CharType.ESCAPE;
                default:
                    return CharType.SYMBOL;
            }
        }

        public bool IsMore()
        {
            return this.reader.Peek() != -1;
        }
    }
}
