using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject player;
    public bool win = false;
    PlayerHealth health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            win = true;
            winText.SetActive(true);
            health.BecomeInvinciblePermanently();
        }
    }
}
