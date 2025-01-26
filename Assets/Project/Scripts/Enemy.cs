using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDetectable, IDamageable
{
    [SerializeField] int _maxHealth = 10;
    public int MaxHealth => _maxHealth;
    
    [SerializeField] public int _health = 1;
    int IDamageable.Health
    {
        get => _health;
        set
        {
            _health = value > 0 ? value : 0;
            OnHealthChange_event?.Invoke(value);
        }
    }

    public Action<int> OnHealthChange_event { get; set; }
    public Action<int> OnDamage_event { get; set; }
    public Action OnDeath_event { get; set; }
}