using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;

    public int Current { get; private set; }

    void Awake()
    {
        Current = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (Current <= 0) return;   // already dead

        Current -= amount;
        Debug.Log($"Ship hit! HP: {Current}/{maxHealth}");

        if (Current <= 0)
        {
            Debug.Log("GAME OVER");
            gameObject.SetActive(false);   // replace with win/lost condition when ready
        }
    }
}