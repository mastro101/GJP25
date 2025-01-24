using Eastermaster;
using Eastermaster.TreeBehaviour;
using UnityEngine;

public class MoveToTargetNode : TreeNode<IContext>
{
    [SerializeField] Transform target;
    [SerializeField] float offset;
    [SerializeField] float speed;

    bool moved;

    public override void OnEnter()
    {
        base.OnEnter();
        moved = false;
    }

    public override NodeStatus Evaluate()
    {
        base.Evaluate();
        if (moved)
        {
            if ((target.position - ctx.transform.position).magnitude <= offset)
                return NodeStatus.SUCCESS;

            return NodeStatus.FAILUR;
        }
        return NodeStatus.RUNNING;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        ctx.transform.Translate((target.position - ctx.transform.position).normalized * speed * Time.fixedDeltaTime);
    }
}
