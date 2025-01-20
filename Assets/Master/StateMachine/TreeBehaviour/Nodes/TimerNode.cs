using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public class TimerNode : TreeNode<IContext>
    {
        [SerializeField] float timeout;
        float timer;

        public override void OnEnter()
        {
            base.OnEnter();
            timer = timeout;
            Debug.Log("Wait " + timer);
        }

        public override NodeStatus Evaluate()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                return NodeStatus.SUCCESS;
            return NodeStatus.RUNNING;
        }

        protected override void OnExit()
        {
            base.OnExit();
            timer = timeout;
            Debug.Log("Ring!!!");
        }
    }
}
