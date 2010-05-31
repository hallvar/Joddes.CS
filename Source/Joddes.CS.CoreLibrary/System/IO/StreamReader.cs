
using System;

namespace System.IO
{
	public class StreamReader
	{
		Stream stream;
		
		public StreamReader (Stream stream)
		{
			this.stream = stream;
		}
		
		public string ReadToEnd ()
		{
		    if (!stream.CanRead) 
			{
		        throw new System.NotSupportedException ();
		    }
		 
			byte[] result = new byte[0];
		    // length can be zero in JS
		    //string buffer = "                                                   ";
		    byte[] buffer = new byte[256];
		    int pos = 0;
		    int read = -1;
		 
			while ((read = stream.Read ((byte[])(object)buffer, 0, 256/*buffer.Length*/)) > 0)
			{
		        for (int i = 0; i < read; i++) {
		            result[pos + i] = buffer[i];
		        }
		        pos += read;
		    }
		 
			/*
		var result = stream.BeginRead (buffer, 0, 256, r =>
		{
		var length = stream.EndRead (r);
		}, null);
		
			result.AsyncWaitHandle.WaitOne ();
		*/
		if (pos == 0) {
				return null;
			}
			
			//return string.Join("", (string[])(object)result);
			return result.join ("");
		}
	}
}