
using System.Net;

namespace System.Net
{
	public abstract class WebRequest
	{
		
		public WebRequest ()
		{
		}
		
		public static WebRequest Create (Uri uri)
		{
			return new HttpWebRequest () {
				uri = uri
			};
		}
		
		public IAsyncResult BeginGetResponse (AsyncCallback callback, object state)
		{
			throw new NotImplementedException ();
		}
		
		public WebResponse EndGetResponse (IAsyncResult result)
		{
			throw new NotImplementedException ();
		}
	}
}