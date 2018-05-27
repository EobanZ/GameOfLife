using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Launcher : GenericSingletonClass<Launcher> {

    [SerializeField] MapContainerSO maps;
    [SerializeField] CellSO cellProperties;
    [SerializeField] TMP_Dropdown mapsDropdown;

    List<string> mapliste = new List<string>();

    // Use this for initialization
    void Start () {

        mapliste.Clear();
        mapsDropdown.ClearOptions();

        mapliste.Add("Random");

        foreach (var map in maps.Maps)
        {
            mapliste.Add(map.Key);
        }
        mapsDropdown.AddOptions(mapliste);
        mapsDropdown.RefreshShownValue();

        maps.SetChosenMap(mapsDropdown.captionText.text);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChooseMap(int dropdownIdex)
    {
        maps.SetChosenMap(mapsDropdown.captionText.text);

        if(dropdownIdex != 0)
        {
            //cells in x ausblenden
        }
    }

    public void StartSimulation()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenMapEditor()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGameOfLife()
    {
        Application.Quit();
    }

   
}
