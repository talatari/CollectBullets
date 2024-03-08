using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Codebase.Infrastructure.Services
{
    public class MultiCallHandler
    {
        private readonly Dictionary<string, bool> _callers = new();

        public event Action Called;
        public event Action Released;

        public bool IsCalled { get; private set; }

        public void Call(string key)
        {
            _callers[key] = true;
            Time.timeScale = 0;

            CheckStatus();
        }

        public void Release(string key)
        {
            _callers[key] = false;
            Time.timeScale = 1;

            CheckStatus();
        }

        public void Reset()
        {
            _callers.Clear();
            IsCalled = false;
            Released?.Invoke();
        }

        private void CheckStatus()
        {
            bool isAnyActiveCall = _callers.Values.Any(caller => caller);

            if (isAnyActiveCall == IsCalled)
                return;

            IsCalled = isAnyActiveCall;

            if (IsCalled)
                Called?.Invoke();
            else
                Released?.Invoke();
        }
    }
}