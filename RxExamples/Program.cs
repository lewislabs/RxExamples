using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Timer = System.Timers.Timer;

namespace RxExamples
{
   class Program
    {
        static void Main(string[] args)
        {
            var observable = Observable.Create<int>(observer =>
                            {
                                Console.Out.WriteLine("Starting the timer.........");
                                var timer = new Timer {Interval = 1000};
                                var count = 0;
                                timer.Elapsed += (sender, eventArgs) =>
                                                {
                                                    Console.Out.WriteLine($"Publishing {count}");
                                                    observer.OnNext(count++);
                                                };
                                timer.Start();
                                return Disposable.Create(() =>
                                                {
                                                    timer.Stop();
                                                    Console.Out.WriteLine("Disposed..");
                                                    timer.Dispose();
                                                });
                            });
            Console.Out.WriteLine("Making the observable hot");
            var refCountObservable = observable
                .Publish()
                .RefCount();
            Thread.Sleep(1000);
            Console.Out.WriteLine("Subscription 1");
            var subscription1 = refCountObservable.Subscribe(i => Console.Out.WriteLine($"Subscription 1 Got {i}"));
            Thread.Sleep(2000);
            Console.Out.WriteLine("Subscription 2");
            var subscription2 = refCountObservable.Subscribe(i => Console.Out.WriteLine($"Subscription 2 Got {i}"));
            Console.ReadKey();
            Console.Out.WriteLine("Disposing Subscription 1");
            subscription1.Dispose();
            Console.ReadKey();
            Console.Out.WriteLine("Disposing Subscription 2....the stream dies as well.");
            subscription2.Dispose();
            Console.ReadKey();
        }
    }
}
