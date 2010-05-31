using System.IO;

namespace System.Net
{
	public abstract class WebResponse
	{
		public int ContentLength { get; private set; }
		public string ContentType { get; private set; }
		public string Headers { get; private set; }
		public bool IsFromCache { get; private set; }
		public bool IsMutuallyAuthenticated { get; private set; }
		public Uri ResponseUri {get; private set; }
		
		public WebResponse ()
		{
		}
		
		public void Close ()
		{
		}
		
		public virtual Stream GetResponseStream ()
		{
			return null;
		}
	}
}