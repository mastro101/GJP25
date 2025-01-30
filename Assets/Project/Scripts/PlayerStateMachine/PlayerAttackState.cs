using Eastermaster.StateMachineAnimator;
using UnityEngine;

public class PlayerAttackState : StateAnimator<PlayerContext>
{
    public override void OnEnter()
    {
        base.OnEnter();
        ctx.playerMovement.CanAttack(true);
    }

    public override void OnTick()
    {
        base.OnTick();
        if (ctx.playerMovement.GetAttackDurationTimer() <= 0)
        {
            ctx.Next();
        }
        Debug.Log(ctx.playerMovement.GetAttackDurationTimer());
    }

    public override void OnExit()
    {
        base.OnExit();
        ctx.playerMovement.CanAttack(false);
    }
}
