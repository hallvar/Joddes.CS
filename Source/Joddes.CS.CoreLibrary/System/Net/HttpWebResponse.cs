using System.IO;

namespace System.Net
{
	public class HttpWebResponse : WebResponse
	{
		internal byte[] buffer {get;set;}
		
		internal HttpWebResponse ()
		{
		}
		
		public override Stream GetResponseStream ()
		{
			return new MemoryStream (this.buffer);
		}
	}
}