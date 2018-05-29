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
    [SerializeField] GameObject cellsInXUi;
    [SerializeField] Slider fpsSlider;
    [SerializeField] TMP_InputField fpsInputField;
    [SerializeField] Slider xCellsSlider;
    [SerializeField] TMP_InputField xCellsInputField;
    int minCellsInX;
    int maxCellsInX;
    int minFPS;
    int maxFPS;

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

        minCellsInX = cellProperties.minCellsInX;
        maxCellsInX = cellProperties.maxCellsInX;
        minFPS = cellProperties.minFPS;
        maxFPS = cellProperties.maxFPS;

        UpdateFPSValueFromFloat(10);
        UpdateXCellsValueFromFloat(50);
	}
	

    public void ChooseMap(int dropdownIdex)
    {
        maps.SetChosenMap(mapsDropdown.captionText.text);

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

    public void OpenMapEditor()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGameOfLife()
    {
        Application.Quit();
    }

    public void UpdateFPSValueFromFloat(float value)
    { 
        fpsSlider.value = (int)value;
        fpsInputField.text = value.ToString();
        cellProperties.fps = (int)value;
    }

    public void UpdateFPSValueFromString(string value)
    {
        int intValue = int.Parse(value);

        intValue = intValue > maxFPS ? maxFPS : intValue;
        intValue = intValue < minFPS ? minFPS : intValue;

        fpsSlider.value = intValue;
        fpsInputField.text = intValue.ToString();
        cellProperties.fps = intValue;
    }

    public void UpdateXCellsValueFromFloat(float value)
    {
        xCellsSlider.value = (int)value;
        xCellsInputField.text = value.ToString();
    }

    public void UpdateXCellsValueFromString(string value)
    {
        int intValue = int.Parse(value);

        intValue = intValue > maxCellsInX ? maxCellsInX : intValue;
        intValue = intValue < minCellsInX ? minCellsInX : intValue;

        xCellsSlider.value = intValue;
        xCellsInputField.text = intValue.ToString();
    }

    IEnumerator LoadSceneAsync(int i)
    {
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(i);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
   
}
