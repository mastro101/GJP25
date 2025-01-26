using System;
using UnityEngine;

public class PlayerData : MonoBehaviour, IDamageable, IStaminable
{
    //----Health----//
    [SerializeField] int _maxHealth = 10;
    public int MaxHealth { get => _maxHealth; private set { _maxHealth = value; } }
    [SerializeField] int _health;
    public int Health
    {
        get => _health;
        set
        {
            if (value == _health)
                return;

            _health = value > 0 ? value : 0;
            OnHealthChange_event?.Invoke(_health);
        }
    }

    public Action<int> OnHealthChange_event { get; set; }
    public Action<int> OnDamage_event { get; set; }
    public Action OnDeath_event { get; set; }
    //------------//

    //----Stamina----//
    [SerializeField] int _maxStamina = 10;
    public int MaxStamina { get => _maxStamina; private set { _maxStamina = value; } }
    
    [SerializeField] float _stamina = 10;
    public float Stamina
    {
        get => _stamina;
        set
        {
            _stamina = value > 0 ? value : 0;
            OnStaminaChange_event?.Invoke(value);
        }
    }

    [SerializeField] float _waitTimeToGenerateStamina = 1f;
    public float WaitTimeToGenerateStamina { get => _waitTimeToGenerateStamina; set => _waitTimeToGenerateStamina = value; }
    [SerializeField] float _staminaGenerateSpeed = 1f;
    public float StaminaGenerateSpeed { get => _staminaGenerateSpeed; set => _staminaGenerateSpeed = value; }
    public Action<float> OnStaminaChange_event { get; set; }
    public Action<float> OnStaminaConsume_event { get; set; }
    public Action OnStaminaGenerate_event { get; set; }
    public Action OnEmpty_event { get; set; }

    float cooldownGenerationStamina;
    void ResetTimerStamina(float i)
    {
        cooldownGenerationStamina = WaitTimeToGenerateStamina;
    }
    //------------//

    //----Attack----//
    [SerializeField] public int power = 1;
    //------------//

    private void OnEnable()
    {
        (this as IDamageable).Setup();
        (this as IStaminable).Setup();
        OnStaminaConsume_event += ResetTimerStamina;
    }

    private void OnDisable()
    {
        OnStaminaConsume_event -= ResetTimerStamina;
    }

    private void Update()
    {
        if (Stamina < MaxStamina && cooldownGenerationStamina <= 0)
        {
            (this as IStaminable).GenerateStamina(Time.deltaTime);
        }
        else
        {
            cooldownGenerationStamina -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            (this as IDamageable).Damage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            (this as IStaminable).Consume();
        }
    }
}