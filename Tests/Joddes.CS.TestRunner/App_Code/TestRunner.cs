using System;
using Joddes.CS.TestFramework;
using Joddes.CS.Html5;

namespace Joddes.CS.TestRunner
{
    public class TestRunner
    {
        Joddes.CS.TestFramework.TestRunner testRunner;
        Date startTime;

        public TestRunner ()
        {
            testRunner = new Joddes.CS.TestFramework.TestRunner ();

            testRunner.OnTestFinished += (test, method, time, exception) =>
            {
                var el = Window.Self.Document.GetElementById (method.Name);

                el.className = exception == null ? "succeeded" : "failed";

                el.innerHTML += " <span class=\"time\">"+time.TotalMilliseconds+" ms</span>";

                if (exception != null) {
                    var type = exception.GetType ();

                    var msg = Window.Self.Document.CreateElement ("div");
                    msg.className = "errorMessage";
                    if (exception.Message != null) {
                        msg.innerHTML = type.FullName +": " + exception.Message;
                    } else {
                        msg.innerHTML = ""+exception;
                    }
                    el.AppendChild(msg);
                }
            };

            testRunner.OnTestsFinished += () =>
            {
                var el = Window.Self.Document.GetElementById ("timer");
                var date = new Date ();
                var time = date.GetTime ();
                long ms = time - (long)((Joddes.CS.Html5.Object)(object)Window.Self)["startTime"];
                el.innerHTML = ms.ToString ();

                el = Window.Self.Document.GetElementById ("timer2");
                var stime = startTime.GetTime ();
                ms = time - stime;
                el.innerHTML = ms.ToString();
            };
        }

        public void Run (Test[] tests)
        {
            startTime = new Date ();

            foreach (Test test in tests)
            {
                var title = Window.Self.Document.CreateElement ("h3");
                Type type = test.GetType ();
                title.innerHTML = "<span class=\"testclass\">" + type.FullName + "</span>";
                var el = Window.Self.Document.CreateElement ("ul");
    
                var testEl = Window.Self.Document.GetElementById ("tests");

                testEl.AppendChild (title);
                testEl.AppendChild (el);
    
                var methods = testRunner.GetTestMethods (test);
                foreach (System.Reflection.MethodInfo method in methods) {
                    var tel = Window.Self.Document.CreateElement("li");
                    tel.id = method.Name;
                    tel.innerHTML = "<span class=\"name\">"+method.Name.Replace("_", " ")+"</span>";
                    el.AppendChild(tel);
                }
            }

            testRunner.Run(tests);
        }
    }
}