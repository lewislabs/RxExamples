using System;
using System.Reactive.Linq;

namespace RxExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var observable = new MyRandomObservable();
            var disposable = observable
                .Scan(0, (acc, current) => acc + current)
                .Subscribe(i => Console.Out.WriteLine($"Running Total: {i}"));
            Console.ReadKey();
            disposable.Dispose();
            Console.ReadKey();
        }
    }
}
