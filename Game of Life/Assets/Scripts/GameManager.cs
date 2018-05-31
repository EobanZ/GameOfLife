using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingletonClass<GameManager> {

    private static float timer = 0;  
    private bool fieldIsReady = false;
    private Cell[,] cells; //refs to the instantiated cells will be stored here
    private Camera camera;
    private int fieldSizeY;
    private int fieldSizeX;
    private bool simulate = false;
    private float refreshTime; //fps
    
    [SerializeField] private GameObject toolPanel; //ref to the Tool Panel
    [SerializeField] private TMPro.TMP_Text fpsText; //ref to the FPS text  
    [SerializeField] private GameObject field; //ref to the field
    [SerializeField] private GameObject cellPrefab; //the Prefab used for the Cells
    [SerializeField] private MapContainerSO maps; //ref to the MapContainer ScriptableObject
    [SerializeField] private CellSO cellProperties; //ref tho the Cell Properties ScriptableObject

    public Sprite PlayButtonIcon, PauseButtonIcon;
    public Image PlayPauseButtonImage;

    public GameObject CellPrefab { get { return cellPrefab; } }
    public GameObject Field { get { return field; } }
    public Cell[,] Cells { get { return cells; } }
    public Vector2Int FieldSize { get { return new Vector2Int(fieldSizeX, fieldSizeY); } }


#region Private Methods 
    private void Start ()
    {
        //If a Map was chosen, get the Maps settings. Else use the randomWith and calculate Y Cells
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
	
	private void Update () {
        timer += Time.deltaTime;

        //updates the field depending on the framerate. field has to be ready and simulate flag has to be true
        if(timer > (float)1/refreshTime && fieldIsReady && simulate)
        {
            UpdateField();
            timer = 0;
        }
        
	}

    /// <summary>
    /// Positions the camera depending on the width and height of the field
    /// </summary>
    void PositionCamera()
    {
          
        camera.transform.position = new Vector3((float)fieldSizeX / 2 - 0.5f, (float)fieldSizeY / 2 - 0.5f, -1);
        camera.orthographicSize = fieldSizeX >= fieldSizeY? fieldSizeY/2 : fieldSizeX/2 ;
    }

    /// <summary>
    /// Coroutine to build the field from a map or random
    /// </summary>
    /// <returns></returns>
    IEnumerator BuildFieldCR()
    {
        for (int y = 0; y < fieldSizeY; y++)
        {
            for (int x = 0; x < fieldSizeX; x++)
            {
                GameObject go = (GameObject)Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity, field.transform); //create cell gameobject in the scene as child of the field
                Cell c = go.GetComponent<Cell>(); //get the Cell Script
                c.SetFieldPosition(x, y); //tell the cell its position
                cells[x, y] = c; //write a reference to the created cell in our array
                if (maps.ChosenMap == null) //If no map is chosen, build a random field
                {
                    int i = (int)Random.Range(0, 1.99f);
                    bool b = i == 0 ? false : true;
                    go.GetComponent<Cell>().SetAlive(b);
                }
                else //Build the chosen Map
                {
                    go.GetComponent<Cell>().SetAlive(maps.ChosenMap.Field[x, y]);
                }
            }
        }
        fieldIsReady = true;
        yield return null;
    }

    /// <summary>
    /// Updates each cell in the Field
    /// </summary>
    void UpdateField()
    {
        
        //Get the next state for each cell
        for (int y = 0; y < fieldSizeY; y++)
        {
            for (int x = 0; x < fieldSizeX; x++)
            {
                cells[x, y].GetNextState();
               
            }
        }

        //Apply the next state
        for (int y = 0; y < fieldSizeY; y++)
        {
            for (int x = 0; x < fieldSizeX; x++)
            {
                cells[x, y].ApplyNextGeneration();

            }
        }
    }

    #endregion

#region public Methods

    public void ToggleSimulate()
    {
        simulate = !simulate;


        if (simulate)
        {
            toolPanel.SetActive(false);
            PlayPauseButtonImage.sprite = PauseButtonIcon; 
        }
        else
        {
            toolPanel.SetActive(true);
            PlayPauseButtonImage.sprite = PlayButtonIcon;
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

#endregion
}
