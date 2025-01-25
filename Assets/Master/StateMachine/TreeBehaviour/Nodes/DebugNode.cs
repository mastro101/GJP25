using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
	public class DebugNode : TreeNode<IContext>
	{
        [SerializeField] bool returnTrue = false;
        [SerializeField] string text;

        public override NodeStatus Evaluate()
        {
            Debug.LogFormat("{0}/{1}: {2}", transform.parent.name, transform.name, text);

            if (returnTrue)
            {
                return NodeStatus.SUCCESS;
            }
            else
            {
                return NodeStatus.FAILUR;
            }
        }
    } 
}