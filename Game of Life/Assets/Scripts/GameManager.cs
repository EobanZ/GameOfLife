using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : GenericSingletonClass<GameManager> {

    private static float timer = 0;
    [SerializeField]private float refreshTime = 5;
    private bool fieldIsReady = false;
    private Cell[,] cells;
    private Camera camera;
    private int fieldSizeY;
    [SerializeField] private int fieldSizeX; //<- später über UI setzten
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject cellPrefab;

    public GameObject CellPrefab { get { return cellPrefab; } }
    public GameObject Field { get { return field; } }
    public Cell[,] Cells { get { return cells; } }
    public Vector2Int FieldSize { get { return new Vector2Int(fieldSizeX, fieldSizeY); } }


    void Start () {
        camera = Camera.main; 
        fieldSizeY = Mathf.RoundToInt(fieldSizeX * (1 / camera.aspect));
        cells = new Cell[fieldSizeX,fieldSizeY];

        PositionCamera();
        StartCoroutine(BuildFieldCR(true));
	}
	
	void Update () {
        timer += Time.deltaTime;
        if(timer > (float)1/refreshTime && fieldIsReady)
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

    IEnumerator BuildFieldCR(bool random)
    {
        for (int y = 0; y < fieldSizeY; y++)
        {
            for (int x = 0; x < fieldSizeX; x++)
            {
                GameObject go = (GameObject)Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity, field.transform);
                Cell c = go.GetComponent<Cell>();
                c.SetFieldPosition(x, y);
                cells[x, y] = c;
                if (random)
                {
                    int i = (int)Random.Range(0, 1.99f);
                    bool b = i == 0 ? false : true;
                    go.GetComponent<Cell>().SetAlive(b);
                }
            }
        }
        fieldIsReady = true;
        yield return null;
    }

    //Updates each cell in the Field
    void UpdateField()
    {
        
 
        for (int y = 0; y < fieldSizeY; y++)
        {
            for (int x = 0; x < fieldSizeX; x++)
            {
                cells[x, y].UpdateCell();
               
            }
        }
    }

    
}
