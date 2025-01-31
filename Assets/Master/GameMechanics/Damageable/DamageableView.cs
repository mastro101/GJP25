using Eastermaster;
using UnityEngine;
using UnityEngine.UI;

public class DamageableView : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IDamageable))] GameObject _damageable;
    [SerializeField] Image image;
    IDamageable damageable;
    float currentHealth;
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
            currentHealth = health;
            //image.fillAmount = (float)health / (float)damageable.MaxHealth;
        }
        else
            currentHealth = 0;
    }

    private void Update()
    {
        if (image.fillAmount != (currentHealth / (float)damageable.MaxHealth))
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, (currentHealth / (float)damageable.MaxHealth), 0.4f);
        }
    }
}