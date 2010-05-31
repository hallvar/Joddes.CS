namespace Joddes.CS.Html5 {
	[Hidden, Native("HTMLDocument")]
	public class Document
	{
		[Native("getElementById")]
		public object GetElementById (string id)
		{
			return null;
		}
		
		[Native("getElementsByTagName")]
		public object[] GetElementsByTagName (string tagName)
		{
			return null;
		}
		
		[Native("createElement")]
		public HTMLElement CreateElement (string tagName)
		{
			return null;
		}
		
		[Native("createTextNode")]
		public HTMLElement CreateTextNode (string text)
		{
			return null;
		}
		
		[Native("body")]
		public HTMLElement Body {
			get {
				return null;
			}
		}

        [Native("outerHTML")]
        public string OuterHtml {
            get {
                throw new System.NotSupportedException ();
            }
            set {
                throw new System.NotSupportedException();
            }
        }
	}
}