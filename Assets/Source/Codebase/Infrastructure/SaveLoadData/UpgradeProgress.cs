using System;

namespace Source.Codebase.Infrastructure.SaveLoadData
{
    [Serializable]
    public class UpgradeProgress
    {
        // TODO: QUESTION: нужен ли мне этот класс?

        public int Id;
        public int Level;
    }
}