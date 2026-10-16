using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] float spawnInterval = 1.5f;
    [SerializeField] float spawnMargin = 1f;

    float radius;
    float timer;

    void Start()
    {
        Camera cam = Camera.main;
        Vector2[] viewportCorners = { new(0, 0), new(1, 0), new(0, 1), new(1, 1) };

        float farthest = 0f;
        foreach (Vector2 v in viewportCorners)
        {
            Vector2 corner = cam.ViewportToWorldPoint(v);   // world position of that screen corner
            farthest = Mathf.Max(farthest, corner.magnitude);   // distance from the ship at origin
        }
        radius = farthest + spawnMargin;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        Instantiate(enemyPrefab, pos, Quaternion.identity);

        timer = spawnInterval;
    }
}