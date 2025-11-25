using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float _currentHealth;
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private GameObject lossText;

    void Start()
    {
        _currentHealth = maxHealth;
    }

    void Update()
    {
        if (_currentHealth <= 0)
        {
            lossText.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
    }
}
