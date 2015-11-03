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
            Observable.Range(0, 10)
                .Subscribe(i => Console.Out.WriteLine($"Observable.Range Got {i}"),
                         () => Console.Out.WriteLine("Observable.Range Completed"));

            Observable.Generate(0,
                i => i <= 10,
                i => i + 2,
                i => i)
                .Subscribe(i => Console.Out.WriteLine($"Observable.Generate Got {i}"),
                        () => Console.Out.WriteLine("Observable.Generate Completed"));

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(i => Console.Out.WriteLine($"Observable.Interval Got {i}"));

            Observable.Timer(TimeSpan.FromSeconds(10))
                .Subscribe(i => Console.Out.WriteLine($"Observable.Timer Got {i}"),
                        () => Console.Out.WriteLine("Observable.Timer Completed"));

            Console.ReadKey();
        }
    }
}
