using System;
using System.Threading.Tasks;
using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Utils;


namespace MovieMatchMakerLib
{
    public class ThreadedMovieConnectionBuilder : MovieConnectionBuilderBase
    {
        private readonly RequestProcessingLoopThread<Model.Movie> _findMovieConnectionsLoopThread;

        private bool disposedValue;

        public ThreadedMovieConnectionBuilder(IDataCache dataCache)
            : base(dataCache)
        {
            _findMovieConnectionsLoopThread = new RequestProcessingLoopThread<Model.Movie>(FindMovieConnectionsFor);
        }
             
        public override async Task FindMovieConnections()
        {
            await Task.Run(() =>
            {
                foreach (var sourceMovie in _dataCache.Movies)
                {                    
                    _findMovieConnectionsLoopThread.AddRequest(sourceMovie);
                }
            });           
        }        

        public override void Start()
        {
            _started = DateTime.UtcNow;
            _findMovieConnectionsLoopThread.StartProcessingRequests();
        }

        public override void Stop()
        {
           _findMovieConnectionsLoopThread.StopProcessingRequests();
            _started = DateTime.UtcNow;
        }

        public override void Wait()
        {
            _findMovieConnectionsLoopThread.Wait();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Stop();                    
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }

            base.Dispose(disposing);
        }      
    }
}
