using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class RequestProcessingLoopThread<TRequest>
    {
        private readonly ConcurrentQueue<TRequest> _requests;        
        private readonly Func<TRequest, Task> _processRequestFunc;

        private readonly Thread _processRequestsLoopThread;
        private readonly AutoResetEvent _processRequestsLoopEvent;        
        private volatile bool _stopProcessRequests;

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

        public RequestProcessingLoopThread(Func<TRequest, Task> processRequestFunc, bool useThreadPool)
            : this(processRequestFunc)
        {
            _useThreadPool |= useThreadPool;
        }

        public void StartProcessingRequests()
        {            
            _processRequestsLoopThread.Start();
        }        

        public void StopProcessingRequests()
        {            
            _stopProcessRequests = true;
            _processRequestsLoopEvent.Set();
            Wait();
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
    }
}
