using System;

namespace Joddes.CS.Html5
{
	[Hidden, Native("NodeList")]
	public class NodeList
	{
        public Node this[int index] {
            get {
                throw new NotSupportedException ();
            }
        }
        /*
        public Node this[string name] {
            get {
                throw new NotSupportedException();
            }
        }*/

        [Native("length")]
        public int Length {
            get {
                throw new NotSupportedException ();
            }
        }

        /*[Native("item")]
        public Node Item (int index)
        {
            throw new NotSupportedException();
        }*/
	}
}