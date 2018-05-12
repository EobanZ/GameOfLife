using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellProperties", menuName = "Cell")]
public class CellSO : ScriptableObject {

    [SerializeField] public Sprite m_sprite;
    [SerializeField] public float m_maxLifetime;
    [SerializeField] public Color m_aliveColor;
    [SerializeField] public Color m_deadColor;
    [SerializeField] public Color m_gradientColor;
}
