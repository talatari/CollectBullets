using System;

namespace Source.Codebase.Infrastructure.Services
{
    public class GamePauseService : IDisposable
    {
        private readonly MultiCallHandler _multiCallHandler;

        public GamePauseService(MultiCallHandler multiCallHandler)
        {
            _multiCallHandler = multiCallHandler;

            _multiCallHandler.Called += OnPaused;
            _multiCallHandler.Released += OnResumed;
        }

        private void OnPaused() => 
            Paused?.Invoke();
        
        private void OnResumed() =>
            Resumed?.Invoke();

        public event Action Paused;
        public event Action Resumed;

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

        private void HandleInvoke(string invokeBy, bool isCall)
        {
            if (isCall)
                _multiCallHandler.Call(invokeBy);
            else
                _multiCallHandler.Release(invokeBy);
        }
    }
}