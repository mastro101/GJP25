using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public class SelectorNode : NestedNode<IContext>
    {
        public override NodeStatus Evaluate()
        {
            NodeStatus status = currentNode.Evaluate();

            switch (status)
            {
                case NodeStatus.SUCCESS:
                    currentNode.Exit(true);
                    return Exit(true);
                case NodeStatus.FAILUR:
                    currentNode.Exit(false);
                    break;
                case NodeStatus.RUNNING:
                    return NodeStatus.RUNNING;
            }

            if (currentNode == childNode[childNode.Length - 1])
            {
                return Exit(false);
            }

            NextNode();
            return NodeStatus.RUNNING;
        }
    }
}
