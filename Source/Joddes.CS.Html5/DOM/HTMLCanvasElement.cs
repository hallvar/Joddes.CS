
using System;

namespace Jsm.Html5 {
	[Hidden]
	public class HTMLCanvasElement : HTMLElement
	{
		[Native("getContext")]
	     public object GetContext (string name)
	     {
	     	return null;
	     }
		
		[Native("getContext")]
		public object GetContext (string name, WebGLContextAttributes attribtues)
		{
			return null;
		}
	}
}