using System;
using System.Collections.Generic;

namespace System.Linq
{
	public static class Enumerable
	{
		public static IEnumerable<TSource> Where<TSource> (this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			//Check.SourceAndPredicate (source, predicate);
			
			//return CreateWhereIterator (source, predicate);
			return source;
		}
	}
}