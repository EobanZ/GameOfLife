using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    bool? isAlive = true;
    bool? nextState = null;
    float lifeTime = 0; 
    Vector2Int position;
    Vector2Int fieldSize;
    float maxlifeTime; 
    Color aliveColor;
    Color deadColor;
    Color gradientColor;
    SpriteRenderer spriteRenderer;
    [SerializeField] CellSO CellScriptableObject; //ref to the Cell Properties Scriptable Object to get the settings

    public bool? IsAlive { get { return isAlive; } }

#region private methods

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fieldSize = GameManager.Instance.FieldSize;

        //Get the settings from the Scriptable Object
        aliveColor = CellScriptableObject.m_aliveColor;
        deadColor = CellScriptableObject.m_deadColor;
        gradientColor = CellScriptableObject.m_gradientColor;
        maxlifeTime = CellScriptableObject.m_maxLifetime;
        spriteRenderer.sprite = CellScriptableObject.m_sprite;

        UpdateSprites(); //Updates the Sprites Colors once the game is started
    }

    private void Update()
    {
        //update the lifeTime for GradientColor
        if(isAlive == true)
            lifeTime += Time.deltaTime;
        else
            lifeTime = 0;
       
    }

    /// <summary>
    /// returns the number of living neighbors
    /// </summary>
    /// <returns></returns>
    private int CountLivingNeighbors()
    {
        // The number of living neighbors
        int value = 0;

        //This nested loop goes through all neighbours and checks if they are alive
        for (var j = -1; j <= 1; j++)
        {
            int k = (position.y + j + fieldSize.y) % fieldSize.y;

            for (int i = -1; i <= 1; i++)
            {
                int h = (position.x + i + fieldSize.x) % fieldSize.x;

                value += GameManager.Instance.Cells[h, k].IsAlive == true ? 1 : 0; 
            }
        }
        //Substract 1 if this Cell is alive since we counted it as a neighbor
        return  value - (this.IsAlive == true ? 1 : 0);
        
    }

    /// <summary>
    /// Returns the State for the next generation depending on the numbers of living neighbors
    /// </summary>
    /// <param name="livingNeighbors"></param>
    /// <returns>true if alive</returns>
    private bool ChooseNextState(int livingNeighbors)
    {
        return isAlive == true && (livingNeighbors == 2 || livingNeighbors == 3) || isAlive == false && livingNeighbors == 3;
  
    }

    private void UpdateSprites()
    {
        if (isAlive == false)
        {
            spriteRenderer.color = deadColor;    
        }
        else
        {
            spriteRenderer.color = Color.Lerp(aliveColor, gradientColor, Mathf.Clamp01(lifeTime/maxlifeTime));
        }
        
    }


#endregion

#region public methods

    public void ApplyNextGeneration()
    {
        if (nextState != null)
        {
            isAlive = nextState;
            nextState = null;

        }
        UpdateSprites();
    }

    public void SetFieldPosition(int x, int y)
    {
        position = new Vector2Int(x, y);
    }

    /// <summary>
    /// Sets the cell alive or dead
    /// </summary>
    /// <param name="b">true -> alive, false -> dead</param>
    public void SetAlive(bool b)
    {      
            isAlive = b;   
    }

    public void GetNextState()
    {
 
        nextState = ChooseNextState(CountLivingNeighbors());
        
        
    }

#endregion

}
