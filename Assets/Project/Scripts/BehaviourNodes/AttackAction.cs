using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[katana] [animator] Attack with [triggerToSet]", category: "Action", id: "1fe0fc718ab0d8fbcf40acf01090dd39")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Katana> katana; //type IAttacker in the future
    [SerializeReference] public BlackboardVariable<Animator> animator;
    [SerializeReference] public BlackboardVariable<string> triggerToSet;
    [SerializeReference] public BlackboardVariable<AnimationClip> clip;
    //[SerializeReference] public BlackboardVariable<float> time;

    float timer;
    bool hitSomething;
    protected override Status OnStart()
    {
        if (katana == null)
        {
            Debug.LogException(new Exception("Katana on Node Attack is missing"), katana);
            return Status.Failure;
        }
        if (animator == null)
        {
            Debug.LogException(new Exception("Animator on Node Attack is missing"), animator);
            return Status.Failure;
        }
        if (clip == null)
        {
            Debug.LogException(new Exception("clip on Node Attack is missing"), clip);
            return Status.Failure;
        }
            
        timer = clip.Value.length;
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