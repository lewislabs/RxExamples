using System;
using System.Reactive.Linq;

namespace RxExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var observable = new MyRandomObservable();
            var disposable1= observable
                .Where(i=>i%2==0)
                .Subscribe(i =>
                {
                    Console.Out.WriteLine($"Subscription 1 - Got {i}");
                });
            var disposable2 = observable
                .Skip(5)
                .Subscribe(i =>
                {
                    Console.Out.WriteLine($"Subscription 2 - Got {i}");
                });
            var disposable3 = observable
                .FirstAsync()
                .Subscribe(i =>
                {
                    Console.Out.WriteLine($"Subscription 3 - Got {i}");
                }, () =>
                {
                    Console.Out.WriteLine("I'm Done!");
                });
            Console.ReadKey();
            disposable1.Dispose();
            disposable2.Dispose();
            disposable3.Dispose();
            Console.ReadKey();
        }
    }
}
