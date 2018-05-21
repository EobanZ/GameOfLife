using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    string name;
    int width;
    int height;
    bool[,] field;

    public string Name { get { return name; } }

    public Map(bool[,] Field, int width, int height, string mname)
    {
        createMap(Field,width,height, mname);
    }

    void createMap(bool[,] Field, int width, int height, string mname)
    {
        this.field = Field;
        this.width = width;
        this.height = height;
        this.name = mname; }

}
