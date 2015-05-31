using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp
{

    
    class Parser
    {

        public Parser()
        {
        }

        public dynamic Parse(string expr)
        {
            var tokens = this._Tokenize(expr);
            var list = _ReadTokens(tokens);
            return list;
        }

        public string Stringify(dynamic parsed)
        {
            if((parsed is List<dynamic>) == false)
            {
                return Convert.ToString(parsed);
            }
            else
            {
                var list = (List<dynamic>)parsed;

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

        private List<string> _Tokenize(string expr)
        {
            expr = expr.Replace("(", " ( ");
            expr = expr.Replace(")", " ) ");
            string[] arr = expr.Split(' ');
            var list = arr.Where((str) => { return str != ""; }).ToList<string>();
            return list;
        }

        private dynamic _ReadTokens(List<string> tokens)
        {
            if (tokens.Count <= 0)
                throw new SyntaxException("Invalid input.");

            var token = tokens[0];
            tokens.Remove(token);

            if("(" == token)
            {
                var list = new List<dynamic>();
                while (tokens[0] != ")")
                {
                    list.Add(_ReadTokens(tokens));
                }
                tokens.Remove(tokens[0]);
                return list;
            }
            else if (")" == token)
            {
                throw new SyntaxException("Unexpected close delimeter )");
            }
            else
            {
                return _IsAtom(token);
            }
        }

        private dynamic _IsAtom(dynamic token)
        {
            // try unsigned integer
            uint uintres;
            bool success = uint.TryParse(token, out uintres);
            if (success) return uintres;

            // try integer
            int intres;
            success = int.TryParse(token, out intres);
            if (success) return intres;

            // try float
            float floatres;
            success = float.TryParse(token, out floatres);
            if (success) return floatres;

            // must be string
            return token;
        }

    }
}
