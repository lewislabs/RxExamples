using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RxExamples
{
   class Program
    {
        static void Main(string[] args)
        {
            var observable = Observable.Create<int>(observer =>
                            {
                                Console.Out.WriteLine($"Thread ID={Thread.CurrentThread.ManagedThreadId}");
                                Thread.Sleep(5000);
                                observer.OnNext(1);
                                observer.OnNext(2);
                                observer.OnNext(3);
                                observer.OnCompleted();
                                return Disposable.Empty;
                            });
            observable
                .SubscribeOn(NewThreadScheduler.Default)
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(i =>
                           {
                               Console.Out.WriteLine($"Got {i} on ThreadId={Thread.CurrentThread.ManagedThreadId}");
                               Thread.Sleep(1000);
                           });
            Thread.Sleep(5000);
            Console.ReadKey();
        }
    }
}
