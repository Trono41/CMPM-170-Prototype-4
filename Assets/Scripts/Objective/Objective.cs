using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private GameObject winText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            winText.SetActive(true);
        }
    }
}
