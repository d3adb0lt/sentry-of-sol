using UnityEngine;
using UnityEngine.UI;

public class SignRailUI : MonoBehaviour
{
    [SerializeField] SignRail rail;
    [SerializeField] Image[] cellImages;
    [SerializeField] Color emptyColor = new(1f, 1f, 1f, 0.15f);
    [SerializeField] Color fullColor = new(0.96f, 0f,.27f, 1f);

    void OnEnable()  => rail.CellsChanged += Refresh;   // start listening
    void OnDisable() => rail.CellsChanged -= Refresh;   // stop listening
    void Start()     => Refresh(rail.Cells);

    void Refresh(int cells)
    {
        for (int i = 0; i < cellImages.Length; i++)
            cellImages[i].color = i < cells ? fullColor : emptyColor;
    }
}