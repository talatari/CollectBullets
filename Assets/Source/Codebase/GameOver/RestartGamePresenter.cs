using System;
using Source.Codebase.Infrastructure.Services;

namespace Source.Codebase.GameOver
{
    public class RestartGamePresenter : IDisposable
    {
        private readonly GameLoopService _gameLoopService;
        private readonly GamePauseService _gamePauseService;
        private readonly RestartGameView _restartView;

        public RestartGamePresenter(GameLoopService gameLoopService, GamePauseService gamePauseService,
            RestartGameView restartView)
        {
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
            _restartView = restartView ? restartView : throw new ArgumentNullException(nameof(restartView));

            _gameLoopService.GameOver += OnShowRestartView;
            _restartView.RestartButton.onClick.AddListener(RestartGame);
        }

        public void Dispose()
        {
            _gameLoopService.GameOver -= OnShowRestartView;
            _restartView.RestartButton.onClick.RemoveListener(RestartGame);
        }

        private void RestartGame()
        {
            _gamePauseService.InvokeByUI(false);
            _restartView.Hide();
            _gameLoopService.NotifyRestartGame();
        }

        private void OnShowRestartView()
        {
            _gamePauseService.InvokeByUI(true);
            _restartView.Show();
        }
    }
}