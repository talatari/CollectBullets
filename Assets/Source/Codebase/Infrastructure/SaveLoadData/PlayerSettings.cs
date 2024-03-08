using System;

namespace Source.Codebase.Infrastructure.SaveLoadData
{
    [Serializable]
    public class PlayerSettings
    {
        public bool IsSoundOn { get; private set; }
        public bool IsMusicOn { get; private set; }
        
        public void SetSound(bool value) => 
            IsSoundOn = value;

        public void SetMusic(bool value) =>
            IsMusicOn = value;
    }
}