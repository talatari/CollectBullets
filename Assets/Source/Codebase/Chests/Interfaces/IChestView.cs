using System;

namespace Source.Codebase.Chests.Interfaces
{
    public interface IChestView
    {
        event Action KeyUsed;

        void CollectKey();
        void UseKey();
    }
}