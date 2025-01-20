using Eastermaster.StateMachineAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster
{
    public class SA_Sample_Idle : StateAnimator<SampleCTX>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.LogFormat("idle with ctx {0} and {1}", ctx.camera, ctx.go);
            ctx.Next?.Invoke();
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("exit idle");
        }
    }
}
