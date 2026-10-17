using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] private RunManager run;
    [SerializeField] float spawnMargin = 1f;

    [Header("Direction")]
    [Tooltip("Chance of enemy spawning to sides of straight up")]
    [SerializeField, Range(0f, 1f)] float flankChance = 0.2f;
    [Tooltip("Degrees either side of straight up")]
    [SerializeField, Range(5f, 60f)] float aheadSpread = 30f;

    float radius;
    float minAngle;   // lowest allowed angle. It's the screen's horizontal midline
    float timer;

    void Start()
    {
        Camera cam = Camera.main;
        Vector2[] viewportCorners = { new(0, 0), new(1, 0), new(0, 1), new(1, 1) };

        float farthest = 0f;
        foreach (Vector2 v in viewportCorners)
        {
            Vector2 corner = cam.ViewportToWorldPoint(v);
            farthest = Mathf.Max(farthest, corner.magnitude);
        }
        radius = farthest + spawnMargin;

        float midY = cam.transform.position.y;   // camera center = middle of the screen
        minAngle = Mathf.Asin(Mathf.Clamp(midY / radius, -1f, 1f)) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if (!run.IsRunning) return; // run over? halt spawning
        
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        float angle = PickSpawnAngle() * Mathf.Deg2Rad;
        Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        Instantiate(enemyPrefab, pos, Quaternion.identity);

        timer = Mathf.Lerp(run.Planet.startSpawnInterval, run.Planet.endSpawnInterval, run.Progress);
    }

    float PickSpawnAngle()
    {
        if (Random.value > flankChance)
            return 90f + Random.Range(-aheadSpread, aheadSpread);   // ahead

        float rightFlank = Random.Range(minAngle, 90f - aheadSpread);
        return Random.value < 0.5f ? rightFlank : 180f - rightFlank;   // mirror to the left
    }
}