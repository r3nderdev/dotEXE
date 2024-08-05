using UnityEngine;

public class LaserCollision : MonoBehaviour
{
    [SerializeField] GameObject particles;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Laser") && !other.gameObject.CompareTag("Dialogue"))
        {
            // Spawn particles and destroy
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
