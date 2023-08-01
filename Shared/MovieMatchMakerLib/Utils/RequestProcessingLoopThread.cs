using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class RequestProcessingLoopThread<TRequest> : IDisposable
    {
        public int TaskCount => _requestProcessingTasks.Count;
        public int RemainingRequests => _requests.Count;

        private readonly ConcurrentQueue<TRequest> _requests;        
        private readonly Func<TRequest, Task> _processRequestFunc;

        private readonly Thread _processRequestsLoopThread;
        private readonly AutoResetEvent _processRequestsLoopEvent;
        private readonly ConcurrentQueue<Task> _requestProcessingTasks;

        private readonly bool _useThreadPool;
        private readonly int _loopDelayMs = 0;

        private volatile bool _stopProcessingRequests;
        private volatile bool _forceStop;

        // create LongRunning tasks since we process (relatively) heavy long-running requests
        private const TaskCreationOptions RequestProcessingTaskCreationOptions = TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning;

        private bool disposedValue;

        public RequestProcessingLoopThread(Func<TRequest, Task> processRequestFunc)
        {
            _requests = new ConcurrentQueue<TRequest>();
            _processRequestFunc = processRequestFunc;
            _processRequestsLoopThread = new Thread(ProcessRequestsLoop);
            _processRequestsLoopEvent = new AutoResetEvent(false);
            _stopProcessingRequests = false;

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

        public void StopProcessingRequests(bool force = false)
        {
            _forceStop = force;
            _stopProcessingRequests = true;            
            _processRequestsLoopEvent.Set();
            Wait();            
        }

        public void AddRequest(TRequest request)
        {
            if (!_stopProcessingRequests)
            {
                _requests.Enqueue(request);
                _processRequestsLoopEvent.Set();
            }
        }

        public void AddRequests(IEnumerable<TRequest> requests)
        {
            if (!_stopProcessingRequests)
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
            while (!_stopProcessingRequests)
            {
                _processRequestsLoopEvent.WaitOne();

                while (!_forceStop && 
                       _requests.TryDequeue(out var request))
                {
                    try
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
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Log($"{GetType()}: exception processing request\n{ex}");
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
                while (!_forceStop &&
                       _requestProcessingTasks.TryDequeue(out var task))
                {
                    try
                    {
                        // should we not wait on threads that haven't started yet?
                        //if (!task.IsCompleted)
                        //if (task.Status == TaskStatus.Running)
                        {
                            task.Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Log(ex);
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
                    // dispose managed state (managed objects)
                    // TODO: don't wait (waiting in Dispose might have consequences?)
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
