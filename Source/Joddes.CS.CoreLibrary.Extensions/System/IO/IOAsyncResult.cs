using System;
using System.Threading;

namespace Joddes.CS.CoreLibrary.Extensions.System.IO
{
    internal class IOAsyncResult : IAsyncResult
    {
        public object AsyncState { get; private set; }

        public object Data { get; set; }

        public bool CompletedSynchronously { get; private set; }

        public bool IsCompleted { get; private set; }

        public WaitHandle AsyncWaitHandle { get; private set;
        }
        
        public IOAsyncResult (object asyncState)
        {
            AsyncState = asyncState;
        }
    }
}