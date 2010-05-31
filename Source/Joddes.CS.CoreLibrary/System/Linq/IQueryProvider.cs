namespace System.Linq
{
	public interface IQueryProvider
	{
		IQueryable<T> CreateQuery<T> (Expressions.Expression expression);
		IQueryable CreateQuery (System.Linq.Expressions.Expression expression);
	}
}