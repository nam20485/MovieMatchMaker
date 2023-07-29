using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class RequestProcessingLoopThread<TRequest>
    {
        private readonly ConcurrentQueue<TRequest> _requests;        
        private readonly Action<TRequest> _processRequestFunc;

        private readonly Thread _processRequestsLoopThread;
        private readonly AutoResetEvent _processRequestsLoopEvent;
        private bool _stopProcessRequests;        

        public RequestProcessingLoopThread(Action<TRequest> processRequestFunc)
        {
            _requests = new ConcurrentQueue<TRequest>();
            _processRequestFunc = processRequestFunc;
            _processRequestsLoopThread = new Thread(ProcessRequestsLoop);
            _processRequestsLoopEvent = new AutoResetEvent(false);
            _stopProcessRequests = false;
        }

        public void StartProcessingRequests()
        {            
            _processRequestsLoopThread.Start();
        }        

        public void StopProcessingRequests()
        {
            _stopProcessRequests = true;
            _processRequestsLoopEvent.Set();
            _processRequestsLoopThread.Join();
        }      

        public void AddRequest(TRequest request)
        {
            _requests.Enqueue(request);
        }

        private void ProcessRequestsLoop()
        {
            while (!_stopProcessRequests)
            {
                _processRequestsLoopEvent.WaitOne();

                while (_requests.TryDequeue(out var request))
                {
                    _processRequestFunc(request);
                }
            }
        }
    }
}
