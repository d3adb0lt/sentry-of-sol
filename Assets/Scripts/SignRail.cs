using System;
using UnityEngine;

public class SignRail : MonoBehaviour
{
    [SerializeField] int maxCells = 3;

    public int Cells { get; private set; }
    public int MaxCells => maxCells;
    public bool IsFull => Cells >= maxCells;

    public event Action<int> CellsChanged;   // announces the new cell count

    public bool TryAddCell()
    {
        if (IsFull) return false;
        Cells++;
        CellsChanged?.Invoke(Cells);
        return true;
    }

    public bool TryConsumeCell()
    {
        if (Cells <= 0) return false;
        Cells--;
        CellsChanged?.Invoke(Cells);
        return true;
    }

    // Right-click the component header in Play mode to run these, sick as ell
    [ContextMenu("Debug: Add Cell")]     void DebugAdd()     => TryAddCell();
    [ContextMenu("Debug: Consume Cell")] void DebugConsume() => TryConsumeCell();
}