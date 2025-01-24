using UnityEngine;

public class Enemy : MonoBehaviour , IDetectable
{
    [SerializeField] int _health = 2;
    public int health
    {
        get => _health;
        set
        {
            if (_health <= 0)
            {
                _health = 0;
                OnHealth0();
            }
            else
            {
                _health = value;
            }
        }
    }
    public System.Action<int> OnDamage_event;

    private void OnEnable()
    {
        OnDamage_event += OnDamage;
    }

    private void OnDisable()
    {
        OnDamage_event -= OnDamage;
    }

    public void Hit(int damage = 1)
    {
        health -= damage;
        OnDamage(damage);
    }

    public void OnDamage(int damage)
    {
        Debug.LogFormat("-{0} {1}", damage, gameObject.name);
    }

    public void OnHealth0()
    {
        Debug.Log("è morto");
    }
}