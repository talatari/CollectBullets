using System;
using Source.Codebase.Infrastructure.Services;

namespace Source.Codebase.Chests
{
    public class ChestService : IDisposable
    {
        private ChestPresenter _chestPresenter;
        private KeyService _keyService;

        public void Init(ChestPresenter chestPresenter, KeyService keyService)
        {
            _chestPresenter = chestPresenter ? chestPresenter : 
                throw new ArgumentNullException(nameof(chestPresenter));
            _keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));

            _chestPresenter.KeyDropped += OnKeyDropped;
        }

        private void OnKeyDropped()
        {
            _keyService.DropKey();
        }

        public void Dispose()
        {
            if (_chestPresenter == null)
                return;
            
            _chestPresenter.KeyDropped -= OnKeyDropped;
        }
    }
}