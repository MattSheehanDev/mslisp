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
        private readonly StringReader reader;

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
        

        public bool IsMore()
        {
            return this.reader.Peek() != -1;
        }
    }
}
