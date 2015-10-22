using System;

namespace RxExamples
{
    public sealed class MyRandomObservable : IObservable<int>
    {
        public IDisposable Subscribe(IObserver<int> observer)
        {
            observer.OnNext(0);
            observer.OnNext(2);
            observer.OnNext(3);
            observer.OnCompleted();
            return new MyDisposable();
        }

        private sealed class MyDisposable : IDisposable
        {
            public void Dispose()
            {
                Console.Out.WriteLine("Disposed Subscription");
            }
        }
    }
}
