using System;
using System.Threading;

namespace Joddes.CS.CoreLibrary.Tests
{
	internal class NetAsyncResult : IAsyncResult
	{
		public object AsyncState {
			get; private set;
		}
		
		public bool CompletedSynchronously {
			get; private set;
		}
		
		public bool IsCompleted {
			get; private set;
		}
		
		public WaitHandle AsyncWaitHandle {
			get; private set;
		}
		
		public NetAsyncResult (object asyncState)
		{
			AsyncState = asyncState;
		}
	}
}