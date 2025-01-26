using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToPlayer", story: "Move [Agent] to Player", category: "Action", id: "7ec1039b288d300867ce41616dcf7365")]
public partial class MoveToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");
    GameObject Target;

    // This will only be used in movement without a navigation agent.
    [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new BlackboardVariable<float>(1.0f);

    private Animator m_Animator;
    private float m_PreviousStoppingDistance;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_ColliderAdjustedTargetPosition;
    private float m_ColliderOffset;

    protected override Status OnStart()
    {
        PlayerMovement temp = MonoBehaviour.FindFirstObjectByType<PlayerMovement>();
        if (temp != null)
        {
            Target = temp.gameObject;
        }
        if (Agent.Value == null || Target == null)
        {
            return Status.Failure;
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target == null)
        {
            return Status.Failure;
        }

        // Check if the target position has changed.
        bool boolUpdateTargetPosition = !Mathf.Approximately(m_LastTargetPosition.x, Target.transform.position.x) || !Mathf.Approximately(m_LastTargetPosition.y, Target.transform.position.y) || !Mathf.Approximately(m_LastTargetPosition.z, Target.transform.position.z);
        if (boolUpdateTargetPosition)
        {
            m_LastTargetPosition = Target.transform.position;
            m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();
        }

        float distance = GetDistanceXZ();
        if (distance <= (DistanceThreshold + m_ColliderOffset))
        {
            return Status.Success;
        }


        float speed = Speed;

        if (SlowDownDistance > 0.0f && distance < SlowDownDistance)
        {
            float ratio = distance / SlowDownDistance;
            speed = Mathf.Max(0.1f, Speed * ratio);
        }

        Vector3 agentPosition = Agent.Value.transform.position;
        Vector3 toDestination = m_ColliderAdjustedTargetPosition - agentPosition;
        toDestination.y = 0.0f;
        toDestination.Normalize();
        agentPosition += toDestination * (speed * Time.deltaTime);
        Agent.Value.transform.position = agentPosition;

        // Look at the target.
        Agent.Value.transform.forward = toDestination;
        

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, 0);
        }

        m_Animator = null;
    }

    protected override void OnDeserialize()
    {
        Initialize();
    }

    private Status Initialize()
    {
        m_LastTargetPosition = Target.transform.position;
        m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();

        // Add the extents of the colliders to the stopping distance.
        m_ColliderOffset = 0.0f;
        Collider agentCollider = Agent.Value.GetComponentInChildren<Collider>();
        if (agentCollider != null)
        {
            Vector3 colliderExtents = agentCollider.bounds.extents;
            m_ColliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.z);
        }

        if (GetDistanceXZ() <= (DistanceThreshold + m_ColliderOffset))
        {
            return Status.Success;
        }

        // If using animator, set speed parameter.
        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, Speed);
        }

        return Status.Running;
    }


    private Vector3 GetPositionColliderAdjusted()
    {
        Collider targetCollider = Target.GetComponentInChildren<Collider>();
        if (targetCollider != null)
        {
            return targetCollider.ClosestPoint(Agent.Value.transform.position);
        }
        return Target.transform.position;
    }

    private float GetDistanceXZ()
    {
        Vector3 agentPosition = new Vector3(Agent.Value.transform.position.x, m_ColliderAdjustedTargetPosition.y, Agent.Value.transform.position.z);
        return Vector3.Distance(agentPosition, m_ColliderAdjustedTargetPosition);
    }
}