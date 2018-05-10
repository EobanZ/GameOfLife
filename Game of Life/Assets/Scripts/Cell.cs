using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    bool? isAlive = true;
    bool? nextState = null;
    float lifeTime = 0; //the amount of seconds this cell is alive
    Vector2Int position;
    Vector2Int fieldSize;
    int livingNeighbours;
    [SerializeField] float maxlifeTime = 10f; //After this amount of seconds the Cell is the gradientColor
    [SerializeField] Color aliveColor = Color.white;
    [SerializeField] Color deadColor = Color.black;
    [SerializeField] Color gradientColor = Color.red;

    public bool? IsAlive { get { return isAlive; } }

#region private methods

    private void Start()
    {
        fieldSize = GameManager.Instance.FieldSize;
        UpdateSprites(); //Updates the Sprites Colors once the game is started
    }

    private void Update()
    {
        if(isAlive == true)
            lifeTime += Time.deltaTime;
        else
            lifeTime = 0;
       
    }

    IEnumerator CountLivingNeighbors()
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
        livingNeighbours = value - (this.IsAlive == true ? 1 : 0);
        yield return null;
    }

    private bool ChooseNextState(int livingNeighbors)
    {
        return isAlive == true && (livingNeighbours == 2 || livingNeighbours == 3) || isAlive == false && livingNeighbours == 3;
  
    }

    private void UpdateSprites()
    {
        if (isAlive == false)
        {
            GetComponent<SpriteRenderer>().color = deadColor;    
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(aliveColor, gradientColor, Mathf.Clamp01(lifeTime/maxlifeTime));
        }
        
    }

    private void ApplyNextGeneration()
    {
        if(nextState != null)
        {
            isAlive = nextState;
            nextState = null;
            
        }
        UpdateSprites();
    }

#endregion

#region public methods

    public void SetFieldPosition(int x, int y) { position = new Vector2Int(x, y);}

    public void SetAlive(bool b)
    {
        if (b)
            isAlive = true;
        else
            isAlive = false;
    }

    public void UpdateCell()
    {
        StartCoroutine(CountLivingNeighbors());
        nextState = ChooseNextState(livingNeighbours);
        ApplyNextGeneration();
        
    }

#endregion

}
