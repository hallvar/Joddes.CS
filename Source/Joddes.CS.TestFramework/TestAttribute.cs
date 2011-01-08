using System;
namespace Joddes.CS.TestFramework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class TestAttribute : Attribute
    {
        public bool Async { get; set; }

        public TestAttribute ()
        {
        }
    }
}