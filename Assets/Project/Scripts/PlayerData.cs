using System;
using UnityEngine;

public class PlayerData : MonoBehaviour, IDamageable
{
    [SerializeField] int _health = 1;
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
    public Action<int> OnHit_event { get; set; }
    public Action OnDeath_event { get; set; }

    [SerializeField] int _stamina = 10;
    int Stamina
    {
        get => _stamina;
        set
        {
            _stamina = value > 0 ? value : 0;
            OnStaminaChange_event?.Invoke(value);
        }
    }
    public Action<int> OnStaminaChange_event;

    public void ConsumeStamina(int value = 1)
    {
        Stamina -= value;
    }

    [SerializeField] public int power;

    IDamageable myself;
    private void Awake()
    {
        myself = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            myself.Hit();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ConsumeStamina();
        }
    }
}