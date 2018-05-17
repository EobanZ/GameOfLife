
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMapContainer", menuName = "MapContainer")]
public class MapContainerSO : ScriptableObject
{
    Dictionary<string, Map> maps = new Dictionary<string, Map>();

    public Dictionary<string, Map> Maps { get { return maps; } }

    public void AddMap(Map m)
    {
        if(!maps.ContainsKey(m.Name))
        maps.Add(m.Name, m);
    }

    public void removeMap(string mapName)
    {
        if (maps.ContainsKey(mapName))
            maps.Remove(mapName);
    }

    

}
