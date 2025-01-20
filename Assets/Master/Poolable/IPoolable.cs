using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster
{
    public interface IPoolable
    {
        GameObject gameObject { get; }
        Transform transform { get; }

        bool onPool { get; set; }

        Action<IPoolable> onReturn { get; set; }

        void Spawn(Vector3 pos, Vector3 dir);
        void Return();
    } 
}