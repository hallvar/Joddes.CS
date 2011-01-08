using System;
using System.Collections.Generic;
using Joddes.CS.TestFramework;

namespace Joddes.CS.TestFramework
{
    public class TestRunner
    {
        List<object[]> testMethods;
        IEnumerator<object[]> testRun;

        public delegate void TestFinishedEventHandler(Test test, System.Reflection.MethodInfo method, TimeSpan time, Exception exception);
        public event TestFinishedEventHandler OnTestFinished;

        public delegate void TestsFinishedEventHandler();
        public event TestsFinishedEventHandler OnTestsFinished;

        public TestRunner ()
        {
            testMethods = new List<object[]>();
        }

        public void Run (Test[] tests)
        {
            foreach (Test test in tests)
            {
                var methods = GetTestMethods (test);
                foreach (System.Reflection.MethodInfo method in methods) {
                    this.testMethods.Add (new object[] { test, method });
                }
            }

            this.testRun = testMethods.GetEnumerator ();
            OnTestFinished += (t, method, time, exception) =>
            {
                if (testRun.MoveNext ()) {
                    RunTest (((Test)((object[])testRun.Current)[0]), ((System.Reflection.MethodInfo)((object[])testRun.Current)[1]));
                } else {
                    if (OnTestsFinished != null)
                    {
                        OnTestsFinished();
                    }
                }
            };

            if (testRun.MoveNext ()) {
                RunTest ((Test)((object[])testRun.Current)[0], (System.Reflection.MethodInfo)((object[])testRun.Current)[1]);
            }
        }

        public void RunTest (Test test, System.Reflection.MethodInfo method)
        {
            var startTime = DateTime.Now;

            test.Setup ();
            var attributes = (TestAttribute[])method.GetCustomAttributes (typeof(TestAttribute), false);

            if (attributes[0].Async) {
                try {
                    method.Invoke (test, new object[] { (AsyncCallback)(result =>
                    {
                        var t = (Test)((object[])result.AsyncState)[0];
                        var m = (System.Reflection.MethodInfo)((object[])result.AsyncState)[1];
                        var s = (DateTime)((object[])result.AsyncState)[2];

                        t.TearDown ();

                        if (OnTestFinished != null)
                        {
                            OnTestFinished (t, m, DateTime.Now - s, null);
                        }
                    }), new object[] { test, method, startTime } });
                } catch (Exception ex)
                {
                    if (OnTestFinished != null)
                    {
                        OnTestFinished(test, method, DateTime.Now - startTime, ex);
                    }
                }
            } else {
                Exception ex = null;
                try {
                    method.Invoke (test, null);
                } catch (Exception e)
                {
                    ex = e;
                }
                test.TearDown ();

                if (OnTestFinished != null) {
                    OnTestFinished (test, method, DateTime.Now - startTime, ex);
                }
            }
        }

        public List<System.Reflection.MethodInfo> GetTestMethods (Test test)
        {
            var methods = new List<System.Reflection.MethodInfo> ();

            var type = test.GetType ();
            System.Reflection.MethodInfo[] allMethods = type.GetMethods ();

            foreach (System.Reflection.MethodInfo m in allMethods)
            {
                object[] attributes = m.GetCustomAttributes (typeof(TestAttribute), false);
                if (attributes.Length > 0)
                {
                    methods.Add(m);
                }
            }

            return methods;
        }
    }
}