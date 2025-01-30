using Eastermaster;
using UnityEngine;
using UnityEngine.UI;

public class StaminaView : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IStaminable))] GameObject _staminable;
    [SerializeField] Image image;
    IStaminable staminable;
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
            image.fillAmount = 0;
            return;
        }

        float currentFill = image.fillAmount;
        image.fillAmount = stamina / (float)staminable.MaxStamina;
    }
}
