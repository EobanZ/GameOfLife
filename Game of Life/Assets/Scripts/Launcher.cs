using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Launcher : GenericSingletonClass<Launcher> {

    [SerializeField] MapContainerSO mapContainer; //ref to the MapContainer Scriptable Object
    [SerializeField] CellSO cellProperties; //ref to the CellProperties Scriptable Object
    [SerializeField] TMP_Dropdown mapsDropdown; //ref to the Maps Dropdown
    [SerializeField] GameObject cellsInXUi; //ref to the CellsInX Panel
    [SerializeField] Slider fpsSlider; //ref to the fps slider
    [SerializeField] TMP_InputField fpsInputField; //ref to the fps InputField
    [SerializeField] Slider xCellsSlider; //ref to the CellsInX Slider
    [SerializeField] TMP_InputField xCellsInputField; //ref to the CellsInX InputField

    int minCellsInX;
    int maxCellsInX;
    int minFPS;
    int maxFPS;

    List<string> mapliste = new List<string>();

    void Start () {
        

        mapliste.Clear();
        mapsDropdown.ClearOptions();

        mapliste.Add("Random"); //set Random to index 0

        //add the Maps to the list
        foreach (var map in mapContainer.Maps)
        {
            mapliste.Add(map.Key);
        }
        mapsDropdown.AddOptions(mapliste); //add the list to show in dropdown menu
        mapsDropdown.RefreshShownValue();

        mapContainer.SetChosenMap(mapsDropdown.captionText.text); //Reset the chosenMap to random

        minCellsInX = cellProperties.minCellsInX;
        maxCellsInX = cellProperties.maxCellsInX;
        minFPS = cellProperties.minFPS;
        maxFPS = cellProperties.maxFPS;

        UpdateFPSValueFromFloat(10);
        UpdateXCellsValueFromFloat(100);
	}
	

    public void ChooseMap(int dropdownIdex)
    {
        mapContainer.SetChosenMap(mapsDropdown.captionText.text);

        //if the index is not 0 -> a map was chosen so we dont need the CellsInX Panel
        if (dropdownIdex != 0)
        {
            cellsInXUi.SetActive(false);
        }
        else
            cellsInXUi.SetActive(true);
    }

    public void StartSimulation()
    {
        StartCoroutine(LoadSceneAsync(2));
    }

    public void LoadMapEditor()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGameOfLife()
    {
        Application.Quit();
    }

    public void DeleteAllMaps()
    {
        mapContainer.Maps.Clear();
        mapsDropdown.options.Clear();
        mapsDropdown.options.Add(new TMP_Dropdown.OptionData("Random"));
        mapsDropdown.RefreshShownValue();
    }

    //used tho sync slider and inputfield
    public void UpdateFPSValueFromFloat(float value)
    { 
        fpsSlider.value = (int)value;
        fpsInputField.text = value.ToString();
        cellProperties.fps = (int)value;
    }

    //used tho sync slider and inputfield
    public void UpdateFPSValueFromString(string value)
    {
        int intValue = int.Parse(value);

        intValue = intValue > maxFPS ? maxFPS : intValue;
        intValue = intValue < minFPS ? minFPS : intValue;

        fpsSlider.value = intValue;
        fpsInputField.text = intValue.ToString();
        cellProperties.fps = intValue;
    }

    //used tho sync slider and inputfield
    public void UpdateXCellsValueFromFloat(float value)
    {
        xCellsSlider.value = (int)value;
        xCellsInputField.text = value.ToString();

        mapContainer.randomWidth = (int)value;
    }

    //used tho sync slider and inputfield
    public void UpdateXCellsValueFromString(string value)
    {
        int intValue = int.Parse(value);

        intValue = intValue > maxCellsInX ? maxCellsInX : intValue;
        intValue = intValue < minCellsInX ? minCellsInX : intValue;

        xCellsSlider.value = intValue;
        xCellsInputField.text = intValue.ToString();

        mapContainer.randomWidth = intValue;
    }

    IEnumerator LoadSceneAsync(int i)
    {
        yield return new WaitForSeconds(1); //to fill 1 complete loading circle

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(i);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
   
}
