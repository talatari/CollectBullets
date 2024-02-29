using System;
using UnityEngine;

namespace Source.Scripts.Infrastructure.Providers
{
    public class TargetProvider
    {
        public Transform Target { get; set; }

        public void SetTarget(Transform target) => 
            Target = target ? target : throw new ArgumentNullException(nameof(target));
    }
}