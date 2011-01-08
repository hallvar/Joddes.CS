using System;
namespace Joddes.CS.Tests
{
    public class ClassWithMethodsPropertiesFieldsEventsAndDelegates
    {
        public delegate void MyDelegate(string str);
        public event MyDelegate MyEvent;

        public string MyField;

        public string MyProperty {get;set;}

        public ClassWithMethodsPropertiesFieldsEventsAndDelegates ()
        {
        }

        public string MyMethodCausesMyEventWithString (string str)
        {
            if (MyEvent != null)
            {
                MyEvent (str);
            }

            return str;
        }
    }
}