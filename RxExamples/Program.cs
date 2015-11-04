using System;
using System.Reactive.Subjects;

namespace RxExamples
{
    public class MyMutable
    {
        public int Item { get; set; }

        public MyMutable(int item)
        {
            Item = item;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var subject = new Subject<int>();
            subject.Subscribe(i =>
            {
                Console.Out.WriteLine($"Subscription 1 Got {i}");
            });
            subject.OnNext(1);
            Console.ReadKey();
            var replaySubject = new ReplaySubject<MyMutable>(1);
            replaySubject.Subscribe(i =>
            {
                Console.Out.WriteLine($"Replay Subscription 1 Got {i.Item}");
                i.Item = i.Item + 1; // BAD IDEA!
            });
            //Publish
            replaySubject.OnNext(new MyMutable(1));
            // Make a 2nd subscription
            replaySubject.Subscribe(i =>
            {
                Console.Out.WriteLine($"Replay Subscription 2 Got {i.Item}");
            });
            Console.ReadKey();
        }
    }
}
