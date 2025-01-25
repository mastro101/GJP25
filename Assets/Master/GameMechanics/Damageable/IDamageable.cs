using UnityEngine;

public interface IDamageable
{
    GameObject gameObject { get; }

    protected int Health { get; set; }

    System.Action<int> OnHealthChange_event { get; set; }
    System.Action<int> OnHit_event { get; set; }
    System.Action OnDeath_event { get; set; }

    public void Hit(int damage = 1)
    {
        Health -= damage;
        if (Health < 0)
            return;

        OnHit_event?.Invoke(damage);
    }
}
