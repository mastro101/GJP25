using UnityEngine;
using Eastermaster.Helper;

public class Katana : Enemy
{
    [SerializeField] Transform hitBoxTop;
    [SerializeField] Transform hitBoxBottom;
    [SerializeField] float hitBoxRadius = 1;

    Animator animator;
    PlayerMovement player;
    bool canAttack;
    bool canSlowLookingAtPlayer;

    override protected void Awake()
    {
        base.Awake();
        canAttack = false;
        canSlowLookingAtPlayer = true;
        player = FindFirstObjectByType<PlayerMovement>();
        animator = this.GetComponentInNearChildren<Animator>();
    }

    private void OnEnable()
    {
        OnDamage_event += SetGotHit;
    }

    override protected void OnDisable()
    {
        OnDamage_event -= SetGotHit;
    }

    void SetGotHit(int dontuse)
    {
        animator.SetTrigger("GotHit");
    }

    public Transform GetPlayerTransform() { return player.transform; }

    public bool Attack()
    {
        if (!canAttack)
            return false;

        Collider[] colliders = Physics.OverlapCapsule(hitBoxTop.position, hitBoxBottom.position, hitBoxRadius);

        foreach (Collider collider in colliders)
        {
            IDamageable damagable = collider.GetComponentInNearParents<IDamageable>();
            if (damagable == null)
                continue;
            if (damagable is Enemy)
                continue;
            damagable.Damage();
            return true;
        }
        return false;
    }

    public void SlowLookAtPlayer(float speed, float delta)
    {
        if (!canSlowLookingAtPlayer)
            return;

        var rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, delta * speed);
    }

    public void ToggleSlowLookAtPlayer()
    {
        canSlowLookingAtPlayer = !canSlowLookingAtPlayer;
    }

    public void ToggleAttackCollide()
    {
        canAttack = !canAttack;
    }

    public void SetAttackCollide(bool b)
    {
        canAttack = b;
    }

    public void SetSlowLookAtPlayer(bool b)
    {
        canSlowLookingAtPlayer = b;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            (this as IDamageable).Damage(MaxHealth);
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