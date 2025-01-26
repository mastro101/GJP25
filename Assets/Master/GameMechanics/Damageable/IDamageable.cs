public interface IDamageable
{
    public int MaxHealth { get; }
    public int Health { get; set; }

    System.Action<int> OnHealthChange_event { get; set; }
    System.Action<int> OnDamage_event { get; set; }
    System.Action OnDeath_event { get; set; }

    public void Damage(int damage = 1)
    {
        Health -= damage;
        if (Health <= 0)
        {
            OnDeath_event?.Invoke();
            return;
        }

        OnDamage_event?.Invoke(damage);
    }

    public void Setup()
    {
        Health = MaxHealth;
    }
}
