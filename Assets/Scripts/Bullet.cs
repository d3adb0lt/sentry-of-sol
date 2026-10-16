using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 14f;
    [SerializeField] float lifetime = 2f;

    private void Start()
    {
        // transform.up -> dir bullet is spawned facing
        GetComponent<Rigidbody2D>().linearVelocity = transform.up * speed;
        Destroy(gameObject, lifetime);
    }
}