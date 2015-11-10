using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RxExamples
{
    public class HealthStatusEventArgs : EventArgs
    {
        public bool Status { get; private set; }

        public HealthStatusEventArgs(bool status)
        {
            Status = status;
        }
    }

    public class WorkerNode
    {
        private bool _running;

        public event EventHandler<HealthStatusEventArgs> NodeHealth;

        public void StartWork()
        {
            if (!_running)
            {
                Task.Run(() =>
                {
                    _running = true;
                    while (_running)
                    {
                        //do some work.
                        Thread.Sleep(200);
                        //report health
                        NodeHealth?.Invoke(this, new HealthStatusEventArgs(true));
                    }
                    NodeHealth?.Invoke(this, new HealthStatusEventArgs(false));
                }); 
            }
        }

        public void Stop()
        {
            _running = false;
        }
    }


    public class NodeManager
    {
        public IObservable<int> NodesRunning { get; private set; }
        public int NodeCount { get; private set; }

        public NodeManager(IEnumerable<WorkerNode> nodes)
        {
            IObservable<bool> allNodesRunningStatus=null;
            foreach (var node in nodes)
            {
                var nodeStatusAsBools = Observable.FromEventPattern<HealthStatusEventArgs>(
                                            h => node.NodeHealth += h,
                                            h => node.NodeHealth -= h)
                                                  .Select(i => i.EventArgs.Status);
                var nodeStatusObs = Observable.Return(false)
                                              .Concat(nodeStatusAsBools)
                                              .DistinctUntilChanged();
                if (allNodesRunningStatus == null)
                {
                    allNodesRunningStatus = nodeStatusObs;
                    continue;
                }
                allNodesRunningStatus = allNodesRunningStatus.Merge(nodeStatusObs);
            }
            NodesRunning = allNodesRunningStatus
                .ObserveOn(NewThreadScheduler.Default) // Observing on a single thread so that the count update is syncronized
                .Scan(nodes.Count(),(currentCount, newStatus) => newStatus ? currentCount + 1 : currentCount - 1)
                .DistinctUntilChanged()
                .Publish()
                .RefCount();
            NodeCount = nodes.Count();
        }
    }

    public class NodeReporter
    {
        [Flags]
        public enum ReportingLevel
        {
            None = 0,
            Debug = 1,
            Error = 2,
            Warning = 4,
            All = Debug | Error | Warning
        }

        private NodeManager _nodeManager;

        private IDisposable _debugDisposable;
        private IDisposable _warnDisposable;
        private IDisposable _errorDisposable;
        public NodeReporter(NodeManager nodeManager)
        {
            _nodeManager = nodeManager;
            _debugDisposable = Disposable.Empty;
            _warnDisposable = Disposable.Empty;
            _errorDisposable = Disposable.Empty;
        }

        public void SetReportingLevel(ReportingLevel level)
        {
            if (level == ReportingLevel.None)
            {
                _errorDisposable.Dispose();
                _debugDisposable.Dispose();
                _warnDisposable.Dispose();
                return;
            }
            var errorLevel = (int)Math.Round(0.5 * _nodeManager.NodeCount, MidpointRounding.AwayFromZero);
            var warnLevel = (int)Math.Round(0.8 * _nodeManager.NodeCount, MidpointRounding.AwayFromZero);
            if (level.HasFlag(ReportingLevel.Error))
            {
                _errorDisposable.Dispose();
                _errorDisposable = _nodeManager.NodesRunning
                                               .Where(i => i < errorLevel)
                                               .Subscribe(i =>
                                               {
                                                   Console.ForegroundColor = ConsoleColor.DarkRed;
                                                   Console.Out.WriteLine($"We're dying over here! There are {i} nodes runnning.");
                                               });
            }
            if (level.HasFlag(ReportingLevel.Warning))
            {
                _warnDisposable.Dispose();
                _warnDisposable = _nodeManager.NodesRunning
                                              .Where(i => i>= errorLevel && i < warnLevel)
                                              .Subscribe(i =>
                                              {
                                                  Console.ForegroundColor = ConsoleColor.DarkYellow;
                                                  Console.Out.WriteLine($"Things are getting rusty...There are {i} nodes running.");
                                              });

            }
            if (level.HasFlag(ReportingLevel.Debug))
            {
                _debugDisposable.Dispose();
                _debugDisposable = _nodeManager.NodesRunning
                                             .Where(i => i == _nodeManager.NodeCount)
                                             .Subscribe(i =>
                                             {
                                                 Console.ForegroundColor = ConsoleColor.Green;
                                                 Console.Out.WriteLine($"Things are all good. There are {i} nodes running.");
                                             });
            }
        }

    }
   

   class Program
   {
        static void Main(string[] args)
        {
            var nodes = Enumerable.Range(0, 10)
                                  .Select(i =>
                                  {
                                      var n = new WorkerNode();
                                      n.StartWork();
                                      return n;
                                  }).ToList();
            var nodeManager = new NodeManager(nodes);
           // nodeManager.NodesRunning.Subscribe(i => Console.Out.WriteLine(i));
            var nodeReporter = new NodeReporter(nodeManager);
            nodeReporter.SetReportingLevel(NodeReporter.ReportingLevel.All);
            Console.ReadKey();
            nodes[0].Stop();
            nodes[1].Stop();
            nodes[2].Stop();
            Console.ReadKey();
            nodes[3].Stop();
            nodes[4].Stop();
            nodes[5].Stop();
            Console.ReadKey();
            nodes[0].StartWork();
            nodes[1].StartWork();
            nodes[2].StartWork();
            Console.ReadKey();
            nodes[3].StartWork();
            nodes[4].StartWork();
            nodes[5].StartWork();
            Console.ReadKey();
        }
    }
}
