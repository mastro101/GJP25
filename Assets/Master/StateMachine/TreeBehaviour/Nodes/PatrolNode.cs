using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public class PatrolNode : TreeNode<IContext>
    {
        [SerializeField] ViewTriggerFromTransform ViewTriggerFromTransform;

        public override NodeStatus Evaluate()
        {
            if (ViewTriggerFromTransform != null)
            {
                if (ViewTriggerFromTransform.CheckTrigger())
                {
                    return NodeStatus.SUCCESS;
                }
            }

            return NodeStatus.FAILUR;
        }
    }
}
