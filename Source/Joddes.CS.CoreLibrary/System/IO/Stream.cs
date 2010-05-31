namespace System.IO
{
	public abstract class Stream
	{
		public bool CanRead { get; private set; }
		public bool CanWrite { get; private set; }
		public bool CanSeek { get; private set; }
		public bool CanTimeout { get; private set; }
		public long Length { get; private set; }
		public long Position { get; private set; }
		
		public virtual IAsyncResult BeginRead (byte[] buffer, int offset, int length, AsyncCallback callback, object state)
		{
			throw new NotSupportedException ();
		}
		
		public virtual int EndRead (IAsyncResult result)
		{
			throw new NotSupportedException ();
		}
		
		public virtual IAsyncResult BeginWrite (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException ();
		}
		
		public virtual void EndWrite (IAsyncResult result)
		{
			throw new NotSupportedException ();
		}
		
		public virtual int Read (byte[] buffer, int offset, int length)
		{
			throw new NotSupportedException ();
		}
		
		public virtual void Write (byte[] buffer, int offseth, int count)
		{
			throw new NotSupportedException ();
		}
	}
}