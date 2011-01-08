using System;
using System.Collections.Generic;

namespace Joddes.CS.CoreLibrary.Extensions.System.IO
{
    public class IOObservable<T> : IObservable<T>
    {
        List<IObserver<T>> observers = new List<IObserver<T>>();

        public IOObservable ()
        {
        }

        public IDisposable Subscribe (IObserver<T> observer)
        {
            observers.Add (observer);

            // TODO
            return null;
        }

        public void OnNext (T obj)
        {
            foreach (IObserver<T> o in observers)
            {
                o.OnNext (obj);
            }
        }

        public void OnCompleted ()
        {
            //observers.ForEach(o => o.OnCompleted);
            foreach (IObserver<T> o in observers)
            {
                o.OnCompleted ();
            }
        }

        public void OnError (Exception ex)
        {
            foreach (IObserver<T> o in observers) {
                o.OnError (ex);
            }
        }
    }
}