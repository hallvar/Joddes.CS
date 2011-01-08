namespace Joddes.CS.Html5 {
	[Hidden]
	public class HTMLCanvasElement : HTMLElement
	{
		[Native("getContext")]
	    public object GetContext (string name)
	    {
            throw new System.NotSupportedException ();
	    }
		
		[Native("getContext")]
		public object GetContext (string name, WebGLContextAttributes attribtues)
		{
		    throw new System.NotSupportedException ();
		}

        [Native("toDataURL")]
        public string ToDataURL ()
        {
            throw new System.NotSupportedException ();
        }

        [Native("toDataURL")]
        public string ToDataURL (string type)
        {
            throw new System.NotSupportedException();
        }

        public int width;
        public int height;
	}
}