using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    bool? isAlive = true;
    bool? nextState = null;
    float lifeTime = 0; //the amount of seconds this cell is alive
    Vector2 position;
    [SerializeField] float maxlifeTime = 10f; //After this amount of seconds the Cell is the gradientColor
    [SerializeField] Color aliveColor = Color.white;
    [SerializeField] Color deadColor = Color.black;
    [SerializeField] Color gradientColor = Color.red;

#region private methods

    private void Start()
    {
        UpdateSprites(); //Updates the Sprites Colors once the game is started
    }

    private void Update()
    {
        if(isAlive == true)
            lifeTime += Time.deltaTime;
    }

    private int CountLivingNeighbors() { return 0; }

    private bool ChooseNextState(int livingNeighbors) { return true; }

    private void UpdateSprites()
    {
        if (isAlive == false)
        {
            GetComponent<SpriteRenderer>().color = deadColor;
            lifeTime = 0;
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

    public void SetFieldPosition(int x, int y) { position = new Vector2(x, y);}

    public void SetAlive(bool b)
    {
        if (b)
            isAlive = true;
        else
            isAlive = false;
    }

    public void UpdateCell()
    {
        ChooseNextState(CountLivingNeighbors());
        ApplyNextGeneration();
    }

#endregion

}
