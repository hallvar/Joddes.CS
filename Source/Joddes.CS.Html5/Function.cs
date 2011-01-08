using System;
namespace Joddes.CS.Html5
{
    [Hidden, Native("Function")]
    public class Function
    {
        [Native("apply")]
        public object apply (object scope, object[] args)
        {
            throw new NotSupportedException ();
        }

        [Native("name")]
        public string name {
            get {
                throw new NotSupportedException();
            }
        }
    }
}