
using System;

namespace Jsm.Html5
{
	public interface IWindow
	{
		Document Document { get; set; }		
		Window Open (string uri);
		
		/*[Native("setTimeout")]
		public TimeoutHandle SetTimeout (Func<Void> func, int milliseconds)
		{
			return null;
		}
		
		[Native("setInterval")]
		public IntervalHandle SetInterval (Func<Void> func, int milliseconds)
		{
			return null;
		}*/
		
		void ClearTimeout (TimeoutHandle handle);
		void ClearInterval (IntervalHandle handle);
		void Alert (object o);
		/*
		event DOMContentLoadEventHandler DOMContentLoaded;
		event DOMContentLoadEventHandler Load;
		event DOMEventHander Online;
		event DOMEventHander Offline;*/
	}
}