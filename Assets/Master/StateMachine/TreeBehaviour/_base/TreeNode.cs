using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public abstract class TreeNode<T> : MonoBehaviour where T : IContext
    {
        protected T ctx;

        public virtual NodeStatus Evaluate() => NodeStatus.FAILUR;
        public virtual void OnEnter()       { }
        public virtual void OnFixedUpdate() { }
        protected virtual void OnExit()     { }

        public NodeStatus Exit(bool success)
        {
            OnExit();
            return success ? NodeStatus.SUCCESS : NodeStatus.FAILUR;
        }

        public virtual TreeNode<T> Init(T _ctx)
        {
            ctx = _ctx;
            return this;
        }
    }
}
