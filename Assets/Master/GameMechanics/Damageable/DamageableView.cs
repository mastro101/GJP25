using Eastermaster;
using UnityEngine;
using UnityEngine.UI;

public class DamageableView : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IDamageable))] GameObject _damageable;
    [SerializeField] Image image;
    IDamageable damageable;
    private void OnEnable()
    {
        damageable = _damageable.GetComponent<IDamageable>();
        damageable.OnHealthChange_event += UpdateView;
        UpdateView(damageable.MaxHealth);
    }

    private void OnDisable()
    {
        damageable.OnHealthChange_event -= UpdateView;
    }

    void UpdateView(int health)
    {
        if (health > 0)
        {
            image.fillAmount = (float)health / (float)damageable.MaxHealth;
        }
        else
            image.fillAmount = 0;
    }
}