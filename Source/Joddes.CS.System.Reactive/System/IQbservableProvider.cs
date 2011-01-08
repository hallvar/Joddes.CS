using System;
using System.Linq.Expressions;

namespace System
{
    public interface IQbservableProvider
    {
        IQbservable<TResult> CreateQuery<TResult> (Expression expression);
    }
}