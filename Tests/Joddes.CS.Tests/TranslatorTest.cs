using System;
using Joddes.CS.TestFramework;

namespace Joddes.CS.Tests
{
    public class TranslatorTest : Test
    {
        [Test]
        public void TestInt ()
        {
            //var test = 1;

            //Assert.AreEqual (1, test);
        }

        [Test]
        public void TestString ()
        {
            var test = "test";

            Assert.AreEqual ("test", test);
        }

        [Test]
        public void TestBooleanTrue ()
        {
            var test = true;

            //Assert.AreEqual (true, test);
        }

        [Test]
        public void TestBooleanFalse ()
        {
            var test = false;

            //Assert.AreEqual (false, test);
        }

        [Test]
        public void TestBoolean1 ()
        {
            var test = 1 == 1;

            //Assert.AreEqual (true, test);
        }

        [Test]
        public void TestBoolean2 ()
        {
            var test = 1 != 1;

            //Assert.AreEqual (false, test);
        }
    }
}