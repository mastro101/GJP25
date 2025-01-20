using Eastermaster.StateMachineAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster
{
    public class SA_Sample_Gameplay : StateAnimator<SampleCTX>
    {
        [SerializeField] Light light;

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Gameplay");
        }
    }
}
