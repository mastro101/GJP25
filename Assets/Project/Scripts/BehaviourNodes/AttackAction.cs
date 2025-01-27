using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnityEditor.Experimental.GraphView.GraphView;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[katana] [animator] Attack with [triggerToSet]", category: "Action", id: "1fe0fc718ab0d8fbcf40acf01090dd39")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Katana> katana; //type IAttacker in the future
    [SerializeReference] public BlackboardVariable<Animator> animator;
    [SerializeReference] public BlackboardVariable<string> triggerToSet;
    //[SerializeReference] public BlackboardVariable<float> time;

    float timer;
    AnimatorStateInfo info;
    bool hitSomething;
    bool didOnce = false;
    protected override Status OnStart()
    {
        if (katana == null || animator.Value == null)
            return Status.Failure;
        didOnce = false;
        foreach (var i in animator.Value.parameters)
        {
            if (i.name == triggerToSet)
            {
                animator.Value.SetTrigger(triggerToSet);
                hitSomething = false;
                return Status.Running;
            }
        }
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (didOnce == false)
        {
            timer = animator.Value.GetCurrentAnimatorStateInfo(0).length;
            didOnce = true;
        }
        bool hit = katana.Value.Attack();
        if (hitSomething == false)
            hitSomething = hit;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (hitSomething)
                return Status.Success;
            else
                return Status.Failure;
        }
        return Status.Running;
    }
}