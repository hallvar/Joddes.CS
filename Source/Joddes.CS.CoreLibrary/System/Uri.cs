namespace System
{
	public class Uri
	{
		public string AbsoluteUri { get; private set; }
		
		public Uri (string uri)
		{
			AbsoluteUri = uri;
		}
	}
}