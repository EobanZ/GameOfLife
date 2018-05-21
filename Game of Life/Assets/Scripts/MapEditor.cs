using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MapEditor : GenericSingletonClass<MapEditor> {

    [SerializeField] MapContainerSO mapContainer;
    [SerializeField] TMP_InputField nameInputfield;
    [SerializeField] TMP_InputField cellsInXInputfield;
    bool[,] field;
    int fieldX, fieldY;
    string fieldName;

    private bool CheckInputfields()
    {
        if (nameInputfield.text == "" || cellsInXInputfield.text == "" || int.Parse(cellsInXInputfield.text) < 4 || int.Parse(cellsInXInputfield.text) > 300)
            return false;
        else
            return true;
    }

    private int CalculateYCells(int fieldX)
    {
        return Mathf.RoundToInt(fieldX * (1 / Camera.main.aspect));
    }

    public void CreateField() //Assign button and check if all input fields are filled
    {
        if (!CheckInputfields()) //Show Error Window?
            return;

        fieldX = int.Parse(cellsInXInputfield.text);
        fieldY = CalculateYCells(fieldX);

        field = new bool[fieldX, fieldY];
        for (int i = 0; i < fieldY; i++)
        {
            for (int j = 0; j < fieldX; j++)
            {
                field[i, j] = true;
                //Instanticate MapCellPrefabs
                //MoveCamera
            }
        }
    }

 

    public void CheckIfNumber(string s)
    {
        int numberInteger;
        if (int.TryParse(s, out numberInteger))
            return;
        else
            cellsInXInputfield.text = cellsInXInputfield.text.Remove(cellsInXInputfield.text.Length-1);
    }
    public void ToggleCellAt(Vector2Int pos)
    {
        field[pos.x, pos.y] = !field[pos.x, pos.y];
    }

    public void ActivateCellAt(Vector2Int pos)
    {
        field[pos.x, pos.y] = true;
    }

    public void DeactivateCellAt(Vector2Int pos)
    {
        field[pos.x, pos.y] = false;
    }

    public void SaveMap()
    {
        mapContainer.AddMap(new Map(field,fieldX, fieldY, fieldName));
    }
   
    void Update()
    {
        //Get User input and check what mode is chosen
    }
}
