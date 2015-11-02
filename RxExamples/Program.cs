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
                .Where(i=>i%2==0)
                .Subscribe(new MyRandomObserver());
            Console.ReadKey();
            disposable.Dispose();
            Console.ReadKey();
        }
    }
}
