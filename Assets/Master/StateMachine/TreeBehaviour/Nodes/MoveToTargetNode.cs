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
        if (CheckOnTarget())
            return NodeStatus.SUCCESS;
        if (moved)
        {
            Debug.Log("fallito");
            return NodeStatus.FAILUR;
        }
        return NodeStatus.RUNNING;
    }

    bool CheckOnTarget()
    {
        return (target.position - ctx.transform.position).magnitude <= offset;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (CheckOnTarget())
            return;
        ctx.transform.Translate((target.position - ctx.transform.position).normalized * speed * Time.fixedDeltaTime);
        moved = true;
    }
}
