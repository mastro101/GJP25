using System;
using Unity.VisualScripting;
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

    public event Action<bool> OnDetectableChange;

    bool _detectable = true;
    public bool Detectable
    {
        get => _detectable;
        protected set 
        {
            if (_detectable == value) return;
            _detectable = value;
            OnDetectableChange?.Invoke(_detectable);
        }
    }

    void StopDetectable() => Detectable = false;

    protected virtual void Awake()
    {
        Detectable = true;
        OnDeath_event += StopDetectable;
    }

    protected virtual void OnDisable()
    {
        OnDeath_event -= StopDetectable;
        Detectable = false;
    }
}