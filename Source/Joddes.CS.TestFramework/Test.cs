using System;
namespace Joddes.CS.TestFramework
{
    public abstract class Test
    {
        public Test ()
        {
        }

        public virtual void Setup ()
        {
        }

        public virtual void TearDown ()
        {
        }
    }
}