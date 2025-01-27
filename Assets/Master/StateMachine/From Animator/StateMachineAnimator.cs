using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.StateMachineAnimator
{
    public abstract class StateMachineAnimator<T> : MonoBehaviour where T : BaseStateMachineContext
    {
        [SerializeField] protected Animator animator;

        protected T ctx;

        private void OnEnable()
        {
            InitCtx();
            var states = animator.GetBehaviours<StateAnimator<T>>();
            foreach (var state in states)
            {
                state.Init(ctx);
            }
        }

        protected abstract void InitCtx();
    }
}
