using System;

namespace RxExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var observable = new MyRandomObservable();
            observable.Subscribe(new MyRandomObserver());
            Console.ReadKey();
        }
    }
}
