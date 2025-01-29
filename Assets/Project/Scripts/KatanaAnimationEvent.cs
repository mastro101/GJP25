using Eastermaster.Helper;
using UnityEngine;

public class KatanaAnimationEvent : MonoBehaviour
{
    Katana katana;

    private void Awake()
    {
        katana = this.GetComponentInNearParents<Katana>();
    }

    public void ToggleAttackCollider()
    {
        katana.ToggleAttackCollide();
    }

    public void ToggleSlowLookAtPlayer()
    {
        katana.ToggleSlowLookAtPlayer();
    }
}