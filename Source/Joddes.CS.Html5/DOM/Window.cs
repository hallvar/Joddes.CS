namespace Jsm.Html5
{
	[Hidden, Native("window")]
	public class Window : IWindow
	{
		[Native("self")]
		public static Window Self { get; set; }
		
		[Native("document")]
		public Document Document { get; set; }
		
		[Native("open")]
		public Window Open (string uri)
		{
			return null;
		}
		
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
		
		[Native("clearTimeout")]
		public void ClearTimeout (TimeoutHandle handle)
		{
		}
		
		[Native("clearInterval")]
		public void ClearInterval (IntervalHandle handle)
		{
		}
		
		[Native("alert")]
		public void Alert (object o)
		{
		}
		
		[NativeInline("debugger")]
		public static void Debugger() {
		}
		
		public delegate void DOMContentLoadEventHandler (object e);
		
		public delegate void DOMEventHander(object e);
		
		[Native("DOMContentLoaded")]
		public event DOMContentLoadEventHandler DOMContentLoaded;
		
		[Native("load")]
		public event DOMContentLoadEventHandler Load;
		
		[Native("online")]
		public event DOMEventHander Online;
		
		[Native("offline")]
		public event DOMEventHander Offline;
	}
}