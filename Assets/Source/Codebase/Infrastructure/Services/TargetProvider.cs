using System;
using UnityEngine;

namespace Source.Codebase.Infrastructure.Services
{
    public class TargetProvider
    {
        public Transform Target { get; set; }

        public void Init(Transform target) => 
            Target = target ? target : throw new ArgumentNullException(nameof(target));
    }
}