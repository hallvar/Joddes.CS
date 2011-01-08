using System;

namespace System
{
    public static class IObservableExtensions
    {
        /*public static IDisposable Subscribe<TSource> (this IObservable<TSource> source)
        {
            throw new NotImplementedException();
        }*/

        public static IDisposable Subscribe<TSource> (this IObservable<TSource> source, Action<TSource> onNext)
        {
            return source.Subscribe<TSource> (onNext, exception =>
            {
                //throw exception.PrepareForRethrow ();
                throw exception;
            }, () => { });
        }

        public static IDisposable Subscribe<TSource> (this IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            throw new NotImplementedException ();
        }
        public static IDisposable Subscribe<TSource> (this IObservable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            throw new NotImplementedException ();
        }
        public static IDisposable Subscribe<TSource> (this IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            throw new NotImplementedException ();
        }
    }
}