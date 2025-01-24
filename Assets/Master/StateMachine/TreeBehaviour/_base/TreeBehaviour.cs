using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public abstract class TreeBehaviour<T> : MonoBehaviour where T : IContext
    {
        [SerializeField] bool loop;

        TreeNode<IContext> currentNode;
        TreeNode<IContext>[] nodes;
        T ctx;
        NodeStatus currentStatus = NodeStatus.RUNNING;
        bool active = false;

        protected virtual void SetContext(T _ctx)
        {
            this.ctx = _ctx;
        }

        public void Init(T _ctx)
        {
            SetContext(_ctx);
            int l = transform.childCount;
            nodes = this.GetComponentsInChildren<TreeNode<IContext>>();
            foreach (TreeNode<IContext> node in nodes)
            {
                node.Init(ctx);
            }
            currentNode = nodes[0];
            currentNode.OnEnter();
            active = true;
        }

        protected virtual void Update()
        {
            if (!active)
                return;

            if (loop)
            {
                currentStatus = currentNode.Evaluate();
                if (currentStatus != NodeStatus.RUNNING)
                {
                    currentNode.OnEnter();
                }
            }
            else if (currentStatus == NodeStatus.RUNNING)
                currentStatus = currentNode.Evaluate();
        }

        protected virtual void FixedUpdate()
        {
            if (!active)
                return;

            if (currentStatus == NodeStatus.RUNNING)
                currentNode.OnFixedUpdate();
        }

        public void ChangeNode(TreeNode<IContext> node)
        {
            if (!nodes.Contains(node))
                return;

            currentNode.Exit(node);
        }
    }

    public enum NodeStatus
    {
        SUCCESS,
        FAILUR,
        RUNNING
    }
}
