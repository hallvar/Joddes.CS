using System;
namespace System
{
    public interface IObserver<T>
    {
        void OnNext(T value);
        void OnCompleted();
        void OnError(Exception error);
    }
}