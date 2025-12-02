using UnityEngine;

namespace Lightning
{
    public class LightningBolt : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float lifespan = 1f;
        [SerializeField] private AudioClip lightningSound;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            lifespan = Time.unscaledTime + lifespan;

            if (audioSource != null && lightningSound != null)
            {
                audioSource.PlayOneShot(lightningSound);
            }
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
                health.TakeDamage(damage);
            }
        }
    }
}