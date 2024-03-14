using System;
using Source.Codebase.Infrastructure.Services;

namespace Source.Codebase.GameOver
{
    public class RestartGamePresenter : IDisposable
    {
        private readonly GameLoopMediator _gameLoopMediator;
        private readonly GamePauseService _gamePauseService;
        private readonly RestartGameView _restartView;

        public RestartGamePresenter(GameLoopMediator gameLoopMediator, GamePauseService gamePauseService,
            RestartGameView restartView)
        {
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
            _restartView = restartView ? restartView : throw new ArgumentNullException(nameof(restartView));

            _gameLoopMediator.GameOver += OnShowRestartView;
            _restartView.RestartButton.onClick.AddListener(RestartGame);
        }

        public void Dispose()
        {
            _gameLoopMediator.GameOver -= OnShowRestartView;
            _restartView.RestartButton.onClick.RemoveListener(RestartGame);
        }

        private void RestartGame()
        {
            _gamePauseService.InvokeByUI(false);
            _restartView.Hide();
            _gameLoopMediator.NotifyRestartGame();
        }

        private void OnShowRestartView()
        {
            _gamePauseService.InvokeByUI(true);
            _restartView.Show();
        }
    }
}