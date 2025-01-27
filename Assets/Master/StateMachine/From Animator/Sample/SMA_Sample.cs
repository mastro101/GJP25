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
            ctx = new SampleCTX(gameObject, animator)
            {
                camera = _camera,
            };
        }
    }

    public class SampleCTX : BaseStateMachineContext
    {
        public Camera camera;

        public SampleCTX(GameObject self, Animator animator) : base(self, animator)
        {
        }
    }
}
