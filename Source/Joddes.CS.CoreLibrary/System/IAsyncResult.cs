using System.Threading;

namespace System
{
	public interface IAsyncResult
	{
		object AsyncState { get; }
		bool CompletedSynchronously { get; }
		bool IsCompleted { get; }
	    WaitHandle AsyncWaitHandle { get; }
	}
}