namespace Joddes.CS.Html5
{
	[Hidden, Native("XMLHttpRequestProgressEvent")]
	public class XmlHttpRequestProgressEvent
	{
		public bool lengthComputable;
		public long loaded;
		public long total;
		public long loadedItems;
		public long totalItems;		
	}
}