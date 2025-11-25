using UnityEngine;

namespace Lightning
{
    public class LightningBolt : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float lifespan = 1f;

        private void Start()
        {
            lifespan = Time.unscaledTime + lifespan;
        }

        private void Update()
        {
            float time = Time.unscaledTime;
            if (time > lifespan)
            { 
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerHealth health))
            {
                Debug.Log("Dealing damage to Player!");
                health.TakeDamage(damage);
            }
        }
    }
}
