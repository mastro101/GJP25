using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public class SequenceNode : NestedNode<IContext>
    {
        public override NodeStatus Evaluate()
        {
            NodeStatus status = currentNode.Evaluate();

            switch (status)
            {
                case NodeStatus.SUCCESS:
                    currentNode.Exit(true);
                    break;
                case NodeStatus.FAILUR:
                    currentNode.Exit(false);
                    return NodeStatus.FAILUR;
                case NodeStatus.RUNNING:
                    return NodeStatus.RUNNING;
            }

            if (currentNode == childNode[childNode.Length - 1])
            {
                return Exit(true);
            }

            NextNode();
            return NodeStatus.RUNNING;
        }
    }
}
