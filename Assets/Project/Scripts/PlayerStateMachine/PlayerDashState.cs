using Eastermaster.StateMachineAnimator;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashState : StateAnimator<PlayerContext>
{
    public override void OnEnter()
    {
        base.OnEnter();
        ctx.playerMovement.CanDash(true);
    }

    public override void OnTick()
    {
        base.OnTick();
        if (ctx.playerMovement.GetDashDurationTimer() <= 0)
        {
            ctx.Next();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        ctx.playerMovement.CanDash(false);
    }
}