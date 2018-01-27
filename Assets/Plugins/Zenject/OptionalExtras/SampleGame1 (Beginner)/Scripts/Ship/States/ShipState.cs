using System;
using UnityEngine;
using System.Collections;
using Zenject;

namespace Zenject.Asteroids
{
    public abstract class ShipState : IDisposable
    {
        public virtual void Update ( ) { }

        public virtual void Start()
        {
            // optionally overridden
        }

        public virtual void Dispose()
        {
            // optionally overridden
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            // optionally overridden
        }
    }
}
