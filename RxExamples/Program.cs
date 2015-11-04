using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Cryptography.X509Certificates;

namespace RxExamples
{
    public class EventClass
    {
        public event EventHandler<int> AnEvent;

        public void DoIt(int i)
        {
            AnEvent?.Invoke(this, i);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var eventClass = new EventClass();
            var eventObservable = Observable.FromEventPattern<int>(h =>
            {
                eventClass.AnEvent += h;
                Console.Out.WriteLine("Added handler to the event");
            }, h =>
            {
                eventClass.AnEvent -= h;
                Console.Out.WriteLine("Removed handler from the event");
            });
            Console.ReadKey();
            var disposable = eventObservable.Subscribe(i =>
            {
                Console.Out.WriteLine($"Got {i.EventArgs}");
            });
            eventClass.DoIt(10);
            Console.ReadKey();
            disposable.Dispose();
            Console.ReadKey();
        }
    }
}
