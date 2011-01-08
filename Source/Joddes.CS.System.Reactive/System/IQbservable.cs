using System;
using System.Linq.Expressions;

namespace System
{
    public interface IQbservable
    {
        Type ElementType { get; }
        Expression Expression { get; }
        IQbservableProvider Provider { get; }
    }

    public interface IQbservable<T> : IQbservable, IObservable<T>
    {
    }
}