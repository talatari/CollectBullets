using System;
using Source.Codebase.Chests.Interfaces;
using Source.Codebase.Infrastructure.Services;

namespace Source.Codebase.Chests
{
    public class ChestPresenter : IDisposable
    {
        private IChestView _chestView;
        private GameLoopMediator _gameLoopMediator;

        public void Init(IChestView chestView, GameLoopMediator gameLoopMediator)
        {
            _chestView = chestView ?? throw new ArgumentNullException(nameof(chestView));
            _gameLoopMediator = gameLoopMediator ?? throw new ArgumentNullException(nameof(gameLoopMediator));

            _chestView.KeyUsed += OnKeyUsed;
        }

        public void Dispose()
        {
            if (_chestView == null)
                return;
            
            _chestView.KeyUsed -= OnKeyUsed;
        }

        private void OnKeyUsed() => 
            _gameLoopMediator.NotityKeyUsed();
    }
}