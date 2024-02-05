using System;
using Source.Scripts.Infrastructure.Interfaces;

namespace Source.Scripts.Infrastructure
{
    public class TargetService
    {
        public ITarget Target { get; set; }

        public void SetTarget(ITarget target) => 
            Target = target ?? throw new ArgumentNullException(nameof(target));
    }
}