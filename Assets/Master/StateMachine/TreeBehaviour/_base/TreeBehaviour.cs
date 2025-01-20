using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public abstract class TreeBehaviour<T> : MonoBehaviour where T : IContext
    {
        [SerializeField] bool loop;

        TreeNode<T> currentNode;
        TreeNode<T>[] nodes;
        T ctx;
        NodeStatus currentStatus = NodeStatus.RUNNING;

        protected abstract void SetContext();

        private void Awake()
        {
            SetContext();
            int l = transform.childCount;
            nodes = this.GetComponentsInChildren<TreeNode<T>>();
            foreach (TreeNode<T> node in nodes)
            {
                node.Init(ctx);
            }
            currentNode = nodes[0];
        }

        protected virtual void Start()
        {
            currentNode.OnEnter();
        }

        protected virtual void Update()
        {
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
            if (loop || currentStatus == NodeStatus.RUNNING)
                currentNode.OnFixedUpdate();
        }

        public void ChangeNode(TreeNode<T> node)
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
