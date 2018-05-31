using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    [SerializeField] string name;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] bool[] field; //Had to use 1d array because 2d arrays aren't serializable

    public string Name { get { return this.name; } }
    public int Width { get { return this.width; } }
    public int Height { get { return this.height; } }
    public bool[,] Field
    {
        get
        {
            //1D Array to 2D
            bool[,] fld = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    fld[x, y] = this.field[y * width + x];
                }
            }
            return fld;

            } }


    public Map(bool[,] Field, int width, int height, string mname)
    {
        createMap(Field,width,height, mname);
        
    }

    void createMap(bool[,] Field, int width, int height, string mname)
    {
        
        this.width = width;
        this.height = height;
        this.name = mname;
        this.field = new bool[width * height];
        //2D array to 1D
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                this.field[y * width + x] = Field[x, y];
            }
        }

    }



}
