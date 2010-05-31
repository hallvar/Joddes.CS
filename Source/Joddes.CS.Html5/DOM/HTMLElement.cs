using System;

namespace Jsm.Html5 {
	[Hidden]
	public abstract class HTMLElement
	{
		public NodeList getElementsByClassName (string classNames)
	
		{
			return null;
		}
		
		public string innerHTML {
			get; set;
		}
		
		public string outerHTML {
			get; set;
		}
		
		public string id {
			get; set;
		}
		
		public string title {
			get; set;
		}
		
		public string lang {
			get; set;
		}
		
		public string dir {
			get; set;
		}
		
		public string className {
			get; set;
		}
		
		public DOMTokenList classList {
			get; private set;
		}
		
		public DOMStringMap dataset {
			get; private set;
		}
		
		public bool itemScope {
			get; set;
		}
		
		public string itemType {
			get; set;
		}
		
		public string itemId {
			get; set;
		}
		
		public void click ()
		{}
		
		public void scrollIntoView ()
		{}
		
		public void scrollIntoView (bool top)
		{}
		
		public long tabIndex {
			get; set;
		}
		
		public void blur ()
		{}
		public void focus ()
		{}
		
		public CSSStyleDecleration style {
			get; set;
		}
		/*
		public Func<object, object> onabort {
			get; set;
		}*/
		
		[Native("appendChild")]
		public void AppendChild (HTMLElement element)
		{
		}
	}
}