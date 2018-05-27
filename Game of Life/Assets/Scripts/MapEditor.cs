using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MapEditor : GenericSingletonClass<MapEditor> {

    [SerializeField] MapContainerSO mapContainer;
    [SerializeField] TMP_InputField nameInputfield;
    [SerializeField] TMP_InputField cellsInXInputfield;
    [SerializeField] GameObject EditoCellPrefab;
    [SerializeField] GameObject CreatePanel;
    [SerializeField] GameObject ToolbarPanel;
    [SerializeField] GameObject SavePanel;
    [SerializeField] Slider cellsInXSlider;
    [SerializeField] CellSO cellProperties;
    bool[,] field;
    int fieldX, fieldY;
    string fieldName;
    bool pencil = true;

    private void Start()
    {
        UpdateXCellsValueFromFloat(100);
    }

    private bool CheckInputfields()
    {
        if (nameInputfield.text == "" || cellsInXInputfield.text == "" || int.Parse(cellsInXInputfield.text) < cellProperties.minCellsInX || int.Parse(cellsInXInputfield.text) > cellProperties.maxCellsInX)
            return false;
        else
            return true;
    }

    private int CalculateYCells(int fieldX)
    {
        return Mathf.RoundToInt(fieldX * (1 / Camera.main.aspect));
    }

    public void CreateField()
    {
        if (!CheckInputfields()) //Show Error Window?
            return;

        fieldX = int.Parse(cellsInXInputfield.text);
        fieldY = CalculateYCells(fieldX);

        field = new bool[fieldX, fieldY];
        for (int y = 0; y < fieldY; y++)
        {
            for (int x = 0; x < fieldX; x++)
            {
                field[x, y] = false; //Set all Cells in Array to dead
                GameObject go = (GameObject)Instantiate(EditoCellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                MapCell mapCell = go.GetComponent<MapCell>();
                mapCell.Position = new Vector2Int(x, y); //Write the position in the prefab script so we can get it with a raycast laster
                mapCell.ChangeColor(false); //Set the color of all cells to black: true==white, false==black
                
            }
        }

        PositionCamera(fieldX, fieldY);

        fieldName = nameInputfield.text;

        CreatePanel.SetActive(false);
        ToolbarPanel.SetActive(true);
        
    }

    private void PositionCamera(int fieldSizeX, int fieldSizeY)
    {
        Camera.main.transform.position = new Vector3((float)fieldSizeX / 2 - 0.5f, (float)fieldSizeY / 2 - 0.5f, -1);
        Camera.main.orthographicSize = fieldSizeX >= fieldSizeY ? fieldSizeY / 2 : fieldSizeX / 2;
    }

    public void ActivatePencil()
    {
        pencil = true;
    }

    public void ActivateEraser()
    {
        pencil = false;
    }

    public void SaveMap()
    {
        mapContainer.AddMap(new Map(field,fieldX, fieldY, fieldName));

        SceneManager.LoadScene(0);
    }

    public void LoadLauncher()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateXCellsValueFromFloat(float value)
    {
        cellsInXSlider.value = (int)value;
        cellsInXInputfield.text = value.ToString();
    }

    public void UpdateXCellsValueFromString(string value)
    {
        int intValue = int.Parse(value);

        intValue = intValue > cellProperties.maxCellsInX ? cellProperties.maxCellsInX : intValue;
        intValue = intValue < cellProperties.minCellsInX ? cellProperties.minCellsInX : intValue;

        cellsInXSlider.value = intValue;
        cellsInXInputfield.text = intValue.ToString();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            if (SavePanel.activeSelf)
                return;
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if(hit != null && hit.collider != null)
            {
                MapCell cell = hit.transform.GetComponent<MapCell>();

                if (pencil)
                {
                    field[cell.Position.x, cell.Position.y] = true;
                    cell.ChangeColor(true);
                }
                else if (!pencil)
                {
                    field[cell.Position.x, cell.Position.y] = false;
                    cell.ChangeColor(false);
                }
                
                
            }
                
               
         
        }
    }
}
