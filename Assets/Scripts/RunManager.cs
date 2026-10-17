using UnityEngine;

public class RunManager : MonoBehaviour
{
    [SerializeField] PlanetData planet;

    public PlanetData Planet => planet;
    public float Progress { get; private set; }          // 0 = start, 1 = arrived
    public bool IsRunning { get; private set; } = true;

    float elapsed;

    void Update()
    {
        if (!IsRunning) return;

        elapsed += Time.deltaTime;
        Progress = Mathf.Clamp01(elapsed / planet.runDuration);

        if (Progress >= 1f)
        {
            IsRunning = false;
            Debug.Log($"Reached {planet.displayName}! YOU WIN");   // placeholder until Phase 6
        }
    }
}