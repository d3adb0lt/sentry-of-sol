using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] int maxHealth = 2;
    [SerializeField] int contactDamage = 1;

    int health;

    void Start()
    {
        health = maxHealth;

        // The ship sits at origin, toward ship = toward origin
        Vector2 dir = -((Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
        transform.up = dir;   // face the ship
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            Destroy(bullet.gameObject);
            TakeDamage(1);
        }
        else if (other.TryGetComponent(out ShipHealth ship))
        {
            ship.TakeDamage(contactDamage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Destroy(gameObject);
    }
}