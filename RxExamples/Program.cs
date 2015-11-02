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
            var observable = Observable.Throw<Exception>(new Exception("Burp"));
            observable.Subscribe(i => Console.Out.WriteLine(i),
                error => Console.Out.WriteLine(error),
                () => Console.Out.WriteLine("Completed"));
            Console.ReadKey();
        }
    }
}
