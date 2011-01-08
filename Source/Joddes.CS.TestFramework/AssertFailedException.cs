using System;
namespace Joddes.CS.TestFramework
{
    public class AssertFailedException : Exception
    {
        public AssertFailedException (string message) : base(message)
        {
        }
    }
}