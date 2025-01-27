using Eastermaster.StateMachineAnimator;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : StateAnimator<PlayerContext>
{
    public override void OnTick()
    {
        base.OnTick();
        ctx.playerMovement.Move();
    }
}