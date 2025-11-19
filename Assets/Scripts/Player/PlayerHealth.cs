using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float _currentHealth;
    [SerializeField] private float maxHealth = 30f;
    
    void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
    }
}
