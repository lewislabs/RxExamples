using System;

namespace RxExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var observable = new MyRandomObservable();
            var disposable = observable.Subscribe(new MyRandomObserver());
            Console.ReadKey();
            disposable.Dispose();
            Console.ReadKey();
        }
    }
}
