using UnityEngine;
using System.Collections.Generic;

namespace Source.Scripts.Infrastructure.Services
{
    public class GamePauseService
    {
        private Dictionary<IGamePauseListener, bool> _pauseListeners;
        private bool _isPaused;

        public bool IsPaused => _isPaused;

        public GamePauseService() => 
            _pauseListeners = new Dictionary<IGamePauseListener, bool>();

        public void AddPauseListener(IGamePauseListener listener)
        {
            if (_pauseListeners.ContainsKey(listener) == false)
                _pauseListeners.Add(listener, false);
        }

        public void RemovePauseListener(IGamePauseListener listener)
        {
            if (_pauseListeners.ContainsKey(listener))
                _pauseListeners.Remove(listener);
        }

        public void PauseGame()
        {
            if (_isPaused == false)
            {
                _isPaused = true;
                Time.timeScale = 0;
                
                foreach (var listener in _pauseListeners.Keys)
                {
                    listener.OnGamePaused();
                    _pauseListeners[listener] = true;
                }
                
                Debug.Log("Game paused");
            }
        }

        public void ResumeGame()
        {
            if (_isPaused)
            {
                _isPaused = false;
                Time.timeScale = 1;
                
                foreach (var listener in _pauseListeners.Keys)
                {
                    listener.OnGameResumed();
                    _pauseListeners[listener] = false;
                }
                
                Debug.Log("Game resumed");
            }
        }
    }
}