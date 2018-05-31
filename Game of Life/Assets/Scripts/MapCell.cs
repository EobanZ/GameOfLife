using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {

    Vector2Int position;

    public Vector2Int Position { get { return position; } set { position = value; } }

    public void ChangeColor(bool b)
    {
        if (b)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

}
