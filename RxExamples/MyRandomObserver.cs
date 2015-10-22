using System;

namespace RxExamples
{
    public sealed class MyRandomObserver : IObserver<int>
    {
        public void OnNext(int value)
        {
            Console.Out.WriteLine("Got {0}",value);
        }

        public void OnError(Exception error)
        {
            Console.Out.WriteLine("Something terrible happened: {0}", error);
        }

        public void OnCompleted()
        {
            Console.Out.WriteLine("And we're done!");
        }
    }
}
