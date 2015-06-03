using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.env
{
    class Globals
    {

        public static IToken CAR(IToken t)
        {
            if (!(t is TokenList))
                throw new SyntaxException(string.Format("{0} is not a valid list.", t.Value));

            TokenList list = (TokenList)t;

            if (list.Count == 0)
                return null;
            
            return list[0];
        }

        public static IToken CDR(IToken t)
        {
            if(!(t is Token))
                throw new SyntaxException(string.Format("{0} is not a valid list.", t.Value));

            TokenList list = (TokenList)t;

            if (list.Count <= 1)
                return null;

            return list[0];
        }

        public static Token Quote(Token t)
        {
            return t;
        }

    }
}
