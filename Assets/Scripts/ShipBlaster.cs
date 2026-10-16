using UnityEngine;

[RequireComponent(typeof(ShipAim))]
public class ShipBlaster : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] float fireRate = 6f;   // shots per second

    ShipAim aim;
    float cooldown;

    void Awake()
    {
        aim = GetComponent<ShipAim>();
    }

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (aim.IsAligned && cooldown <= 0f)
        {
            Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            cooldown = 1f / fireRate;
        }
    }
}