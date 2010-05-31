
using System;

namespace Joddes.CS.Html5
{
	[Hidden, Native("XMLHttpRequest")]
	public class XmlHttpRequest
	{
		public XmlHttpRequest ()
		{
		}
		
		// event handler attributes
        public delegate void ReadyStateChangeDelegate (XmlHttpRequestProgressEvent e);
		
		[Native("readystatechange")]
		public event ReadyStateChangeDelegate ReadyStateChange;

		  // states
		  public short UNSENT = 0;
		  public short OPENED = 1;
		  public short HEADERS_RECEIVED = 2;
		  public short LOADING = 3;
		  public short DONE = 4;
		
		  public short readyState { get; private set;}
		
		  // request
		[Native("open")]
		  public void Open (string method, string url)
		  {
			
		  }
		[Native("open")]
		  public void Open (string method, string url, bool async)
		  {
		  }
		[Native("open")]
		  public void Open (string method, string url, bool async, string user)
		  {
		  }
		[Native("open")]
		  public void Open (string method, string url, bool async, string user, string password)
		  {
		  }
		[Native("setRequestHeader")]
		  public void SetRequestHeader (string header, object value)
		  {
			
		  }
		[Native("send")]
		  public void Send ()
		  {
			
		  }
		[Native("send")]
		  public void Send (Document data)

		{
		  }
		[Native("send")]
		  public void Send (string data)

		{
		  }
		[Native("abort")]
		  public void Abort ()

		{
		}
		
		  // response
		[Native("status")]  
		public short Status {get; private set;}
		
		[Native("statusText")]  
		public string StatusText {get; private set;}
		
		[Native("getResponseHeader")]
		public string GetResponseHeader (string header)
		  {
		  	return null;
		}
		
		[Native("getAllResponseHeaders")]
		  public string GetAllResponseHeaders ()

		{
		  	return null;
		  }
		
		[Native("responseText")]
		public string ResponseText { get; private set; }
		
		[Native("responseXML")] 
		public Document ResponseXML { get; private set; }
	}
}