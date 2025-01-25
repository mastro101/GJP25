using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public class RandomSelectorNode : NestedNode<IContext>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            currentNode = childNodes[Random.Range(0, childNodes.Length)];
        }

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
                    return Exit(false);
                case NodeStatus.RUNNING:
                    return NodeStatus.RUNNING;
                default:
                    return Exit(false);
            }
        }
    }
}