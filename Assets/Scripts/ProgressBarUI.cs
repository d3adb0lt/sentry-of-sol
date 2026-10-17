using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] RunManager run;
    [SerializeField] RectTransform shipMarker;
    [SerializeField] Image planetIcon;

    void Start()
    {
        planetIcon.color = run.Planet.color;   // the icon matches the current planet's data
    }

    void Update()
    {
        Vector2 anchor = new Vector2(0.5f, run.Progress);
        shipMarker.anchorMin = anchor;
        shipMarker.anchorMax = anchor;
        shipMarker.anchoredPosition = Vector2.zero;
    }
}