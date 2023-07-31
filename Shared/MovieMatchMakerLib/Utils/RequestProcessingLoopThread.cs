using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class RequestProcessingLoopThread<TRequest> : IDisposable
    {
        private readonly ConcurrentQueue<TRequest> _requests;        
        private readonly Func<TRequest, Task> _processRequestFunc;

        private readonly Thread _processRequestsLoopThread;
        private readonly AutoResetEvent _processRequestsLoopEvent;        
        private volatile bool _stopProcessRequests;
        private bool disposedValue;
        private readonly int _loopDelayMs = 0;

        private readonly bool _useThreadPool;
        private readonly ConcurrentQueue<Task> _requestProcessingTasks;
        // create LongRunning tasks since we process (relatively) heavy long-running requests
        private const TaskCreationOptions RequestProcessingTaskCreationOptions = TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning;


        public int TaskCount => _requestProcessingTasks.Count;

        public RequestProcessingLoopThread(Func<TRequest, Task> processRequestFunc)
        {
            _requests = new ConcurrentQueue<TRequest>();
            _processRequestFunc = processRequestFunc;
            _processRequestsLoopThread = new Thread(ProcessRequestsLoop);
            _processRequestsLoopEvent = new AutoResetEvent(false);
            _stopProcessRequests = false;

            _useThreadPool = false;
            _requestProcessingTasks = new ConcurrentQueue<Task>();
        }

        public RequestProcessingLoopThread(Func<TRequest, Task> processRequestFunc, bool useThreadPool, int loopDelayMs)
            : this(processRequestFunc)
        {
            _useThreadPool = useThreadPool;
            _loopDelayMs = loopDelayMs;
        }

        public RequestProcessingLoopThread(Func<TRequest, Task> processRequestFunc, int loopDelayMs)
            : this(processRequestFunc, false, loopDelayMs)
        {           
            _loopDelayMs = loopDelayMs;
        }

        public RequestProcessingLoopThread(Func<TRequest, Task> processRequestFunc, bool useThreadPool)
            : this(processRequestFunc, useThreadPool, 0)
        {
            _useThreadPool = useThreadPool;           
        }

        public void StartProcessingRequests()
        {            
            _processRequestsLoopThread.Start();
        }        

        public void StopProcessingRequests(bool wait = true)
        {
            _stopProcessRequests = true;
            _processRequestsLoopEvent.Set();
            if (wait)
            {
                Wait();
            }            
        }

        public void AddRequest(TRequest request)
        {
            if (!_stopProcessRequests)
            {
                _requests.Enqueue(request);
                _processRequestsLoopEvent.Set();
            }
        }

        public void AddRequests(IEnumerable<TRequest> requests)
        {
            if (!_stopProcessRequests)
            {
                foreach (var request in requests)
                {
                    _requests.Enqueue(request);
                }
                _processRequestsLoopEvent.Set();
            }
        }

        private void ProcessRequestsLoop()
        {
            while (!_stopProcessRequests)
            {
                _processRequestsLoopEvent.WaitOne();

                while (_requests.TryDequeue(out var request))
                {
                    if (_useThreadPool)
                    {                    
                        var task = new Task((r) => _processRequestFunc((TRequest)r), request, RequestProcessingTaskCreationOptions);
                        _requestProcessingTasks.Enqueue(task);
                        task.Start();                                                
                    }
                    else
                    {
                        _processRequestFunc(request);
                    }

                    if (_loopDelayMs > 0)
                    {
                        Thread.Sleep(_loopDelayMs);
                    }
                }
            }          
        }       

        public void Wait()
        {
            _processRequestsLoopThread.Join();
            if (_useThreadPool)
            {                
                // wait for any tasks to complete if we are using the pool
                while (_requestProcessingTasks.TryDequeue(out var task))
                {
                    // should we not wait on threads that haven't started yet?
                    //if (!task.IsCompleted)
                    //if (task.Status == TaskStatus.Running)
                    {
                        task.Wait();
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    // don't wait (waiting in Dispose might have consequences?)
                    StopProcessingRequests(false);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RequestProcessingLoopThread()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
