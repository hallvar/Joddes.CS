using System.Collections.Generic;

namespace System.Linq
{
    public interface IQueryable {

    }

	public interface IQueryable<T> : IQueryable, IEnumerable<T>
	{
		//IQueryProvider 
	}
}