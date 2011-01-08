namespace System
{
	//[Hidden]
	public class Exception
	{
        public Exception InnerException { get; private set; }
        public virtual string Message { get; private set; }
        /*
		public Exception ()
		{
		}*/
		
		public Exception (string message)
		{
            Message = message;
		}
		/*
		public Exception (string message, Exception innerException)
		{
            Message = message;
            InnerException = innerException;
		}*/

        public override string ToString ()
        {
            Type type = this.GetType();
            return type.FullName + ": " + Message;
        }
	}
}
