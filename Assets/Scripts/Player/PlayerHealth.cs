using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float _currentHealth;
    private bool _isInvincible = false;
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private GameObject lossText;
    public bool lose = false;

    void Start()
    {
        _currentHealth = maxHealth;
    }

    void Update()
    {
        if (_currentHealth <= 0)
        {
            lossText.SetActive(true);
            lose = true;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_isInvincible)
        {
            _currentHealth -= damage;
        }
    }

    public void BecomeInvinciblePermanently()
    {
        _isInvincible = true;
    }

    public IEnumerator BecomeInvincibleTemporarily(float time)
    {
        _isInvincible = true;

        yield return new WaitForSeconds(time);

        _isInvincible = false;
    }
}
