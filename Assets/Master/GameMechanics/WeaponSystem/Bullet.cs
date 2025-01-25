using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] float speed = 5;
        [SerializeField] float duration = 1.5f;

        public bool onPool { get; set; }
        public Action<IPoolable> onReturn { get; set; }

        float timer = 0;

        public void Spawn(Vector3 pos, Vector3 dir)
        {
            transform.position = pos;
            transform.LookAt(dir);
            timer = duration;
        }

        private void FixedUpdate()
        {
            transform.SetPositionAndRotation(transform.position + transform.forward * speed * Time.fixedDeltaTime, transform.rotation);
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Return();
            }
        }

        public void Return()
        {
            onReturn?.Invoke(this);
        }
    }
}