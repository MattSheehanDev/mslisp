using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Datums
{

    class Vector : IDatum
    {
        private readonly DatumType type;
        private readonly IDatum[] value;

        public DatumType Type
        {
            get { return this.type; }
        }
        public object Value
        {
            get { return this.value; }
        }


        public IDatum this[int i]
        {
            get { return this.value[i]; }
            set { this.value[i] = value; }
        }

        public int Length
        {
            get { return this.value.Length; }
        }


        public Vector(IDatum[] value)
        {
            this.type = DatumType.LIST;
            this.value = value;
        }
        

        public IDatum CAR()
        {
            if (this.value.Length == 0)
                return null;

            return this.value[0];
        }

        public Vector CDR()
        {
            if (this.value.Length <= 1)
                return null;

            var rest = new List<IDatum>();
            for (var i = 1; i < this.value.Length; i++)
            {
                rest.Add(this.value[i]);
            }
            return new Vector(rest.ToArray());
        }

        
        public override string ToString()
        {
            var str = "(";
            var atoms = this.value.Select((atom) => atom.ToString());
            str += string.Join(" ", atoms);
            str += ")";

            return str;
        }

    }

}
