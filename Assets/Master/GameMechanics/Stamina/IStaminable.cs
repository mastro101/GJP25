public interface IStaminable
{
    public int MaxStamina { get; }
    public float Stamina { get; set; }
    public float WaitTimeToGenerateStamina { get; set; }
    public float StaminaGenerateSpeed { get; set; }

    System.Action<float> OnStaminaChange_event { get; set; }
    System.Action<int> OnStaminaConsume_event { get; set; }
    System.Action OnStaminaGenerate_event { get; set; }
    System.Action OnEmpty_event { get; set; }

    public void Consume(int value = 1)
    {
        Stamina -= value;
        OnStaminaConsume_event?.Invoke(value);
        if (Stamina <= 0)
        {
            Stamina = 0;
            OnEmpty_event?.Invoke();
        }
    }

    public void GenerateStamina(float deltaTime)
    {
        Stamina += StaminaGenerateSpeed * deltaTime;
        OnStaminaGenerate_event?.Invoke();
    }

    public void GenerateStamina(int value)
    {
        Stamina += value;
        OnStaminaGenerate_event?.Invoke();
    }

    public void Setup()
    {
        Stamina = MaxStamina;
    }
}
