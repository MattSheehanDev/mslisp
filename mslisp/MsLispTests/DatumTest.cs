using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsLisp;
using MsLisp.Datums;

namespace MsLispTests
{
    [TestClass]
    public class DatumTest
    {
        [TestMethod]
        public void stringify(string s, IDatum datum)
        {
            Assert.AreEqual(s, datum.ToString());
        }


    }
}
