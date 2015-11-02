using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace RxExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var observable = Observable.Create<int>(observer =>
            {
                Console.Out.WriteLine("New Subscription");
                observer.OnNext(0);
                observer.OnNext(1);
                observer.OnNext(2);
                observer.OnCompleted();
                return Disposable.Empty;
            });
            observable.Subscribe(i => Console.Out.WriteLine(i),
                error => Console.Out.WriteLine(error),
                () => Console.Out.WriteLine("Completed"));
            Console.ReadKey();
        }
    }
}
