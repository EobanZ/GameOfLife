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

    Map(bool[,] Field, int width, int height) { createMap(Field,width,height); }

    void createMap(bool[,] Field, int width, int height) { this.field = Field; this.width = width; this.height = height; }

}
