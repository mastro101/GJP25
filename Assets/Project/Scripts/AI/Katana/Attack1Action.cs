using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack1", story: "[Katana] Attack1", category: "Action", id: "1fe0fc718ab0d8fbcf40acf01090dd39")]
public partial class Attack1Action : Action
{
    [SerializeReference] public BlackboardVariable<Katana> Katana;
    [SerializeReference] public BlackboardVariable<float> animationSpeed;
    [SerializeReference] public BlackboardVariable<float> time;

    float timer;
    protected override Status OnStart()
    {
        if (Katana.Value == null)
            return Status.Failure;
        timer = time;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        timer -= Time.deltaTime;
        Katana.Value.transform.Rotate(animationSpeed * Time.deltaTime, 0, 0);
        Katana.Value.Attack();
        if (timer <= 0)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Katana.Value.transform.rotation = Quaternion.identity;
    }
}

