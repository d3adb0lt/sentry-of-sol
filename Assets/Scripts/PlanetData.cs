using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData", menuName = "Game/Planet Data")]
public class PlanetData : ScriptableObject
{
    public string displayName = "Neptune";

    [Min(1f)]
    public float runDuration = 60f;

    [Tooltip("Seconds between spawns at the start of the run")]
    public float startSpawnInterval = 1.6f;

    [Tooltip("Seconds between spawns right before reaching the planet")]
    public float endSpawnInterval = 0.8f;

    public Color color = new Color(0.3f, 0.5f, 1f);
}