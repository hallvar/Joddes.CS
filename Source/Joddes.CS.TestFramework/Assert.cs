using System;
namespace Joddes.CS.TestFramework
{
    public class Assert
    {
        public static void AreEqual (string expected, string actual)
        {
            if (expected != actual)
            {
                throw new AssertFailedException ("Expected: " + expected + ", Actual: " + actual);
            }
        }

        public static void NotNull (object obj)
        {
            if (obj == null)
            {
                throw new AssertFailedException ("Expected something else than null");
            }
        }

        public static void Null (object obj)
        {
            if (obj == null) {
                throw new AssertFailedException ("Expected null");
            }
        }

        public static void Fail (string message)
        {
            throw new AssertFailedException(message);
        }
    }
}