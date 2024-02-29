using System;

namespace Source.Scripts.Infrastructure.SaveLoadData
{
    [Serializable]
    public class UpgradeProgress
    {
        // TODO: QUESTION: нужен ли мне этот класс?

        public int Id;
        public int Level;
    }
}