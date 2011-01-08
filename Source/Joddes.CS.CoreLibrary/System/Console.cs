using System;
using Joddes.CS.Html5;

namespace System
{
    public class Console
    {
        private Console ()
        {
        }

        public static void Write(string message) {
            Joddes.CS.Html5.Console.Log(message);
        }

        public static void WriteLine(string message) {
            Joddes.CS.Html5.Console.Log(message+"\n");
        }
    }
}