namespace System.Threading
{
    public class AutoResetEvent : WaitHandle
    {
        bool state;

        public AutoResetEvent (bool initialState)
        {
            state = initialState;
        }

        public void Set ()
        {
            state = true;
        }

        public override bool WaitOne ()
        {
            while (!state)
            {
            }
            return true;
        }
    }
}