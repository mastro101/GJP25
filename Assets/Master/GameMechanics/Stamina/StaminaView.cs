using Eastermaster;
using UnityEngine;
using UnityEngine.UI;

public class StaminaView : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IStaminable))] GameObject _staminable;
    [SerializeField] Image image;
    IStaminable staminable;
    float currentStamina;

    private void OnEnable()
    {
        staminable = _staminable.GetComponent<IStaminable>();
        staminable.OnStaminaChange_event += UpdateView;
        UpdateView(staminable.MaxStamina);
    }

    private void OnDisable()
    {
        staminable.OnStaminaChange_event -= UpdateView;
    }

    void UpdateView(float stamina)
    {
        if (stamina <= 0)
        {
            currentStamina = 0;
            return;
        }

        currentStamina = stamina;
        //image.fillAmount = stamina / (float)staminable.MaxStamina;
    }

    private void Update()
    {
        if(image.fillAmount != (currentStamina / (float)staminable.MaxStamina))
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, (currentStamina / (float)staminable.MaxStamina), 0.4f);
        }
    }
}
