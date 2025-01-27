using UnityEngine;

namespace Eastermaster.StateMachineAnimator
{
    public class BaseStateMachineContext
    {
        public GameObject self;
        public Transform transform => self.transform;

        private Animator animator;
        public void Next()
        {
            animator.SetTrigger("Next");
        }

        public BaseStateMachineContext(GameObject self, Animator animator)
        {
            this.self = self;
            this.animator = animator;
        }
    }
}