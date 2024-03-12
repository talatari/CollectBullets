using System;

namespace Source.Codebase.Infrastructure.Services
{
    public class GamePauseService : IDisposable
    {
        private MultiCallHandler _multiCallHandler = new ();

        public event Action Paused;
        public event Action Resumed;

        public void Init()
        {
            _multiCallHandler.Called += OnPaused;
            _multiCallHandler.Released += OnResumed;
        }
        
        public void Dispose()
        {
            _multiCallHandler.Called -= OnPaused;
            _multiCallHandler.Released -= OnResumed;
        }

        public void InvokeByAds(bool isCall) =>
            HandleInvoke(nameof(InvokeByAds), isCall);

        public void InvokeByUI(bool isCall) =>
            HandleInvoke(nameof(InvokeByUI), isCall);

        public void InvokeByFocusChanging(bool isCall) =>
            HandleInvoke(nameof(InvokeByFocusChanging), isCall);
        
        private void OnPaused() => 
            Paused?.Invoke();
        
        private void OnResumed() =>
            Resumed?.Invoke();
        
        private void HandleInvoke(string invokeBy, bool isCall)
        {
            if (isCall)
                _multiCallHandler.Call(invokeBy);
            else
                _multiCallHandler.Release(invokeBy);
        }
    }
}