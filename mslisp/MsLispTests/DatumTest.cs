using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsLisp;
using MsLisp.Datums;

namespace MsLispTests
{
    [TestClass]
    public class DatumTest
    {
        public void Stringify(string s, IDatum datum)
        {
            Assert.AreEqual(s, datum.ToString());
        }

        [TestMethod]
        public void AtomString()
        {
            Stringify("test", new Atom("test"));
        }

        [TestMethod]
        public void BoolString()
        {
            Stringify("T", Bool.True);
        }

        [TestMethod]
        public void NullString()
        {
            Stringify("NIL", Null.Instance);
        }

        [TestMethod]
        public void NumberString()
        {
            Stringify("5", new Number(5));
            Stringify("5.5", new Number(5.5));
        }

        [TestMethod]
        public void VectorString()
        {
            var arr = new IDatum[3];
            arr[0] = new Atom("test");
            arr[1] = new Number(8);
            arr[2] = Bool.True;
            var vec = new Vector(arr);

            Stringify("(test 8 T)", vec);
        }

        //[TestMethod]
        //public void AtomType()
        //{
        //    Assert.Equals(Null.Instance.GetType(), typeof(Atom));
        //}

    }
}
