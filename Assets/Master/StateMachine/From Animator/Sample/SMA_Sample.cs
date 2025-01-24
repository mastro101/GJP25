using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.StateMachineAnimator
{
    public class SMA_Sample : StateMachineAnimator<SampleCTX>
    {
        [SerializeField] Camera _camera;

        protected override void InitCtx()
        {
            ctx = new SampleCTX()
            {
                camera = _camera,
                go = gameObject,
                Next = _Next,
            };

            void _Next()
            {
                animator.SetTrigger("Next");
            }
        }
    }

    public class SampleCTX : IContext
    {
        public Camera camera;
        public GameObject go;

        public Action Next;

        public Transform transform => go.transform;
    }
}
