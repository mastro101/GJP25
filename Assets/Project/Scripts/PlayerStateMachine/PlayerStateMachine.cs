using Eastermaster.StateMachineAnimator;
using UnityEngine;

public class PlayerStateMachine : StateMachineAnimator<PlayerContext>
{
    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerMovement playerMovement;
    protected override void InitCtx()
    {
        ctx = new PlayerContext(playerMovement.gameObject, animator, playerData, playerMovement);
    }
}

public class PlayerContext : BaseStateMachineContext
{
    public PlayerData playerData;
    public PlayerMovement playerMovement;

    public PlayerContext(GameObject self, Animator animator, PlayerData playerData, PlayerMovement playerMovement) : base(self, animator)
    {
        this.playerData = playerData;
        this.playerMovement = playerMovement;
    }
}
