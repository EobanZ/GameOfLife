using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {
    bool isChosen;
    Vector2Int position;

	public void ToggleActiveInMapEditor()
    {
        MapEditor.Instance.ToggleCellAt(position);
    }

    public void ActivateCellInMapEditor()
    {
        MapEditor.Instance.ActivateCellAt(position);
    }

    public void DeactivateCellInMapEditor()
    {
        MapEditor.Instance.DeactivateCellAt(position);
    }
}
