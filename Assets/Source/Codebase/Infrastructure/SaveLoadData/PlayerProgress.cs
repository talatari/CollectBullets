using System;

namespace Source.Codebase.Infrastructure.SaveLoadData
{
    [Serializable]
    public class PlayerProgress
    {
        public int CurrentLevel { get; private set; }

        public void SetLevel(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            CurrentLevel = value;
        }
    }
}