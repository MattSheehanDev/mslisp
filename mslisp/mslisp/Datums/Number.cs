using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsLisp.Datums
{
    /*
     * Numbers are Atoms but there are certain operator overloads
     * that make doing arithmatic on number atoms easier.
     */
    public class Number : Atom
    {

        public Number(object num) : base(num)
        {
        }


        public static Number operator +(Number left, Number right)
        {
            return new Number(Convert.ToDouble(left.Value) + Convert.ToDouble(right.Value));
        }

        public static Number operator *(Number left, Number right)
        {
            return new Number(Convert.ToDouble(left.Value) * Convert.ToDouble(right.Value));
        }

        public static Number operator -(Number left, Number right)
        {
            return new Number(Convert.ToDouble(left.Value) - Convert.ToDouble(right.Value));
        }

        public static Number operator /(Number left, Number right)
        {
            return new Number(Convert.ToDouble(left.Value) / Convert.ToDouble(right.Value));
        }
        

        public static bool operator >(Number left, Number right)
        {
            // we don't really care if it's 1 or 1.0 for comparisons
            return Convert.ToDouble(left.Value) > Convert.ToDouble(right.Value);
        }

        public static bool operator <(Number left, Number right)
        {
            return Convert.ToDouble(left.Value) < Convert.ToDouble(right.Value);
        }

        public static bool operator >=(Number left, Number right)
        {
            return Convert.ToDouble(left.Value) >= Convert.ToDouble(right.Value);
        }

        public static bool operator <=(Number left, Number right)
        {
            return Convert.ToDouble(left.Value) <= Convert.ToDouble(right.Value);
        }

        //public static bool operator ==(Number left, Number right)
        //{
        //    return Convert.ToDouble(left.Value) == Convert.ToDouble(right.Value);
        //}

        //public static bool operator !=(Number left, Number right)
        //{
        //    return Convert.ToDouble(left.Value) == Convert.ToDouble(right.Value);
        //}


        public override bool Equals(object obj)
        {
            if (base.Equals(obj)) return true;
            if (ReferenceEquals(obj, Null.Instance)) return false;
            return this.Equals((Number)obj);
        }

        public bool Equals(Number num)
        {
            if (ReferenceEquals(null, num)) return false;
            if (ReferenceEquals(this, num)) return true;
            return Convert.ToDouble(this.Value) == Convert.ToDouble(num.Value);
        }

    }
}
