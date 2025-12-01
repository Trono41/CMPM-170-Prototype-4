using UnityEngine;

public class TornadoLifetime : MonoBehaviour
{
    public float lifetimeSeconds = 5f;

    void Start()
    {
        Destroy(gameObject, lifetimeSeconds);
    }
}