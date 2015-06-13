using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Tokens
{
    class Vector : List<IDatum>, IDatum
    {
        private readonly DatumType type;
        private readonly object value;

        public DatumType Type { get { return this.type; } }
        public object Value { get { return this.value; } }


        public Vector()
        {
            this.type = DatumType.LIST;
            this.value = this;
        }

        public IDatum Shift()
        {
            var item = this.First();
            this.RemoveAt(0);
            return item;
        }

        public IDatum CAR()
        {
            if (this.Count == 0)
                return null;

            return this[0];
        }

        public Vector CDR()
        {
            if (this.Count <= 1)
                return null;

            var rest = new Vector();
            for (var i = 1; i < this.Count; i++)
            {
                rest.Add(this[i]);
            }
            return rest;
        }

    }

}
