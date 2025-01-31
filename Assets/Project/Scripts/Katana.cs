using UnityEngine;
using Eastermaster.Helper;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.Behavior;

public class Katana : Enemy
{
    [SerializeField] Transform hitBoxTop;
    [SerializeField] Transform hitBoxBottom;
    [SerializeField] float hitBoxRadius = 1;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] ParticleSystem deathShatteringParticles;

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

    private void Start()
    {
        (this as IDamageable).Health = MaxHealth;
    }

    private void OnEnable()
    {
        OnDamage_event += SetGotHit;
        OnDeath_event += Death;
    }

    override protected void OnDisable()
    {
        OnDamage_event -= SetGotHit;
    }

    public void Death()
    {
        deathShatteringParticles.Play();
        gameOverText.text = "Steel Shattered";
        gameOverText.color = new Color(0.9882353f, 0.8901961f, 0.4627451f);
        StartCoroutine(ShowGameOver());
        var tree = this.GetComponentInNearParents<BehaviorGraphAgent>();
        tree.enabled = false;
        animator.gameObject.SetActive(false);
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(3);
        gameOverPanel.SetActive(true);
    }

    void SetGotHit(int dontuse)
    {
        animator.SetTrigger("GotHit");
    }

    public Transform GetPlayerTransform() { return player.transform; }

    List<IDamageable> hittedCollider = new List<IDamageable>();
    public bool Attack(int damage)
    {
        if (!canAttack)
            return false;

        Collider[] colliders = Physics.OverlapCapsule(hitBoxTop.position, hitBoxBottom.position, hitBoxRadius);

        foreach (Collider collider in colliders)
        {
            IDamageable damagable = collider.GetComponentInNearParents<IDamageable>();
            if (damagable == null)
                continue;
            if (damagable == this as IDamageable)
                continue;
            if (hittedCollider.Contains(damagable))
                continue;
            hittedCollider.Add(damagable);
            damagable.Damage(damage);
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
        SetAttackCollide(!canAttack);
    }

    public void SetAttackCollide(bool b)
    {
        canAttack = b;
        if (canAttack == false)
        {
            hittedCollider.Clear();
        }
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