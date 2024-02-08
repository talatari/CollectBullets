using System;
// using Cysharp.Threading.Tasks;

namespace Source.Scripts.Common
{
    public class CooldownTimer
    {
        private float _cooldownInSeconds;
        // private UniTaskCompletionSource<bool> _completionSource;
        private bool _isPaused;
        private bool _isStopped;
        private float _remainingDuration;
        private bool _isDebug;
        private string _name;

        public CooldownTimer(float cooldownInSeconds)
        {
            _cooldownInSeconds = cooldownInSeconds;
        }

        public event Action Started;
        public event Action Stopped;
        public event Action Completed;
        public event Action<float> RemainingDurationChanged;

        // public bool IsRunning => _completionSource != null && !_completionSource.Task.Status.IsCompleted();
        // public bool IsPaused => _isPaused;

        public async void Run()
        {
            if (_cooldownInSeconds <= 0)
                throw new Exception($"{nameof(_cooldownInSeconds)} < 0");

            // if (_completionSource != null && !_completionSource.Task.Status.IsCompleted())
            //     return;
            //
            // _completionSource = new UniTaskCompletionSource<bool>();

            _remainingDuration = _cooldownInSeconds;
            _isPaused = false;
            _isStopped = false;

            Started?.Invoke();

            while (_remainingDuration > 0 && _isPaused == false && _isStopped == false)
            {
                // await UniTask.Delay(TimeSpan.FromSeconds(Math.Min(_remainingDuration, 0.1f)), ignoreTimeScale: true);

                if (!_isPaused)
                {
                    _remainingDuration -= 0.1f;

                    if (_remainingDuration < 0)
                        _remainingDuration = 0;

                    RemainingDurationChanged?.Invoke(_remainingDuration);
                }
            }

            // _completionSource?.TrySetResult(true);

            if (_remainingDuration <= 0)
            {
                Completed?.Invoke();
            }
        }

        public void Stop()
        {
            Stopped?.Invoke();
            _isStopped = true;
            // _completionSource?.TrySetResult(false);
            // _completionSource = null;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            if (!_isPaused)
                return;

            _isPaused = false;
            Run();
        }

        public void Restart()
        {
            Stop();
            Run();
        }

        public void SetCooldownDuration(float newCooldownDuration) =>
            _cooldownInSeconds = newCooldownDuration;

        public CooldownTimer SetDebugMode(string name)
        {
            _name = name;
            _isDebug = true;

            return this;
        }
    }
}