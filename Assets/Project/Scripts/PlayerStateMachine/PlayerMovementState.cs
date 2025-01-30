using Eastermaster.StateMachineAnimator;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : StateAnimator<PlayerContext>
{
    public override void OnEnter()
    {
        base.OnEnter();
        ctx.playerMovement.CanMove(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        ctx.playerMovement.CanMove(false);
    }
}