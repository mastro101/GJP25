using UnityEngine;
using Eastermaster.Helper;

public class Katana : Enemy
{
    [SerializeField] Transform hitBoxTop;
    [SerializeField] Transform hitBoxBottom;
    [SerializeField] float hitBoxRadius = 1;

    PlayerMovement player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>();
    }

    public Transform GetPlayerTransform() { return player.transform; }

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapCapsule(hitBoxTop.position, hitBoxBottom.position, hitBoxRadius);

        foreach (Collider collider in colliders)
        {
            IDamageable damagable = collider.GetComponentInNearParents<IDamageable>();
            if (damagable == null)
                continue;
            if (damagable is Enemy)
                continue;
            damagable.Damage();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitBoxBottom.position, hitBoxRadius);
        Gizmos.DrawWireSphere(hitBoxTop.position, hitBoxRadius);
        Gizmos.DrawLine(hitBoxTop.position + (Vector3.right * hitBoxRadius), hitBoxBottom.position + (Vector3.right * hitBoxRadius));
        Gizmos.DrawLine(hitBoxTop.position + (Vector3.left * hitBoxRadius), hitBoxBottom.position + (Vector3.left * hitBoxRadius));
        Gizmos.DrawLine(hitBoxTop.position + (Vector3.forward * hitBoxRadius), hitBoxBottom.position + (Vector3.forward * hitBoxRadius));
        Gizmos.DrawLine(hitBoxTop.position + (Vector3.back * hitBoxRadius), hitBoxBottom.position + (Vector3.back * hitBoxRadius));
    }
#endif
}