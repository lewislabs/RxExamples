using System;
using System.Reactive.Disposables;
using System.Timers;

namespace RxExamples
{
    public sealed class MyRandomObservable : IObservable<int>
    {
        public IDisposable Subscribe(IObserver<int> observer)
        {
            var count = 0;
            ElapsedEventHandler handler = (s, e) =>
            {
                count++;
                observer.OnNext(count);
            };
            observer.OnNext(count);
            var t = new Timer { Interval = 500 };
            t.Elapsed += handler;
            t.Start();
            return Disposable.Create(() =>
            {
                t.Stop();
                t.Elapsed -= handler;
                Console.Out.WriteLine("Disposed");
            });
        }
    }
}
