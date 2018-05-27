using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingletonClass<GameManager> {

    private static float timer = 0;  
    private bool fieldIsReady = false;
    private Cell[,] cells;
    private Camera camera;
    private int fieldSizeY;
    bool simulate = false;
    [SerializeField] private float refreshTime = 5;
    [SerializeField] private GameObject toolPanel;
    [SerializeField] private TMPro.TMP_Text fpsText;
    [SerializeField] private int fieldSizeX; //<- später über UI setzten
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private MapContainerSO maps;
    [SerializeField] private CellSO cellProperties;

    public GameObject CellPrefab { get { return cellPrefab; } }
    public GameObject Field { get { return field; } }
    public Cell[,] Cells { get { return cells; } }
    public Vector2Int FieldSize { get { return new Vector2Int(fieldSizeX, fieldSizeY); } }


    void Start () {
        if(maps.ChosenMap != null)
        {
            fieldSizeX = maps.ChosenMap.Width;
            fieldSizeY = maps.ChosenMap.Height;
        }
        else
        {
            fieldSizeX = maps.randomWidth;
            fieldSizeY = Mathf.RoundToInt(fieldSizeX * (1 / Camera.main.aspect));
        }
        
        camera = Camera.main; 
        cells = new Cell[fieldSizeX,fieldSizeY];

        refreshTime = cellProperties.fps;
        fpsText.text = "FPS: " + refreshTime.ToString();

        PositionCamera();
        StartCoroutine(BuildFieldCR());
	}
	
	void Update () {
        timer += Time.deltaTime;
        if(timer > (float)1/refreshTime && fieldIsReady && simulate)
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

    IEnumerator BuildFieldCR()
    {
        for (int y = 0; y < fieldSizeY; y++)
        {
            for (int x = 0; x < fieldSizeX; x++)
            {
                GameObject go = (GameObject)Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity, field.transform);
                Cell c = go.GetComponent<Cell>();
                c.SetFieldPosition(x, y);
                cells[x, y] = c;
                if (maps.ChosenMap == null) //If no map is chosen, build a random field
                {
                    int i = (int)Random.Range(0, 1.99f);
                    bool b = i == 0 ? false : true;
                    go.GetComponent<Cell>().SetAlive(b);
                }
                else
                {
                    go.GetComponent<Cell>().SetAlive(maps.ChosenMap.Field[x, y]);
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
                cells[x, y].GetNextState();
               
            }
        }

        for (int y = 0; y < fieldSizeY; y++)
        {
            for (int x = 0; x < fieldSizeX; x++)
            {
                cells[x, y].ApplyNextGeneration();

            }
        }
    }

    public void ToggleSimulate()
    {
        simulate = !simulate;


        if (simulate)
        {
            toolPanel.SetActive(false);
        }
        else
        {
            toolPanel.SetActive(true);
        }
    }

    public void LoadLauncher()
    {
        SceneManager.LoadScene(0);
    }

    public void IncrementFPS()
    {
        if (refreshTime == cellProperties.maxFPS)
            return;

        refreshTime++;
        fpsText.text = "FPS: "+ refreshTime.ToString();
    }

    public void DecrementFPS()
    {
        if (refreshTime == cellProperties.minFPS)
            return;

        refreshTime--;
        fpsText.text = "FPS: " + refreshTime.ToString();
    }
    
}
