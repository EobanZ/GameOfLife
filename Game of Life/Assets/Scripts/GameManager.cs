using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : GenericSingletonClass<GameManager> {

    float timer = 0;
    float refreshTime = 1/30;
    public int fieldSizeX, fieldSizeY;
    GameObject[,] cells;
    Camera camera;

    public GameObject cellPrefab;
    public GameObject Field;
    public GameObject[,] Cells { get { return cells; } }
    


    void Start () {
        camera = Camera.main; 
        fieldSizeY = Mathf.RoundToInt(fieldSizeX * (1 / camera.aspect));
        cells = new GameObject[fieldSizeX,fieldSizeY];

        PositionCamera();
        BuildField(true); 
	}
	
	void Update () {
        timer += Time.deltaTime;
        if(timer >= refreshTime)
        {
            UpdateField();
            timer = 0;
        }
        
	}

    void PositionCamera()
    {
          
        camera.transform.position = new Vector3((float)fieldSizeX / 2 - 0.5f, (float)fieldSizeY / 2 - 0.5f, -1);
        camera.orthographicSize = fieldSizeX >= fieldSizeY? fieldSizeY/2 : fieldSizeX/2 ;
    }

    void BuildField(bool random)
    {
       
        for (int x = 0; x < fieldSizeX; x++)
        {
            for (int y = 0; y < fieldSizeY; y++)
            {
                GameObject go = (GameObject)Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity,Field.transform);
                go.GetComponent<Cell>().SetFieldPosition(x, y);
                cells[x, y] = go;
                if (random)
                {
                    int i = (int)Random.Range(0, 1.99f);
                    bool b = i == 0 ? false : true;
                    go.GetComponent<Cell>().SetAlive(b);
                }
            }
        }
    }

    //Updates each cell in the Field
    void UpdateField()
    {
        for (int x = 0; x < fieldSizeX; x++)
        {
            for (int y = 0; y < fieldSizeY; y++)
            {

                cells[x, y].GetComponent<Cell>().UpdateCell();
               
            }
        }
    }

    
}
