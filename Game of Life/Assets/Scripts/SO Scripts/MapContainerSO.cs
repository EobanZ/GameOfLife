
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMapContainer", menuName = "MapContainer")]
public class MapContainerSO : ScriptableObject
{
    [SerializeField, HideInInspector]DictionaryOfMaps maps;
    [SerializeField]private Map chosenMap;

    public DictionaryOfMaps Maps { get { return maps; } } 
    public Map ChosenMap { get { return chosenMap; } }

    public int randomWidth { get; set; }

    private void OnEnable()
    {
        if(maps == null)
        {
            maps = new DictionaryOfMaps();
        }
        //maps.Clear();
    }
    public void AddMap(Map m)
    {
        if(!maps.ContainsKey(m.Name))
            maps.Add(m.Name, m);
           
        
    }

    public void RemoveMap(string mapName)
    {
        if (maps.ContainsKey(mapName))
            maps.Remove(mapName);
    }


    public void SetChosenMap(string name)
    {
        if (Maps.ContainsKey(name))
        {
            chosenMap = Maps[name];
        }
        else
        {
            chosenMap = null;
        } 
    }

    

}

