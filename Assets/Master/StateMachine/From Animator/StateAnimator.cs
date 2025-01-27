using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.StateMachineAnimator
{
    public abstract class StateAnimator<T> : StateMachineBehaviour where T : BaseStateMachineContext
    {
        protected T ctx;

        public void Init(T _ctx)
        {
            ctx = _ctx;
        }

        public virtual void OnEnter() { }
        public virtual void OnTick()  { }
        public virtual void OnExit()  { }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            OnEnter();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            OnTick();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            OnExit();
        }
    }
}
