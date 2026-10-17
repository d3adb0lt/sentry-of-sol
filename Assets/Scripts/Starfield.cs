using UnityEngine;

public class Starfield : MonoBehaviour
{
    [SerializeField] RunManager run;
    [SerializeField] Sprite starSprite;
    [SerializeField] int starCount = 80;
    [SerializeField] float minSpeed = 1f;    // at progress 0 to planet
    [SerializeField] float maxSpeed = 8f;    // at progress 1 to planet
    [SerializeField] AnimationCurve speedCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    Transform[] stars;
    float[] depths;   // 0.3 = far (small, dim, slow) and 1 = near (big, bright, fast)
    float left, right, bottom, top;

    void Start()
    {
        Camera cam = Camera.main;
        Vector2 min = cam.ViewportToWorldPoint(new Vector2(0f, 0f));
        Vector2 max = cam.ViewportToWorldPoint(new Vector2(1f, 1f));
        left = min.x;
        right = max.x;
        bottom = min.y - 0.5f;   // slightly offscreen so stars don't pop in and out visibly
        top = max.y + 0.5f;

        stars = new Transform[starCount];
        depths = new float[starCount];

        for (int i = 0; i < starCount; i++)
        {
            var go = new GameObject("Star");
            go.transform.SetParent(transform);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = starSprite;
            sr.sortingOrder = -10;   // behind everything

            depths[i] = Random.Range(0.3f, 1f);
            go.transform.localScale = Vector3.one * 0.08f * depths[i];
            sr.color = new Color(1f, 1f, 1f, depths[i]);
            go.transform.position = new Vector3(Random.Range(left, right), Random.Range(bottom, top), 0f);

            stars[i] = go.transform;
        }
    }

    void Update()
    {
        float speed = Mathf.Lerp(minSpeed, maxSpeed, speedCurve.Evaluate(run.Progress));

        for (int i = 0; i < stars.Length; i++)
        {
            Vector3 p = stars[i].position;
            p.y -= speed * depths[i] * Time.deltaTime;

            if (p.y < bottom)   // wrap back to the top
            {
                p.y = top;
                p.x = Random.Range(left, right);
            }
            stars[i].position = p;
        }
    }
}