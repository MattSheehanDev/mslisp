using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslisp.Datums
{

    class Atom : IDatum
    {
        private readonly object value;

        
        public object Value { get { return this.value; } }


        public Atom(object value)
        {
            this.value = value;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            // if obj is null then it's not a Atom obj and should return false.
            if (ReferenceEquals(null, obj)) return false;

            // if obj references this, then they are the same Atom objects,
            // and should return true.
            if (ReferenceEquals(this, obj)) return true;

            // different types cannot be the same, return false.
            if (!(obj is Atom)) return false;

            // check for object equality
            return this.Equals((Atom)obj);
        }

        public bool Equals(Atom Atom)
        {
            if (ReferenceEquals(null, Atom)) return false;
            if (ReferenceEquals(this, Atom)) return true;
            return Equals(Atom.value, value);
        }

        // usually == resolves to ReferenceEquals and checks if the object
        // references the same memory. Since these are Atoms (strings, numbers, booleans),
        // we don't really want reference equality but instead just check for
        // object equality, which checks if object properties are the same but can
        // still be different memory references.
        public static bool operator ==(Atom left, Atom right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Atom left, Atom right)
        {
            return !Equals(left, right);
        }


        public override string ToString()
        {
            return string.Format("{0}", this.value);    // number or string
        }
        
    }

}
