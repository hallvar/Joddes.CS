namespace System.IO
{
	public class MemoryStream : Stream
	{
		private byte[] buffer;
		private int position;
		
		public bool CanRead {get { return true; }}
		
		public MemoryStream (byte[] buffer)
		{
			this.buffer = buffer;
		}
		
		public int Read (byte[] buffer, int offset, int length)
		{
			int i = offset;
			
			int min = (length < this.buffer.Length - this.position) ? length : this.buffer.Length - this.position;
			for (; i < min; i++) {
				buffer[i] = this.buffer[this.position++];
			}
			
			return min;
		}

        public int Write (byte[] buffer, int offset, int length)
        {
            this.buffer = buffer;

            return this.buffer.Length;
        }
	}
}